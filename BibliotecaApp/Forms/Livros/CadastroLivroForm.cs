using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using BibliotecaApp.Utils.Etiquetas;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class CadastroLivroForm : Form
    {
        #region Propriedades e Campos
        public event EventHandler LivroAtualizado;

        private static bool IsAdminLogado() => string.Equals(Sessao.NomeBibliotecariaLogada, "Administrador", StringComparison.OrdinalIgnoreCase);

        private readonly List<string> generosPadronizados = new List<string>
        {
            "Poesia", "Literatura de Cordel", "Biografia", "Autobiografia", "Diálogo",
            "Hábito", "Psicologia", "Cultura Afro-brasileira", "História", "Teatro",
            "Educação", "Romance", "Ficção", "Fantasia", "Mitologia", "Literatura Infantil",
            "Adolescentes", "Infantojuvenil", "Suspense", "Lenda", "Folclore", "Novela",
            "Fábula", "Narrativa", "Afetividade", "Letramento", "Filosofia",
            "Política", "Culinária", "Crônica", "Conto", "Didático", "Literatura",
        };

        private bool generoSelecionadoDaLista = false;
        private Timer focoTimer;
        private Timer validationTimer;

        // Backup para “mutar” Accept/Cancel enquanto a lista está aberta
        private IButtonControl _acceptBackup, _cancelBackup;
        #endregion

        #region Inicialização do Formulário
        public CadastroLivroForm()
        {
            focoTimer = new Timer { Interval = 50 };
            focoTimer.Tick += FocoTimer_Tick;

            validationTimer = new Timer { Interval = 100 };
            validationTimer.Tick += ValidationTimer_Tick;

            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            ConfigurarEventos();
            ConfigurarNavegacao();

            if (lstSugestoesGenero != null) lstSugestoesGenero.TabStop = false;
        }

        private void CadastroLivroForm_Load(object sender, EventArgs e)
        {
            
        }

        private void ConfigurarEventos()
        {
            // Sugestões de gênero
            txtGenero.TextChanged += txtGenero_TextChanged;
            txtGenero.KeyDown += txtGenero_KeyDown;
            txtGenero.Leave += txtGenero_Leave;

            lstSugestoesGenero.Click += lstSugestoesGenero_Click;
            lstSugestoesGenero.KeyDown += lstSugestoesGenero_KeyDown;
            lstSugestoesGenero.Leave += lstSugestoesGenero_Leave;

            // Botões (se não estiverem no designer)
            // btnCadastrar.Click += btnCadastrar_Click;
            // btnLimpar.Click += btnLimpar_Click;

            mtxCodigoBarras.KeyDown += mtxCodigoBarras_KeyDown;
        }

        private void ConfigurarNavegacao()
        {
            // Captura teclas no form antes dos filhos (ganha do AcceptButton)
            this.KeyPreview = true;
        }
        #endregion

        #region Overrides de Teclado (prioridade para a lista)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (lstSugestoesGenero != null && lstSugestoesGenero.Visible)
            {
                if (keyData == Keys.Enter)
                {
                    if (ConfirmarSugestaoGenero())
                        return true;
                }
                else if (keyData == Keys.Down)
                {
                    if (!lstSugestoesGenero.Focused)
                    {
                        validationTimer.Stop();                  // <- evita validação indevida
                        lstSugestoesGenero.Focus();
                        if (lstSugestoesGenero.Items.Count > 0 && lstSugestoesGenero.SelectedIndex < 0)
                            lstSugestoesGenero.SelectedIndex = 0;
                        return true;
                    }
                }
                else if (keyData == Keys.Tab)
                {
                    // Tab confirma a sugestão e segue fluxo
                    if (ConfirmarSugestaoGenero())
                        return true;
                }
                else if (keyData == Keys.Escape)
                {
                    lstSugestoesGenero.Visible = false;
                    MutarAcceptCancelEnquantoSugestao(false);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Eventos dos Botões
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "Tem certeza de que deseja limpar tudo?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
                LimparFormulario();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode cadastrar livros.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (!ValidarCampos()) return;
            if (!ValidarQuantidade(out int quantidade)) return;

            string codigoBarras = ObterCodigoDeBarrasFormatado();

            // Verifica se o código de barras está vazio
            if (string.IsNullOrWhiteSpace(codigoBarras))
            {
                var resultado = MessageBox.Show(
                    "O código de barras não foi preenchido. Deseja gerar um código de barras único automaticamente?",
                    "Código de Barras",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    codigoBarras = GerarCodigoDeBarrasUnico();

                   
                    mtxCodigoBarras.Text = codigoBarras;
                }

                else
                {
                    MessageBox.Show("O cadastro foi cancelado. Preencha o código de barras ou aceite gerar um automaticamente.",
                                    "Cadastro Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (!ValidarCodigoBarras(out string codigoBarrasValidado))
            {
                MessageBox.Show("O código de barras gerado ou informado é inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CadastrarLivro(quantidade, codigoBarrasValidado);
        }

        /// <summary>
        /// Gera um código de barras único baseado em um padrão próprio.
        /// </summary>
        /// <returns>Um código de barras único.</returns>
        private static readonly Random _random = new Random();

        private string GerarCodigoDeBarrasUnico()
        {
            string codigoBarras;
            lock (_random) // evita colisões em threads
            {
                do
                {
                    // prefixo 999 + 10 dígitos aleatórios = 13 dígitos
                    string part1 = _random.Next(0, 100000).ToString("D5"); // 5 dígitos
                    string part2 = _random.Next(0, 100000).ToString("D5"); // 5 dígitos
                    codigoBarras = "999" + part1 + part2; // 3 + 5 + 5 = 13
                } while (CodigoBarrasExisteNoBanco(codigoBarras));
            }
            return codigoBarras;
        }


        /// <summary>
        /// Verifica se o código de barras já existe no banco de dados.
        /// </summary>
        /// <param name="codigoBarras">Código de barras a ser verificado.</param>
        /// <returns>True se o código já existir, caso contrário, False.</returns>
        private bool CodigoBarrasExisteNoBanco(string codigoBarras)
        {
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();
                string query = "SELECT COUNT(*) FROM Livros WHERE CodigoBarras = @CodigoBarras";
                using (var comando = new SqlCeCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@CodigoBarras", codigoBarras);
                    int count = (int)comando.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        #endregion

        #region Sistema de Sugestão de Gêneros
        private void txtGenero_TextChanged(object sender, EventArgs e)
        {
            generoSelecionadoDaLista = false;

            string texto = txtGenero.Text.Trim();
            if (string.IsNullOrEmpty(texto))
            {
                lstSugestoesGenero.Visible = false;
                lstSugestoesGenero.DataSource = null;
                MutarAcceptCancelEnquantoSugestao(false);
                return;
            }

            // Ranking: prefixo primeiro; depois Levenshtein <= 2
            var lower = texto.ToLower();
            var sugestoes = generosPadronizados
                .Where(g => g.StartsWith(texto, StringComparison.OrdinalIgnoreCase)
                         || CalcularDistanciaLevenshtein(g.ToLower(), lower) <= 2)
                .OrderBy(g => g.StartsWith(texto, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .ThenBy(g => CalcularDistanciaLevenshtein(g.ToLower(), lower))
                .ThenBy(g => g)
                .ToList();

            if (sugestoes.Any())
            {
                lstSugestoesGenero.DataSource = sugestoes;
                lstSugestoesGenero.Visible = true;

                // posiciona e traz pra frente
                lstSugestoesGenero.Location = new Point(txtGenero.Left, txtGenero.Bottom);
                lstSugestoesGenero.Width = txtGenero.Width;
                lstSugestoesGenero.BringToFront();

                // seleciona o primeiro por padrão
                if (lstSugestoesGenero.Items.Count > 0)
                    lstSugestoesGenero.SelectedIndex = 0;

                // Muta Accept/Cancel enquanto a lista estiver aberta
                MutarAcceptCancelEnquantoSugestao(true);
            }
            else
            {
                lstSugestoesGenero.Visible = false;
                lstSugestoesGenero.DataSource = null;
                MutarAcceptCancelEnquantoSugestao(false);
            }
        }

        private void txtGenero_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesGenero.Visible || lstSugestoesGenero.Items.Count == 0)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    e.SuppressKeyPress = true;
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Down:
                    e.SuppressKeyPress = true;
                    validationTimer.Stop();                      // <- para não validar ao focar a lista
                    lstSugestoesGenero.Focus();
                    if (lstSugestoesGenero.Items.Count > 0 && lstSugestoesGenero.SelectedIndex < 0)
                        lstSugestoesGenero.SelectedIndex = 0;
                    break;

                case Keys.Enter:
                case Keys.Tab:                                   // <- Tab confirma e avança
                    e.SuppressKeyPress = true;
                    ConfirmarSugestaoGenero();
                    break;

                case Keys.Escape:
                    e.SuppressKeyPress = true;
                    lstSugestoesGenero.Visible = false;
                    MutarAcceptCancelEnquantoSugestao(false);
                    break;
            }
        }

        private void txtGenero_Leave(object sender, EventArgs e)
        {
            // Se a lista está aberta (ou iremos para ela), não validar agora
            if (lstSugestoesGenero.Visible)
                return;

            validationTimer.Stop();
            validationTimer.Start();
        }

        private void lstSugestoesGenero_Click(object sender, EventArgs e)
        {
            if (lstSugestoesGenero.SelectedItem != null)
            {
                SelecionarGenero(lstSugestoesGenero.SelectedIndex);
            }
        }

        private void lstSugestoesGenero_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Tab:                                   // <- Tab também confirma na lista
                    e.SuppressKeyPress = true;
                    ConfirmarSugestaoGenero();
                    break;

                case Keys.Escape:
                    e.SuppressKeyPress = true;
                    lstSugestoesGenero.Visible = false;
                    MutarAcceptCancelEnquantoSugestao(false);
                    txtGenero.Focus();
                    break;
            }
        }

        private void lstSugestoesGenero_Leave(object sender, EventArgs e)
        {
            lstSugestoesGenero.Visible = false;
            MutarAcceptCancelEnquantoSugestao(false);
        }
        #endregion

        #region Validações
        private void ValidationTimer_Tick(object sender, EventArgs e)
        {
            validationTimer.Stop();

            // Se a lista estiver visível, adia a validação (evita popup indevido)
            if (lstSugestoesGenero.Visible)
                return;

            ValidarGeneroAoSair();
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtAutor.Text) ||
                string.IsNullOrWhiteSpace(txtGenero.Text) ||
                string.IsNullOrWhiteSpace(txtQuantidade.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos antes de cadastrar.",
                                "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }


        private bool ValidarQuantidade(out int quantidade)
        {
            if (!int.TryParse(txtQuantidade.Text.Trim(), out quantidade))
            {
                MessageBox.Show("Por favor, insira apenas números no campo 'Quantidade'.",
                                "Erro de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidarCodigoBarras(out string codigoBarras)
        {
            codigoBarras = ObterCodigoDeBarrasFormatado();

            if (codigoBarras.Length != 13)
            {
                MessageBox.Show("O código de barras deve conter exatamente 13 dígitos.",
                                "Código inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ValidarGeneroAoSair()
        {
            // Se veio da lista, não precisa validar
            if (generoSelecionadoDaLista)
            {
                generoSelecionadoDaLista = false;
                lstSugestoesGenero.Visible = false;
                MutarAcceptCancelEnquantoSugestao(false);
                return;
            }

            string entrada = txtGenero.Text.Trim();

            // Se vazio, nada a validar
            if (string.IsNullOrEmpty(entrada))
            {
                lstSugestoesGenero.Visible = false;
                MutarAcceptCancelEnquantoSugestao(false);
                return;
            }

            // Não validar se a lista está aberta (proteção extra)
            if (lstSugestoesGenero.Visible)
                return;

            if (!GeneroExisteOuSimilar(entrada))
            {
                if (ConfirmarNovoGenero(entrada))
                {
                    AdicionarNovoGenero(entrada);
                }
                else
                {
                    txtGenero.Text = "";
                }
            }

            lstSugestoesGenero.Visible = false;
            MutarAcceptCancelEnquantoSugestao(false);
        }
        #endregion

        #region Banco de Dados
        private void CadastrarLivro(int quantidade, string codigoBarras)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();
                using (var trans = conexao.BeginTransaction())
                {
                    try
                    {
                        using (SqlCeCommand comando = conexao.CreateCommand())
                        {
                            comando.Transaction = trans;
                            comando.CommandText = @"INSERT INTO Livros 
                        (Nome, Autor, Genero, Quantidade, CodigoBarras, Disponibilidade)
                        VALUES
                        (@Nome, @Autor, @Genero, @Quantidade, @CodigoBarras, @Disponibilidade)";

                            comando.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
                            comando.Parameters.AddWithValue("@Autor", txtAutor.Text.Trim());
                            comando.Parameters.AddWithValue("@Genero", txtGenero.Text.Trim());
                            comando.Parameters.AddWithValue("@Quantidade", quantidade);
                            comando.Parameters.AddWithValue("@CodigoBarras", codigoBarras);
                            comando.Parameters.AddWithValue("@Disponibilidade", 1);

                            comando.ExecuteNonQuery();
                        }

                        // Agora obrigamos a geração/impresão da etiqueta; se falhar ou cancelar, damos rollback
                        bool etiquetasGeradas = OferecerImpressaoOuPdfEtiquetas(txtNome.Text.Trim(), txtGenero.Text.Trim(), codigoBarras, quantidade);
                        if (!etiquetasGeradas)
                        {
                            trans.Rollback();
                            MessageBox.Show("Cadastro cancelado porque as etiquetas não foram geradas.", "Cadastro cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // commit porque etiqueta gerada com sucesso
                        trans.Commit();

                        MessageBox.Show("Livro salvo com sucesso!",
                                        "Cadastro realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LivroAtualizado?.Invoke(this, EventArgs.Empty);
                        BibliotecaApp.Utils.EventosGlobais.OnLivroCadastradoOuAlterado();

                        LimparFormulario();
                    }
                    catch (Exception ex)
                    {
                        try { trans.Rollback(); } catch { /* ignore */ }
                        MessageBox.Show($"Erro ao cadastrar livro: {ex.Message}",
                                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        // Retorna true se o usuário gerou/imprimiu o conjunto de etiquetas com sucesso.
        // Retorna false se o usuário cancelou em qualquer etapa.
        private bool OferecerImpressaoOuPdfEtiquetas(string titulo, string genero, string codigoBarras, int quantidade)
        {
            if (string.IsNullOrWhiteSpace(codigoBarras))
                return false; // não podemos gerar etiquetas sem código

            var r1 = MessageBox.Show("Deseja gerar etiquetas agora?",
                             "Etiquetas",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);
            if (r1 != DialogResult.Yes) return false;

            var itens = new List<EtiquetaLabelItem>();
            int n = Math.Max(1, quantidade);
            for (int i = 0; i < n; i++)
            {
                itens.Add(new EtiquetaLabelItem
                {
                    CodigoBarras = codigoBarras,
                    Titulo = titulo,
                    Genero = genero
                });
            }

            while (true)
            {
                using (var dlg = new LabelActionDialog(titulo))
                {
                    dlg.ConfigureQuantity(allowSelection: false, fixedQuantity: n);
                    var dr = dlg.ShowDialog(this);
                    if (dr != DialogResult.OK || dlg.Choice == LabelActionChoice.None)
                        return false; // usuário cancelou

                    try
                    {
                        if (dlg.Choice == LabelActionChoice.Pdf)
                        {
                            using (var sfd = new SaveFileDialog
                            {
                                Title = "Salvar etiquetas em PDF",
                                Filter = "PDF (*.pdf)|*.pdf",
                                FileName = $"{SanitizeFileName(titulo)} - {n} etiquetas.pdf",
                                OverwritePrompt = true,
                                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                            })
                            {
                                if (sfd.ShowDialog(this) != DialogResult.OK)
                                    continue; // reabre o LabelActionDialog

                                var opts = new LabelPrintOptions
                                {
                                    StartPosition = 1,
                                    OffsetXmm = 0f,
                                    OffsetYmm = 0f,
                                    GroupByGenre = true
                                };
                                LabelPdfExporter.ExportPdf(itens, opts, sfd.FileName, barcodeDpi: 200);
                                MessageBox.Show("PDF gerado com sucesso!", "Etiquetas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            return true; // sucesso
                        }
                        else if (dlg.Choice == LabelActionChoice.Print)
                        {
                            using (var pd = new PrintDialog { UseEXDialog = true })
                            using (var dummyDoc = new PrintDocument())
                            {
                                pd.Document = dummyDoc;

                                if (pd.ShowDialog(this) != DialogResult.OK)
                                    continue;

                                var chosenPrinter = pd.PrinterSettings.PrinterName ?? string.Empty;

                                var opts = new LabelPrintOptions
                                {
                                    StartPosition = 1,
                                    OffsetXmm = 0f,
                                    OffsetYmm = 0f,
                                    GroupByGenre = true,
                                    Preview = false,
                                    ShowPrintDialog = false,
                                    PrinterName = chosenPrinter
                                };

                                if (chosenPrinter.IndexOf("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    using (var sfd = new SaveFileDialog
                                    {
                                        Title = "Salvar etiquetas em PDF",
                                        Filter = "PDF (*.pdf)|*.pdf",
                                        FileName = $"{SanitizeFileName(titulo)} - {n} etiquetas.pdf",
                                        OverwritePrompt = true,
                                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                    })
                                    {
                                        if (sfd.ShowDialog(this) != DialogResult.OK)
                                            continue; // reabre
                                        opts.PrintToFile = true;
                                        opts.OutputFilePath = sfd.FileName;
                                    }
                                }
                                else
                                {
                                    opts.PrintToFile = false;
                                    opts.OutputFilePath = null;
                                }

                                LabelPrinterService.PrintLabels(itens, opts);
                                return true; // sucesso
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var root = ex.GetBaseException()?.Message ?? ex.Message;
                        MessageBox.Show("Falha ao gerar etiquetas: " + root, "Etiquetas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // volta ao diálogo de ação
                    }
                }
            }
        }


        private static string SanitizeFileName(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "Livro";
            foreach (var c in System.IO.Path.GetInvalidFileNameChars()) s = s.Replace(c, '_');
            return s;
        }
        #endregion

        #region Utilitários
        private void LimparFormulario()
        {
            txtNome.Text = "";
            txtAutor.Text = "";
            txtGenero.Text = "";
            txtQuantidade.Text = "";
            mtxCodigoBarras.Text = "";
            lstSugestoesGenero.Visible = false;
            lstSugestoesGenero.DataSource = null;
            MutarAcceptCancelEnquantoSugestao(false);
            mtxCodigoBarras.Focus();
        }

        private string ObterCodigoDeBarrasFormatado()
        {
            return new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray());
        }

        private void FocoTimer_Tick(object sender, EventArgs e)
        {
            focoTimer.Stop();
            this.SelectNextControl(txtGenero, true, true, true, true);
        }

        private bool GeneroExiste(string entrada)
        {
            return generosPadronizados
                .Any(g => string.Equals(g.Trim(), entrada.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private bool GeneroExisteOuSimilar(string entrada)
        {
            if (GeneroExiste(entrada)) return true;
            var lower = entrada.ToLower();
            return generosPadronizados.Any(g => CalcularDistanciaLevenshtein(g.ToLower(), lower) <= 1);
        }

        private int CalcularDistanciaLevenshtein(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1)) return s2?.Length ?? 0;
            if (string.IsNullOrEmpty(s2)) return s1.Length;

            int[,] matriz = new int[s1.Length + 1, s2.Length + 1];
            for (int i = 0; i <= s1.Length; i++) matriz[i, 0] = i;
            for (int j = 0; j <= s2.Length; j++) matriz[0, j] = j;

            for (int i = 1; i <= s1.Length; i++)
            {
                for (int j = 1; j <= s2.Length; j++)
                {
                    int custo = s1[i - 1] == s2[j - 1] ? 0 : 1;
                    matriz[i, j] = Math.Min(
                        Math.Min(matriz[i - 1, j] + 1, matriz[i, j - 1] + 1),
                        matriz[i - 1, j - 1] + custo);
                }
            }
            return matriz[s1.Length, s2.Length];
        }

        private bool ConfirmarNovoGenero(string genero)
        {
            var resultado = MessageBox.Show(
                $"O gênero '{genero}' não existe na lista padrão.\nDeseja adicioná-lo como um novo gênero?",
                "Novo Gênero",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            return resultado == DialogResult.Yes;
        }

        private void AdicionarNovoGenero(string genero)
        {
            if (!string.IsNullOrWhiteSpace(genero) && !GeneroExiste(genero))
            {
                generosPadronizados.Add(genero.Trim());
                generosPadronizados.Sort();
                MessageBox.Show($"Gênero '{genero}' adicionado com sucesso!",
                               "Gênero Adicionado",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
        }
        #endregion

        #region Fluxo de Seleção (confirma/avança)
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

            string genero = lstSugestoesGenero.Items[index].ToString();
            generoSelecionadoDaLista = true;
            txtGenero.Text = genero;

            lstSugestoesGenero.Visible = false;
            lstSugestoesGenero.DataSource = null;
            MutarAcceptCancelEnquantoSugestao(false);

            // Enter/Tab seguem o tab order
            this.SelectNextControl(txtGenero, true, true, true, true);
        }

        private void mtxCodigoBarras_KeyDown(object sender, KeyEventArgs e)
        {
            // Garante que o evento será disparado mesmo se o controle for customizado
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
                btnBuscar.Focus(); // Garante que o botão está focado antes de disparar o click
                btnBuscar.PerformClick();
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Program.IsOfflineMode)
            {
                MessageBox.Show("A busca online de livros (Google Books/OpenLibrary) está indisponível no Modo Offline.\n\n" +
                                "Por favor, preencha os dados do livro manualmente.",
                                "Modo Offline", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Foca no campo Nome para agilizar o preenchimento manual
                txtNome.Focus();
                return; // Interrompe a execução para não tentar conectar
            }

            string isbn = new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray());
            if (isbn.Length != 13)
            {
                MessageBox.Show("Digite um ISBN válido de 13 dígitos.", "ISBN inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mtxCodigoBarras.Focus();
                return;
            }

            btnBuscar.Enabled = false;
            try
            {
                var resultado = await BuscarLivroPorIsbnAsync(isbn);

                if (resultado == null)
                {
                    MessageBox.Show("Nenhum livro encontrado para o ISBN informado.", "Não encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNome.Focus();
                    return;
                }

                // Preenche os campos encontrados
                string msg = "Dados encontrados:\n";
                bool nomePreenchido = false, autorPreenchido = false, generoPreenchido = false;

                if (!string.IsNullOrEmpty(resultado.Nome))
                {
                    txtNome.Text = resultado.Nome;
                    msg += "- Nome\n";
                    nomePreenchido = true;
                }
                if (!string.IsNullOrEmpty(resultado.Autor))
                {
                    txtAutor.Text = resultado.Autor;
                    msg += "- Autor\n";
                    autorPreenchido = true;
                }
                if (!string.IsNullOrEmpty(resultado.Genero))
                {
                    var generoPadrao = generosPadronizados.FirstOrDefault(g => string.Equals(g, resultado.Genero, StringComparison.OrdinalIgnoreCase));
                    if (generoPadrao != null)
                    {
                        txtGenero.Text = generoPadrao;
                        msg += "- Gênero\n";
                        generoPreenchido = true;
                    }
                }

                MessageBox.Show(msg, "Busca concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Foco no primeiro campo não preenchido, ou txtQuantidade se todos preenchidos
                if (!nomePreenchido)
                    txtNome.Focus();
                else if (!autorPreenchido)
                    txtAutor.Focus();
                else if (!generoPreenchido)
                    txtGenero.Focus();
                else
                    txtQuantidade.Focus();
            }
            finally
            {
                btnBuscar.Enabled = true;
            }
        }

        // Classe auxiliar para resultado
        private class LivroBuscaResultado
        {
            public string Nome { get; set; }
            public string Autor { get; set; }
            public string Genero { get; set; }
        }

        // Busca nas duas APIs
        private async Task<LivroBuscaResultado> BuscarLivroPorIsbnAsync(string isbn)
        {
            // 1. Tenta Open Library
            var resultado = await BuscarOpenLibraryAsync(isbn);
            if (resultado != null && (!string.IsNullOrEmpty(resultado.Nome) || !string.IsNullOrEmpty(resultado.Autor)))
                return resultado;

            // 2. Tenta Google Books
            resultado = await BuscarGoogleBooksAsync(isbn);
            if (resultado != null && (!string.IsNullOrEmpty(resultado.Nome) || !string.IsNullOrEmpty(resultado.Autor)))
                return resultado;

            return null;
        }

        private async Task<LivroBuscaResultado> BuscarOpenLibraryAsync(string isbn)
        {
            using (var client = new HttpClient())
            {
                string url = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";
                var resp = await client.GetAsync(url);
                if (!resp.IsSuccessStatusCode) return null;

                var json = await resp.Content.ReadAsStringAsync();
                var obj = JObject.Parse(json);
                var livro = obj[$"ISBN:{isbn}"];
                if (livro == null) return null;

                return new LivroBuscaResultado
                {
                    Nome = livro["title"]?.ToString(),
                    Autor = livro["authors"]?.First?["name"]?.ToString(),
                    Genero = livro["subjects"]?.First?["name"]?.ToString()
                };
            }
        }

        private async Task<LivroBuscaResultado> BuscarGoogleBooksAsync(string isbn)
        {
            using (var client = new HttpClient())
            {
                string apiKey = "AIzaSyAl09C8xUcIavKjzK3M4-L39_n0E46lE_Y";
                string url = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}&key={apiKey}";
                var resp = await client.GetAsync(url);
                if (!resp.IsSuccessStatusCode) return null;

                var json = await resp.Content.ReadAsStringAsync();
                var obj = JObject.Parse(json);
                var item = obj["items"]?.First;
                if (item == null) return null;

                var volumeInfo = item["volumeInfo"];
                string genero = null;
                if (volumeInfo["categories"] != null)
                {
                    // Tenta casar com os gêneros padronizados
                    var categorias = volumeInfo["categories"].Select(c => c.ToString());
                    genero = categorias.Select(cat =>
                        generosPadronizados.FirstOrDefault(g => cat.IndexOf(g, StringComparison.OrdinalIgnoreCase) >= 0)
                    ).FirstOrDefault(g => g != null);
                }

                return new LivroBuscaResultado
                {
                    Nome = volumeInfo["title"]?.ToString(),
                    Autor = volumeInfo["authors"]?.First?.ToString(),
                    Genero = genero
                };
            }
        }


        #endregion

        #region Infra de Accept/Cancel “mute”
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
