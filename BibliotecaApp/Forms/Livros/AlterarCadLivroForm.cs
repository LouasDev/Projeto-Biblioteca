using BibliotecaApp.Forms.Usuario;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using BibliotecaApp.Utils.Etiquetas; // + Etiquetas
using System;
using System.Collections.Generic;
using System.ComponentModel; // MaskedTextProvider
using System.Data.SqlServerCe;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace BibliotecaApp
{
    public partial class AlterarCadLivroForm : Form
    {
        #region Estado / Campos
        private int livroId;
        private IButtonControl _acceptBackup, _cancelBackup;

        private static bool IsAdminLogado()
=> string.Equals(Sessao.NomeBibliotecariaLogada, "Administrador", StringComparison.OrdinalIgnoreCase);

        // Flags de controle do autocomplete
        private bool _suppressGeneroSuggest = false;   // evita loop ao setar Text programaticamente
        private bool _isClickingSugestoes = false;     // zona segura ao clicar na lista
        #endregion

        #region Evento público
        public event EventHandler LivroAtualizado;
        #endregion

        #region Construtor / Init
        public AlterarCadLivroForm()
        {
            InitializeComponent();

            this.KeyPreview = true;

            if (lstSugestoesGenero != null)
            {
                lstSugestoesGenero.Visible = false;
                lstSugestoesGenero.TabStop = false;
            }

            txtGenero.Enter += txtGenero_Enter;
            txtGenero.MouseDown += txtGenero_MouseDown;
            txtGenero.TextChanged += txtGenero_TextChanged;
            txtGenero.KeyUp += txtGenero_KeyUp;
            txtGenero.KeyDown += txtGenero_KeyDown;
            txtGenero.Leave += txtGenero_Leave;

            lstSugestoesGenero.MouseDown += lstSugestoesGenero_MouseDown;
            lstSugestoesGenero.MouseUp += lstSugestoesGenero_MouseUp;
            lstSugestoesGenero.Click += lstSugestoesGenero_Click;
            lstSugestoesGenero.KeyDown += lstSugestoesGenero_KeyDown;
            lstSugestoesGenero.Leave += lstSugestoesGenero_Leave;

            // EVITAR DUPLICIDADE: remove se já estiver assinado (Designer) e adiciona apenas uma vez.
            if (btnGerarEtiqueta != null)
            {
                btnGerarEtiqueta.Click -= btnGerarEtiqueta_Click;
                btnGerarEtiqueta.Click += btnGerarEtiqueta_Click;
            }
        }
        #endregion

        #region Teclado Global
        // Enter/Tab/Down/Escape priorizam a lista quando visível
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (lstSugestoesGenero != null && lstSugestoesGenero.Visible)
            {
                if (keyData == Keys.Enter || keyData == Keys.Tab)
                {
                    if (ConfirmarSugestaoGenero())
                        return true; // consome a tecla
                }
                else if (keyData == Keys.Down)
                {
                    if (!lstSugestoesGenero.Focused)
                    {
                        lstSugestoesGenero.Focus();
                        if (lstSugestoesGenero.Items.Count > 0 && lstSugestoesGenero.SelectedIndex < 0)
                            lstSugestoesGenero.SelectedIndex = 0;
                        return true;
                    }
                }
                else if (keyData == Keys.Escape)
                {
                    EsconderSugestoes();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Carregar Livro
        public void PreencherLivro(Livro livro)
        {
            // Evita disparar sugestões por alteração programática
            _suppressGeneroSuggest = true;

            livroId = livro.Id;
            txtNome.Text = livro.Nome;
            txtAutor.Text = livro.Autor;
            txtGenero.Text = livro.Genero;
            txtQuantidade.Text = livro.Quantidade.ToString();

            // Mostra no UI exatamente o que veio do BD (com/sem máscara)
            mtxCodigoBarras.Text = livro.CodigoDeBarras;

            EsconderSugestoes();

            _suppressGeneroSuggest = false;
        }
        #endregion

        #region Persistência
        private string ObterSenha(string titulo, string mensagem)
        {
            using (var f = new PasswordForm())
            {
                f.Titulo = titulo;
                f.Mensagem = mensagem;
                return f.ShowDialog() == DialogResult.OK ? f.SenhaDigitada : null;
            }
        }

        // === NOVO: Captura com MÁSCARA, mesmo sem TextMaskFormat, usando MaskedTextProvider ===
        private string ObterCodigoDeBarrasFormatado()
        {
            return new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray());
        }



        private bool VerificarSenhaBibliotecaria(string senha)
        {
            string nome = Sessao.NomeBibliotecariaLogada;
            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Nenhum bibliotecário está logado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                using (var cx = Conexao.ObterConexao())
                {
                    cx.Open();
                    using (var cmd = new SqlCeCommand(
                        @"SELECT Senha_hash, Senha_salt FROM usuarios 
                          WHERE Nome = @n AND TipoUsuario LIKE '%Bibliotec%'", cx))
                    {
                        cmd.Parameters.AddWithValue("@n", nome);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                return CriptografiaSenha.VerificarSenha(
                                    senha, r["Senha_hash"].ToString(), r["Senha_salt"].ToString());
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

        private void btnSalvar_Click(object sender, EventArgs e)
        {

            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode editar livros.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string nome = txtNome.Text.Trim();
            string autor = txtAutor.Text.Trim();
            string genero = txtGenero.Text.Trim();
            int quantidade = int.Parse(txtQuantidade.Text.Trim());

            // >>> Salvar COM máscara, sempre que possível
            string codigoBarras = ObterCodigoDeBarrasFormatado();

            using (var conn = Conexao.ObterConexao())
            {
                conn.Open();
                // FIX: recolocar cbA na declaração
                string nA = "", aA = "", gA = "", cbA = ""; int qA = 0; bool mudou = false;

                using (var cmdL = new SqlCeCommand(
                    "SELECT Nome, Autor, Genero, Quantidade, CodigoBarras FROM Livros WHERE Id=@id", conn))
                {
                    cmdL.Parameters.AddWithValue("@id", livroId);
                    using (var rd = cmdL.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            nA = rd.GetString(0); aA = rd.GetString(1); gA = rd.GetString(2);
                            qA = rd.GetInt32(3); cbA = rd.GetString(4);
                            mudou = nome != nA || autor != aA || genero != gA || quantidade != qA || codigoBarras != cbA;
                        }
                    }
                }

                if (!mudou) { MessageBox.Show("Nenhuma alteração foi feita."); return; }

                var msg = MontarMensagemConfirmacaoLivro(nA, aA, gA, qA, cbA, nome, autor, genero, quantidade, codigoBarras);
                if (MessageBox.Show(msg, "Confirmar Alterações", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                using (var cmd = new SqlCeCommand(
                    @"UPDATE Livros 
                      SET Nome=@n, Autor=@a, Genero=@g, Quantidade=@q, CodigoBarras=@c 
                      WHERE Id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@n", nome);
                    cmd.Parameters.AddWithValue("@a", autor);
                    cmd.Parameters.AddWithValue("@g", genero);
                    cmd.Parameters.AddWithValue("@q", quantidade);
                    cmd.Parameters.AddWithValue("@c", codigoBarras);
                    cmd.Parameters.AddWithValue("@id", livroId);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Livro atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LivroAtualizado?.Invoke(this, EventArgs.Empty);
                        EventosGlobais.OnLivroCadastradoOuAlterado();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma alteração foi feita.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode excluir livros.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // >>> Somente UMA senha
            var senha = ObterSenha("Confirmação de Senha", "Digite sua senha para confirmar a exclusão:");
            if (string.IsNullOrEmpty(senha)) { MessageBox.Show("Operação cancelada."); return; }
            if (!VerificarSenhaBibliotecaria(senha)) { MessageBox.Show("Senha incorreta."); return; }

            if (MessageBox.Show("Tem certeza que deseja excluir este livro?", "Confirmação",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                using (var conn = Conexao.ObterConexao())
                {
                    conn.Open();
                    using (var cmd = new SqlCeCommand("DELETE FROM Livros WHERE Id=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", livroId);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Livro excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LivroAtualizado?.Invoke(this, EventArgs.Empty);
                            EventosGlobais.OnLivroCadastradoOuAlterado();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Nenhum livro foi excluído.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir o livro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => this.Close();
        #endregion

        #region Etiquetas (BtnGerarEtiqueta)

        private string TentarObterImpressoraPdf()
        {
            try
            {
                foreach (string name in PrinterSettings.InstalledPrinters)
                {
                    if (name.IndexOf("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase) >= 0)
                        return name;
                }
            }
            catch
            {
                // ignore
            }
            return null;
        }

        private void btnGerarEtiqueta_Click(object sender, EventArgs e)
        {
            try
            {
                var titulo = (txtNome.Text ?? "").Trim();
                var genero = (txtGenero.Text ?? "").Trim();
                var codigo = ObterCodigoDeBarrasFormatado();

                if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(codigo))
                {
                    MessageBox.Show("Preencha o Nome do Livro e o Código de Barras antes de gerar etiquetas.",
                        "Dados incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse((txtQuantidade.Text ?? "0").Trim(), out int maxQtd)) maxQtd = 0;
                if (maxQtd <= 0) maxQtd = 1;

                while (true)
                {
                    using (var act = new BibliotecaApp.Utils.Etiquetas.LabelActionDialog(titulo, maxQtd))
                    {
                        // Aqui, em AlterarCadLivroForm, mantemos a seleção de quantidade visível (padrão)

                        if (act.ShowDialog(this) != DialogResult.OK || act.Choice == LabelActionChoice.None)
                            return;

                        int qtd = Math.Max(1, Math.Min(act.Quantity, maxQtd));

                        var items = Enumerable.Range(0, qtd)
                            .Select(_ => new EtiquetaLabelItem
                            {
                                CodigoBarras = codigo,
                                Titulo = titulo,
                                Genero = genero
                            })
                            .ToList();

                        if (act.Choice == LabelActionChoice.Pdf)
                        {
                            using (var sfd = new SaveFileDialog
                            {
                                Title = "Salvar etiquetas em PDF",
                                Filter = "PDF (*.pdf)|*.pdf",
                                FileName = $"{SanitizeFileName(titulo)} - {qtd} etiquetas.pdf",
                                OverwritePrompt = true,
                                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                            })
                            {
                                if (sfd.ShowDialog(this) != DialogResult.OK) 
                                    continue; // reabre o LabelActionDialog

                                string pdfPrinter = TentarObterImpressoraPdf();
                                if (string.IsNullOrWhiteSpace(pdfPrinter))
                                {
                                    MessageBox.Show("Impressora 'Microsoft Print to PDF' não encontrada no sistema. Instale-a para salvar em PDF.",
                                        "PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    continue; // reabre o LabelActionDialog
                                }

                                var options = new LabelPrintOptions
                                {
                                    StartPosition = 1,
                                    OffsetXmm = 0f,
                                    OffsetYmm = 0f,
                                    GroupByGenre = true,
                                    Preview = false,
                                    ShowPrintDialog = false,
                                    PrinterName = pdfPrinter,
                                    PrintToFile = true,
                                    OutputFilePath = sfd.FileName
                                };

                                LabelPrinterService.PrintLabels(items, options);

                                try
                                {
                                    if (File.Exists(sfd.FileName))
                                        System.Diagnostics.Process.Start(sfd.FileName);
                                }
                                catch { }
                                break; // finaliza fluxo após salvar
                            }
                        }
                        else // Imprimir
                        {
                            using (var pd = new PrintDialog { UseEXDialog = true })
                            using (var dummyDoc = new PrintDocument())
                            {
                                pd.Document = dummyDoc;

                                // Abre o PrintDialog; se cancelar, reabre o LabelActionDialog
                                if (pd.ShowDialog(this) != DialogResult.OK)
                                    continue;

                                var chosenPrinter = pd.PrinterSettings.PrinterName ?? string.Empty;

                                var options = new LabelPrintOptions
                                {
                                    StartPosition = 1,
                                    OffsetXmm = 0f,
                                    OffsetYmm = 0f,
                                    GroupByGenre = true,
                                    Preview = false,
                                    ShowPrintDialog = false, // já escolhemos a impressora
                                    PrinterName = chosenPrinter
                                };

                                // Se for Microsoft Print to PDF, perguntar nome/local e salvar direto
                                if (chosenPrinter.IndexOf("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    using (var sfd = new SaveFileDialog
                                    {
                                        Title = "Salvar etiquetas em PDF",
                                        Filter = "PDF (*.pdf)|*.pdf",
                                        FileName = $"{SanitizeFileName(titulo)} - {qtd} etiquetas.pdf",
                                        OverwritePrompt = true,
                                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                    })
                                    {
                                        if (sfd.ShowDialog(this) != DialogResult.OK)
                                            continue; // reabre o LabelActionDialog

                                        options.PrintToFile = true;
                                        options.OutputFilePath = sfd.FileName;
                                    }
                                }
                                else
                                {
                                    options.PrintToFile = false;
                                    options.OutputFilePath = null;
                                }

                                LabelPrinterService.PrintLabels(items, options);
                                break; // finaliza após imprimir
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar etiquetas: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string SanitizeFileName(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "Etiquetas";
            var invalid = Path.GetInvalidFileNameChars();
            var safe = new string(s.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray()).Trim();
            return string.IsNullOrWhiteSpace(safe) ? "Etiquetas" : safe;
        }

        private static string BuildUniquePath(string folder, string fileName)
        {
            try
            {
                Directory.CreateDirectory(folder);
            }
            catch { /* ignore */ }

            string path = Path.Combine(folder, fileName);
            if (!File.Exists(path)) return path;

            string name = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            int i = 2;
            while (File.Exists(path) && i < 1000)
            {
                path = Path.Combine(folder, $"{name} ({i}){ext}");
                i++;
            }
            return path;
        }
        #endregion

        #region Mensagem de Confirmação
        private string MontarMensagemConfirmacaoLivro(string nA, string aA, string gA, int qA, string cbA,
                                                      string nN, string aN, string gN, int qN, string cbN)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Confirme as alterações a serem salvas:\n");
            if (nA != nN) sb.AppendLine($"Nome: {nA} → {nN}");
            if (aA != aN) sb.AppendLine($"Autor: {aA} → {aN}");
            if (gA != gN) sb.AppendLine($"Gênero: {gA} → {gN}");
            if (qA != qN) sb.AppendLine($"Quantidade: {qA} → {qN}");
            if (cbA != cbN) sb.AppendLine($"Código de Barras: {cbA} → {cbN}");
            sb.AppendLine("\nDeseja salvar estas alterações?");
            return sb.ToString();
        }
        #endregion

        #region Autocomplete de Gênero
        private readonly List<string> generosPadronizados = new List<string>
        {
            "Poesia","Literatura de Cordel","Biografia","Autobiografia","Diálogo","Hábito","Psicologia",
            "Cultura Afro-brasileira","História","Teatro","Educação","Romance","Ficção","Fantasia",
            "Mitologia","Literatura Infantil","Adolescentes","Infantojuvenil","Suspense","Lenda",
            "Folclore","Novela","Fábula","Narrativa","Afetividade","Letramento","Filosofia",
            "Política","Culinária","Crônica","Conto","Didático","Literatura",
        };

        // Eventos de abertura
        private void txtGenero_Enter(object sender, EventArgs e) { AtualizarSugestoesGenero(); }
        private void txtGenero_MouseDown(object sender, MouseEventArgs e) { AtualizarSugestoesGenero(); }

        // Em RoundedTextBox, TextChanged pode não propagar como no TextBox comum; KeyUp garante refresh.
        private void txtGenero_TextChanged(object sender, EventArgs e)
        {
            if (_suppressGeneroSuggest) return;
            if (!txtGenero.ContainsFocus) return;   // RoundedTextBox: usa ContainsFocus
            AtualizarSugestoesGenero();
        }

        private void txtGenero_KeyUp(object sender, KeyEventArgs e)
        {
            if (_suppressGeneroSuggest) return;
            if (!txtGenero.ContainsFocus) return;

            // Ignora teclas de navegação; o resto dispara refresh
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.Left || e.KeyCode == Keys.Right ||
                e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab ||
                e.KeyCode == Keys.Escape)
                return;

            AtualizarSugestoesGenero();
        }

        private void AtualizarSugestoesGenero()
        {
            if (_suppressGeneroSuggest) return;

            string texto = (txtGenero.Text ?? string.Empty).Trim();

            // Vazio => esconde; senão filtra por prefixo
            List<string> sug = string.IsNullOrEmpty(texto)
                ? new List<string>()
                : generosPadronizados
                    .Where(g => g.StartsWith(texto, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(g => g)
                    .ToList();

            // Sempre desamarrar antes de reatribuir
            lstSugestoesGenero.DataSource = null;

            if (sug.Count > 0)
            {
                lstSugestoesGenero.DataSource = sug;
                lstSugestoesGenero.Location = new Point(txtGenero.Left, txtGenero.Bottom);
                lstSugestoesGenero.Width = txtGenero.Width;
                lstSugestoesGenero.BringToFront();
                lstSugestoesGenero.Visible = true;

                lstSugestoesGenero.SelectedIndex = 0;

                MutarAcceptCancelEnquantoSugestao(true);
            }
            else
            {
                EsconderSugestoes();
            }
        }

        private void txtGenero_KeyDown(object sender, KeyEventArgs e)
        {
            // Lista fechada: Enter/Tab só navegam
            if (!lstSugestoesGenero.Visible || lstSugestoesGenero.Items.Count == 0)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    e.SuppressKeyPress = true;
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
                lstSugestoesGenero.Focus();
                if (lstSugestoesGenero.Items.Count > 0 && lstSugestoesGenero.SelectedIndex < 0)
                    lstSugestoesGenero.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                ConfirmarSugestaoGenero();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                EsconderSugestoes();
            }
        }

        private void txtGenero_Leave(object sender, EventArgs e)
        {
            // Se ainda está dentro do RoundedTextBox (filhos), não esconda
            if (txtGenero.ContainsFocus) return;

            // Se o mouse está clicando na lista, não esconda aqui — deixe a lista receber o foco
            if (_isClickingSugestoes) return;

            if (!lstSugestoesGenero.Focused)
                EsconderSugestoes();
        }

        private void lstSugestoesGenero_MouseDown(object sender, MouseEventArgs e) { _isClickingSugestoes = true; }
        private void lstSugestoesGenero_MouseUp(object sender, MouseEventArgs e) { _isClickingSugestoes = false; }

        private void lstSugestoesGenero_Click(object sender, EventArgs e)
        {
            if (lstSugestoesGenero.SelectedIndex >= 0)
                SelecionarGenero(lstSugestoesGenero.SelectedIndex);
        }

        private void lstSugestoesGenero_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                ConfirmarSugestaoGenero();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                EsconderSugestoes();
                txtGenero.Focus();
            }
        }

        private void lstSugestoesGenero_Leave(object sender, EventArgs e)
        {
            EsconderSugestoes();
        }

        private bool ConfirmarSugestaoGenero()
        {
            if (!lstSugestoesGenero.Visible || lstSugestoesGenero.Items.Count == 0)
                return false;

            int idx = lstSugestoesGenero.SelectedIndex;
            if (idx < 0) idx = 0;

            SelecionarGenero(idx);
            return true;
        }

        private void SelecionarGenero(int index)
        {
            if (index < 0 || index >= lstSugestoesGenero.Items.Count)
                return;

            // Suprime TextChanged enquanto atualiza o texto programaticamente
            _suppressGeneroSuggest = true;
            txtGenero.Text = lstSugestoesGenero.Items[index].ToString();
            _suppressGeneroSuggest = false;

            EsconderSugestoes();

            // Enter/Tab seguem o tab order
            this.SelectNextControl(txtGenero, true, true, true, true);
        }
        #endregion

        #region Infra
        private void EsconderSugestoes()
        {
            lstSugestoesGenero.Visible = false;
            lstSugestoesGenero.DataSource = null;
            MutarAcceptCancelEnquantoSugestao(false);
        }

     

        private void MutarAcceptCancelEnquantoSugestao(bool mutar)
        {
            if (mutar)
            {
                if (_acceptBackup == null) _acceptBackup = this.AcceptButton;
                if (_cancelBackup == null) _cancelBackup = this.CancelButton;
                this.AcceptButton = null;
                this.CancelButton = null;
            }
            else
            {
                this.AcceptButton = _acceptBackup;
                this.CancelButton = _cancelBackup;
            }
        }
        #endregion
    }
}
