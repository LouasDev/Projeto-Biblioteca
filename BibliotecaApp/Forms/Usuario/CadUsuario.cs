using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BibliotecaApp.Froms.Usuario
{
    public partial class CadUsuario : Form
    {
        #region Construtores e Variáveis
        private List<string> turmasCadastradas = new List<string>();


        private static bool IsAdminLogado()=> string.Equals(Sessao.NomeBibliotecariaLogada, "Administrador", StringComparison.OrdinalIgnoreCase);

        private List<string> todasTurmasPadrao;
        public event EventHandler UsuarioCriado;


        public CadUsuario(List<Usuarios> usuarios)
        {
            InitializeComponent();
        }

        public CadUsuario()
        {
            InitializeComponent();
        }
        #endregion

        #region Eventos do Formulário
        private void CadUsuario_Load(object sender, EventArgs e)
        {
            // Configuração inicial do formulário
            dtpDataNasc.Value = DateTime.Today;
            HabilitarCampos(false);
            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;
            chkMostrarSenha.ForeColor = Color.LightGray;
            SetAsteriscoVisibility(false);
            CarregarTurmasDoBanco();

            

            // Eventos para o autocomplete de Turma
            txtTurma.KeyDown += txtTurma_KeyDown;
            txtTurma.Leave += txtTurma_Leave;
            txtTurma.Validating += txtTurma_Validating; // <- validação de turma

            lstSugestoesTurma.Click += lstSugestoesTurma_Click;
            lstSugestoesTurma.KeyDown += lstSugestoesTurma_KeyDown;
            lstSugestoesTurma.Leave += lstSugestoesTurma_Leave;

            // Estilo do ListBox e z-order
            EstilizarListBoxSugestao(lstSugestoesTurma);
            lstSugestoesTurma.BringToFront();

            // CORREÇÃO: não dispare validação ao ir para a lista; esconda inicialmente
            lstSugestoesTurma.CausesValidation = false;
            lstSugestoesTurma.Visible = false;
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            // Se a lista de turmas estiver visível, confirma antes de avançar
            if (lstSugestoesTurma.Visible)
            {
                e.SuppressKeyPress = true;
                if (ConfirmarSugestaoTurma()) return;
            }

            // Fluxo padrão
            e.SuppressKeyPress = true;
            this.SelectNextControl(this.ActiveControl, true, true, true, true);
        }
        #endregion

        

        #region Métodos Públicos
        public void LimparCampos()
        {
            cbUsuario.SelectedIndex = -1;
            txtNome.Text = "";
            txtEmail.Text = "";
            txtTurma.Text = "";
            mtxTelefone.Text = "";
            mtxCPF.Text = "";
            dtpDataNasc.Value = DateTime.Today;
            txtSenha.Text = "";
            txtConfirmSenha.Text = "";
            cbUsuario.Focus();
        }

        public bool ValidarCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public bool ValidarTelefone(string telefone)
        {
            string apenasNumeros = new string(telefone.Where(char.IsDigit).ToArray());
            return apenasNumeros.Length >= 11 && apenasNumeros.Length <= 12;
        }

        public bool ValidarEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        #endregion

        #region Eventos de Controles
        private void cbUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            string funcaoSelecionada = cbUsuario.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(funcaoSelecionada))
            {
                ConfigurarCamposGenericos();
                LimparCamposOcultos(); // limpa tudo que estiver oculto/desabilitado
                return;
            }

            switch (funcaoSelecionada)
            {
                case "Bibliotecário(a)":
                    ConfigurarParaBibliotecario();
                    break;

                case "Professor(a)":
                    ConfigurarParaProfessor();
                    break;

                case "Outros":
                    ConfigurarParaOutros();
                    break;

                default:
                    ConfigurarParaAluno();
                    break;
            }

            // Ao final, limpa TODOS os campos ocultos/desabilitados
            LimparCamposOcultos();
        }

        // Limpa valores de todos os controles invisíveis ou desabilitados (TextBox, MaskedTextBox, ComboBox, CheckBox)
        private void LimparCamposOcultos()
        {
            LimparCamposOcultosRecursivo(this);
        }

        private void LimparCamposOcultosRecursivo(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.HasChildren)
                    LimparCamposOcultosRecursivo(c);

                if (!c.Visible || !c.Enabled)
                {
                    // Campos de texto, masked e combos
                    if (c is TextBoxBase || c is ComboBox)
                    {
                        c.Text = string.Empty;
                    }
                    // CheckBox (ex.: mostrar senha)
                    else if (c is CheckBox cb)
                    {
                        cb.Checked = false;
                    }
                }
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            string tipoUsuario = cbUsuario.SelectedItem?.ToString() ?? string.Empty;
            if(tipoUsuario!= "Bibliotecário(a)")
            {
                if (IsAdminLogado())
                {
                    MessageBox.Show("Administradores não podem cadastrar usuários diferentes do tipo: Bibliotecário(a).", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }

            if (!ValidarCamposObrigatorios())
                return;

            if (!ValidarDadosUsuario())
                return;


            CadastrarNovoUsuario();
        }

        private void chkMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
            txtConfirmSenha.UseSystemPasswordChar = !chkMostrarSenha.Checked;
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show(
                "Tem certeza de que deseja limpar tudo?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmar == DialogResult.Yes)
            {
                LimparCampos();
            }
        }

        private bool _suppressEmailTextChanged = false;
        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                e.KeyChar = char.ToLowerInvariant(e.KeyChar);
            }
        }
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            // trim + normalização final
            txtEmail.Text = txtEmail.Text.Trim().ToLowerInvariant();
        }
        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            lblAvisoEmail.Visible = string.IsNullOrWhiteSpace(txtEmail.Text);

            if (_suppressEmailTextChanged) return;

          
            var tb = sender as TextBox;
            if (tb == null) return;

            string lower = tb.Text.ToLowerInvariant();
            if (tb.Text != lower)
            {
                int selStart = tb.SelectionStart;
                int selLen = tb.SelectionLength;

                _suppressEmailTextChanged = true;
                tb.Text = lower;
                // restaura a posição do cursor de forma segura
                tb.SelectionStart = Math.Min(selStart, tb.Text.Length);
                tb.SelectionLength = selLen;
                _suppressEmailTextChanged = false;
            }
        }

        private void txtTurma_Validating(object sender, CancelEventArgs e)
        {
            if (!txtTurma.Enabled || !txtTurma.Visible) return;

            var turma = txtTurma.Text.Trim();
            if (string.IsNullOrEmpty(turma)) return; // obrigatoriedade tratada no botão Cadastrar

            if (!BibliotecaApp.Utils.TurmasUtil.TurmasPermitidas.Contains(turma))
            {
                e.Cancel = true; // impede perder o foco neste momento
                MessageBox.Show(
                    "Selecione uma turma válida da lista de turmas permitidas.",
                    "Turma inválida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Limpa o campo e fecha a lista de sugestões
                txtTurma.Text = string.Empty;
                lstSugestoesTurma.Visible = false;

                BeginInvoke(new Action(() =>
                {
                    txtTurma.Focus();
                }));
            }
        }
        #endregion

        #region Métodos Privados
        private void HabilitarCampos(bool ativo)
        {
            
            
            txtNome.Enabled = ativo;
            txtEmail.Enabled = ativo;
            txtTurma.Enabled = ativo;
            mtxCPF.Enabled = ativo;
            mtxTelefone.Enabled = ativo;
            txtSenha.Enabled = ativo;
            txtConfirmSenha.Enabled = ativo;
            dtpDataNasc.Enabled = ativo;
        }

        private string RemoverMascara(string texto)
        {
            return string.Concat(texto.Where(c => char.IsDigit(c)));
        }
        #endregion

        #region Métodos de Configuração de Interface
        private void ConfigurarCamposGenericos()
        {
            HabilitarCampos(true);
            SetLabelColors(enabled: true);
            SetAsteriscoVisibility(false);
            chkMostrarSenha.ForeColor = Color.LightGray;
        }

        private void ConfigurarParaBibliotecario()
        {
            HabilitarCampos(true);
            lblTelefone.Text = "Telefone:";
            TelefoneAst.Location = new Point(105, 17);
            txtTurma.Enabled = false;
            txtTurma.Visible = false;
            TurmaAst.Visible = false;
            lblTurma.Visible = false;
            lblTurma.Enabled = false;
            txtSenha.Visible = true;
            txtConfirmSenha.Visible = true;
            lblSenha.Visible = true;
            lblConfirmSenha.Visible = true;
            chkMostrarSenha.Visible = true;
            panel2.Location = new Point(288,353);
            btnCadastrar.Location = new Point(491, 485);
            btnLimpar.Location = new Point(23, 485);
            panel2.Anchor = AnchorStyles.Top;


            SetLabelColors(enabled: true);
           

            SetAsteriscoVisibility(true);
            TurmaAst.ForeColor = Color.Transparent;
            

         

           

            txtSenha.PlaceholderText = "Digite aqui uma senha...";
            txtConfirmSenha.PlaceholderText = "Confirme a senha...";
            txtEmail.PlaceholderText = "Digite aqui o email...";

            chkMostrarSenha.Enabled = true;
            chkMostrarSenha.ForeColor = Color.FromArgb(20, 41, 60);
        }
      

       

        private void ConfigurarParaProfessor()
        {
            HabilitarCampos(true);
            lblTelefone.Text = "Telefone:";
            TelefoneAst.Location = new Point(105, 17);
            txtTurma.Enabled = false;
            txtTurma.Visible = false;
            TurmaAst.Visible = false;
            lblTurma.Visible = false;
            lblTurma.Enabled=false;
            txtSenha.Visible = false;
            txtConfirmSenha.Visible = false;
            lblSenha.Visible = false;
            lblConfirmSenha.Visible = false;
            chkMostrarSenha.Visible = false;
            btnCadastrar.Location = new Point(491, 304);
            btnLimpar.Location = new Point(23, 304);
            panel2.Location = new Point(288,353);
            panel2.Anchor = AnchorStyles.Top;

            SetLabelColors(enabled: true);

            

            SetAsteriscoVisibility(true);
            TurmaAst.ForeColor = Color.Transparent;
            SenhaAst.ForeColor = Color.Transparent;
            ConfirmSenhaAst.ForeColor = Color.Transparent;
            EmailAst.ForeColor = Color.Transparent;
          
            txtSenha.Text = "";
            txtConfirmSenha.Text = "";

            

            txtEmail.PlaceholderText = "Digite aqui o email...";
            chkMostrarSenha.ForeColor = Color.LightGray;
            chkMostrarSenha.Enabled = false;
        }

        private void ConfigurarParaOutros()
        {
            HabilitarCampos(true);
            lblTelefone.Text = "Telefone:";
            TelefoneAst.Location = new Point(105, 17);
            txtTurma.Enabled = false;
            txtTurma.Visible = false;
            TurmaAst.Visible = false;
            txtSenha.Enabled = false;
            txtConfirmSenha.Enabled = false;

            txtSenha.Visible = false;
            txtConfirmSenha.Visible = false;
            lblSenha.Visible = false;
            lblConfirmSenha.Visible = false;
            chkMostrarSenha.Visible = false;
            btnCadastrar.Location = new Point(491, 304);
            btnLimpar.Location = new Point(23, 304);
            panel2.Location = new Point(288, 353);
            panel2.Anchor = AnchorStyles.Top;

            SetLabelColors(enabled: true);
           

            lblTurma.Visible = false;

            txtTurma.Text = "";
            txtSenha.Text = "";
            txtConfirmSenha.Text = "";
            txtEmail.Text = "";

            SetAsteriscoVisibility(true);
            TurmaAst.ForeColor = Color.Transparent;
            SenhaAst.ForeColor = Color.Transparent;
            ConfirmSenhaAst.ForeColor = Color.Transparent;
            EmailAst.ForeColor = Color.Transparent;
          

           

            chkMostrarSenha.ForeColor = Color.LightGray;
            chkMostrarSenha.Enabled = false;
            chkMostrarSenha.Checked = false;
        }

        private void ConfigurarParaAluno()
        {
            HabilitarCampos(true);
            lblTelefone.Text = "Telefone do Aluno / Responsável:";
            TelefoneAst.Location = new Point(311, 17);
            txtSenha.Enabled = false;
            txtConfirmSenha.Enabled = false;
            txtTurma.Visible = true;
            txtTurma.Enabled = true;
            lblTurma.Enabled = true;
            lblTurma.Visible=true;
            TurmaAst.Visible = true;
            SetLabelColors(enabled: true);
            lblSenha.ForeColor = Color.LightGray;
            lblConfirmSenha.ForeColor = Color.LightGray;
            txtSenha.Text = "";
            txtConfirmSenha.Text = "";

            txtSenha.Visible = false;
            txtConfirmSenha.Visible = false;
            lblSenha.Visible = false;
            lblConfirmSenha.Visible = false;
            chkMostrarSenha.Visible = false;
            btnCadastrar.Location = new Point(491, 304);
            btnLimpar.Location = new Point(23, 304);
            panel2.Location = new Point(288, 430);
            panel2.Anchor = AnchorStyles.Top;



            SetAsteriscoVisibility(true);
            SenhaAst.ForeColor = Color.Transparent;
            ConfirmSenhaAst.ForeColor = Color.Transparent;
            EmailAst.ForeColor = Color.Transparent;

           

            txtTurma.PlaceholderText = "Digite aqui a turma...";
            txtEmail.PlaceholderText = "Digite aqui o email...";

            chkMostrarSenha.ForeColor = Color.LightGray;
            chkMostrarSenha.Enabled = false;
        }

       
        private void SetLabelColors(bool enabled)
        {
            Color color = enabled ? Color.FromArgb(20, 41, 60) : Color.LightGray;
            lblNome.ForeColor = color;
            lblSenha.ForeColor = color;
            lblConfirmSenha.ForeColor = color;
            lblCPF.ForeColor = color;
            lblDataNasc.ForeColor = color;
            lblTelefone.ForeColor = color;
            lblEmail.ForeColor = color;
            lblTurma.ForeColor = color;
        }

        private void SetAsteriscoVisibility(bool visible)
        {
            Color color = visible ? Color.Red : Color.Transparent;
            NomeAst.ForeColor = color;
            EmailAst.ForeColor = color;
            TurmaAst.ForeColor = color;
            TelefoneAst.ForeColor = color;
            DataNascAst.ForeColor = color;
            SenhaAst.ForeColor = color;
            ConfirmSenhaAst.ForeColor = color;
        }
        #endregion

        #region Métodos de Validação
        private bool ValidarCamposObrigatorios()
        {
            

            string nome = txtNome.Text.Trim();
            string tipoUsuario = cbUsuario.SelectedItem?.ToString() ?? string.Empty;
            string telefone = RemoverMascara(mtxTelefone.Text);
            string turma = txtTurma.Text.Trim();
            string senha = txtSenha.Text.Trim();
            string confirmar = txtConfirmSenha.Text.Trim();


            if (string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(tipoUsuario) ||
                string.IsNullOrWhiteSpace(telefone) ||
                (txtTurma.Enabled && string.IsNullOrWhiteSpace(turma)) ||
                (txtSenha.Visible && string.IsNullOrWhiteSpace(senha)) ||
                (txtConfirmSenha.Visible && string.IsNullOrWhiteSpace(confirmar)))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (tipoUsuario == "Aluno(a)" && string.IsNullOrWhiteSpace(turma))
            {
                MessageBox.Show("A turma é obrigatória para alunos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidarDadosUsuario()
        {
            string cpf = RemoverMascara(mtxCPF.Text);
            string telefone = RemoverMascara(mtxTelefone.Text);
            string email = txtEmail.Text.Trim().ToLowerInvariant();
            string senha = txtSenha.Text.Trim();
            string confirmar = txtConfirmSenha.Text.Trim();

            if (!string.IsNullOrWhiteSpace(cpf) && !ValidarCPF(cpf))
            {
                MessageBox.Show("CPF inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!ValidarTelefone(telefone))
            {
                MessageBox.Show("Telefone inválido. Insira o DDD e o número corretamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(email) && !ValidarEmail(email))
            {
                MessageBox.Show("E-mail inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (senha != confirmar)
            {
                MessageBox.Show("As senhas não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(senha) && senha.Length < 4)
            {
                MessageBox.Show("A senha deve ter pelo menos 4 caracteres.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void CadastrarNovoUsuario()
        {
            string tipoUsuario = cbUsuario.Text;
            string hash = null;
            string salt = null;

            // Gera hash/salt somente se for Bibliotecário(a) e os campos estiverem ativos e preenchidos
            if (tipoUsuario == "Bibliotecário(a)" && txtSenha.Visible && txtSenha.Enabled && !string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                BibliotecaApp.Utils.CriptografiaSenha.CriarHash(txtSenha.Text, out hash, out salt);
            }

            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    using (SqlCeCommand comando = conexao.CreateCommand())
                    {
                        comando.CommandText = @"INSERT INTO usuarios
(Nome, Email, Senha_Hash, Senha_Salt, CPF, DataNascimento, Turma, Telefone, TipoUsuario) 
VALUES 
(@Nome, @Email, @Senha_hash, @Senha_salt, @CPF, @DataNascimento, @Turma, @Telefone, @TipoUsuario)";

                        comando.Parameters.AddWithValue("@Nome", (object)(txtNome.Text ?? string.Empty).ToString().Trim());
                        comando.Parameters.AddWithValue("@Email", DbValue(txtEmail));
                        comando.Parameters.AddWithValue("@Senha_hash", string.IsNullOrEmpty(hash) ? (object)DBNull.Value : hash);
                        comando.Parameters.AddWithValue("@Senha_salt", string.IsNullOrEmpty(salt) ? (object)DBNull.Value : salt);
                        comando.Parameters.AddWithValue("@CPF", DbValue(mtxCPF));
                        comando.Parameters.AddWithValue("@DataNascimento", dtpDataNasc.Value);
                        comando.Parameters.AddWithValue("@Turma", DbValue(txtTurma));
                        comando.Parameters.AddWithValue("@Telefone", DbValue(mtxTelefone));
                        comando.Parameters.AddWithValue("@TipoUsuario", tipoUsuario);

                        comando.ExecuteNonQuery();
                        LimparCampos();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    conexao.Close();
                }
            }

            MessageBox.Show("Cadastro concluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UsuarioCriado?.Invoke(this, EventArgs.Empty);

            if (cbUsuario.Text == "Bibliotecário(a)")
                BibliotecaApp.Utils.EventosGlobais.OnBibliotecariaCadastrada();
            if (cbUsuario.Text == "Professor(a)")
                BibliotecaApp.Utils.EventosGlobais.OnProfessorCadastrado();

            this.DialogResult = DialogResult.OK;
            LimparCampos();
        }

        // Retorna DBNull.Value se o controle estiver oculto/desabilitado ou se o texto estiver vazio;
        // caso contrário, retorna o texto trimmed.
        private object DbValue(Control c)
        {
            if (c == null) return DBNull.Value;
            if (!c.Visible || !c.Enabled) return DBNull.Value;

            var txt = (c.Text ?? string.Empty).Trim();
            return string.IsNullOrEmpty(txt) ? (object)DBNull.Value : txt;
        }
        #endregion

        #region Métodos de Turma
       

        private string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            texto = texto.ToLower().Trim();

            // Remover acentos
            texto = Regex.Replace(texto.Normalize(NormalizationForm.FormD), @"[\p{Mn}]", "");

            // Remover caracteres especiais, exceto números, letras e °
            texto = Regex.Replace(texto, @"[^a-z0-9°\s]", "");

            // Remover espaços extras
            texto = Regex.Replace(texto, @"\s+", " ");

            return texto;
        }

        private string CorrigirTurma(string turmaDigitada)
        {
            if (string.IsNullOrWhiteSpace(turmaDigitada))
                return turmaDigitada;

            // Adicionar ° automaticamente se não tiver
            if (!turmaDigitada.Contains("°") && Regex.IsMatch(turmaDigitada, @"^\d+"))
            {
                turmaDigitada = Regex.Replace(turmaDigitada, @"^(\d+)", "$1°");
            }

            // Extrair número e tipo
            string numeroStr = "";
            string tipo = "";
            string numeroTurma = "";

            // Extrair o número principal (antes do °)
            Match matchNumero = Regex.Match(turmaDigitada, @"^(\d+)°");
            if (matchNumero.Success)
            {
                numeroStr = matchNumero.Groups[1].Value;
            }
            else
            {
                // Se não encontrou o padrão com °, tentar sem °
                matchNumero = Regex.Match(turmaDigitada, @"^(\d+)\s");
                if (matchNumero.Success)
                {
                    numeroStr = matchNumero.Groups[1].Value;
                    turmaDigitada = turmaDigitada.Replace(matchNumero.Value, matchNumero.Groups[1].Value + "° ");
                }
            }

            // Determinar o tipo de turma
            string turmaLower = turmaDigitada.ToLower();
            if (turmaLower.Contains("p"))
            {
                tipo = "Propedêutico";
            }
            else if (turmaLower.Contains("d"))
            {
                tipo = "Desenvolvimento";
            }
            else if (turmaLower.Contains("ag"))
            {
                tipo = "Agronegócio";
            }
            else if (turmaLower.Contains("an"))
            {
                tipo = "Ano";
            }

            // Extrair número da turma (no final)
            Match matchNumeroTurma = Regex.Match(turmaDigitada, @"(\d+)$");
            if (matchNumeroTurma.Success)
            {
                numeroTurma = matchNumeroTurma.Value;
            }

            // Corrigir número principal se for impossível
            if (!string.IsNullOrEmpty(numeroStr))
            {
                int numero;
                if (int.TryParse(numeroStr, out numero))
                {
                    if (tipo == "Ano")
                    {
                        // Para turmas de Ano: 6° a 9°
                        if (numero < 6) numero = 6;
                        else if (numero > 9) numero = 9;
                    }
                    else if (!string.IsNullOrEmpty(tipo))
                    {
                        // Para outras turmas: 1° a 3°
                        if (numero < 1) numero = 1;
                        else if (numero > 3) numero = 3;
                    }

                    numeroStr = numero.ToString();
                }
            }

            // Corrigir número da turma se for impossível
            if (!string.IsNullOrEmpty(numeroTurma))
            {
                int numero;
                if (int.TryParse(numeroTurma, out numero))
                {
                    if (numero < 1) numero = 1;
                    // Não há limite máximo para o número da turma
                    numeroTurma = numero.ToString();
                }
            }

            // NOVA LÓGICA: Remover apenas o número "1" no final para cursos técnicos
            if (!string.IsNullOrEmpty(tipo) && tipo != "Ano" && !string.IsNullOrEmpty(numeroTurma) && numeroTurma == "1")
            {
                // Para cursos técnicos, remover apenas o número "1" da turma (final)
                numeroTurma = "";
            }

            // Montar turma corrigida
            string turmaCorrigida = !string.IsNullOrEmpty(numeroStr) ? numeroStr + "°" : "";

            if (!string.IsNullOrEmpty(tipo))
            {
                turmaCorrigida += " " + tipo;
            }

            if (!string.IsNullOrEmpty(numeroTurma))
            {
                turmaCorrigida += " " + numeroTurma;
            }

            return !string.IsNullOrEmpty(turmaCorrigida) ? turmaCorrigida.Trim() : turmaDigitada;
        }

        private void CarregarTurmasDoBanco()
        {
            turmasCadastradas.Clear();

            using (var conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();
                    using (var comando = conexao.CreateCommand())
                    {
                        comando.CommandText = "SELECT DISTINCT Turma FROM usuarios WHERE Turma IS NOT NULL AND Turma <> ''";

                        using (var leitor = comando.ExecuteReader())
                        {
                            while (leitor.Read())
                            {
                                turmasCadastradas.Add(leitor["Turma"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar turmas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conexao.Close();
                }
            }
        }

        private void txtTurma_TextChanged(object sender, EventArgs e)
        {
            string texto = txtTurma.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                lstSugestoesTurma.Visible = false;
                return;
            }

            // Buscar sugestões apenas nas turmas permitidas
            var sugestoes = BibliotecaApp.Utils.TurmasUtil.BuscarSugestoes(texto);

            lstSugestoesTurma.Items.Clear();

            if (sugestoes.Count > 0)
            {
                foreach (var s in sugestoes)
                    lstSugestoesTurma.Items.Add(s);

                int visibleItems = Math.Min(5, sugestoes.Count);
                int extraPadding = 8;
                lstSugestoesTurma.Height = visibleItems * lstSugestoesTurma.ItemHeight + extraPadding;
                lstSugestoesTurma.Width = txtTurma.Width;

                // POSICIONAMENTO REVERTIDO (como era antes)
                lstSugestoesTurma.Left = txtTurma.Left;
                lstSugestoesTurma.Top = txtTurma.Bottom;

                lstSugestoesTurma.BringToFront();
                lstSugestoesTurma.Visible = true;

                // Seleciona o primeiro item por padrão
                lstSugestoesTurma.SelectedIndex = 0;
            }
            else
            {
                lstSugestoesTurma.Visible = false;
            }
        }

        private void txtTurma_Leave(object sender, EventArgs e)
        {

            BeginInvoke(new Action(() =>
            {
                if (!lstSugestoesTurma.Focused)
                {
                    lstSugestoesTurma.Visible = false;
                    // Não valida aqui para evitar mensagens duplicadas
                }
            }));
        }

        

        private void lstSugestoesTurma_Click(object sender, EventArgs e)
        {
            if (lstSugestoesTurma.SelectedItem != null)
            {
                string turmaSelecionada = lstSugestoesTurma.SelectedItem.ToString();
                txtTurma.Text = turmaSelecionada;
                lstSugestoesTurma.Visible = false;
                txtTurma.Focus();

                // Opcional: avançar para o próximo controle, igual ao EmprestimoRapidoForm
                this.SelectNextControl(txtTurma, true, true, true, true);
            }
        }

        private void txtTurma_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesTurma.Visible || lstSugestoesTurma.Items.Count == 0)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                lstSugestoesTurma.Focus();
                if (lstSugestoesTurma.Items.Count > 0)
                    lstSugestoesTurma.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ConfirmarSugestaoTurma();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesTurma.Visible = false;
            }
        }

        private void lstSugestoesTurma_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                // NOVO: garante selecionar a primeira opção se nada estiver selecionado
                if (lstSugestoesTurma.SelectedIndex < 0 && lstSugestoesTurma.Items.Count > 0)
                    lstSugestoesTurma.SelectedIndex = 0;

                ConfirmarSugestaoTurma();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesTurma.Visible = false;
                txtTurma.Focus();
            }
        }

        private void lstSugestoesTurma_Leave(object sender, EventArgs e)
        {
            lstSugestoesTurma.Visible = false;
        }

        private bool ConfirmarSugestaoTurma()
        {
            if (!lstSugestoesTurma.Visible || lstSugestoesTurma.Items.Count == 0)
                return false;

            if (lstSugestoesTurma.SelectedIndex < 0)
                lstSugestoesTurma.SelectedIndex = 0;

            var valor = lstSugestoesTurma.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(valor)) return false;

            txtTurma.Text = valor;
            lstSugestoesTurma.Visible = false;
            txtTurma.Focus();
            this.SelectNextControl(txtTurma, true, true, true, true);
            return true;
        }
        #endregion

        #region Estilizacao do ListBox de Sugestões
        private int hoveredIndex = -1;

        private void EstilizarListBoxSugestao(ListBox listBox)
        {
            listBox.DrawMode = DrawMode.OwnerDrawFixed;
            listBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            listBox.ItemHeight = 40;
            listBox.BackColor = Color.White;
            listBox.ForeColor = Color.FromArgb(30, 61, 88);
            listBox.BorderStyle = BorderStyle.FixedSingle;
            listBox.IntegralHeight = false;

            listBox.DrawItem -= ListBoxSugestao_DrawItem;
            listBox.DrawItem += ListBoxSugestao_DrawItem;
            listBox.MouseMove -= ListBoxSugestao_MouseMove;
            listBox.MouseMove += ListBoxSugestao_MouseMove;
            listBox.MouseLeave -= ListBoxSugestao_MouseLeave;
            listBox.MouseLeave += ListBoxSugestao_MouseLeave;
        }

        private void ListBoxSugestao_DrawItem(object sender, DrawItemEventArgs e)
        {
            var listBox = sender as ListBox;
            if (e.Index < 0) return;

            bool hovered = (e.Index == hoveredIndex);

            Color backColor = hovered
                ? Color.FromArgb(235, 235, 235)
                : Color.White;
            Color textColor = Color.FromArgb(60, 60, 60);

            using (SolidBrush b = new SolidBrush(backColor))
                e.Graphics.FillRectangle(b, e.Bounds);

            string text = listBox.Items[e.Index].ToString();
            Font font = listBox.Font;

            Rectangle textRect = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, text, font, textRect, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            if (e.Index < listBox.Items.Count - 1)
            {
                using (Pen p = new Pen(Color.FromArgb(220, 220, 220)))
                    e.Graphics.DrawLine(p, e.Bounds.Left + 8, e.Bounds.Bottom - 1, e.Bounds.Right - 8, e.Bounds.Bottom - 1);
            }
        }

        private void ListBoxSugestao_MouseMove(object sender, MouseEventArgs e)
        {
            var listBox = sender as ListBox;
            int index = listBox.IndexFromPoint(e.Location);
            if (index != hoveredIndex)
            {
                hoveredIndex = index;
                listBox.Invalidate();
            }
        }

        private void ListBoxSugestao_MouseLeave(object sender, EventArgs e)
        {
            hoveredIndex = -1;
            (sender as ListBox).Invalidate();
        }


        #endregion

       


    }
}