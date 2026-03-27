using BibliotecaApp.Models;
using BibliotecaApp.Services;
using BibliotecaApp.Utils; // ADICIONE ESTA LINHA
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BibliotecaApp.Forms.Livros
{
    public partial class EmprestimoForm : Form
    {
        #region Propriedades
        public List<Usuarios> Usuarios { get; set; }
        public List<Livro> Livros { get; set; }
        public List<Emprestimo> Emprestimos { get; set; }
        
        private bool _carregandoLivroAutomaticamente = false;
        private List<Livro> _cacheLivros = new List<Livro>();
        private List<Usuarios> _cacheUsuarios = new List<Usuarios>();
        
        private const int LIMITE_CODIGO_BARRAS = 13;

        #endregion

        #region Construtores
        public EmprestimoForm()
        {
            InitializeComponent();

            EstilizarListBoxSugestao(lstSugestoesUsuario);
            EstilizarListBoxSugestao(lstLivros);

            Usuarios = new List<Usuarios>();
            Livros = new List<Livro>();
            Emprestimos = new List<Emprestimo>();

            CarregarUsuariosDoBanco();
            CarregarLivrosDoBanco();
            CarregarBibliotecarias();

            if (!string.IsNullOrWhiteSpace(Sessao.NomeBibliotecariaLogada))
            {
                int idx = cbBibliotecaria.Items.IndexOf(Sessao.NomeBibliotecariaLogada);
                if (idx >= 0)
                    cbBibliotecaria.SelectedIndex = idx;
            }

            txtNomeUsuario.TextChanged += txtNomeUsuario_TextChanged;
            txtNomeUsuario.Leave += txtNomeUsuario_Leave;
            lstSugestoesUsuario.Leave += lstSugestoesUsuario_Leave;
            txtBarcode.Leave += txtBarcode_Leave;

            txtLivro.TextChanged += txtLivro_TextChanged;
            txtLivro.Leave += txtLivro_Leave;
            
            txtLivro.KeyDown += txtLivro_KeyDown;
            lstLivros.Click += lstLivros_Click;
            lstLivros.KeyDown += lstLivros_KeyDown;

            BibliotecaApp.Utils.EventosGlobais.BibliotecariaCadastrada += (s, e) => CarregarBibliotecarias();
        }
        #endregion

        private static bool IsAdminLogado()
            => string.Equals(Sessao.NomeBibliotecariaLogada, "Administrador", StringComparison.OrdinalIgnoreCase);

        #region Eventos do Formulário

        public event EventHandler LivroAtualizado;
        private void EmprestimoForm_Load(object sender, EventArgs e)
        {
            dtpDataEmprestimo.Value = DateTime.Today;
            dtpDataDevolucao.Value = DateTime.Today.AddDays(7);

            this.KeyPreview = true;
            this.KeyDown += Form_KeyDown;

            // Limite de caracteres para o código de barras (RoundedTextBox não tem MaxLength)
            txtBarcode.KeyPress += txtBarcode_KeyPressLimiter;
            txtBarcode.TextChanged += txtBarcode_TextChangedLimiter;
        }

        private void label2_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void lstSugestoesUsuario_SelectedIndexChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void txtLivro_Load(object sender, EventArgs e) { }
        private void txtBarcode_Load(object sender, EventArgs e) { }
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void cbBibliotecaria_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbBibliotecaria.DrawMode = DrawMode.OwnerDrawFixed;
            cbBibliotecaria.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBibliotecaria.ItemHeight = 35;
        }
        #endregion

        #region Métodos de Empréstimo
        private void btnEmprestar_Click(object sender, EventArgs e)
        {
            // Bloqueia ação por administrador
            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode realizar empréstimos.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Obtendo o nome do usuário, livro e responsável (bibliotecário)
            string nomeUsuario = txtNomeUsuario.Text.Trim();
            string nomeLivro = txtLivro.Text.Trim();
            string nomeBibliotecaria = cbBibliotecaria.SelectedItem?.ToString();

            if (!ValidarDatas())
            {
                return;
            }

            // Valida se todos os campos foram preenchidos
            if (string.IsNullOrEmpty(nomeUsuario) || string.IsNullOrEmpty(nomeLivro) || string.IsNullOrEmpty(nomeBibliotecaria))
            {
                MessageBox.Show("Preencha todos os campos antes de emprestar.");
                return;
            }

            // Buscando o usuário, livro e bibliotecário selecionados
            Usuarios usuario = null;

            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string sqlUsuario = "SELECT TOP 1 * FROM usuarios WHERE Nome = @nome";

                using (var cmd = new SqlCeCommand(sqlUsuario, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", nomeUsuario);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuarios
                            {
                                Id = (int)reader["Id"],
                                Nome = reader["Nome"].ToString(),
                                TipoUsuario = reader["TipoUsuario"].ToString(),
                                Email = reader["Email"].ToString()?.Trim()
                            };
                        }
                    }
                }
            }
            var livro = Livros.FirstOrDefault(l => l.Nome.Equals(nomeLivro, StringComparison.OrdinalIgnoreCase));
            var responsavel = Usuarios.FirstOrDefault(u => u.Nome.Equals(nomeBibliotecaria, StringComparison.OrdinalIgnoreCase));

            // Verifica se algum dos dados não foi encontrado
            if (usuario == null || livro == null || responsavel == null)
            {
                MessageBox.Show("Usuário, livro ou responsável não encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verifica se o aluno já tem empréstimo ativo
            if (usuario.TipoUsuario == "Aluno(a)")
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sqlVerificaEmprestimo = @"
                SELECT COUNT(*) 
                FROM Emprestimo 
                WHERE Alocador = @usuarioId AND Status = 'Ativo'";

                    using (var cmdVerifica = new SqlCeCommand(sqlVerificaEmprestimo, conexao))
                    {
                        cmdVerifica.Parameters.AddWithValue("@usuarioId", usuario.Id);
                        int emprestimosAtivos = (int)cmdVerifica.ExecuteScalar();

                        // Se o aluno já tiver empréstimo ativo, bloqueia o novo
                        if (emprestimosAtivos > 0)
                        {
                            MessageBox.Show("Este aluno já possui um empréstimo ativo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }

            if (!livro.Disponibilidade || livro.Quantidade <= 0)
            {
                string email = usuario.Email?.Trim();
                bool emailValido = !string.IsNullOrWhiteSpace(email) && email.Contains("@");

                if (emailValido)
                {
                    MessageBox.Show(
                        $"O livro \"{livro.Nome}\" está indisponível para empréstimo.\n\n" +
                        "Você pode ser notificado por e-mail quando o livro estiver disponível.",
                        "Livro Indisponível",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    DialogResult resposta = MessageBox.Show(
                        "Deseja receber um e-mail quando o livro estiver disponível?",
                        "Notificação de Disponibilidade",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (resposta == DialogResult.Yes)
                    {
                        using (var conexao = Conexao.ObterConexao())
                        {
                            conexao.Open();
                            string sql = @"INSERT INTO NotificacoesDisponibilidade (UsuarioId, LivroId, Email, Enviado)
                   VALUES (@usuarioId, @livroId, @email, 0)";
                            using (var cmd = new SqlCeCommand(sql, conexao))
                            {
                                cmd.Parameters.AddWithValue("@usuarioId", usuario.Id);
                                cmd.Parameters.AddWithValue("@livroId", livro.Id);
                                cmd.Parameters.AddWithValue("@email", usuario.Email);
                                cmd.ExecuteNonQuery();
                            }
                        }


                        string assunto = "📚 Notificação de disponibilidade - Biblioteca Monteiro Lobato";
                        string corpo = $@"
<html>
<body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
        <h2 style='color: #2c3e50;'>Olá, {usuario.Nome} 👋</h2>
        <p>Você será avisado por e-mail assim que o livro <strong>{livro.Nome}</strong> estiver disponível para empréstimo.</p>
        <hr />
        <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
    </div>
</body>
</html>";
                        EmailService.Enviar(email, assunto, corpo);
                        LivroAtualizado?.Invoke(this, EventArgs.Empty);
                        MessageBox.Show("Você será notificado por e-mail quando o livro estiver disponível.", "Notificação registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(
                        $"O livro \"{livro.Nome}\" está indisponível para empréstimo.\n\n" +
                        "Nenhum e-mail cadastrado para notificação.",
                        "Livro Indisponível",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

                return; // interrompe o fluxo de empréstimo
            }



            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Inserção do novo empréstimo
                    string sqlInserir = @"
    INSERT INTO Emprestimo 
        (Alocador, Livro, LivroNome, Responsavel, DataEmprestimo, DataDevolucao, DataProrrogacao, DataRealDevolucao, Status, CodigoBarras)
    VALUES 
        (@alocador, @livro, @livroNome, @responsavel, @dataEmprestimo, @dataDevolucao, NULL, NULL, 'Ativo', @codigoBarras)";

                    using (var cmdInsert = new SqlCeCommand(sqlInserir, conexao))
                    {
                        cmdInsert.Parameters.AddWithValue("@alocador", usuario.Id);
                        cmdInsert.Parameters.AddWithValue("@livro", livro.Id);
                        cmdInsert.Parameters.AddWithValue("@livroNome", livro.Nome);
                        cmdInsert.Parameters.AddWithValue("@responsavel", responsavel.Id);
                        cmdInsert.Parameters.AddWithValue("@dataEmprestimo", DateTime.Now);

                        var dataDevolucaoParaInserir = dtpDataDevolucao.Value; // respeita o DTP (professor pode ter prazo maior)
                        cmdInsert.Parameters.AddWithValue("@dataDevolucao", dataDevolucaoParaInserir);

                        // Prioriza texto do scanner; se vazio usa o código do livro; se ainda vazio grava string vazia ""
                        string codigoBarras = !string.IsNullOrWhiteSpace(txtBarcode.Text)
                            ? txtBarcode.Text.Trim()
                            : (string.IsNullOrWhiteSpace(livro.CodigoDeBarras) ? "" : livro.CodigoDeBarras);

                        // grava sempre uma string (vazia quando não existir)
                        cmdInsert.Parameters.AddWithValue("@codigoBarras", codigoBarras);

                        cmdInsert.ExecuteNonQuery();
                    }



                    // Atualiza a quantidade e disponibilidade do livro
                    livro.Quantidade--;
                    livro.Disponibilidade = livro.Quantidade > 0;

                    string sqlAtualizaLivro = "UPDATE Livros SET Quantidade = @quantidade, Disponibilidade = @disponivel WHERE Id = @id";

                    using (var cmdLivro = new SqlCeCommand(sqlAtualizaLivro, conexao))
                    {
                        cmdLivro.Parameters.AddWithValue("@quantidade", livro.Quantidade);
                        cmdLivro.Parameters.AddWithValue("@disponivel", livro.Disponibilidade);
                        cmdLivro.Parameters.AddWithValue("@id", livro.Id);
                        cmdLivro.ExecuteNonQuery();
                    }
                }
               

                // dispara atualização global
                BibliotecaApp.Utils.EventosGlobais.OnEmprestimoRealizado();
                MessageBox.Show("Empréstimo registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                LivroAtualizado?.Invoke(this, EventArgs.Empty);

                // Envio de e-mail apenas se o e-mail for válido
                string email = usuario.Email?.Trim();
                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
                {
                    string assunto = "✅ Empréstimo Confirmado - Biblioteca Monteiro Lobato";
                    string corpo = $@"
<html>
<body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
        <h2 style='color: #2c3e50;'>Olá, {usuario.Nome} 👋</h2>
        <p>Seu empréstimo foi registrado com sucesso! Aqui estão os detalhes:</p>
        <p><strong>📖 Livro:</strong> {livro.Nome}</p>
        <p><strong>📅 Data do Empréstimo:</strong> {DateTime.Now:dd/MM/yyyy}</p>
        <p><strong>📆 Data de Devolução:</strong> {dtpDataDevolucao.Value:dd/MM/yyyy}</p>
        <p style='margin-top: 20px;'>Por favor, devolva o livro no prazo para evitar bloqueios no sistema e restrições na secretaria.</p>
        <hr />
        <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
    </div>
</body>
</html>";
                    EmailService.Enviar(email, assunto, corpo);
                }
                else
                {
                    MessageBox.Show("Empréstimo registrado, porém o usuário não possui e-mail cadastrado ou válido. Nenhum e-mail foi enviado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar empréstimo:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion





        #region Métodos de Usuário
        private void txtNomeUsuario_TextChanged(object sender, EventArgs e)
        {
            string nomeBusca = txtNomeUsuario.Text.Trim();

            lstSugestoesUsuario.Items.Clear();
            lstSugestoesUsuario.Visible = false;
            _cacheUsuarios.Clear();

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

                                // Se houver turma mostra "Nome - Turma", senão mostra "Nome - TipoUsuario"
                                string sufixo = !string.IsNullOrWhiteSpace(usuario.Turma) ? usuario.Turma :
                                                (!string.IsNullOrWhiteSpace(usuario.TipoUsuario) ? usuario.TipoUsuario : "");
                                if (!string.IsNullOrWhiteSpace(sufixo))
                                    lstSugestoesUsuario.Items.Add($"{usuario.Nome} - {sufixo}");
                                else
                                    lstSugestoesUsuario.Items.Add(usuario.Nome);
                            }
                        }
                    }
                }
                lstSugestoesUsuario.Visible = lstSugestoesUsuario.Items.Count > 0;
                lstSugestoesUsuario.Enabled = lstSugestoesUsuario.Items.Count > 0;

                // Seleciona o primeiro item por padrão
                if (lstSugestoesUsuario.Visible)
                    lstSugestoesUsuario.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca: " + ex.Message);
            }
        }


        private void lstSugestoesUsuario_Click(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedIndex >= 0)
                SelecionarUsuario(lstSugestoesUsuario.SelectedIndex);
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
                if (lstSugestoesUsuario.Items.Count > 0)
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

        private void lstSugestoesUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ConfirmarSugestaoUsuario();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesUsuario.Visible = false;
                txtNomeUsuario.Focus();
            }
        }

        private void SelecionarUsuario(int index)
        {
            var usuario = _cacheUsuarios[index];
            txtNomeUsuario.Text = usuario.Nome;
            lstSugestoesUsuario.Visible = false;

            if (usuario.TipoUsuario == "Professor(a)")
            {
                chkDevolucaoPersonalizada.Checked = true;
                chkDevolucaoPersonalizada.Enabled = false;
                dtpDataDevolucao.Enabled = true;
            }
            else
            {
                chkDevolucaoPersonalizada.Checked = false;
                chkDevolucaoPersonalizada.Enabled = true;
                dtpDataDevolucao.Enabled = false;
                dtpDataDevolucao.Value = DateTime.Today.AddDays(7); // 7 dias por padrão
            }
        }

        private void txtNomeUsuario_Leave(object sender, EventArgs e)
        {
            // Se o foco sair do textbox e da listbox
            if (!lstSugestoesUsuario.Focused)
                lstSugestoesUsuario.Visible = false;
        }

        private void lstSugestoesUsuario_Leave(object sender, EventArgs e)
        {
            lstSugestoesUsuario.Visible = false;
        }

        private void CarregarUsuariosDoBanco()
        {
            Usuarios.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Id, Nome, TipoUsuario FROM Usuarios";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuarios.Add(new Usuarios
                            {
                                Id = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                TipoUsuario = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Métodos de Livros
        private void txtLivro_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstLivros.Visible || lstLivros.Items.Count == 0)
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
                lstLivros.Focus();
                if (lstLivros.Items.Count > 0)
                    lstLivros.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ConfirmarSugestaoLivro();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstLivros.Visible = false;
            }
        }

        private void btnBuscarLivro_Click(object sender, EventArgs e)
        {
            string filtro = txtLivro.Text.Trim();
            if (string.IsNullOrEmpty(filtro))
                return;

            _cacheLivros.Clear();
            lstLivros.Items.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade 
                           FROM Livros 
                           WHERE Nome LIKE @nome
                           ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        // Busca só nomes que começam com o filtro
                        cmd.Parameters.AddWithValue("@nome", filtro + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Livro livro = new Livro
                                {
                                    Id = reader.GetInt32(0),
                                    Nome = reader.GetString(1),
                                    Autor = reader.GetString(2),
                                    Genero = reader.GetString(3),
                                    Quantidade = reader.GetInt32(4),
                                    CodigoDeBarras = reader.GetString(5),
                                    Disponibilidade = reader.GetBoolean(6)
                                };

                                _cacheLivros.Add(livro);

                                // Adiciona nome e gênero na lista
                                lstLivros.Items.Add($"{livro.Nome} - {livro.Autor}");
                            }
                        }
                    }
                }

                lstLivros.Visible = lstLivros.Items.Count > 0;
                if (lstLivros.Visible)
                    lstLivros.BringToFront();
            }
catch (Exception ex)
{
    MessageBox.Show("Erro ao buscar livros:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
}

private void lstLivros_Click(object sender, EventArgs e)
{
    if (lstLivros.SelectedIndex >= 0)
        SelecionarLivro(lstLivros.SelectedIndex);
}

private void lstLivros_KeyDown(object sender, KeyEventArgs e)
{
    if (e.KeyCode == Keys.Enter)
    {
        e.SuppressKeyPress = true;
        ConfirmarSugestaoLivro();
    }
    else if (e.KeyCode == Keys.Escape)
    {
        e.SuppressKeyPress = true;
        lstLivros.Visible = false;
        txtLivro.Focus();
    }
}

        private void SelecionarLivro(int index)
        {
            var livro = _cacheLivros[index];

            // Alteração programática do nome (não deve limpar o barcode)
            SetLivroTextProgrammatic(livro.Nome, origemBarcode: false);

            // Mantém o mesmo comportamento de preencher o barcode do livro selecionado
            txtBarcode.Enabled = true;
            txtBarcode.Text = livro.CodigoDeBarras;
           

            lstLivros.Visible = false;
        }



        private void txtLivro_TextChanged(object sender, EventArgs e)
{
    string filtro = txtLivro.Text.Trim();

    // Se veio do scanner, não exibir listbox agora
    if (_preenchendoPorBarcode)
    {
        lstLivros.Items.Clear();
        lstLivros.Visible = false;
        _preenchendoPorBarcode = false; // consome o estado do scanner
        return;
    }

    // Qualquer alteração manual no nome do livro limpa o código de barras
    if (!_alterandoTxtLivroProgramaticamente)
    {
        if (!string.IsNullOrEmpty(txtBarcode.Text))
        {
            txtBarcode.Text = "";
        }
        // Se estava marcado como "veio do barcode", invalida esse estado
        if (!string.IsNullOrEmpty(_nomePreenchidoPorBarcode))
            _nomePreenchidoPorBarcode = null;
    }

    lstLivros.Items.Clear();
    lstLivros.Visible = false;
    _cacheLivros.Clear();

    if (string.IsNullOrWhiteSpace(filtro))
        return;

    try
    {
        using (var conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string sql = @"SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade 
                           FROM Livros 
                           WHERE Nome LIKE @nome
                           ORDER BY Nome";
            using (var cmd = new SqlCeCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@nome", filtro + "%");
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Livro livro = new Livro
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Autor = reader.GetString(2),
                            Genero = reader.GetString(3),
                            Quantidade = reader.GetInt32(4),
                            CodigoDeBarras = reader.GetString(5),
                            Disponibilidade = reader.GetBoolean(6)
                        };
                        _cacheLivros.Add(livro);
                        lstLivros.Items.Add($"{livro.Nome} - {livro.Autor}");
                    }
                }
            }
        }
        lstLivros.Visible = lstLivros.Items.Count > 0;
        if (lstLivros.Visible)
            lstLivros.SelectedIndex = 0; // Seleciona o primeiro por padrão
        if (lstLivros.Visible)
            lstLivros.BringToFront();
    }
    catch (Exception ex)
    {
        MessageBox.Show("Erro ao buscar livros:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}


private void txtLivro_Leave(object sender, EventArgs e)
{
    if (!lstLivros.Focused)
        lstLivros.Visible = false;
}

private void CarregarLivrosDoBanco()
{
    Livros.Clear();

    try
    {
        using (var conexao = Conexao.ObterConexao())
        {
            conexao.Open();
            string sql = "SELECT Id, Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade FROM Livros";

            using (var cmd = new SqlCeCommand(sql, conexao))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Livro livro = new Livro(
                        reader.GetString(1), // Nome
                        reader.GetString(2), // Autor
                        reader.GetString(3), // Genero
                        reader.GetBoolean(6), // Disponibilidade
                        reader.GetInt32(4),   // Quantidade
                        reader.GetString(5)   // CodigoBarras
                    );

                    // Setando o ID (tornar public set temporariamente ou criar outro construtor com ID)
                    typeof(Livro).GetProperty("Id").SetValue(livro, reader.GetInt32(0));
                    Livros.Add(livro);
                }
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Erro ao carregar livros: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
#endregion

#region Métodos de Código de Barras
private void txtBarcode_Leave(object sender, EventArgs e)
{// Não verifica se foi aberto pela reserva
            


            // Só busca se o campo estiver preenchido
            if (!string.IsNullOrEmpty(txtBarcode.Text))
            {
                BuscarEPreencherLivroPorCodigo();
            }
        }

        private void BuscarEPreencherLivroPorCodigo()
        {
            string codigo = txtBarcode.Text.Trim();
            if (string.IsNullOrEmpty(codigo)) return;

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT TOP 1 Nome FROM Livros WHERE CodigoBarras = @codigo";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Preenche o nome sem exibir a lista
                                SetLivroTextProgrammatic(reader.GetString(0), origemBarcode: true);

                                // Oculta qualquer lista de sugestão
                                lstLivros.Visible = false;
                            }
                            else
                            {
                                MessageBox.Show("Livro não encontrado. Escaneie novamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtBarcode.Focus();
                                txtBarcode.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar o livro: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Métodos de Bibliotecárias
        private void CarregarBibliotecarias()
        {
            cbBibliotecaria.Items.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sql = "SELECT Nome FROM Usuarios WHERE TipoUsuario = 'Bibliotecário(a)'";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nome = reader.GetString(0);
                            cbBibliotecaria.Items.Add(nome);
                        }
                    }
                }

                if (cbBibliotecaria.Items.Count == 0)
                {
                    MessageBox.Show("Nenhuma bibliotecária encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar bibliotecárias:\n" + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Métodos de Navegação e Data
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            // Se alguma lista estiver visível, confirma a seleção correspondente
            if (lstSugestoesUsuario.Visible || lstLivros.Visible)
            {
                e.SuppressKeyPress = true;

                if (lstSugestoesUsuario.Focused || (txtNomeUsuario.Focused && lstSugestoesUsuario.Visible))
                    if (ConfirmarSugestaoUsuario()) return;

                if (lstLivros.Focused || (txtLivro.Focused && lstLivros.Visible))
                    if (ConfirmarSugestaoLivro()) return;

                // Fallback na ordem
                if (ConfirmarSugestaoUsuario()) return;
                if (ConfirmarSugestaoLivro()) return;

                return;
            }

            // Fluxo normal do Enter
            e.SuppressKeyPress = true;
            this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
        }

        private void txtBarcode_KeyPressLimiter(object sender, KeyPressEventArgs e)
        {
            // Bloqueia entrada quando atingir o limite (permitindo teclas de controle e substituição de seleção)
            if (!char.IsControl(e.KeyChar))
            {
                int textoAtual = txtBarcode.Text?.Length ?? 0;
                int selecao = txtBarcode.SelectionLength;
                int novoTamanho = textoAtual - selecao + 1; // +1 pelo novo char
                if (novoTamanho > LIMITE_CODIGO_BARRAS)
                    e.Handled = true;
            }
        }

        private void txtBarcode_TextChangedLimiter(object sender, EventArgs e)
        {
            // Trunca conteúdo excedente (cobre colagens, entrada do leitor, etc.)
            var texto = txtBarcode.Text ?? string.Empty;
            if (texto.Length > LIMITE_CODIGO_BARRAS)
            {
                int caret = txtBarcode.SelectionStart;
                txtBarcode.Text = texto.Substring(0, LIMITE_CODIGO_BARRAS);
                txtBarcode.SelectionStart = Math.Min(caret, LIMITE_CODIGO_BARRAS);
            }
        }

        private void chkDevolucaoPersonalizada_CheckedChanged(object sender, EventArgs e)
        {
            dtpDataDevolucao.Enabled = chkDevolucaoPersonalizada.Checked;

            if (!chkDevolucaoPersonalizada.Checked)
                dtpDataDevolucao.Value = DateTime.Today.AddDays(7);
        }

        public void LimparCampos()
        {
            txtNomeUsuario.Text = "";
            txtLivro.Text = "";
            txtBarcode.Text = "";
            cbBibliotecaria.Text = "";
            dtpDataDevolucao.Value = DateTime.Today.AddDays(7);
            chkDevolucaoPersonalizada.Checked = false;

        }
        #endregion

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



        private bool ValidarDatas()
        {
            if (dtpDataDevolucao.Value < dtpDataEmprestimo.Value)
            {
                MessageBox.Show("A data de devolução não pode ser anterior à data de empréstimo.",
                              "Data Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDataDevolucao.Focus();
                return false;
            }
            return true;
        }

        private void dtpDataDevolucao_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDataDevolucao.Value < dtpDataEmprestimo.Value)
            {
                MessageBox.Show("A data de devolução não pode ser anterior à data de empréstimo.",
                              "Data Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpDataDevolucao.Value = dtpDataEmprestimo.Value.AddDays(7);
            }
        }

        private void dtpDataEmprestimo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDataDevolucao.Value < dtpDataEmprestimo.Value)
            {
                // Ajusta automaticamente a data de devolução
                dtpDataDevolucao.Value = dtpDataEmprestimo.Value.AddDays(7);
            }
        }

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

        private bool ConfirmarSugestaoLivro()
        {
            if (!lstLivros.Visible || lstLivros.Items.Count == 0)
                return false;

            if (lstLivros.SelectedIndex < 0)
                lstLivros.SelectedIndex = 0;

            SelecionarLivro(lstLivros.SelectedIndex);
            txtLivro.Focus();
            this.SelectNextControl(txtLivro, true, true, true, true);
            return true;
        }
        
        // Flags para diferenciar alterações programáticas x usuário
private bool _alterandoTxtLivroProgramaticamente = false;
private bool _preenchendoPorBarcode = false;
private string _nomePreenchidoPorBarcode = null;

// Helper para definir texto do livro de forma programática
private void SetLivroTextProgrammatic(string value, bool origemBarcode)
{
    _alterandoTxtLivroProgramaticamente = true;

    if (origemBarcode)
    {
        _preenchendoPorBarcode = true;     // sinaliza que veio do scanner
        _nomePreenchidoPorBarcode = value; // guarda o nome encontrado pelo código
    }

    txtLivro.Text = value;

    _alterandoTxtLivroProgramaticamente = false;
}
    }
}