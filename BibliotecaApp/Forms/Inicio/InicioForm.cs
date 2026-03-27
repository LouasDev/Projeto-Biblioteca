using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Forms.Relatorio;
using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using System.Drawing.Imaging;

namespace BibliotecaApp.Forms.Inicio
{
    public partial class InicioForm : Form
    {
        private class EmprestimoAtrasadoInfo
        {
            public int Id { get; set; }
            public int UsuarioId { get; set; }    // <-- novo: id do usuário (PK)
            public string Nome { get; set; }
            public string Turma { get; set; }
            public string Livro { get; set; }
            public DateTime DataDevolucao { get; set; }
            public int DiasAtraso { get; set; }
        }

      

        private (int Id, string Nome, string Turma, int Qtd) topUsuarioMaisEmprestimos = (0, "-", "", 0);




        // controls dinâmicos
        private FlowLayoutPanel flowCards;
        private Panel topPanelInside;         // container para cards
        private Panel actionsPanel;           // painel no canto superior direito com botão de Empréstimo Rápido
        private Button btnEmprestimoRapido;   // botão principal no topo-direito, bem visível (maior)
        private Label lblStatusSmall;
        private ToolTip formToolTip;

        // auto-refresh timer
        private System.Windows.Forms.Timer timerAutoRefresh;

        private EmprestimoRapidoForm emprestimoRap = null;
        private Dictionary<string, bool> mainButtonsOriginalState = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        public InicioForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += InicioForm_KeyDown;

            BibliotecaApp.Utils.EventosGlobais.LivroCadastradoOuAlterado += (s, e) => _ = CarregarEstatisticasAsync();
            BibliotecaApp.Utils.EventosGlobais.BibliotecariaCadastrada += (s, e) => _ = CarregarEstatisticasAsync();
            BibliotecaApp.Utils.EventosGlobais.ProfessorCadastrado += (s, e) => _ = CarregarEstatisticasAsync();
            BibliotecaApp.Utils.EventosGlobais.LivroDidaticoCadastrado += (s, e) => _ = CarregarEstatisticasAsync();
            BibliotecaApp.Utils.EventosGlobais.LivroDevolvido += (s, e) => _ = CarregarEstatisticasAsync();
            BibliotecaApp.Utils.EventosGlobais.EmprestimoProrrogado += (s, e) => _ = CarregarEstatisticasAsync();
            BibliotecaApp.Utils.EventosGlobais.EmprestimoRealizado += (s, e) => SafeInvoke(() => _ = CarregarEstatisticasAsync());


            this.Activated += (s, e) => _ = CarregarEstatisticasAsync();

            // timerAutoRefresh pode ser mantido ou removido conforme sua preferência
        }


        private void SafeInvoke(Action action)
        {
            if (this.IsHandleCreated && !this.IsDisposed && !this.Disposing)
            {
                try { this.BeginInvoke(action); }
                catch { /* nada — evita crash se o form estiver sendo fechado */ }
            }
        }

        private void InicioForm_Load(object sender, EventArgs e)
        {
            AppPaths.EnsureFolders();

            // relógio
            timerRelogio.Interval = 1000; // 1 segundo
            timerRelogio.Tick += timerRelogio_Tick;
            timerRelogio.Start();
            AtualizarRelogio();

            // tooltip
            formToolTip = new ToolTip { AutoPopDelay = 6000, InitialDelay = 300, ReshowDelay = 150, IsBalloon = false };

            // auto-refresh:
            timerAutoRefresh = new System.Windows.Forms.Timer();
            timerAutoRefresh.Interval = 20000; // 
            timerAutoRefresh.Tick += (s, ev) => { _ = CarregarEstatisticasAsync(); };
            timerAutoRefresh.Start();

            // construir UI dinâmica (cards, tabs, botão de empréstimo rápido no topo direito)
            ConstruirDashboardUI();

            // primeira carga
            _ = CarregarEstatisticasAsync();
        }

        private void ConstruirDashboardUI()
        {
            // remover controles dinâmicos antigos do panel1 (se houver)
            foreach (Control c in panel1.Controls.OfType<Control>().ToArray())
            {
                if (!(c is TabControl && c.Name == "tabEstatisticas"))
                {
                    panel1.Controls.Remove(c);
                    c.Dispose();
                }
            }

            // === TOP PANEL INSIDE (cards) ===
            if (topPanelInside != null) { panel1.Controls.Remove(topPanelInside); topPanelInside.Dispose(); }
            topPanelInside = new Panel
            {
                Name = "topPanelInside",
                Dock = DockStyle.Top,
                Height = 160,
                BackColor = Color.White
            };
            panel1.Controls.Add(topPanelInside);

            flowCards = new FlowLayoutPanel
            {
                Name = "flowCards",
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(12),
                BackColor = Color.White
            };
            topPanelInside.Controls.Add(flowCards);

            var cardsInfo = new[]
            {
                new { Key = "Usuarios", Title = "Usuários", Sub = "Total de usuários", Color = Color.FromArgb(30,61,88) },
                new { Key = "Livros", Title = "Livros", Sub = "Total de livros", Color = Color.FromArgb(9,74,158) },
                new { Key = "EmpAtivos", Title = "Empréstimos Ativos", Sub = "Empréstimos não devolvidos", Color = Color.FromArgb(34,139,34) },
                new { Key = "Atrasados", Title = "Atrasados", Sub = "Empréstimos em atraso", Color = Color.FromArgb(178,34,34) },
                new { Key = "RapidosHoje", Title = "Rápidos (hoje)", Sub = "Empréstimos rápidos hoje", Color = Color.FromArgb(92,92,205) },
                new { Key = "MediaMes", Title = "Média/Mês", Sub = "Média de empréstimos deste mês", Color = Color.FromArgb(255, 140, 0) },
                new { Key = "TopUsuario", Title = "Top Usuário 🏆", Sub = "Usuário que mais emprestou", Color = Color.FromArgb(0, 123, 167) }
            };

            // Calcula o tamanho ideal para os cards
            int cardWidth = 210; // 16 = margem
            int cardHeight = 110;

            foreach (var c in cardsInfo) flowCards.Controls.Add(CriarCard(c.Key, c.Title, c.Sub, c.Color, cardWidth, cardHeight));

            // === ACTIONS PANEL (topo direito) com botão visível e maior ===
            if (actionsPanel != null) { panelTop.Controls.Remove(actionsPanel); actionsPanel.Dispose(); }
            actionsPanel = new Panel
            {
                Name = "actionsPanel",
                Size = new Size(260, 64),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelTop.Width - 280, 12),
                BackColor = Color.Transparent
            };
            panelTop.Controls.Add(actionsPanel);
            actionsPanel.BringToFront();

            var btnFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                AutoSize = false,
                Padding = new Padding(8),
                Margin = new Padding(0)
            };
            actionsPanel.Controls.Add(btnFlow);

            // Botão de Empréstimo Rápido (maior, topo direito, com leve arredondamento)
            btnEmprestimoRapido = new Button
            {
                Text = "Empréstimo Rápido",
                AutoSize = false,
                Size = new Size(220, 44), // Aumentado
                BackColor = Color.FromArgb(9, 74, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new System.Drawing.Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                AccessibleName = "Empréstimo Rápido",
                Cursor = Cursors.Hand
            };
            btnEmprestimoRapido.FlatAppearance.BorderSize = 0;
            btnEmprestimoRapido.Click += BtnEmprestimoRapido_Click;

            // aplicar cantos arredondados (suave)
            btnEmprestimoRapido.Paint += (s, e) =>
            {
                var btn = s as Button;
                if (btn == null) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new System.Drawing.Rectangle(0, 0, btn.Width, btn.Height), 8))
                using (var brush = new SolidBrush(btn.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
                // desenhar texto manualmente para garantir centralização
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, new System.Drawing.Rectangle(0, 0, btn.Width, btn.Height), btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            // esconder border padrão para evitar sobreposição
            btnEmprestimoRapido.FlatAppearance.BorderSize = 0;
            btnFlow.Controls.Add(btnEmprestimoRapido);
            formToolTip.SetToolTip(btnEmprestimoRapido, "Abrir Empréstimo Rápido (atalho: Ctrl+R)");

            // reajusta posição quando redimensionar o panelTop
            panelTop.Resize += (s, e) =>
            {
                actionsPanel.Location = new Point(Math.Max(12, panelTop.Width - actionsPanel.Width - 20), actionsPanel.Location.Y);
                CenterClock(); // reposicionar relógio para centro
            };

            // lblStatusSmall (texto de status) – posicionado logo abaixo dos cards
            if (lblStatusSmall != null) { panel1.Controls.Remove(lblStatusSmall); lblStatusSmall.Dispose(); }
            lblStatusSmall = new Label
            {
                Name = "lblStatusSmall",
                Text = "",
                AutoSize = true,
                Location = new Point(18, topPanelInside.Bottom + 6),
                ForeColor = Color.Gray,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            panel1.Controls.Add(lblStatusSmall);

            // === TabControl moderno ===
            TabControl tabEstatisticas = new TabControl
            {
                Name = "tabEstatisticas",
                Dock = DockStyle.Fill,
                Appearance = TabAppearance.Normal,
                ItemSize = new Size(120, 32),
                SizeMode = TabSizeMode.Fixed,
                DrawMode = TabDrawMode.OwnerDrawFixed,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            tabEstatisticas.DrawItem += (sender, e) =>
            {
                var tabControl = (TabControl)sender;
                var tabPage = tabControl.TabPages[e.Index];
                var rect = e.Bounds;
                var isSelected = tabControl.SelectedIndex == e.Index;

                var backColor = isSelected ? Color.White : Color.FromArgb(250, 250, 250);
                var textColor = isSelected ? Color.FromArgb(30, 61, 88) : Color.FromArgb(110, 110, 110);
                var borderColor = Color.FromArgb(230, 230, 230);

                using (var brush = new SolidBrush(backColor)) e.Graphics.FillRectangle(brush, rect);
                using (var pen = new Pen(borderColor)) e.Graphics.DrawRectangle(pen, rect);

                TextRenderer.DrawText(e.Graphics, tabPage.Text,
                    new System.Drawing.Font("Segoe UI", 9, isSelected ? FontStyle.Bold : FontStyle.Regular),
                    rect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            var tabDevedores = new TabPage("Devedores") { BackColor = Color.White, Padding = new Padding(12) };
            var tabEstEmp = new TabPage("Empréstimos") { BackColor = Color.White, Padding = new Padding(12) };
            var tabLivros = new TabPage("Livros Populares") { BackColor = Color.White, Padding = new Padding(12) };

            tabEstatisticas.TabPages.Add(tabDevedores);
            tabEstatisticas.TabPages.Add(tabEstEmp);
            tabEstatisticas.TabPages.Add(tabLivros);

            // DataGrids com margin top aumentado para dar 'respiração'
            int topMarginDataGrid = 36; // já aumentado conforme seu pedido
            var dgvDevedores = CriarDataGridBasico("dgvDevedores");
            dgvDevedores.Margin = new Padding(12, topMarginDataGrid, 12, 12);
            dgvDevedores.Columns.Clear();
            dgvDevedores.Columns.AddRange(new DataGridViewColumn[] {
    new DataGridViewTextBoxColumn { Name = "Nome", HeaderText = "Nome", DataPropertyName = "Nome", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill },
    new DataGridViewTextBoxColumn { Name = "Turma", HeaderText = "Turma", DataPropertyName = "Turma", Width = 250 },
    new DataGridViewTextBoxColumn { Name = "Livro", HeaderText = "Livro", DataPropertyName = "Livro", Width = 250 },
    new DataGridViewTextBoxColumn { Name = "DataDevolucao", HeaderText = "Data Devolução", DataPropertyName = "DataDevolucao", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } },
    new DataGridViewTextBoxColumn { Name = "DiasAtraso", HeaderText = "Dias em Atraso", DataPropertyName = "DiasAtraso", Width = 130 }
});
            dgvDevedores.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "btnImprimir",
                HeaderText = "",
                Text = "Imprimir Carta",
                UseColumnTextForButtonValue = true,
                Width = 110,
                FlatStyle = FlatStyle.Flat
            });
            dgvDevedores.CellContentClick += dgvDevedores_CellContentClick;



            tabDevedores.Controls.Add(dgvDevedores);



            var dgvEstatEmp = CriarDataGridBasico("dgvEstatisticasEmprestimos");
            dgvEstatEmp.Margin = new Padding(12, topMarginDataGrid, 12, 12);
            dgvEstatEmp.Columns.AddRange(new DataGridViewColumn[] {
                new DataGridViewTextBoxColumn { Name = "Categoria", HeaderText = "Categoria", DataPropertyName = "Categoria", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill },
                new DataGridViewTextBoxColumn { Name = "Valor", HeaderText = "Valor", DataPropertyName = "Valor", Width = 100 },
                new DataGridViewTextBoxColumn { Name = "Detalhes", HeaderText = "Detalhes", DataPropertyName = "Detalhes", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
            });

            tabEstEmp.Controls.Add(dgvEstatEmp);



            // === Livros Populares: ajustar colunas para evitar truncamento do título ===
            var dgvLivrosPop = CriarDataGridBasico("dgvLivrosPopulares");
            dgvLivrosPop.Margin = new Padding(12, topMarginDataGrid, 12, 12);



            // Fazemos colunas com sizing misto:
            dgvLivrosPop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // gerenciamos larguras manualmente
            var colRanking = new DataGridViewTextBoxColumn { Name = "Ranking", HeaderText = "Ranking", DataPropertyName = "Posicao", Width = 100 };
            var colTitulo = new DataGridViewTextBoxColumn { Name = "Titulo", HeaderText = "Título", DataPropertyName = "Titulo", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 260 };
            var colAutor = new DataGridViewTextBoxColumn { Name = "Autor", HeaderText = "Autor", DataPropertyName = "Autor", Width = 200 };
            var colEmp = new DataGridViewTextBoxColumn { Name = "Emprestimos", HeaderText = "Empréstimos", DataPropertyName = "Emprestimos", Width = 100 };
            var colDisp = new DataGridViewTextBoxColumn { Name = "Disponibilidade", HeaderText = "Status", DataPropertyName = "Disponibilidade", Width = 220 };

            dgvLivrosPop.CellFormatting += (s, e) =>
            {
                var grid = (DataGridView)s;
                if (grid.Columns[e.ColumnIndex].Name == "Ranking" && e.Value != null)
                {
                    int pos;
                    if (int.TryParse(e.Value.ToString(), out pos))
                    {
                        if (pos == 1)
                        {
                            e.CellStyle.ForeColor = Color.Gold; // dourado
                            e.CellStyle.Font = new System.Drawing.Font(grid.Font, FontStyle.Bold);
                            e.Value = "🏆 #1"; // troféu e #1
                        }
                        else
                        {
                            e.Value = $"#{pos}";
                            e.CellStyle.ForeColor = Color.Black;
                            e.CellStyle.Font = grid.Font;
                        }
                        e.FormattingApplied = true;
                    }
                }
            };

            // adiciona na ordem
            dgvLivrosPop.Columns.AddRange(new DataGridViewColumn[] { colRanking, colTitulo, colAutor, colEmp, colDisp });

            // Depois que o controle estiver em tela, definimos colTitulo como Fill para aproveitar espaço restante.
            // (Aplicamos FillWeight para priorizar bastante espaço ao título)
            colTitulo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTitulo.FillWeight = 150; // maior proporção de espaço
            colAutor.FillWeight = 40;
            colEmp.FillWeight = 20;
            colDisp.FillWeight = 30;

            // Ajustes visuais
            colRanking.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colTitulo.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            colTitulo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colAutor.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colEmp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDisp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;




            tabLivros.Controls.Add(dgvLivrosPop);

            panel1.Controls.Add(tabEstatisticas);
            tabEstatisticas.BringToFront();

            // status inicial
            SetStatus("Pronto.");

            // aumentar e centralizar visual das labels do header: saudação e relógio
            try
            {
                lblOla.Font = new System.Drawing.Font("Segoe UI", 20F, FontStyle.Bold);   // aumentada
                lblOla.ForeColor = Color.FromArgb(30, 61, 88);

                lblRelogio.Font = new System.Drawing.Font("Segoe UI", 16F, FontStyle.Regular); // já aumentado anteriormente
                lblRelogio.ForeColor = Color.FromArgb(60, 60, 60);

                // garantir que o relógio seja centralizado no panelTop
                CenterClock();
            }
            catch { /* ignore se labels não existirem no designer */ }
        }

        private void dgvDevedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null || e.RowIndex < 0) return;

            if (dgv.Columns[e.ColumnIndex].Name == "btnImprimir")
            {
                var emprestimo = dgv.Rows[e.RowIndex].DataBoundItem as EmprestimoAtrasadoInfo;
                if (emprestimo == null)
                {
                    MessageBox.Show("Não foi possível obter os dados do empréstimo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Busca todos os livros atrasados do aluno
                var livros = this.ObterLivrosAtrasadosPorUsuario(emprestimo.UsuarioId);
                GerarCartaCobrancaPDF(emprestimo, livros);  
            }
        }

        // Helper para desenhar retângulo arredondado
        private GraphicsPath RoundedRect(System.Drawing.Rectangle bounds, int radius)
        {
            var gp = new GraphicsPath();
            int d = radius * 2;
            gp.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            gp.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            gp.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            gp.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        private Panel CriarCard(string key, string title, string subtitle, Color headerColor, int cardWidth, int cardHeight)
        {
            var card = new Panel
            {
                Width = cardWidth,
                Height = cardHeight,
                Margin = new Padding(8),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(1), // reserva 1px para a borda
                Tag = key
            };
            card.Paint += (s, e) =>
            {
                using (var p = new Pen(Color.FromArgb(235, 239, 244)))
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
            };

            var header = new Panel { BackColor = headerColor, Height = 36, Dock = DockStyle.Top, Margin = new Padding(0) };
            card.Controls.Add(header);

            var lblTitle = new Label
            {
                Text = title,
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                Location = new Point(10, 6),
                AutoSize = true
            };
            header.Controls.Add(lblTitle);

            // ---------- Card padrão ----------
            if (!string.Equals(key, "TopUsuario", StringComparison.OrdinalIgnoreCase))
            {
                var lblValue = new Label
                {
                    Name = "val_" + key,
                    Text = "0",
                    ForeColor = Color.FromArgb(20, 42, 60),
                    Font = new System.Drawing.Font("Segoe UI", 20F, FontStyle.Bold),
                    Location = new Point(12, header.Bottom + 6),
                    AutoSize = false,
                    Size = new Size(card.Width - 24, 36)
                };
                card.Controls.Add(lblValue);

                var lblSub = new Label
                {
                    Text = subtitle,
                    ForeColor = Color.Gray,
                    Font = new System.Drawing.Font("Segoe UI", 8.5F),
                    Location = new Point(12, lblValue.Bottom + 2),
                    AutoSize = true
                };
                card.Controls.Add(lblSub);
            }
            else
            {
                // ---------- Card Top Usuário com TableLayoutPanel para vertical centering ----------
                var container = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    Margin = new Padding(0),
                    Padding = new Padding(8, 43, 8, 0) // leve padding interno
                };

                var table = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    ColumnCount = 1,
                    RowCount = 4
                };
                // linhas: flex (30%), nome (Auto), turma (Auto), flex (30%)
                table.RowStyles.Clear();
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 30f));
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 30f));

                // Label do nome: dois primeiros nomes (maior)
                var lblName = new Label
                {
                    Name = "lblTopName",
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new System.Drawing.Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(20, 42, 60),
                    Text = "-",
                    MaximumSize = new Size(cardWidth - 40, 0) // evita overflow horizontal
                };
                // centralizar horizontalmente na célula
                lblName.Anchor = AnchorStyles.None;

                // Label da turma: menor e logo abaixo do nome (mais próximo)
                var lblTurma = new Label
                {
                    Name = "lblTopTurma",
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Regular),
                    ForeColor = Color.Gray,
                    Text = "",
                    MaximumSize = new Size(cardWidth - 40, 0),
                    
                };
                lblTurma.Anchor = AnchorStyles.None;

                // Subtítulo embaixo (quem mais emprestou)
                var lblSub = new Label
                {
                    Text = subtitle,
                    Dock = DockStyle.Bottom,
                    Height = 35,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Gray,
                    Font = new System.Drawing.Font("Segoe UI", 8.5F)
                };

                // adicionar aos lugares corretos: row 1 = nome, row 2 = turma
                table.Controls.Add(lblName, 0, 1);
                table.Controls.Add(lblTurma, 0, 2);

                container.Controls.Add(table);
                container.Controls.Add(lblSub); // ficará embaixo do container (Dock = Bottom do lblSub)

                card.Controls.Add(container);
            }

            return card;
        }







        private DataGridView CriarDataGridBasico(string name)
        {
            var dgv = new DataGridView
            {
                Name = name,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 36 },
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                GridColor = Color.FromArgb(245, 245, 245),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                Margin = new Padding(12)
            };

            AplicarEstiloDataGridView(dgv);
            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E7EEF7");
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 42, 60);


            dgv.CellMouseEnter += DataGrid_CellMouseEnter;
            dgv.CellMouseLeave += DataGrid_CellMouseLeave;

            return dgv;
        }

        private void AplicarEstiloDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Color.FromArgb(245, 245, 245);

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#E7EEF7");
            dgv.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#123A5D");
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.DefaultCellStyle.Padding = new Padding(8, 6, 8, 6);

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(70, 70, 70);
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;



            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgv.ColumnHeadersDefaultCellStyle.BackColor;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgv.ColumnHeadersDefaultCellStyle.ForeColor;

            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;

            try
            {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null, dgv, new object[] { true });
            }
            catch { }

            dgv.CellFormatting += (s, e) =>
            {
                try
                {
                    var grid = (DataGridView)s;
                    if (grid.Columns[e.ColumnIndex].Name == "Disponibilidade")
                    {
                        string v = e.Value?.ToString() ?? "";
                        if (v == "Disponível")
                        {
                            e.CellStyle.BackColor = Color.FromArgb(220, 245, 225);
                            e.CellStyle.ForeColor = Color.FromArgb(20, 120, 40);
                            e.Value = "●  " + v;
                        }
                        else if (v == "Poucas unidades")
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 250, 220);
                            e.CellStyle.ForeColor = Color.FromArgb(150, 110, 20);
                            e.Value = "●  " + v;
                        }
                        else
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 235, 235);
                            e.CellStyle.ForeColor = Color.FromArgb(160, 30, 30);
                            e.Value = "●  " + v;
                        }
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                catch { }
            };
        }

        private void DataGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                var dgv = sender as DataGridView;
                if (dgv == null) return;
                var hoverColor = Color.FromArgb(245, 248, 251);
                dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = hoverColor;
            }
            catch { }
        }

        private void DataGrid_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                var dgv = sender as DataGridView;
                if (dgv == null) return;
                var row = dgv.Rows[e.RowIndex];
                row.DefaultCellStyle.BackColor = (e.RowIndex % 2 == 0) ? Color.White : Color.FromArgb(250, 250, 250);
            }
            catch { }
        }

        private void EnsureEmptyOverlay(DataGridView dgv, string message)
        {
            var name = "emptyOverlay_" + dgv.Name;
            var parent = dgv.Parent ?? panel1;
            var existing = parent.Controls.Find(name, true).FirstOrDefault() as Label;

            if (dgv.Rows.Count == 0)
            {
                if (existing == null)
                {
                    var lbl = new Label
                    {
                        Name = name,
                        Text = message,
                        AutoSize = false,
                        Size = dgv.Size,
                        Location = dgv.Location,
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.Gray,
                        BackColor = Color.Transparent,
                        Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular)
                    };
                    parent.Controls.Add(lbl);
                    lbl.BringToFront();
                }
                else
                {
                    existing.Text = message;
                    existing.Visible = true;
                    existing.Location = dgv.Location;
                    existing.Size = dgv.Size;
                    existing.BringToFront();
                }
            }
            else if (existing != null)
            {
                existing.Visible = false;
            }
        }

        private List<EmprestimoAtrasadoInfo> ObterEmprestimosAtrasados()
        {
            var lista = new List<EmprestimoAtrasadoInfo>();
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    // agora trazemos também u.Id (UsuarioId)
                    string sql = @"
SELECT 
    e.Id,
    u.Id AS UsuarioId,
    u.Nome,
    u.Turma,
    l.Nome AS Livro,
    COALESCE(e.DataProrrogacao, e.DataDevolucao) AS DataLimite,
    DATEDIFF(day, COALESCE(e.DataProrrogacao, e.DataDevolucao), GETDATE()) AS DiasAtraso
FROM Emprestimo e
INNER JOIN Usuarios u ON e.Alocador = u.Id
INNER JOIN Livros l ON e.Livro = l.Id
WHERE e.Status <> 'Devolvido'
  AND COALESCE(e.DataProrrogacao, e.DataDevolucao) < GETDATE()
ORDER BY DiasAtraso DESC, DataLimite ASC";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new EmprestimoAtrasadoInfo
                            {
                                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                UsuarioId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                Nome = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                Turma = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                Livro = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                DataDevolucao = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5),
                                DiasAtraso = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var logDir = Path.Combine(Application.StartupPath, "logs");
                    Directory.CreateDirectory(logDir);
                    File.AppendAllText(Path.Combine(logDir, "inicio_obter_devedores.log"),
                        DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                }
                catch { }
            }
            return lista;
        }

        private void AtualizarRelogio()
        {
            DateTime agora = DateTime.Now;
            string saudacao = ObterSaudacao(agora);
            try
            {
                string nomeCompleto = Sessao.NomeBibliotecariaLogada ?? "";
                string primeiroNome = nomeCompleto.Split(' ').FirstOrDefault() ?? "";
                lblOla.Text = $"{saudacao}, {primeiroNome}!";
                lblOla.Font = new System.Drawing.Font("Segoe UI", 20F, FontStyle.Bold);
                lblOla.ForeColor = Color.FromArgb(30, 61, 88);
            }
            catch { }

            string diaSemana = agora.ToString("dddd");
            string data = agora.ToString("dd 'de' MMMM 'de' yyyy");
            string hora = agora.ToString("HH:mm:ss");

            lblRelogio.Text = $"{diaSemana}, {data} - {hora}";
            lblRelogio.Font = new System.Drawing.Font("Segoe UI", 16F, FontStyle.Regular);
            lblRelogio.ForeColor = Color.FromArgb(60, 60, 60);

            // centralizar relógio após atualizar o texto
            CenterClock();
        }

        // centraliza lblRelogio horizontalmente dentro do panelTop (se existir)
        private void CenterClock()
        {
            try
            {
                if (lblRelogio == null || panelTop == null) return;
                // força medida atualizada
                lblRelogio.AutoSize = true;
                lblRelogio.Refresh();
                int centerX = Math.Max(0, (panelTop.ClientSize.Width - lblRelogio.Width) / 2);
                // respeitar margem superior (aprox. mesma Y atual)
                int y = lblRelogio.Location.Y;
                lblRelogio.Location = new Point(centerX, y);
            }
            catch { }
        }

        private string ObterSaudacao(DateTime now)
        {
            int hora = now.Hour;
            if (hora >= 5 && hora < 12) return "Bom dia";
            if (hora >= 12 && hora < 18) return "Boa tarde";
            return "Boa noite";
        }

        private void timerRelogio_Tick(object sender, EventArgs e) => AtualizarRelogio();

        #region Carregamento de dados
        private async Task CarregarEstatisticasAsync()
        {
            try
            {
                SetStatus("Carregando estatísticas...");
                var stats = await Task.Run(() => ObterEstatisticas());

                var topUser = await Task.Run(() => ObterTopUsuarioMaisEmprestimos());
                topUsuarioMaisEmprestimos = topUser;

                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    BeginInvoke(new Action(() =>
                    {
                        AtualizarCards(stats);

                        // Atualiza o card do Top Usuário
                        // Atualiza o card do Top Usuário
                        // Atualiza o card do Top Usuário
                        var card = flowCards.Controls.OfType<Panel>().FirstOrDefault(p => (string)p.Tag == "TopUsuario");
                        if (card != null)
                        {
                            var lblName = card.Controls.Find("lblTopName", true).FirstOrDefault() as Label;
                            var lblTurma = card.Controls.Find("lblTopTurma", true).FirstOrDefault() as Label;

                            if (lblName != null && lblTurma != null)
                            {
                                if (!string.IsNullOrWhiteSpace(topUsuarioMaisEmprestimos.Nome))
                                {
                                    // Pega os dois primeiros nomes
                                    var matches = System.Text.RegularExpressions.Regex.Matches(topUsuarioMaisEmprestimos.Nome.Trim(), @"\S+");
                                    var primeiros = matches.Cast<System.Text.RegularExpressions.Match>().Select(m => m.Value).Take(2).ToArray();
                                    var doisNomes = string.Join(" ", primeiros);

                                    // Normaliza a turma: remove quebras de linha e espaços duplicados
                                    string turmaDisplay = topUsuarioMaisEmprestimos.Turma?.Trim() ?? "";
                                    turmaDisplay = turmaDisplay.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                                    turmaDisplay = System.Text.RegularExpressions.Regex.Replace(turmaDisplay, @"\s+", " ");

                                    // Trunca visualmente se muito longa
                                    if (turmaDisplay.Length > 25)
                                        turmaDisplay = turmaDisplay.Substring(0, 25) + "...";

                                    // Exibe os valores
                                    lblName.Text = doisNomes;
                                    lblTurma.Text = string.IsNullOrWhiteSpace(turmaDisplay) ? "" : turmaDisplay;
                                    lblTurma.Visible = !string.IsNullOrWhiteSpace(turmaDisplay);

                                    // Tooltip detalhado (nome completo + turma + qtd)
                                    try
                                    {
                                        if (formToolTip == null) formToolTip = new ToolTip();
                                        string tip = $"{topUsuarioMaisEmprestimos.Nome}";
                                        if (!string.IsNullOrWhiteSpace(topUsuarioMaisEmprestimos.Turma))
                                            tip += $" • Turma: {topUsuarioMaisEmprestimos.Turma}";
                                        tip += $" • {topUsuarioMaisEmprestimos.Qtd} empréstimos";
                                        formToolTip.SetToolTip(lblName, tip);
                                        formToolTip.SetToolTip(lblTurma, tip);
                                    }
                                    catch { }
                                }
                                else
                                {
                                    lblName.Text = "-";
                                    lblTurma.Text = "";
                                    lblTurma.Visible = false;
                                }
                            }
                        }



                    }));
                }


                var estatisticasEmprestimos = await Task.Run(() => ObterEstatisticasEmprestimos());
                var livrosPopulares = await Task.Run(() => ObterLivrosPopulares(10));

                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    BeginInvoke(new Action(() =>
                    {
                        TabControl tabControl = panel1.Controls.Find("tabEstatisticas", true).FirstOrDefault() as TabControl;
                        if (tabControl != null && tabControl.TabPages.Count >= 3)
                        {
                            DataGridView dgvDevedores = tabControl.TabPages[0].Controls.Find("dgvDevedores", true).FirstOrDefault() as DataGridView;
                            DataGridView dgvEstatisticas = tabControl.TabPages[1].Controls.Find("dgvEstatisticasEmprestimos", true).FirstOrDefault() as DataGridView;
                            DataGridView dgvLivros = tabControl.TabPages[2].Controls.Find("dgvLivrosPopulares", true).FirstOrDefault() as DataGridView;

                            if (dgvDevedores != null)
                            {
                                var emprestimosAtrasados = ObterEmprestimosAtrasados();
                                dgvDevedores.DataSource = null;
                                dgvDevedores.DataSource = emprestimosAtrasados;
                                dgvDevedores.ClearSelection();
                                dgvDevedores.Refresh();
                                EnsureEmptyOverlay(dgvDevedores, "Nenhum empréstimo atrasado no momento.");
                            }
                            if (dgvEstatisticas != null)
                            {
                                dgvEstatisticas.DataSource = null;
                                dgvEstatisticas.DataSource = estatisticasEmprestimos;
                                dgvEstatisticas.ClearSelection();
                                dgvEstatisticas.Refresh();
                            }
                            if (dgvLivros != null)
                            {
                                dgvLivros.DataSource = null;
                                dgvLivros.DataSource = livrosPopulares;
                                dgvLivros.ClearSelection();
                                dgvLivros.Refresh();
                                EnsureEmptyOverlay(dgvLivros, "Nenhum livro encontrado por enquanto");
                            }
                        }

                        SetStatus($"Última atualização: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    }));
                }
            }
            catch (Exception ex)
            {
                BeginInvoke(new Action(() => SetStatus($"Erro ao carregar: {ex.Message}")));
            }
        }
        /// <summary>
        /// Retorna o usuário (exceto professores) que mais realizou empréstimos (apenas tabela Emprestimo).
        /// </summary>
        private (int Id, string Nome, string Turma, int Qtd) ObterTopUsuarioMaisEmprestimos()
        {
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sql = @"
SELECT TOP 1 
    u.Id, 
    u.Nome, 
    COALESCE(u.Turma, '') AS Turma, 
    COUNT(e.Id) AS Qtd,
    MAX(e.DataEmprestimo) AS UltimoEmprestimo
FROM Emprestimo e
INNER JOIN Usuarios u ON e.Alocador = u.Id
WHERE (u.TipoUsuario IS NULL OR UPPER(u.TipoUsuario) NOT LIKE 'PROFESSOR%')
GROUP BY u.Id, u.Nome, u.Turma
ORDER BY Qtd DESC, UltimoEmprestimo DESC;
";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            string nome = reader.IsDBNull(1) ? "-" : reader.GetString(1);
                            string turma = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            int qtd = reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader.GetValue(3));
                            return (id, nome, turma, qtd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var logDir = Path.Combine(Application.StartupPath, "logs");
                    Directory.CreateDirectory(logDir);
                    File.AppendAllText(Path.Combine(logDir, "inicio_obter_top_usuario.log"),
                        DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                }
                catch { }
            }
            return (0, "-", "", 0);
        }





        #region Métodos de obtenção (mantidos do seu código original)
        private List<EstatisticaEmprestimo> ObterEstatisticasEmprestimos()
        {
            var lista = new List<EstatisticaEmprestimo>();
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo", conexao))
                        lista.Add(new EstatisticaEmprestimo { Categoria = "Empréstimos Totais", Valor = Convert.ToInt32(cmd.ExecuteScalar() ?? 0), Detalhes = "Desde o início do sistema" });

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status <> 'Devolvido'", conexao))
                        lista.Add(new EstatisticaEmprestimo { Categoria = "Empréstimos Ativos", Valor = Convert.ToInt32(cmd.ExecuteScalar() ?? 0), Detalhes = "Aguardando devolução" });

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Atrasado'", conexao))
                        lista.Add(new EstatisticaEmprestimo { Categoria = "Empréstimos Atrasados", Valor = Convert.ToInt32(cmd.ExecuteScalar() ?? 0), Detalhes = "Fora do prazo de devolução" });

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Devolvido' AND DataRealDevolucao <= DataDevolucao", conexao))
                    {
                        int noPrazo = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        int totalDevolvidos = 0;
                        using (var cmd2 = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Devolvido'", conexao))
                        {
                            totalDevolvidos = Convert.ToInt32(cmd2.ExecuteScalar() ?? 0);
                        }
                        double taxa = totalDevolvidos > 0 ? (noPrazo * 100.0 / totalDevolvidos) : 0;
                        lista.Add(new EstatisticaEmprestimo { Categoria = "Devolução no Prazo", Valor = Math.Round(taxa, 1), Detalhes = $"{noPrazo} de {totalDevolvidos} empréstimos devolvidos" });
                    }
                }
            }
            catch (Exception ex)
            {
                try { var logDir = Path.Combine(Application.StartupPath, "logs"); Directory.CreateDirectory(logDir); File.AppendAllText(Path.Combine(logDir, "inicio_obter_estatisticas.log"), DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine); } catch { }
            }
            return lista;
        }

        private List<LivroPopular> ObterLivrosPopulares(int topN)
        {
            var lista = new List<LivroPopular>();
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Janela de popularidade (ajuste se quiser outro período)
                    DateTime inicioJanela = DateTime.Now.AddMonths(-12);

                    string sql = $@"
SELECT TOP {topN}
    l.Nome,
    l.Autor,
    COUNT(e.Id) AS TotalEmprestimos,
    l.Quantidade
FROM Livros l
INNER JOIN Emprestimo e 
    ON l.Id = e.Livro
    AND e.DataEmprestimo >= @inicio
WHERE 
    (l.Genero IS NULL OR (l.Genero NOT LIKE 'Didático%' AND l.Genero NOT LIKE 'Didatico%'))
GROUP BY l.Id, l.Nome, l.Autor, l.Quantidade
ORDER BY TotalEmprestimos DESC, l.Nome";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@inicio", inicioJanela);

                        using (var reader = cmd.ExecuteReader())
                        {
                            int posicao = 1;
                            while (reader.Read())
                            {
                                var nome = reader.IsDBNull(0) ? "" : reader.GetString(0);
                                var autor = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                int emprestimos = 0;
                                int quantidade = 0;
                                try { emprestimos = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetValue(2)); } catch { }
                                try { quantidade = reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader.GetValue(3)); } catch { }

                                string disponibilidade = quantidade > 0 ? "Disponível" : "Indisponível";
                                if (quantidade > 0 && quantidade < 5) disponibilidade = "Poucas unidades";

                                lista.Add(new LivroPopular
                                {
                                    Posicao = posicao++,
                                    Titulo = nome,
                                    Autor = autor,
                                    Emprestimos = emprestimos,
                                    Disponibilidade = disponibilidade
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var logDir = Path.Combine(Application.StartupPath, "logs");
                    Directory.CreateDirectory(logDir);
                    File.AppendAllText(Path.Combine(logDir, "inicio_obter_livros.log"),
                        DateTime.Now + " - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                }
                catch { }
            }
            return lista;
        }

        public class EstatisticaEmprestimo { public string Categoria { get; set; } public double Valor { get; set; } public string Detalhes { get; set; } }
        public class LivroPopular { public int Posicao { get; set; } public string Titulo { get; set; } public string Autor { get; set; } public int Emprestimos { get; set; } public string Disponibilidade { get; set; } }

        private void SetStatus(string texto)
        {
            if (lblStatusSmall == null) return;
            try { BeginInvoke(new Action(() => lblStatusSmall.Text = texto)); } catch { }
        }

        private Dictionary<string, int> ObterEstatisticas()
        {
            var dict = new Dictionary<string, int>
            {
                ["Usuarios"] = 0,
                ["Livros"] = 0,
                ["EmpAtivos"] = 0,
                ["Atrasados"] = 0,
                ["RapidosHoje"] = 0,
                ["MediaMes"] = 0 // novo campo
            };

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Usuarios", conexao))
                        dict["Usuarios"] = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                    
                    using (var cmd = new SqlCeCommand("SELECT COALESCE(SUM(Quantidade), 0) FROM Livros", conexao))
                    {
                        var result = cmd.ExecuteScalar();
                        dict["Livros"] = result != null ? Convert.ToInt32(result) : 0;
                    }

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status <> 'Devolvido'", conexao))
                        dict["EmpAtivos"] = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                    using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM Emprestimo WHERE Status = 'Atrasado'", conexao))
                        dict["Atrasados"] = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                    using (var cmd = new SqlCeCommand("SELECT Id, DataHoraEmprestimo FROM EmprestimoRapido WHERE Status = 'Ativo'", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        int cont = 0;
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(1))
                            {
                                var dt = reader.GetDateTime(1);
                                if (dt.Date == DateTime.Now.Date) cont++;
                            }
                        }
                        dict["RapidosHoje"] = cont;
                    }

                    // Média de empréstimos do mês atual
                    var primeiroDia = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);
                    using (var cmd = new SqlCeCommand(
                        "SELECT COUNT(*) FROM Emprestimo WHERE DataEmprestimo >= @inicio AND DataEmprestimo <= @fim", conexao))
                    {
                        cmd.Parameters.AddWithValue("@inicio", primeiroDia);
                        cmd.Parameters.AddWithValue("@fim", ultimoDia);
                        int totalMes = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        int dias = DateTime.Now.Day;
                        dict["MediaMes"] = dias > 0 ? (int)Math.Round(totalMes / (double)dias) : 0;
                    }
                }
            }
            catch { /*silent*/ }

            return dict;
        }



        private void AtualizarCards(Dictionary<string, int> stats)
        {
            if (stats == null) return;
            foreach (Control c in flowCards.Controls)
            {
                if (c is Panel card && card.Tag is string key)
                {
                    // não sobrescrever o card "TopUsuario" aqui (ele mostra texto, não número)
                    if (string.Equals(key, "TopUsuario", StringComparison.OrdinalIgnoreCase))
                        continue;

                    var valLabel = card.Controls.Find("val_" + key, true).FirstOrDefault() as Label;
                    if (valLabel != null)
                    {
                        int v = stats.ContainsKey(key) ? stats[key] : 0;
                        valLabel.Text = v.ToString("N0");
                    }
                }
            }
        }


        #endregion

        #region EmprestimoRapido open as MDI child (com toggle de botões do MainForm)
        private void BtnEmprestimoRapido_Click(object sender, EventArgs e)
        {
            AbrirEmprestimoRapido();
        }

        private async void AbrirEmprestimoRapido()
        {
            try
            {
                MainForm mainForm = this.MdiParent as MainForm;
                if (mainForm == null)
                {
                    MessageBox.Show("Não foi possível identificar a janela principal (MainForm).",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 🔥 chama a animação e clique — MAS só expande o menu de livros se ele ainda NÃO estiver expandido
                // (evita fechar o submenu caso já esteja aberto)
                if (!mainForm.IsLivroExpanded && !mainForm.IsMenuAnimating)
                {
                    mainForm.btnLivro_Click(null, EventArgs.Empty);
                }

                // abrir empréstimo rápido sempre (depois de garantir que submenu de livros esteja aberto)
                mainForm.btnEmprestimoRap_Click(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir Empréstimo Rápido: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void EmprestimoRap_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Form main = this.FindForm();
                if (main == null) main = this.MdiParent;
                if (main == null) return;

                foreach (var kv in mainButtonsOriginalState) SetButtonEnabledOnForm(main, kv.Key, kv.Value);

                emprestimoRap = null;
                _ = CarregarEstatisticasAsync();
            }
            catch { /*silent*/ }
        }

        private Control FindControlOnForm(Form form, string controlName)
        {
            if (form == null || string.IsNullOrWhiteSpace(controlName)) return null;
            var found = form.Controls.Find(controlName, true);
            if (found != null && found.Length > 0) return found[0];
            var t = form.GetType();
            var field = t.GetField(controlName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null)
            {
                var val = field.GetValue(form);
                if (val is Control c) return c;
            }
            var prop = t.GetProperty(controlName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (prop != null)
            {
                var val = prop.GetValue(form);
                if (val is Control c2) return c2;
            }
            return null;
        }

        private void SetButtonEnabledOnForm(Form form, string controlName, bool enabled)
        {
            var ctrl = FindControlOnForm(form, controlName);
            if (ctrl != null) try { ctrl.Enabled = enabled; } catch { }
        }

        private void InicioForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                e.SuppressKeyPress = true;
                AbrirEmprestimoRapido();
            }
        }
        #endregion

        private void lblResultado_Click(object sender, EventArgs e) { }

        private void BtnImprimirCarta_Click(object sender, EventArgs e)
        {
            // Localiza o DataGrid de Devedores
            var tabControl = panel1.Controls.Find("tabEstatisticas", true).FirstOrDefault() as TabControl;
            if (tabControl == null) return;
            var dgvDevedores = tabControl.TabPages[0].Controls.Find("dgvDevedores", true).FirstOrDefault() as DataGridView;
            if (dgvDevedores == null || dgvDevedores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um empréstimo atrasado na lista para imprimir a carta.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var emprestimo = dgvDevedores.SelectedRows[0].DataBoundItem as EmprestimoAtrasadoInfo;
            if (emprestimo == null)
            {
                MessageBox.Show("Não foi possível obter os dados do empréstimo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Buscar os livros em atraso do aluno
            var livros = this.ObterLivrosAtrasadosPorUsuario(emprestimo.UsuarioId);
            GerarCartaCobrancaPDF(emprestimo, livros);
        }

        private void GerarCartaCobrancaPDF(EmprestimoAtrasadoInfo devedor, List<(int Id, string Nome, string Autor)> livros)
        {
            // Buscar telefone do usuário (agora por Id)
            string telefone = "";
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"SELECT Telefone FROM Usuarios WHERE Id = @id";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", devedor.UsuarioId);
                        var result = cmd.ExecuteScalar();
                        telefone = result != null ? result.ToString() : "";
                    }
                }
            }
            catch { telefone = ""; }

            // Nome de arquivo seguro
            string safeName = string.IsNullOrWhiteSpace(devedor?.Nome) ? "Carta_Cobranca" : string.Concat(devedor.Nome.Split(Path.GetInvalidFileNameChars()));
            var dlg = new SaveFileDialog
            {
                Filter = "PDF (*.pdf)|*.pdf",
                FileName = $"Carta_Cobranca_{safeName}.pdf"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            // Criação do documento PDF
            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
            using (var fs = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Fontes
                var fontTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 11);
                var fontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                var fontSmall = FontFactory.GetFont(FontFactory.HELVETICA, 9);

                // Cabeçalho com brasões
                iTextSharp.text.Image imgEsq = null, imgDir = null;
                using (var ms = new MemoryStream())
                {
                    Properties.Resources.Brasao_mg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    imgEsq = iTextSharp.text.Image.GetInstance(ms.ToArray());
                    imgEsq.ScaleAbsolute(48f, 48f);
                    imgEsq.Alignment = Element.ALIGN_LEFT;
                }
                using (var ms = new MemoryStream())
                {
                    Properties.Resources.brasao_Gastao_Black.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    imgDir = iTextSharp.text.Image.GetInstance(ms.ToArray());
                    imgDir.ScaleAbsolute(48f, 48f);
                    imgDir.Alignment = Element.ALIGN_RIGHT;
                }

                var headerTable = new PdfPTable(3) { WidthPercentage = 100 };
                headerTable.SetWidths(new float[] { 1.2f, 5f, 1.2f });

                var cellImgEsq = new PdfPCell { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE };
                if (imgEsq != null) cellImgEsq.AddElement(imgEsq);
                headerTable.AddCell(cellImgEsq);

                var cellTitle = new PdfPCell(new Phrase("ESCOLA ESTADUAL PROFESSOR GASTÃO VALLE   EEPGV", fontTitle))
                { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };
                headerTable.AddCell(cellTitle);

                var cellImgDir = new PdfPCell { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE };
                if (imgDir != null) cellImgDir.AddElement(imgDir);
                headerTable.AddCell(cellImgDir);
                headerTable.SpacingAfter = 30f;
                doc.Add(headerTable);

                // Corpo
                doc.Add(new Paragraph("Eu ____________________________________________________________", fontNormal) { SpacingAfter = 20f });

                var pCompromisso = new Paragraph();
                pCompromisso.Add(new Chunk("Assumo a responsabilidade de devolver todos os livros (descritos abaixo) que estão em meu poder. Ciente que a ", fontNormal));
                pCompromisso.Add(new Chunk("não devolução", fontBold));
                pCompromisso.Add(new Chunk(" dos mesmos, poderá gerar pendência na liberação da bibliotecária, podendo impedir a expedição dos meus documentos quando for solicitado.", fontNormal));
                pCompromisso.SpacingAfter = 18f;
                doc.Add(pCompromisso);

                // Livros
                doc.Add(new Paragraph("Livro:", fontBold) { SpacingAfter = 6f });
                if (livros == null || livros.Count == 0)
                {
                    doc.Add(new Paragraph("Nenhum livro em atraso encontrado.", fontSmall) { SpacingAfter = 13f });
                }
                else
                {
                    foreach (var livro in livros)
                    {
                        doc.Add(new Paragraph($"ID: {livro.Id}", fontNormal));
                        doc.Add(new Paragraph($"Nome: {livro.Nome}", fontNormal));
                        doc.Add(new Paragraph($"Autor: {livro.Autor}", fontNormal) { SpacingAfter = 14f });
                    }
                }
                for (int i = (livros?.Count ?? 0); i < 4; i++)
                    doc.Add(new Paragraph("________________________________________________________________________________", fontNormal) { SpacingAfter = 3f });

                doc.Add(new Paragraph("\n", fontNormal));

                // ===== Bloco Único: Esquerda (Nome/Turma/Data/Contato) + Direita (QR Code) =====
                // Normalizador de telefone para wa.me (adiciona DDI 55 quando apropriado)
                string NormalizePhoneForWa(string phone)
                {
                    if (string.IsNullOrWhiteSpace(phone)) return null;
                    var digits = new string(phone.Where(char.IsDigit).ToArray());
                    if (string.IsNullOrEmpty(digits)) return null;
                    // já tem DDI Brasil?
                    if (digits.StartsWith("55")) return digits;
                    // números locais com 10 ou 11 dígitos: adiciona 55
                    if (digits.Length == 10 || digits.Length == 11) return "55" + digits;
                    // se estiver no formato internacional mas sem 55, tenta usar como está
                    if (digits.Length > 4) return digits;
                    return null;
                }

                var infoTable = new PdfPTable(2) { WidthPercentage = 100, SpacingBefore = 4f, SpacingAfter = 26f };
                infoTable.SetWidths(new float[] { 3.6f, 1.4f });

                var leftCell = new PdfPCell { Border = iTextSharp.text.Rectangle.NO_BORDER, PaddingRight = 10f, VerticalAlignment = Element.ALIGN_MIDDLE };

                // Montagem condicional: se não houver turma, mostrar só "Nome: {Nome}".
                // Se houver turma, mostrar "Nome: {Nome}    Turma: {Turma}    Turno: ______"
                var pInfoTopo = new Paragraph();
                string nomeDisplay = string.IsNullOrWhiteSpace(devedor?.Nome) ? "________________" : devedor.Nome;
                pInfoTopo.Add(new Chunk("Nome: ", fontNormal));
                pInfoTopo.Add(new Chunk(nomeDisplay + "    ", fontBold));

                if (!string.IsNullOrWhiteSpace(devedor?.Turma))
                {
                    pInfoTopo.Add(new Chunk("Turma: ", fontNormal));
                    pInfoTopo.Add(new Chunk(devedor.Turma + "    ", fontBold));
                    pInfoTopo.Add(new Chunk("Turno: ___________", fontNormal));
                }

                var dataAtual = DateTime.Now;
                var pData = new Paragraph($"Data da comunicação: ____/ {dataAtual:MM/yyyy}", fontNormal);
                string contatoStr = "Contato do Aluno/Responsável: " + (string.IsNullOrWhiteSpace(telefone) ? "___________________________" : telefone);
                var pContato = new Paragraph(contatoStr, fontNormal);

                leftCell.AddElement(pInfoTopo);
                leftCell.AddElement(new Paragraph(" ", fontSmall)); // pequeno espaçamento
                leftCell.AddElement(pData);
                leftCell.AddElement(new Paragraph(" ", fontSmall));
                leftCell.AddElement(pContato);
                infoTable.AddCell(leftCell);

                // Direita: somente QR Code (maior) ou vazio se não houver telefone
                var rightCell = new PdfPCell { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE };
                var waNumber = NormalizePhoneForWa(telefone);
                if (!string.IsNullOrEmpty(waNumber))
                {
                    // Monta mensagem do WhatsApp usando a Versão 2 (cordial)
                    string turmaParaMsg = string.IsNullOrWhiteSpace(devedor?.Turma) ? "" : $" da Turma {devedor.Turma}";
                    string nomeParaMsg = string.IsNullOrWhiteSpace(devedor?.Nome) ? "responsável" : devedor.Nome;
                    string livroParaMsg = string.IsNullOrWhiteSpace(devedor?.Livro) ? "livro" : $@"o livro *""{devedor.Livro}""*";

                    // Versão 2: cordial e direta
                    string msg = $"📚 Biblioteca Gastão Valle\nOlá! Tudo bem? Verificamos que {livroParaMsg}, emprestado em nome de *{nomeParaMsg}*{turmaParaMsg}, está com atraso na devolução.\nPodemos conversar sobre isso?";
                    string waUrl = $"https://wa.me/{waNumber}?text={Uri.EscapeDataString(msg)}";

                    var qrWriter = new ZXing.BarcodeWriter
                    {
                        Format = ZXing.BarcodeFormat.QR_CODE,
                        Options = new QrCodeEncodingOptions
                        {
                            Width = 280,
                            Height = 280,
                            Margin = 1,
                            CharacterSet = "UTF-8",
                            ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M
                        }
                    };

                    using (var qrBmp = qrWriter.Write(waUrl))
                    using (var msQr = new MemoryStream())
                    {
                        qrBmp.Save(msQr, System.Drawing.Imaging.ImageFormat.Png);
                        var qrImg = iTextSharp.text.Image.GetInstance(msQr.ToArray());
                        qrImg.ScaleAbsolute(106f, 106f); // tamanho maior
                        qrImg.Alignment = Element.ALIGN_RIGHT;
                        rightCell.AddElement(qrImg);
                    }
                }
                // se waNumber for null/empty, rightCell fica vazio (sem QR)
                infoTable.AddCell(rightCell);

                doc.Add(infoTable);
                // ===== Fim do bloco único =====

                // Assinatura
                doc.Add(new Paragraph("___________________________________________________", fontNormal) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 2f });
                doc.Add(new Paragraph("ASSINATURA DO ALUNO/RESPONSÁVEL", fontNormal) { Alignment = Element.ALIGN_CENTER });

                doc.Close();
            }

            MessageBox.Show("Carta de cobrança gerada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            try { Process.Start(dlg.FileName); } catch { }
        }




        private List<(int Id, string Nome, string Autor)> ObterLivrosAtrasadosPorUsuario(int usuarioId)
        {
            var livros = new List<(int, string, string)>();
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"
SELECT l.Id, l.Nome, l.Autor
FROM Emprestimo e
INNER JOIN Livros l ON e.Livro = l.Id
WHERE e.Alocador = @usuarioId
  AND e.Status <> 'Devolvido'
  AND COALESCE(e.DataProrrogacao, e.DataDevolucao) < GETDATE()";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                string nome = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                string autor = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                livros.Add((id, nome, autor));
                            }
                        }
                    }
                }
            }
            catch { }
            return livros;
        }
    }
}

#endregion