using BibliotecaApp.Forms.Utils;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaApp.Utils;
using System.Net.NetworkInformation;

namespace BibliotecaApp.Forms.Login
{
    public partial class LoginForm : Form
    {
        #region Propriedades
        // Variável para controle externo de login
        public static bool cancelar = false;

        // Indica se o login atual foi realizado por uma bibliotecária
        public static bool UsuarioBibliotecaria { get; set; } = false;

        // Evita reentrância no clique (duplo Enter/click)
        private bool _isAuthenticating = false;
        #endregion

        #region Construtor
        public LoginForm()
        {
            InitializeComponent();

            // AcceptButton só será ativado quando o foco estiver em txtSenha
            this.AcceptButton = null;

            // Ativa/desativa AcceptButton conforme foco na senha
            this.txtSenha.Enter += txtSenha_Enter;
            this.txtSenha.Leave += txtSenha_Leave;

            txtEmail.CharacterCasing = CharacterCasing.Lower;

            // Removido: KeyDown já está inscrito no Designer
            // txtEmail.KeyDown += txtEmail_KeyDown;
            // txtSenha.KeyDown += txtSenha_KeyDown;
        }

        //Fecharo form se o login for cancelado externamente
        private void LoginForm_Load(object sender, EventArgs e)
        {
            AppPaths.EnsureFolders();
            
        }
        #endregion

        #region Eventos de Saída
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

        private void picExit_MouseEnter(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Gainsboro;
        }

        private void picExit_MouseLeave(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Transparent;
        }
        #endregion

        #region Métodos de Login
        private async void BtnEntrar_Click(object sender, EventArgs e)
        {
            if (_isAuthenticating) return;
            _isAuthenticating = true;
            if (BtnEntrar != null) BtnEntrar.Enabled = false;

            try
            {
                string email = txtEmail.Text.Trim().ToLowerInvariant();
                string senha = txtSenha.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
                {
                    MessageBox.Show("Por favor, preencha todos os campos.", "Campos obrigatórios",
                                  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    if (string.IsNullOrEmpty(email)) txtEmail.Focus();
                    else txtSenha.Focus();
                    return;
                }

                try
                {
                    using (SqlCeConnection conexao = Conexao.ObterConexao())
                    {
                        conexao.Open();

                        // 1) Tenta login como ADMIN (credenciais simples via banco)
                        if (LoginAdmin(conexao, email, senha))
                        {
                            Sessao.NomeBibliotecariaLogada = "Administrador";
                            LoginForm.UsuarioBibliotecaria = false;
                            cancelar = true;
                            await AtualizarStatusEmprestimosAsync(false, Program.IsOfflineMode);

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            return;
                        }

                        // 2) Tenta login como Bibliotecário(a)
                        string query = @"SELECT Nome, Senha_hash, Senha_salt FROM usuarios 
                    WHERE Email = @email AND TipoUsuario = 'Bibliotecário(a)'";

                        using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                        {
                            comando.Parameters.AddWithValue("@email", email);

                            using (SqlCeDataReader reader = comando.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string hashSalvo = reader["Senha_hash"].ToString();
                                    string saltSalvo = reader["Senha_salt"].ToString();
                                    string nomeUsuario = reader["Nome"].ToString();

                                    bool senhaCorreta = CriptografiaSenha.VerificarSenha(senha, hashSalvo, saltSalvo);

                                    if (senhaCorreta)
                                    {
                                        // --- INÍCIO DA VERIFICAÇÃO DE INTERNET ---
                                        bool internetOk = VerificarConexaoInternet();
                                        Program.IsOfflineMode = false; // Assume online por padrão

                                        while (!internetOk)
                                        {
                                            string mensagemOffline =
                                                "Não foi possível conectar à internet.\n\n" +
                                                "No MODO OFFLINE, as seguintes funções estarão INDISPONÍVEIS:\n" +
                                                "• Backup automático na nuvem\n" +
                                                "• Envio de notificações por e-mail\n" +
                                                "• Envio do relatório semanal\n" +
                                                "• Consultas de livros online (API)\n\n" +
                                                "Deseja tentar conectar novamente ou continuar offline?";

                                            var result = MessageBox.Show(mensagemOffline,
                                                "Sem Conexão com Internet",
                                                MessageBoxButtons.RetryCancel,
                                                MessageBoxIcon.Warning);

                                            if (result == DialogResult.Retry)
                                            {
                                                // Tenta verificar novamente
                                                internetOk = VerificarConexaoInternet();
                                                if (internetOk)
                                                {
                                                    MessageBox.Show("Conexão restabelecida com sucesso!", "Conectado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    Program.IsOfflineMode = false;
                                                }
                                            }
                                            else // Cancel (Entendido como Continuar Offline)
                                            {
                                                Program.IsOfflineMode = true;
                                                break; // Sai do loop e prossegue offline
                                            }
                                        }
                                        // --- FIM DA VERIFICAÇÃO ---

                                        Sessao.NomeBibliotecariaLogada = nomeUsuario;
                                        LoginForm.UsuarioBibliotecaria = true;

                                        // Passamos o status offline para o método de atualização
                                        await AtualizarStatusEmprestimosAsync(true, Program.IsOfflineMode);

                                        cancelar = true;
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                        return;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Senha incorreta.", "Erro de autenticação",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtSenha.Focus();
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("E-mail não encontrado ou usuário sem permissão.", "Erro de autenticação",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtEmail.Focus();
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro na autenticação: " + ex.Message, "Erro",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                _isAuthenticating = false;
                if (!IsDisposed && BtnEntrar != null) BtnEntrar.Enabled = true;
            }
        }

        private bool LoginAdmin(SqlCeConnection conexao, string email, string senha)
        {
            // Tabela 'Admin' deve conter apenas 1 linha (um único admin)
            const string sql = @"SELECT COUNT(*) FROM Admin 
                                 WHERE Ativo = 1 AND Email = @email AND Senha = @senha";

            using (var cmd = new SqlCeCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@senha", senha);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private void BtnEntrar_MouseLeave(object sender, EventArgs e)
        {
            BtnEntrar.BackColor = Color.FromArgb(9, 74, 158);
            BtnEntrar.Refresh();
        }

        private void BtnEntrar_MouseEnter(object sender, EventArgs e)
        {
            BtnEntrar.BackColor = Color.FromArgb(33, 145, 245);
            BtnEntrar.Refresh();
        }
        #endregion

        #region Métodos de Atualização
        private async Task AtualizarStatusEmprestimosAsync(bool usuarioEhBibliotecaria, bool isOffline)
        {
            using (var progressForm = new frmProgresso())
            {
                progressForm.Show();

                await Task.Run(() =>
                {
                    // Se estiver offline, passamos essa info para dentro dos métodos ou pulamos a execução
                    AtualizarEmprestimos(progressForm, isOffline);

                    if (isOffline)
                    {
                        progressForm.AtualizarProgresso(50, "Modo Offline: Pulando notificações e relatórios...");
                        System.Threading.Thread.Sleep(1000); // Pequena pausa para o usuário ler
                    }
                    else
                    {
                        // Só executa se estiver online
                        AtualizarNotificacoesDisponibilidade(progressForm);
                    }

                    // ---- Envio Semanal Relatorio ----
                    try
                    {
                        // Se não for bibliotecária OU se estiver Offline, aborta o relatório
                        if (!usuarioEhBibliotecaria)
                        {
                            progressForm.AtualizarProgresso(100, "Atualizações concluídas.");
                            return;
                        }

                        if (isOffline)
                        {
                            progressForm.AtualizarProgresso(100, "Modo Offline: Relatório semanal não enviado.");
                            return;
                        }

                        if (!ControleSemanal.JaEnviadoEstaSemana())
                        {
                            progressForm.AtualizarProgresso(90, "Gerando relatório semanal...");

                            using (var conexao = Conexao.ObterConexao())
                            {
                                conexao.Open();
                                string pdfPath = GerarRelatorioAtrasados(conexao);

                                if (string.IsNullOrEmpty(pdfPath))
                                {
                                    // Falha ao gerar PDF; já foi logado em GerarRelatorioAtrasados
                                    progressForm.AtualizarProgresso(100, "Falha ao gerar relatório semanal.");
                                }
                                else
                                {
                                    try
                                    {
                                        progressForm.AtualizarProgresso(95, "Enviando relatório para secretaria...");

                                        string assunto = $"📑 Relatório semanal de alunos não aptos - {DateTime.Now:dd/MM/yyyy}";
                                        string corpo = $@"
<html>
<body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
        <h2 style='color: #2c3e50;'>📚 Biblioteca Monteiro Lobato</h2>
        <p>Prezada secretaria,</p>
        <p>Segue em anexo o relatório semanal de alunos que <strong>não estão aptos</strong> a retirar documentos,
           devido a <span style='color:#d35400; font-weight:bold;'>empréstimos em atraso</span>.</p>
        <p style='font-size: 16px;'><strong>📅 Data do relatório:</strong> {DateTime.Now:dd/MM/yyyy}</p>
        <p>O PDF anexo contém a lista de alunos e suas respectivas turmas.</p>
        <p style='margin-top:20px;'>Atenciosamente,<br/><strong>Sistema da Biblioteca</strong></p>
        <hr />
        <p style='font-size: 13px; color: #888;'>Este é um e-mail automático. Não responda a esta mensagem.</p>
    </div>
</body>
</html>";

                                        // Chamada de envio com anexo (ajuste se sua assinatura for diferente)
                                        BibliotecaApp.Services.EmailService.Enviar(
                                            "secretaria.79448@gmail.com",
                                            assunto,
                                            corpo,
                                            pdfPath
                                        );

                                        // Registrar somente após envio sem exceção
                                        ControleSemanal.RegistrarEnvio();
                                        progressForm.AtualizarProgresso(100, "Relatório semanal enviado com sucesso!");
                                        LogRelatorio($"Relatório semanal enviado com sucesso. Arquivo: {pdfPath}");
                                    }
                                    catch (Exception exEnvio)
                                    {
                                        // Falha no envio por e-mail
                                        LogRelatorio($"Falha ao enviar relatório semanal: {exEnvio}");
                                        progressForm.AtualizarProgresso(100, "Falha ao enviar relatório semanal.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            progressForm.AtualizarProgresso(100, "Relatório semanal já enviado nesta semana.");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogRelatorio($"Erro no processo de envio do relatório semanal: {ex}");
                        progressForm.AtualizarProgresso(100, "Erro ao processar relatório.");
                    }
                });
            }
        }




        // ---- PDF Relatorio Gerador ----
        private string GerarRelatorioAtrasados(SqlCeConnection conexao)
        {
            try
            {
                // Garante a pasta AppData/Relatorios
                string relatoriosFolder = Path.Combine(AppPaths.AppDataFolder, "Relatorios");
                if (!Directory.Exists(relatoriosFolder))
                    Directory.CreateDirectory(relatoriosFolder);

                string friendlyTimestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string caminho = Path.Combine(relatoriosFolder, $"Relatorio_Atrasados_{friendlyTimestamp}.pdf");

                // Consulta: apenas usuários do tipo 'Aluno(a)', sem duplicatas, tratando Turma nula e ordenando
                string sql = @"
            SELECT DISTINCT u.Nome, 
                   CASE WHEN u.Turma IS NULL THEN '' ELSE u.Turma END AS Turma
            FROM Usuarios u
            WHERE LOWER(LTRIM(RTRIM(u.TipoUsuario))) = 'aluno(a)'
              AND EXISTS (
                  SELECT 1 FROM Emprestimo e
                  WHERE e.Alocador = u.Id AND e.Status = 'Atrasado'
              )
            ORDER BY Turma, Nome";

                var resultados = new List<(string Nome, string Turma)>();

                using (var cmd = new SqlCeCommand(sql, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nome = reader["Nome"] == DBNull.Value ? string.Empty : reader["Nome"].ToString();
                        string turma = reader["Turma"] == DBNull.Value ? string.Empty : reader["Turma"].ToString();
                        resultados.Add((nome, turma));
                    }
                }

                // Cria o PDF (mesmo que não haja resultados - geramos um PDF com mensagem)
                using (var fs = new FileStream(caminho, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var doc = new Document(PageSize.A4, 40, 40, 40, 40);
                    PdfWriter.GetInstance(doc, fs);
                    doc.AddAuthor("Sistema da Biblioteca");
                    doc.AddCreationDate();
                    doc.AddTitle("Relatório de Alunos NÃO Aptos a Retirar Documentos");
                    doc.Open();

                    // Título
                    var titulo = new iTextSharp.text.Paragraph(
                        "Relatório de Alunos NÃO Aptos a Retirar Documentos\n\n",
                        new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18f, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY)
                    )
                    { Alignment = Element.ALIGN_CENTER };
                    doc.Add(titulo);

                    // Data
                    var data = new iTextSharp.text.Paragraph(
                        $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n",
                        new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.ITALIC, new BaseColor(128, 128, 128))
                    )
                    { Alignment = Element.ALIGN_RIGHT };
                    doc.Add(data);

                    if (resultados.Count == 0)
                    {
                        var p = new iTextSharp.text.Paragraph(
                            "Nenhum aluno com empréstimos atrasados encontrado no momento.",
                            new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)
                        );
                        doc.Add(p);
                    }
                    else
                    {
                        // Tabela
                        PdfPTable tabela = new PdfPTable(2) { WidthPercentage = 100 };
                        tabela.SetWidths(new float[] { 3, 2 });

                        string[] headers = { "Nome", "Turma" };
                        foreach (var h in headers)
                        {
                            var cell = new PdfPCell(new Phrase(h, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE)))
                            {
                                BackgroundColor = new BaseColor(30, 61, 88),
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            };
                            tabela.AddCell(cell);
                        }

                        foreach (var item in resultados)
                        {
                            tabela.AddCell(new PdfPCell(new Phrase(item.Nome)) { Padding = 4 });
                            tabela.AddCell(new PdfPCell(new Phrase(item.Turma)) { Padding = 4 });
                        }

                        doc.Add(tabela);
                    }

                    doc.Close();
                }

                return caminho;
            }
            catch (Exception ex)
            {
                // Log e retorno nulo (para o chamador decidir o comportamento)
                LogRelatorio($"Erro ao gerar PDF do relatório: {ex}");
                return null;
            }
        }



        private void AtualizarNotificacoesDisponibilidade(frmProgresso progressForm)
        {
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"
                SELECT n.Id, n.Email, u.Nome, l.Nome
                FROM NotificacoesDisponibilidade n
                INNER JOIN Usuarios u ON n.UsuarioId = u.Id
                INNER JOIN Livros l ON n.LivroId = l.Id
                WHERE n.Enviado = 0 AND l.Disponibilidade = 1";

                    var notificacoes = new List<dynamic>();
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            notificacoes.Add(new
                            {
                                Id = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                NomeUsuario = reader.GetString(2),
                                NomeLivro = reader.GetString(3)
                            });
                        }
                    }

                    int total = notificacoes.Count;
                    int enviadas = 0;

                    progressForm.AtualizarProgresso(10, "Processando notificações de disponibilidade...");

                    foreach (var n in notificacoes)
                    {
                        try
                        {
                            string assunto = "📚 Notificação de disponibilidade - Biblioteca Monteiro Lobato";
string corpo = $@"
<html>
<body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
        <h2 style='color: #2c3e50;'>Olá, {n.NomeUsuario} 👋</h2>
        <p>O livro <strong>{n.NomeLivro}</strong> que você solicitou está disponível para empréstimo!</p>
        <p>Compareça à biblioteca para retirar seu exemplar.</p>
        <hr />
        <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
    </div>
</body>
</html>";
                            BibliotecaApp.Services.EmailService.Enviar(n.Email, assunto, corpo);

                            // Marca como enviado
                            string sqlUpdate = "UPDATE NotificacoesDisponibilidade SET Enviado = 1 WHERE Id = @id";
                            using (var cmdUpdate = new SqlCeCommand(sqlUpdate, conexao))
                            {
                                cmdUpdate.Parameters.AddWithValue("@id", n.Id);
                                cmdUpdate.ExecuteNonQuery();
                            }
                            enviadas++;
                            int progresso = 10 + (int)((double)enviadas / total * 80);
                            progressForm.AtualizarProgresso(progresso, $"Enviando notificações ({enviadas}/{total})...");
                        }
                        catch
                        {
                            // Se falhar, apenas continua
                        }
                    }

                    progressForm.AtualizarProgresso(100, "Notificações de disponibilidade atualizadas!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar notificações de disponibilidade: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarEmprestimos(frmProgresso progressForm, bool isOffline)
        {
            try
            {
                using (var connection = Conexao.ObterConexao())
                {
                    connection.Open();

                    string countQuery = "SELECT COUNT(*) FROM Emprestimo WHERE Status <> 'Devolvido'";
                    var countCommand = new SqlCeCommand(countQuery, connection);
                    int totalEmprestimos = (int)countCommand.ExecuteScalar();

                    if (totalEmprestimos == 0)
                    {
                        progressForm.AtualizarProgresso(100, "Nenhum empréstimo para atualizar");
                        return;
                    }

                    string selectQuery = @"
                    SELECT 
                        e.Id,
                        e.DataDevolucao,
                        e.DataProrrogacao,
                        e.DataRealDevolucao,
                        e.NotificadoLembrete,
                        e.NotificadoAtraso,
                        u.Email,
                        u.Nome,
                        l.Nome
                    FROM Emprestimo e
                    INNER JOIN Usuarios u ON e.Alocador = u.Id
                    INNER JOIN Livros l ON e.Livro = l.Id
                    WHERE e.Status <> 'Devolvido'";

                    var selectCommand = new SqlCeCommand(selectQuery, connection);
                    var reader = selectCommand.ExecuteReader();

                    int processados = 0;

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime dataDevolucao = reader.GetDateTime(1);
                        DateTime? dataProrrogacao = reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2);
                        DateTime? dataRealDevolucao = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);

                        bool notificadoLembrete = !reader.IsDBNull(4) && reader.GetBoolean(4);
                        bool notificadoAtraso = !reader.IsDBNull(5) && reader.GetBoolean(5);

                        string emailUsuario = reader.IsDBNull(6) ? null : reader.GetString(6);
                        string nomeUsuario = reader.GetString(7);
                        string tituloLivro = reader.GetString(8);

                        string novoStatus = CalcularStatus(dataDevolucao, dataProrrogacao, dataRealDevolucao);

                        string updateStatusQuery = "UPDATE Emprestimo SET Status = @Status WHERE Id = @Id";
                        var updateStatusCommand = new SqlCeCommand(updateStatusQuery, connection);
                        updateStatusCommand.Parameters.AddWithValue("@Status", novoStatus);
                        updateStatusCommand.Parameters.AddWithValue("@Id", id);
                        updateStatusCommand.ExecuteNonQuery();

                        DateTime dataReferencia = dataProrrogacao ?? dataDevolucao;
                        TimeSpan diferenca = dataReferencia.Date - DateTime.Now.Date;

                        if (diferenca.Days == 3 && !notificadoLembrete)
                        {
                            // Só envia se NÃO estiver offline
                            if (!isOffline && !string.IsNullOrWhiteSpace(emailUsuario))
                            {
                                EnviarEmailLembrete(emailUsuario, nomeUsuario, tituloLivro, dataReferencia);
                            }

                            // Opcional: Se estiver offline, você pode optar por NÃO marcar como notificado 
                            // para tentar enviar amanhã, ou marcar mesmo assim.
                            // Abaixo, marcamos apenas se conseguimos enviar (se online) OU se preferir não acumular, marque sempre.
                            // Recomendação: Marcar apenas se enviou.

                            if (!isOffline && !string.IsNullOrWhiteSpace(emailUsuario))
                            {
                                var updateFlagCmd = new SqlCeCommand("UPDATE Emprestimo SET NotificadoLembrete = 1 WHERE Id = @Id", connection);
                                updateFlagCmd.Parameters.AddWithValue("@Id", id);
                                updateFlagCmd.ExecuteNonQuery();
                            }
                        }

                        if (diferenca.Days < 0 && !notificadoAtraso)
                        {
                            if (!isOffline && !string.IsNullOrWhiteSpace(emailUsuario))
                            {
                                EnviarEmailAtraso(emailUsuario, nomeUsuario, tituloLivro, dataReferencia);

                                var updateFlagCmd = new SqlCeCommand("UPDATE Emprestimo SET NotificadoAtraso = 1 WHERE Id = @Id", connection);
                                updateFlagCmd.Parameters.AddWithValue("@Id", id);
                                updateFlagCmd.ExecuteNonQuery();
                            }
                        }

                        processados++;
                        int progresso = (int)((double)processados / totalEmprestimos * 100);
                        progressForm.AtualizarProgresso(progresso, $"Atualizando empréstimo {processados} de {totalEmprestimos}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar empréstimos: {ex.Message}", "Erro",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string CalcularStatus(DateTime dataDevolucao, DateTime? dataProrrogacao, DateTime? dataRealDevolucao)
        {
            DateTime dataReferencia = dataProrrogacao ?? dataDevolucao;

            if (dataRealDevolucao.HasValue)
            {
                return "Devolvido";
            }
            else if (DateTime.Now > dataReferencia)
            {
                return "Atrasado";
            }
            else
            {
                return "Ativo";
            }
        }
        #endregion

        #region Métodos de Email
        private void EnviarEmailLembrete(string email, string nome, string tituloLivro, DateTime dataDevolucao)
        {
            string assunto = "📚 Lembrete: Devolução de livro se aproxima!";

            string corpo = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333; background-color: #f9f9f9; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #ddd; border-radius: 8px; padding: 20px;'>
                        <h2 style='color: #2c3e50;'>Olá, {nome} 👋</h2>
                        <p>Este é um lembrete de que o livro <strong>{tituloLivro}</strong> precisa ser devolvido em breve.</p>
                        <p style='font-size: 16px;'><strong>📅 Data limite de devolução:</strong> {dataDevolucao:dd/MM/yyyy}</p>
                        <p>Por favor, devolva o livro no prazo para evitar bloqueios no sistema e problemas com a secretaria.</p>
                        <hr />
                        <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
                    </div>
                </body>
                </html>";

            BibliotecaApp.Services.EmailService.Enviar(email, assunto, corpo);
        }

        private void EnviarEmailAtraso(string email, string nome, string tituloLivro, DateTime dataDevolucao)
        {
            string assunto = "⚠️ Atraso na devolução de livro";

            string corpo = $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333; background-color: #fffbe6; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background-color: #fff; border: 1px solid #e1a500; border-radius: 8px; padding: 20px;'>
                        <h2 style='color: #d35400;'>Atenção, {nome} ⚠️</h2>
                        <p>O prazo para devolução do livro <strong>{tituloLivro}</strong> venceu em <strong>{dataDevolucao:dd/MM/yyyy}</strong>.</p>
                        <p>Devido a isso, você <strong>não poderá retirar documentos na secretaria</strong> até regularizar a situação.</p>
                        <p>Pedimos que devolva o material o quanto antes ou entre em contato com a biblioteca.</p>
                        <hr />
                        <p style='font-size: 14px; color: #888;'>Este é um e-mail automático enviado pela Biblioteca Monteiro Lobato.</p>
                    </div>
                </body>
                </html>";

            BibliotecaApp.Services.EmailService.Enviar(email, assunto, corpo);
        }



        #endregion

        #region Eventos de Teclado
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Impede o AcceptButton e vai para a senha
                e.Handled = true;
                e.SuppressKeyPress = true;
                txtSenha.Focus();
            }
        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Deixe o AcceptButton (BtnEntrar) cuidar do Enter no campo senha
            }
        }

        // Ativa o AcceptButton apenas quando a senha tem foco
        private void txtSenha_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = BtnEntrar;
        }

        private void txtSenha_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }
        #endregion

        #region Eventos de Interface
        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Implementação do paint do gradientPanel
        }

        private void gradientPanel1_Paint_1(object sender, PaintEventArgs e)
        {
            // Implementação alternativa do paint do gradientPanel
        }
        #endregion

        #region ControleSemanal (TXT)
        public static class ControleSemanal
        {
            private static readonly string txtPath = Path.Combine(AppPaths.AppDataFolder, "EnvioRelatorioSemanal.txt");
            private const string DateFormat = "yyyy-MM-dd";

            public static bool JaEnviadoEstaSemana()
            {
                try
                {
                    if (!File.Exists(txtPath)) return false;

                    string conteudo = File.ReadAllText(txtPath).Trim();
                    if (DateTime.TryParseExact(conteudo, DateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime ultimaData))
                    {
                        var diff = (DateTime.Now - ultimaData).TotalDays;
                        return diff < 7; // menos de 7 dias = já enviou
                    }
                    // Se não conseguiu parse, considera não enviado e registra um log
                    LogRelatorio($"Formato de data de controle semanal inválido: '{conteudo}'");
                    return false;
                }
                catch (Exception ex)
                {
                    LogRelatorio($"Erro em JaEnviadoEstaSemana: {ex}");
                    return false;
                }
            }

            public static void RegistrarEnvio()
            {
                try
                {
                    File.WriteAllText(txtPath, DateTime.Now.ToString(DateFormat, System.Globalization.CultureInfo.InvariantCulture));
                }
                catch (Exception ex)
                {
                    LogRelatorio($"Erro ao registrar envio semanal: {ex}");
                }
            }
        }

        private static void LogRelatorio(string mensagem)
        {
            try
            {
                string logPath = Path.Combine(AppPaths.AppDataFolder, "Relatorio_Log.txt");
                string texto = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensagem}{Environment.NewLine}";
                File.AppendAllText(logPath, texto);
            }
            catch
            {
                // Se o log falhar, não interrompe a aplicação.
            }
        }


        #endregion

        private void lblEsqueceuSenha_Click(object sender, EventArgs e)
        {
            this.Hide(); // Esconde o formulário atual

            using (EsqueceuSenhaForm popup = new EsqueceuSenhaForm())
            {
                popup.ShowDialog(); // Abre como modal
            }

            this.Show(); // Reexibe o formulário anterior após o fechamento do modal
        }

        private void lblVersion_Click(object sender, EventArgs e)
        {
            this.Hide();

            using (AboutForm popup = new AboutForm())
            {
                popup.ShowDialog(); // Abre como modal
            }

            this.Show();
        }

        private void lblVersion_MouseEnter(object sender, EventArgs e)
        {
            lblVersion.ForeColor = Color.SkyBlue;
        }

        private void lblVersion_MouseLeave(object sender, EventArgs e)
        {
            lblVersion.ForeColor = Color.White;

        }


        private bool VerificarConexaoInternet()
        {
            try
            {
                // Tenta pingar o DNS do Google (8.8.8.8) para garantir saída para internet
                using (var ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8", 2000); // 2 segundos de timeout
                    return reply != null && reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}