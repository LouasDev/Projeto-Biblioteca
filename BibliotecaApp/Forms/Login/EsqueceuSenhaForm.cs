using BibliotecaApp.Services;
using BibliotecaApp.Utils;
using System;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Login
{
    public partial class EsqueceuSenhaForm : Form
    {
        #region Constants
        private const string TabelaUsuarios = "usuarios";
        private const string ColunaNome = "nome";
        private const string ColunaEmail = "email";
        #endregion

        #region Private Fields
        // Estado do código gerado
        private string _codigoAtual;
        private DateTime? _expiraEm;
        private string _emailDestino;

        // Cooldown "Reenviar Código"
        private readonly Timer _cooldownTimer = new Timer { Interval = 1000 };
        private int _secondsRemaining = 0;

        // Exibição temporária do código
        private readonly Timer _revealTimer = new Timer { Interval = 1000 };
        private int _revealSecondsLeft = 0;
        private bool _showingCode = false;

        // Guardas de reentrância (impede cliques duplos gerarem ações duplicadas)
        private bool _isSending = false;
        private bool _isVerifying = false;
        #endregion

        #region Constructor
        public EsqueceuSenhaForm()
        {
            InitializeComponent();
            ConfigurarTimers();
            AtualizarLblReenviar();
        }
        #endregion

        #region Form Events
        private void EsqueceuSenhaForm_Load(object sender, EventArgs e)
        {
            txtTeste.BackColor = Color.White;
            txtTeste.Location = new Point(-200, -200);
            lblCodigo.Location = new Point(-200, -200);
            pnBarra2.Location = new Point(-200, -200);
            btnTeste.Location = new Point(-200, -200);
            pnSenha.Location = new Point(-200, -200);
            btnTrocarSenha.Location = new Point(-200, -200);

            txtNovaSenha.KeyDown += txtNovaSenha_KeyDown;
            txtConfirmarSenha.KeyDown += txtConfirmarSenha_KeyDown;
            txtEmail.KeyDown += txtEmail_KeyDown;
            txtTeste.KeyDown += txtTeste_KeyDown;
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
               btnEnviar.PerformClick();
            }
        }


        private void txtTeste_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnTeste.PerformClick();
            }
        }



        private void txtNovaSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                txtConfirmarSenha.Focus();
            }
        }

        private void txtConfirmarSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnTrocarSenha.PerformClick();
            }
        }


        private void picExit_Click(object sender, EventArgs e)
        {
            const string msg = "Tem certeza de que quer fechar a Aplicação?";
            const string box = "Confirmação de Encerramento";
            var confirma = MessageBox.Show(msg, box, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form loginForm = Application.OpenForms["LoginForm"];

            if (loginForm != null)
            {
                this.Hide();
                loginForm.Show();
                loginForm.BringToFront();
            }
            else
            {
                this.Hide();
                LoginForm novoLogin = new LoginForm();
                novoLogin.Show();
            }
        }
        #endregion

        #region Button Events
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text?.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show(
                    "O campo de e-mail está vazio. Por favor, preencha o e-mail.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (EnviarCodigoComValidacao(email))
            {
                TransicionarParaVerificacaoCodigo();
            }
        }

        private void btnTestar_Click(object sender, EventArgs e)
        {
            if (_isVerifying) return;
            _isVerifying = true;

            try
            {
                var codigoDigitado = txtTeste.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(_emailDestino) || string.IsNullOrWhiteSpace(_codigoAtual) || _expiraEm == null)
                {
                    MessageBox.Show("Nenhum código válido foi gerado. Envie um novo código.");
                    return;
                }

                if (DateTime.UtcNow > _expiraEm)
                {
                    MessageBox.Show("O código expirou. Solicite um novo código.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(codigoDigitado))
                {
                    MessageBox.Show("Digite o código recebido por e-mail.");
                    return;
                }

                if (codigoDigitado == _codigoAtual)
                {
                    MessageBox.Show("Código correto! Você pode prosseguir com a redefinição de senha.");
                    TransicionarParaNovaSenha();
                    InutilizarCodigo();
                    AtualizarLblReenviar();
                }
                else
                {
                    MessageBox.Show("Código incorreto. Tente novamente.");
                }
            }
            finally
            {
                _isVerifying = false;
            }
        }

        private void btnTrocarSenha_Click(object sender, EventArgs e)
        {
            string novaSenha = txtNovaSenha.Text?.Trim();
            string confirmarSenha = txtConfirmarSenha.Text?.Trim();

            if (string.IsNullOrWhiteSpace(novaSenha) || string.IsNullOrWhiteSpace(confirmarSenha))
            {
                MessageBox.Show("Preencha os dois campos de senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (novaSenha.Length < 4)
            {
                MessageBox.Show("A senha deve ter pelo menos 4 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (novaSenha != confirmarSenha)
            {
                MessageBox.Show("As senhas não coincidem. Verifique e tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Gerar hash e salt usando a classe CriptografiaSenha
            string hashGerado;
            string saltGerado;
            CriptografiaSenha.CriarHash(novaSenha, out hashGerado, out saltGerado);

            // Atualizar no banco de dados
            try
            {
                using (SqlCeConnection conn = Conexao.ObterConexao())
                {
                    conn.Open();
                    string query = $"UPDATE usuarios SET Senha_Hash = @Hash, Senha_Salt = @Salt WHERE Email = @Email";

                    using (SqlCeCommand cmd = new SqlCeCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Hash", hashGerado);
                        cmd.Parameters.AddWithValue("@Salt", saltGerado);
                        cmd.Parameters.AddWithValue("@Email", _emailDestino); // ou txtEmail.Text.Trim()

                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Senha alterada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close(); // ou redirecionar para tela de login
                        }
                        else
                        {
                            MessageBox.Show("Usuário não encontrado. Verifique o e-mail.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Label Events
        private void lblReenviar_Click(object sender, EventArgs e)
        {
            if (_secondsRemaining > 0)
            {
                MessageBox.Show("Aguarde o término da contagem para reenviar o código.");
                return;
            }

            var email = txtEmail.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Informe o e-mail antes de reenviar o código.");
                return;
            }

            EnviarCodigo(email, reiniciarCooldown: true);
        }

        private void lblReenviar_DoubleClick(object sender, EventArgs e)
        {
            MostrarCodigoTemporariamente();
        }
        #endregion

        #region UI Transition Methods
        private void TransicionarParaVerificacaoCodigo()
        {
            lblTop.Text = "Digite o Código enviado.";
            lblTop.Location = new Point(545, 90);

            txtEmail.Visible = false;
            txtEmail.Enabled = false;
            pnBarra.Visible = false;
            lblDigite.Visible = false;

            txtTeste.Location = txtEmail.Location;
            pnBarra2.Location = pnBarra.Location;
            lblCodigo.Location = lblDigite.Location;
            btnTeste.Location = btnEnviar.Location;
            lblReenviar.Visible = true;
            btnVoltar.Visible = false;
            txtTeste.Focus();
        }

        private void TransicionarParaNovaSenha()
        {
            lblTop.Text = "Digite uma nova senha.";
            lblTop.Location = new Point(550, 90);

            txtTeste.Visible = false;
            pnBarra2.Visible = false;
            lblCodigo.Visible = false;
            btnTeste.Visible = false;
            lblReenviar.Visible = false;
            btnVoltar.Visible = false;
            pnSenha.Location = new Point(561, 244);
            btnTrocarSenha.Location = btnTeste.Location;

            txtNovaSenha.Focus();
        }
        #endregion

        #region Timer Configuration
        private void ConfigurarTimers()
        {
            _cooldownTimer.Tick += (s, e) =>
            {
                if (_secondsRemaining > 0)
                {
                    _secondsRemaining--;
                    AtualizarLblReenviar();
                }

                if (_secondsRemaining <= 0)
                {
                    _cooldownTimer.Stop();
                    AtualizarLblReenviar();
                }
            };

            _revealTimer.Tick += (s, e) =>
            {
                if (_revealSecondsLeft > 0)
                {
                    _revealSecondsLeft--;
                    if (_showingCode)
                    {
                        lblReenviar.Text = $"Código: {_codigoAtual} (some em {_revealSecondsLeft}s)";
                    }
                }

                if (_revealSecondsLeft <= 0)
                {
                    _revealTimer.Stop();
                    _showingCode = false;
                    AtualizarLblReenviar();
                }
            };
        }

        private void AtualizarLblReenviar()
        {
            if (_showingCode) return;

            if (_secondsRemaining > 0)
            {
                lblReenviar.Text = $"Reenviar em {_secondsRemaining}s";
                lblReenviar.ForeColor = SystemColors.GrayText;
                lblReenviar.Cursor = Cursors.No;
            }
            else
            {
                lblReenviar.Text = "Reenviar código";
                lblReenviar.ForeColor = Color.RoyalBlue;
                lblReenviar.Cursor = Cursors.Hand;
            }
        }

        private void IniciarCooldown(int seconds)
        {
            _secondsRemaining = seconds;
            _cooldownTimer.Stop();
            _cooldownTimer.Start();
            AtualizarLblReenviar();
        }
        #endregion

        #region Email and Code Logic
        private bool EnviarCodigoComValidacao(string email)
        {
            if (_isSending) return false;
            _isSending = true;

            var prevBtnEnviar = btnEnviar.Enabled;
            var prevLblReenviar = lblReenviar.Enabled;
            btnEnviar.Enabled = false;
            lblReenviar.Enabled = false;

            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    MessageBox.Show("Por favor, preencha o campo de e-mail.");
                    return false;
                }

                if (!EmailValido(email))
                {
                    MessageBox.Show("E-mail inválido.");
                    return false;
                }

                string nome;
                try
                {
                    nome = ObterNomePorEmail(email);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o banco de dados: " + ex.Message);
                    return false;
                }

                if (string.IsNullOrEmpty(nome))
                {
                    MessageBox.Show("E-mail não encontrado no sistema.");
                    return false;
                }

                _codigoAtual = GerarCodigoSeisDigitos();
                _expiraEm = DateTime.UtcNow.AddMinutes(10);
                _emailDestino = email;

                var assunto = "🔐 Código de Verificação - Biblioteca Monteiro Lobato";
                var corpoHtml = $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #fff; border-radius: 8px; padding: 20px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
      <h2 style='color: #2c3e50;'>Olá, {Html(nome)} 👋</h2>
      <p>Recebemos uma solicitação de redifinição de senha no sistema da biblioteca.</p>
      <p style='font-size: 18px;'><strong>🔐 Seu código de verificação:</strong></p>
      <div style='font-size: 32px; font-weight: bold; color: #27ae60; margin: 20px 0;'>{_codigoAtual}</div>
      <p>Use este código para concluir sua verificação. Ele expira em 10 minutos.</p>
      <hr />
      <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
    </div>
  </body>
</html>";

                try
                {
                    EmailService.Enviar(email, assunto, corpoHtml);
                    MessageBox.Show("Código enviado com sucesso para " + email);

                    _showingCode = false;
                    _revealTimer.Stop();
                    IniciarCooldown(60);

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao enviar e-mail: " + ex.Message);
                    return false;
                }
            }
            finally
            {
                _isSending = false;
                btnEnviar.Enabled = prevBtnEnviar;
                lblReenviar.Enabled = prevLblReenviar;
            }
        }

        private void EnviarCodigo(string email, bool reiniciarCooldown)
        {
            EnviarCodigoComValidacao(email);
        }

        private void MostrarCodigoTemporariamente()
        {
            if (string.IsNullOrEmpty(_codigoAtual) || _expiraEm == null || DateTime.UtcNow > _expiraEm)
            {
                MessageBox.Show("Nenhum código ativo para exibir. Gere um novo código.");
                return;
            }

            _showingCode = true;
            _revealSecondsLeft = 8;
            lblReenviar.Text = $"Código: {_codigoAtual} (some em {_revealSecondsLeft}s)";
            lblReenviar.ForeColor = Color.DarkGreen;

            _revealTimer.Stop();
            _revealTimer.Start();
        }
        #endregion

        #region Utility Methods
        private static bool EmailValido(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return string.Equals(addr.Address, email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static string GerarCodigoSeisDigitos()
        {
            int numero = GetSecureInt32(100000, 1_000_000);
            return numero.ToString("D6");
        }

        private static int GetSecureInt32(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue deve ser maior que minValue.");

            long range = (long)maxValue - minValue;
            if (range > uint.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(maxValue), "Intervalo muito grande.");

            uint rangeU = (uint)range;
            ulong twoPow32 = 1UL << 32;
            ulong limit = twoPow32 - (twoPow32 % rangeU);

            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[4];
                while (true)
                {
                    rng.GetBytes(bytes);
                    uint value = BitConverter.ToUInt32(bytes, 0);

                    if ((ulong)value >= limit)
                        continue;

                    uint withinRange = value % rangeU;
                    return (int)(minValue + withinRange);
                }
            }
        }

        private static string Html(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }

        private void InutilizarCodigo()
        {
            _codigoAtual = null;
            _expiraEm = null;
        }
        #endregion

        #region Database Methods
        private string ObterNomePorEmail(string email)
        {
            var sql = $"SELECT {ColunaNome} FROM {TabelaUsuarios} WHERE {ColunaEmail} = @Email";

            using (SqlCeConnection conn = Conexao.ObterConexao())
            {
                conn.Open();
                using (SqlCeCommand cmd = new SqlCeCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return null;

                    return Convert.ToString(result);
                }
            }
        }
        #endregion

       
    }
}
