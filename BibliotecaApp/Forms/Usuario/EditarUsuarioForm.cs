using BibliotecaApp.Forms.Inicio;
using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Usuario
{
    public partial class EditarUsuarioForm : Form
    {
        private static bool IsAdminLogado()
       => string.Equals(Sessao.NomeBibliotecariaLogada, "Administrador", StringComparison.OrdinalIgnoreCase);


        private Size originalSize;
        private List<string> turmasCadastradas = new List<string>();
        
        private List<string> todasTurmasPadrao;
        public event EventHandler UsuarioAtualizado;

        public EditarUsuarioForm()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.KeyDown += EditarUsuarioForm_KeyDown;

            // Navegação genérica
            txtNome.KeyDown += Control_KeyDown;
            txtEmail.KeyDown += Control_KeyDown;
            txtTurma.KeyDown += Control_KeyDown;
            mtxCPF.KeyDown += Control_KeyDown;
            mtxTelefone.KeyDown += Control_KeyDown;
            dtpDataNasc.KeyDown += Control_KeyDown;

            EstilizarListBoxSugestao(lstSugestoesUsuario);
            originalSize = this.Size;
            foreach (Control ctrl in this.Controls) ctrl.Tag = ctrl.Bounds;

            this.Resize += EditarUsuarioForm_Resize;

            txtTurma.Enter += txtTurma_Enter;
            txtTurma.MouseClick += txtTurma_MouseClick;

            // Turma
            txtTurma.KeyDown += txtTurma_KeyDown;
            txtTurma.Leave += txtTurma_Leave;
            lstSugestoesTurma.Click += lstSugestoesTurma_Click;
            lstSugestoesTurma.KeyDown += lstSugestoesTurma_KeyDown;
            lstSugestoesTurma.Leave += lstSugestoesTurma_Leave;
            EstilizarListBoxSugestao(lstSugestoesTurma);
            lstSugestoesTurma.BringToFront();

            // NOVO: Nome (eventos para teclado e clique)
            txtNomeUsuario.KeyDown += txtNomeUsuario_KeyDown;
            lstSugestoesUsuario.KeyDown += lstSugestoesUsuario_KeyDown;
            lstSugestoesUsuario.Click += lstSugestoesUsuario_Click;
            lstSugestoesUsuario.Leave += lstSugestoesUsuario_Leave;
        }
        // Substitua o método EditarUsuarioForm_KeyDown
        private void EditarUsuarioForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            // Confirma a lista relacionada ao foco atual (suporta RoundedTextBox via ContainsFocus)
            if (lstSugestoesUsuario.Visible || lstSugestoesTurma.Visible)
            {
                e.SuppressKeyPress = true;

                if (lstSugestoesUsuario.Focused || (txtNomeUsuario != null && txtNomeUsuario.ContainsFocus))
                    if (ConfirmarSugestaoUsuario()) return;

                if (lstSugestoesTurma.Focused || (txtTurma != null && txtTurma.ContainsFocus))
                    if (ConfirmarSugestaoTurma()) return;

                // Fallback: tenta confirmar qualquer uma visível
                if (ConfirmarSugestaoUsuario()) return;
                if (ConfirmarSugestaoTurma()) return;

                return;
            }

            // Fluxo padrão
            e.SuppressKeyPress = true;
            this.SelectNextControl(this.ActiveControl, true, true, true, true);
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
        }


        private void lstSugestoesUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (lstSugestoesUsuario.SelectedIndex < 0 && lstSugestoesUsuario.Items.Count > 0)
                    lstSugestoesUsuario.SelectedIndex = 0;

                ConfirmarSugestaoUsuario();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesUsuario.Visible = false;
                txtNomeUsuario.Focus();
            }
        }

        

        private void EditarUsuarioForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                return; // não faz nada se estiver minimizado

            float xRatio = (float)this.Width / originalSize.Width;
            float yRatio = (float)this.Height / originalSize.Height;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.Tag is Rectangle originalBounds)
                {
                    ctrl.Width = (int)(originalBounds.Width * xRatio);
                    ctrl.Height = (int)(originalBounds.Height * yRatio);
                    ctrl.Left = (int)(originalBounds.Left * xRatio);
                    ctrl.Top = (int)(originalBounds.Top * yRatio);

                    // Ajustar fonte junto
                    ctrl.Font = new Font(ctrl.Font.FontFamily,
                        10 * Math.Min(xRatio, yRatio), // 10 = tamanho base, ajuste se quiser
                        ctrl.Font.Style);
                }
            }
        }

        private bool _suppressSuggestionOnPrefill = false;
        private List<Usuarios> _cacheUsuarios = new List<Usuarios>();
        private Usuarios _usuarioSelecionado;

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            // Verifica se há alterações antes de perguntar
            bool haAlteracoes = (_usuarioSelecionado != null && VerificarAlteracoes());

            if (haAlteracoes)
            {
                DialogResult result = MessageBox.Show(
                    "Tem certeza que deseja cancelar? Todas as alterações serão perdidas.",
                    "Confirmar cancelamento",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return;
            }

            // limpa campos sempre que cancelar
            LimparCampos();

            // fecha somente se este form foi aberto pela tela de usuário
            if (this.FecharAoSalvar)
            {
                var mainForm = this.MdiParent as MainForm
                    ?? Application.OpenForms.OfType<MainForm>().FirstOrDefault();

                if (mainForm != null)
                {
                    mainForm.SetUserButtonsEnabled(false, true);
                }
                this.Close();
            }
        }



        // permite controlar se o form fecha automaticamente depois de salvar
        public bool FecharAoSalvar { get; set; } = false;


        private void btnSalvar_Click(object sender, EventArgs e)
        {

            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode editar usuários.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (_usuarioSelecionado == null)
            {
                MessageBox.Show("Selecione um usuário primeiro.");
                return;
            }

            // Validar turma permitida
            if (txtTurma.Enabled && !string.IsNullOrWhiteSpace(txtTurma.Text))
            {
                var turma = txtTurma.Text.Trim();
                if (!BibliotecaApp.Utils.TurmasUtil.TurmasPermitidas.Contains(turma))
                {
                    MessageBox.Show("Selecione uma turma válida da lista de turmas permitidas.", "Turma inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Verificar se houve alterações
            bool haAlteracoes = VerificarAlteracoes();
            if (!haAlteracoes)
            {
                MessageBox.Show("Nenhuma alteração foi feita.");
                return;
            }

            // Validar e-mail antes de salvar
            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrEmpty(email) && !EmailValido(email))
            {
                MessageBox.Show("E-mail inválido. Digite um e-mail válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Normalizar CPF (apenas dígitos)
            string cpfRaw = (mtxCPF.Text ?? "");
            string cpfDigits = Regex.Replace(cpfRaw, @"\D", "");

            // Verificar duplicidade **antes** de atualizar. CPF vazio => permitido.
            if (!string.IsNullOrEmpty(cpfDigits))
            {
                if (CpfJaExiste(cpfRaw, _usuarioSelecionado.Id))
                {
                    MessageBox.Show("Já existe outro usuário com esse CPF.", "CPF duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Mostrar mensagem de confirmação com as alterações
            var mensagemConfirmacao = MontarMensagemConfirmacao();
            var resultado = MessageBox.Show(mensagemConfirmacao, "Confirmar Alterações",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado != DialogResult.Yes)
            {
                return;
            }

            //  código de salvamento...
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"UPDATE usuarios SET 
                Nome = @nome, Email = @email, CPF = @cpf,
                DataNascimento = @data, Telefone = @tel, Turma = @turma
                WHERE Id = @id";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtNome.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email);

                        // Se não houver dígitos no CPF, grava NULL no banco (se a coluna permitir)
                        if (string.IsNullOrEmpty(cpfDigits))
                            cmd.Parameters.AddWithValue("@cpf", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@cpf", mtxCPF.Text.Trim());

                        cmd.Parameters.AddWithValue("@data", dtpDataNasc.Value);
                        cmd.Parameters.AddWithValue("@tel", string.IsNullOrEmpty(mtxTelefone.Text.Trim()) ? (object)DBNull.Value : mtxTelefone.Text.Trim());
                        cmd.Parameters.AddWithValue("@turma", string.IsNullOrEmpty(txtTurma.Text.Trim()) ? (object)DBNull.Value : txtTurma.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", _usuarioSelecionado.Id);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuário atualizado com sucesso!");
                UsuarioAtualizado?.Invoke(this, EventArgs.Empty);

                if (this.FecharAoSalvar)
                {
                    this.Close();
                    return;
                }

                
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar: " + ex.Message);
            }
        }


        private bool VerificarAlteracoes()
        {
            // Verificar se algum campo foi alterado
            bool nomeAlterado = txtNome.Text != _usuarioSelecionado.Nome;
            bool emailAlterado = txtEmail.Text != _usuarioSelecionado.Email;
            bool cpfAlterado = mtxCPF.Text != _usuarioSelecionado.CPF;

            bool dataAlterada = dtpDataNasc.Value != (_usuarioSelecionado.DataNascimento == DateTime.MinValue ?
                DateTime.Today : _usuarioSelecionado.DataNascimento);

            bool telefoneAlterado = mtxTelefone.Text != _usuarioSelecionado.Telefone;
            bool turmaAlterada = txtTurma.Text != _usuarioSelecionado.Turma;

            return nomeAlterado || emailAlterado || cpfAlterado || dataAlterada || telefoneAlterado || turmaAlterada;
        }

        private string MontarMensagemConfirmacao()
        {
            var mensagem = new StringBuilder();
            mensagem.AppendLine("Confirme as alterações a serem salvas:");
            mensagem.AppendLine();

            // Nome
            if (txtNome.Text != _usuarioSelecionado.Nome)
            {
                mensagem.AppendLine($"Nome: {_usuarioSelecionado.Nome} → {txtNome.Text}");
            }

            // Email
            if (txtEmail.Text != _usuarioSelecionado.Email)
            {
                mensagem.AppendLine($"Email: {_usuarioSelecionado.Email} → {txtEmail.Text}");
            }

            // CPF
            if (mtxCPF.Text != _usuarioSelecionado.CPF)
            {
                mensagem.AppendLine($"CPF: {_usuarioSelecionado.CPF} → {mtxCPF.Text}");
            }

            // Data de Nascimento
            DateTime dataOriginal = _usuarioSelecionado.DataNascimento == DateTime.MinValue ?
                DateTime.Today : _usuarioSelecionado.DataNascimento;

            if (dtpDataNasc.Value != dataOriginal)
            {
                mensagem.AppendLine($"Data Nasc.: {dataOriginal:dd/MM/yyyy} → {dtpDataNasc.Value:dd/MM/yyyy}");
            }

            // Telefone
            if (mtxTelefone.Text != _usuarioSelecionado.Telefone)
            {
                mensagem.AppendLine($"Telefone: {_usuarioSelecionado.Telefone} → {mtxTelefone.Text}");
            }

            // Turma
            if (txtTurma.Text != _usuarioSelecionado.Turma)
            {
                mensagem.AppendLine($"Turma: {_usuarioSelecionado.Turma} → {txtTurma.Text}");
            }

            mensagem.AppendLine();
            mensagem.AppendLine("Deseja salvar estas alterações?");

            return mensagem.ToString();
        }


        private void SelecionarUsuario(int index)
        {
            _usuarioSelecionado = _cacheUsuarios[index];

            // suprime temporariamente sugestões enquanto preenche os controles
            _suppressSuggestionOnPrefill = true;

            // Preenche o campo que dispara TextChanged primeiro
            txtNomeUsuario.Text = _usuarioSelecionado.Nome;
            lstSugestoesUsuario.Visible = false;

            // garantir que a lista de turmas não apareça ao preencher a turma
            lstSugestoesTurma.Visible = false;

            AplicarConfiguracaoEdicaoUsuario();

            txtNome.Text = _usuarioSelecionado.Nome;
            txtEmail.Text = _usuarioSelecionado.Email;
            mtxCPF.Text = _usuarioSelecionado.CPF;
            dtpDataNasc.Value = _usuarioSelecionado.DataNascimento == DateTime.MinValue ? DateTime.Today : _usuarioSelecionado.DataNascimento;
            mtxTelefone.Text = _usuarioSelecionado.Telefone;
            txtTurma.Text = _usuarioSelecionado.Turma;

            // fim do prefill -> reativa sugestões para próximas edições
            _suppressSuggestionOnPrefill = false;

            OnUsuarioSelecionado(true);

            lblTipoUsuario.Text = $"Tipo: {_usuarioSelecionado.TipoUsuario}";
            lblTipoUsuario.Visible = true;
        }


        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode excluir usuários.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (_usuarioSelecionado == null)
            {
                MessageBox.Show("Selecione um usuário primeiro.");
                return;
            }

            // Pedir a senha duas vezes usando o PasswordForm personalizado
            string senha1 = ObterSenha("Confirmação de Senha", "Digite sua senha:");
            if (string.IsNullOrEmpty(senha1))
            {
                MessageBox.Show("Operação cancelada.");
                return;
            }

            string senha2 = ObterSenha("Confirmação de Senha", "Digite sua senha novamente para confirmar:");
            if (string.IsNullOrEmpty(senha2))
            {
                MessageBox.Show("Operação cancelada.");
                return;
            }

            if (senha1 != senha2)
            {
                MessageBox.Show("As senhas não coincidem. Operação cancelada.");
                return;
            }

            // Verificar a senha do bibliotecário logado
            if (!VerificarSenhaBibliotecaria(senha1))
            {
                MessageBox.Show("Senha incorreta. Operação cancelada.");
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Verificar se o usuário tem empréstimos 
                    string sqlVerificar = @"
    SELECT COUNT(*) FROM Emprestimo WHERE Alocador = @id AND Status <> 'Devolvido'";

                    int emprestimosAtivos = 0;

                    using (var cmdVerificar = new SqlCeCommand(sqlVerificar, conexao))
                    {
                        cmdVerificar.Parameters.AddWithValue("@id", _usuarioSelecionado.Id);

                        using (var reader = cmdVerificar.ExecuteReader())
                        {
                            if (reader.Read()) emprestimosAtivos = reader.GetInt32(0);
                            
                        }
                    }

                    // Se houver empréstimos , mostrar aviso mais severo
                    if (emprestimosAtivos > 0)
                    {
                        var resultado = MessageBox.Show(
                            $"⚠️ ATENÇÃO: Este usuário possui registros ativos no sistema:\n\n" +
                            $"- {emprestimosAtivos} empréstimo(s) ativo(s)\n" +
                            $"A exclusão removerá TODOS os dados do usuario, mantendo somente registros do sistema..\n\n" +
                            $"Tem CERTEZA ABSOLUTA que deseja excluir este usuário?",
                            "ALERTA - Exclusão com Registros Ativos",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2);

                        if (resultado != DialogResult.Yes)
                        {
                            MessageBox.Show("Exclusão cancelada.");
                            return;
                        }

                        // Aviso final para confirmação extrema
                        var confirmacaoFinal = MessageBox.Show(
                            "🚨 EXCLUSÃO IRREVERSÍVEL 🚨\n\n" +
                            "Você está prestes a excluir um usuário com registros ativos.\n\n" +
                            "Esta ação NÃO PODE SER DESFEITA!\n\n" +
                            "Confirmar exclusão definitiva?",
                            "CONFIRMAÇÃO FINAL",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button2);

                        if (confirmacaoFinal != DialogResult.Yes)
                        {
                            MessageBox.Show("Exclusão cancelada.");
                            return;
                        }
                    }
                    else
                    {
                        // Confirmação normal para usuários sem registros ativos
                        var confirm = MessageBox.Show(
                            "Tem certeza que deseja excluir este usuário?\n\n" +
                            "Todas os dados do usuario seram removidos, mantendo alguns registros do sistema.",
                            "Confirmação de Exclusão",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (confirm != DialogResult.Yes) return;
                    }

                    

                    // Agora excluir o usuário
                    string sql = "DELETE FROM Usuarios WHERE Id = @id";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", _usuarioSelecionado.Id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuário excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObterSenha(string titulo, string mensagem)
        {
            using (var passwordForm = new PasswordForm())
            {
                passwordForm.Titulo = titulo;
                passwordForm.Mensagem = mensagem;

                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    return passwordForm.SenhaDigitada;
                }
            }
            return null;
        }

        private bool VerificarSenhaBibliotecaria(string senha)
        {
            string nomeBibliotecaria = Sessao.NomeBibliotecariaLogada;
            if (string.IsNullOrEmpty(nomeBibliotecaria))
            {
                MessageBox.Show("Nenhum bibliotecário está logado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string query = @"SELECT Senha_hash, Senha_salt FROM usuarios 
                             WHERE Nome = @nome AND TipoUsuario LIKE '%Bibliotec%'";

                    using (var comando = new SqlCeCommand(query, conexao))
                    {
                        comando.Parameters.AddWithValue("@nome", nomeBibliotecaria);
                        using (var reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashSalvo = reader["Senha_hash"].ToString();
                                string saltSalvo = reader["Senha_salt"].ToString();

                                // Use a mesma classe de criptografia do login
                                return CriptografiaSenha.VerificarSenha(senha, hashSalvo, saltSalvo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar senha: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        
            // Substitua o handler para NÃO preencher automaticamente ao mudar a seleção
private void lstSugestoesUsuario_SelectedIndexChanged(object sender, EventArgs e)
{
    // Evita preenchimento automático; confirmação só por Click ou Enter
    // Intencionalmente vazio.
}

        private void txtNomeUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesUsuario.Visible || lstSugestoesUsuario.Items.Count == 0)
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
                lstSugestoesUsuario.Focus();
                if (lstSugestoesUsuario.SelectedIndex < 0 && lstSugestoesUsuario.Items.Count > 0)
                    lstSugestoesUsuario.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ConfirmarSugestaoUsuario();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesUsuario.Visible = false;
            }
        }


        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            if (_suppressSuggestionOnPrefill) return;

            lstSugestoesUsuario.Items.Clear();
            lstSugestoesUsuario.Visible = false;
            _cacheUsuarios.Clear();

            string nomeBusca = txtNomeUsuario.Text.Trim();
            if (string.IsNullOrWhiteSpace(nomeBusca))
                return;

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT * FROM usuarios WHERE Nome LIKE @nome ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", nomeBusca + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var usuario = new Usuarios
                                {
                                    Id = (int)reader["Id"],
                                    Nome = reader["Nome"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    CPF = reader["CPF"].ToString(),
                                    DataNascimento = reader["DataNascimento"] != DBNull.Value ? Convert.ToDateTime(reader["DataNascimento"]) : DateTime.MinValue,
                                    Telefone = reader["Telefone"].ToString(),
                                    Turma = reader["Turma"].ToString(),
                                    TipoUsuario = reader["TipoUsuario"].ToString()
                                };

                                _cacheUsuarios.Add(usuario);

                                // Monta o texto a ser exibido na lista:
                                // Nome - Turma (se existir), caso contrário Nome - TipoUsuario
                                string exibicao = usuario.Nome;
                                if (!string.IsNullOrWhiteSpace(usuario.Turma))
                                {
                                    exibicao += " - " + usuario.Turma;
                                }
                                else if (!string.IsNullOrWhiteSpace(usuario.TipoUsuario))
                                {
                                    exibicao += " - " + usuario.TipoUsuario;
                                }

                                lstSugestoesUsuario.Items.Add(exibicao);
                            }
                        }
                    }
                }

                bool temItens = lstSugestoesUsuario.Items.Count > 0;
                lstSugestoesUsuario.Visible = temItens;
                lstSugestoesUsuario.Enabled = temItens;

                if (temItens)
                {
                    int visibleItems = Math.Min(5, lstSugestoesUsuario.Items.Count);
                    int extraPadding = 8;
                    lstSugestoesUsuario.ItemHeight = 40;
                    lstSugestoesUsuario.Height = visibleItems * lstSugestoesUsuario.ItemHeight + extraPadding;
                    lstSugestoesUsuario.Width = txtNomeUsuario.Width;
                    lstSugestoesUsuario.Left = txtNomeUsuario.Left;
                    lstSugestoesUsuario.Top = txtNomeUsuario.Bottom;

                    lstSugestoesUsuario.BringToFront();

                    // não pré-seleciona; permite digitar sem auto-preencher
                    lstSugestoesUsuario.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca: " + ex.Message);
            }
        }



        // Configurações de exibição para Edição de Usuário

        private void HabilitarCampos()
        {
            txtNome.Visible = true;
            txtNome.Enabled = true;
            txtEmail.Visible = true;
            txtEmail.Enabled = true;
            txtTurma.Visible = true;
            txtTurma.Enabled = true;
            lblTurma.Visible = true;
            lblTurma.Enabled = true;
            lblEmail.Visible = true;
            lblEmail.Enabled = true;
            lblNome.Visible = true;
            lblNome.Enabled = true;
            lblCPF.Visible = true;
            lblCPF.Enabled = true;
            lblDataNasc.Visible = true;
            lblDataNasc.Enabled = true;
            lblTelefone.Visible = true;
            lblTelefone.Enabled = true;
            mtxCPF.Visible = true;
            mtxCPF.Enabled = true;
            mtxTelefone.Visible = true;
            mtxTelefone.Enabled = true;
            dtpDataNasc.Visible = true;
            dtpDataNasc.Enabled = true;
        }

        private void ConfigurarEdicaoParaBibliotecario()
        {
            HabilitarCampos();
            txtTurma.Visible = false;
            lblTurma.Visible = false;
            panel1.Location = new Point(538, 387);
            panel1.Anchor = AnchorStyles.Top;
        }

        private void ConfigurarEdicaoParaProfessor()
        {
            HabilitarCampos();
            txtTurma.Visible = false;
            lblTurma.Visible = false;
            panel1.Location = new Point(538, 387);
            panel1.Anchor = AnchorStyles.Top;
        }

        private void ConfigurarEdicaoParaAluno()
        {
            HabilitarCampos();
            panel1.Location = new Point(538, 468);
            panel1.Anchor = AnchorStyles.Top;
        }

        private void ConfigurarEdicaoParaOutros()
        {
            HabilitarCampos();
            panel1.Location = new Point(538, 387);
            panel1.Anchor = AnchorStyles.Top;
            txtEmail.Visible = true;
            txtTurma.Visible = false;
            lblTurma.Visible = true;
            lblEmail.Visible = false;
        }

        private void AplicarConfiguracaoEdicaoUsuario()
        {
            string tipo = _usuarioSelecionado.TipoUsuario;

            if (tipo.Equals("Bibliotecário(a)", StringComparison.OrdinalIgnoreCase))
                ConfigurarEdicaoParaBibliotecario();
            else if (tipo.Equals("Professor(a)", StringComparison.OrdinalIgnoreCase))
                ConfigurarEdicaoParaProfessor();
            else if (tipo.Equals("Outros", StringComparison.OrdinalIgnoreCase))
                ConfigurarEdicaoParaOutros();
            else
                ConfigurarEdicaoParaAluno();
        }

        private bool CpfJaExiste(string cpf, int usuarioIdAtual)
        {
            // Normaliza: remove tudo que não é dígito
            string cpfDigits = string.IsNullOrWhiteSpace(cpf) ? "" : Regex.Replace(cpf, @"\D", "");

            // Se não houver dígitos, considerar como vazio -> não é duplicado
            if (string.IsNullOrEmpty(cpfDigits))
                return false;

            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                // Compara apenas os dígitos armazenados (remove . - e espaços)
                string sql = @"
            SELECT COUNT(*) FROM usuarios 
            WHERE REPLACE(REPLACE(REPLACE(CPF, '.', ''), '-', ''), ' ', '') = @CpfNormalized
              AND Id <> @IdAtual";

                using (var cmd = new SqlCeCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@CpfNormalized", cpfDigits);
                    cmd.Parameters.AddWithValue("@IdAtual", usuarioIdAtual);

                    int count = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                    return count > 0;
                }
            }
        }


        private bool EmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #region estilizacao listbox

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

            // Tons de cinza
            Color backColor = hovered
                ? Color.FromArgb(235, 235, 235) // cinza claro no hover
                : Color.White;                  // fundo branco

            Color textColor = Color.FromArgb(60, 60, 60); // cinza escuro

            using (SolidBrush b = new SolidBrush(backColor))
                e.Graphics.FillRectangle(b, e.Bounds);

            string text = listBox.Items[e.Index].ToString();
            Font font = listBox.Font;

            Rectangle textRect = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, text, font, textRect, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            // Linha divisória entre itens (cinza bem suave)
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

        private void HabilitarCampos(bool ativo)
        {
            txtNome.Enabled = ativo;
            txtEmail.Enabled = ativo;
            txtTurma.Enabled = ativo;
            mtxCPF.Enabled = ativo;
            mtxTelefone.Enabled = ativo;
            dtpDataNasc.Enabled = ativo;

            SetCamposColors(ativo);
            SetLabelColors(ativo);
        }

        private void SetCamposColors(bool enabled)
        {
            Color backColor = enabled ? Color.WhiteSmoke : Color.White;
            Color borderColor = enabled ? Color.FromArgb(204, 204, 204) : Color.LightGray;

            if (txtNome is RoundedTextBox rtbNome)
            {
                rtbNome.BackColor = backColor;
                rtbNome.BorderColor = borderColor;
            }
            if (txtEmail is RoundedTextBox rtbEmail)
            {
                rtbEmail.BackColor = backColor;
                rtbEmail.BorderColor = borderColor;
            }
            if (txtTurma is RoundedTextBox rtbTurma)
            {
                rtbTurma.BackColor = backColor;
                rtbTurma.BorderColor = borderColor;
            }
            if (mtxCPF is RoundedMaskedTextBox rmtxCPF)
            {
                rmtxCPF.BackColor = backColor;
                rmtxCPF.BorderColor = borderColor;
            }
            if (mtxTelefone is RoundedMaskedTextBox rmtxTel)
            {
                rmtxTel.BackColor = backColor;
                rmtxTel.BorderColor = borderColor;
            }
            dtpDataNasc.BackColor = backColor;
        }

        private void SetLabelColors(bool enabled)
        {
            Color color = enabled ? Color.FromArgb(20, 41, 60) : Color.LightGray;
            lblNome.ForeColor = color;
            lblEmail.ForeColor = color;
            lblTurma.ForeColor = color;
            lblCPF.ForeColor = color;
            lblDataNasc.ForeColor = color;
            lblTelefone.ForeColor = color;
        }

        // Chame este método quando um usuário for selecionado ou desmarcado
        private void OnUsuarioSelecionado(bool selecionado)
        {
            HabilitarCampos(selecionado);
        }

        // No método LimparCampos ou quando não houver seleção:
        private void LimparCampos()
        {
            txtNomeUsuario.Text = "";
            txtNome.Text = "";
            txtEmail.Text = "";
            mtxCPF.Text = "";
            dtpDataNasc.Value = DateTime.Today;
            mtxTelefone.Text = "";
            txtTurma.Text = "";
            lblTipoUsuario.Text = "";
            _usuarioSelecionado = null;
            lblTipoUsuario.Visible = false;
            lblTipoUsuario.Text = "";

           
            _suppressSuggestionOnPrefill = false;

            OnUsuarioSelecionado(false);
        }


        public void PreencherUsuario(Usuarios usuario)
        {
            HabilitarCampos(true);

            _usuarioSelecionado = usuario;

            // Se o form foi aberto a partir do UsuarioForm (FecharAoSalvar = true),
            // suprimimos temporariamente as sugestões para evitar que apareçam.
            if (this.FecharAoSalvar)
            {
                _suppressSuggestionOnPrefill = true;
                lstSugestoesUsuario.Visible = false;
                lstSugestoesUsuario.SelectedIndex = -1;
                lstSugestoesTurma.Visible = false;
                lstSugestoesTurma.SelectedIndex = -1;
            }

            txtNomeUsuario.Text = usuario.Nome;
            txtNome.Text = usuario.Nome;
            txtEmail.Text = usuario.Email;
            mtxCPF.Text = usuario.CPF;
            dtpDataNasc.Value = usuario.DataNascimento == DateTime.MinValue ? DateTime.Today : usuario.DataNascimento;
            mtxTelefone.Text = usuario.Telefone;
            txtTurma.Text = usuario.Turma;
            lblTipoUsuario.Text = $"Tipo: {usuario.TipoUsuario}";
            lblTipoUsuario.Visible = true;

            AplicarConfiguracaoEdicaoUsuario();
            OnUsuarioSelecionado(true);

            // fim do prefill -> reativa sugestões para próximas edições
            _suppressSuggestionOnPrefill = false;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Remove o foco de qualquer controle
            this.ActiveControl = null;

            // Garante que nenhum controle receba foco após o carregamento
            this.BeginInvoke(new Action(() => this.ActiveControl = null));

            // Desabilita campos inicialmente
            HabilitarCampos(false);

            // Carregar turmas do banco
            CarregarTurmasDoBanco();
        }

        #region Métodos de Turma
        

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

        

        // Substitua o txtTurma_TextChanged: remove a seleção automática
private void txtTurma_TextChanged(object sender, EventArgs e)
{
            if (_suppressSuggestionOnPrefill) return;

            string texto = txtTurma.Text.Trim();

    if (string.IsNullOrEmpty(texto))
    {
        lstSugestoesTurma.Visible = false;
        return;
    }

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
        lstSugestoesTurma.Left = txtTurma.Left;
        lstSugestoesTurma.Top = txtTurma.Bottom;
        lstSugestoesTurma.BringToFront();
        lstSugestoesTurma.Visible = true;

        // NÃO seleciona nada automaticamente aqui
        lstSugestoesTurma.SelectedIndex = -1;
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
                    // Impede sair do campo se não for uma turma permitida
                    var turma = txtTurma.Text.Trim();
                    if (!string.IsNullOrEmpty(turma) && !BibliotecaApp.Utils.TurmasUtil.TurmasPermitidas.Contains(turma))
                    {
                        MessageBox.Show("Selecione uma turma válida da lista de turmas permitidas.", "Turma inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtTurma.Text = "";
                        txtTurma.Focus();
                    }
                }
            }));
        }

        private void txtTurma_Enter(object sender, EventArgs e)
        {
            // Ao entrar no campo, entendemos que o usuário possivelmente quer editar a turma:
            // liberamos a exibição das sugestões para próximas alterações.
            _suppressSuggestionOnPrefill = false;

            // opcional: não mostrar automaticamente só por entrar — se quiser mostrar já as sugestões
            // para o texto atual, descomente a linha abaixo:
            // txtTurma_TextChanged(sender, EventArgs.Empty);
        }

        private void txtTurma_MouseClick(object sender, MouseEventArgs e)
        {
            
            _suppressSuggestionOnPrefill = false;
        }

        private void lstSugestoesTurma_Click(object sender, EventArgs e)
        {
            if (lstSugestoesTurma.SelectedItem != null)
            {
                string turmaSelecionada = lstSugestoesTurma.SelectedItem.ToString();
                txtTurma.Text = turmaSelecionada;
                lstSugestoesTurma.Visible = false;
                txtTurma.Focus();
            }
        }

        // Substitua para usar confirmação centralizada (Turma)
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
                if (lstSugestoesTurma.SelectedIndex < 0 && lstSugestoesTurma.Items.Count > 0)
                    lstSugestoesTurma.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                // Se nada estiver selecionado, seleciona o primeiro e confirma
                if (lstSugestoesTurma.SelectedIndex < 0 && lstSugestoesTurma.Items.Count > 0)
                    lstSugestoesTurma.SelectedIndex = 0;
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

        #endregion

        private void AdicionarTurmaSeNova(string turma)
        {
            // Não corrigir turma automaticamente
            if (string.IsNullOrEmpty(turma)) return;

            // Verificar se já existe (case insensitive)
            bool existe = turmasCadastradas.Any(t =>
                string.Equals(t, turma, StringComparison.OrdinalIgnoreCase));

            if (!existe)
            {
                turmasCadastradas.Add(turma);
                turmasCadastradas.Sort();
            }
        }

        // NOVO: clique/leave do listbox de Nome
        private void lstSugestoesUsuario_Click(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedIndex >= 0)
                SelecionarUsuario(lstSugestoesUsuario.SelectedIndex);
        }

        private void lstSugestoesUsuario_Leave(object sender, EventArgs e)
        {
            lstSugestoesUsuario.Visible = false;
        }

        // Helpers centralizados (adicione próximo às outras rotinas)
        private bool ConfirmarSugestaoUsuario()
        {
            if (!lstSugestoesUsuario.Visible || lstSugestoesUsuario.Items.Count == 0)
                return false;

            if (lstSugestoesUsuario.SelectedIndex < 0)
                lstSugestoesUsuario.SelectedIndex = 0;

            SelecionarUsuario(lstSugestoesUsuario.SelectedIndex);
            txtNomeUsuario.Focus();
            this.SelectNextControl(txtNomeUsuario, true, true, true, true);
            return true;
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
    }
}