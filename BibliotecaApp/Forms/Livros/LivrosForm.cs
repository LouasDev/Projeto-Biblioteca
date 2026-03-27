using BibliotecaApp.Utils;
using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace BibliotecaApp.Forms.Livros
{

    public partial class LivrosForm : Form
    {
        #region Construtor e Inicialização

        private Timer filtroTimer;

        // Inicializa uma nova instância do formulário LivrosForm
        public LivrosForm()
        {
            InitializeComponent();

            dgvLivros.CellFormatting += dgvLivros_CellFormatting;

            mtxCodigoBarras.KeyPress += mtxCodigoBarras_KeyPressLimiter;
            mtxCodigoBarras.TextChanged += mtxCodigoBarras_TextChangedLimiter;

            btnProcurar.PerformClick();

            // Assina o evento global para atualizar a lista automaticamente
            BibliotecaApp.Utils.EventosGlobais.LivroCadastradoOuAlterado += (s, e) => CarregarLivros();

            filtroTimer = new Timer();
            filtroTimer.Interval = 250; // 250ms de espera
            filtroTimer.Tick += FiltroTimer_Tick;

            // Assina o evento TextChanged
            txtNome.TextChanged += TxtNome_TextChanged;
        }

        #endregion

        #region Métodos Utilitários

        /// <summary>
        /// Configura aparência e colunas do DataGridView de livros
        /// </summary>
        private void ConfigurarGridLivros()
        {
            dgvLivros.SuspendLayout();

            dgvLivros.AutoGenerateColumns = false;
            dgvLivros.Columns.Clear();
            dgvLivros.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewTextBoxColumn AddTextCol(string dataProp, string header, float fillWeight, DataGridViewContentAlignment align, int minWidth = 60)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = dataProp,
                    Name = dataProp,
                    HeaderText = header,
                    ReadOnly = true,
                    FillWeight = fillWeight,
                    MinimumWidth = minWidth,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = align, WrapMode = DataGridViewTriState.False }
                };
                dgvLivros.Columns.Add(col);
                return col;
            }

            AddTextCol("Id", "ID", 50, DataGridViewContentAlignment.MiddleCenter, 40);
            AddTextCol("Nome", "Nome do Livro", 310, DataGridViewContentAlignment.MiddleLeft, 120);
            AddTextCol("Autor", "Autor", 160, DataGridViewContentAlignment.MiddleLeft, 100);
            AddTextCol("Genero", "Gênero", 140, DataGridViewContentAlignment.MiddleLeft, 100);
            AddTextCol("Quantidade", "Quantidade", 45, DataGridViewContentAlignment.MiddleCenter, 100);
            AddTextCol("CodigoBarras", "Código de Barras", 110, DataGridViewContentAlignment.MiddleLeft, 120);
            AddTextCol("Status", "Status", 60, DataGridViewContentAlignment.MiddleLeft, 100);


            // Botão Editar
            var btnEditar = new DataGridViewButtonColumn
            {
                Name = "Editar",
                HeaderText = "",
                Text = "Editar",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FillWeight = 60,
                FlatStyle = FlatStyle.Flat
            };
            dgvLivros.Columns.Add(btnEditar);

            dgvLivros.BackgroundColor = Color.White;
            dgvLivros.BorderStyle = BorderStyle.None;
            dgvLivros.GridColor = Color.FromArgb(235, 239, 244);
            dgvLivros.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLivros.RowHeadersVisible = false;
            dgvLivros.ReadOnly = true;
            dgvLivros.MultiSelect = false;
            dgvLivros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLivros.AllowUserToAddRows = false;
            dgvLivros.AllowUserToDeleteRows = false;
            dgvLivros.AllowUserToResizeRows = false;

            dgvLivros.DefaultCellStyle.BackColor = Color.White;
            dgvLivros.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvLivros.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvLivros.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvLivros.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvLivros.RowTemplate.Height = 40;
            dgvLivros.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            dgvLivros.EnableHeadersVisualStyles = false;
            dgvLivros.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLivros.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvLivros.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLivros.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgvLivros.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvLivros.ColumnHeadersHeight = 44;
            dgvLivros.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvLivros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn col in dgvLivros.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.Resizable = DataGridViewTriState.False;
            }

            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgvLivros,
                new object[] { true }
            );


            if (dgvLivros.Columns.Contains("Id"))
            {
                var colId = dgvLivros.Columns["Id"];

                colId.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                var headerStyle = (DataGridViewCellStyle)dgvLivros.ColumnHeadersDefaultCellStyle.Clone();
                headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colId.HeaderCell.Style = headerStyle;
                colId.HeaderCell.Style.Padding = new Padding(14, 0, 0, 0);

                colId.MinimumWidth = 60;   // largura mínima em pixels (ajuste conforme desejar)
                colId.FillWeight = 30f;    // peso relativo (aumente para dar mais espaço proporcional)
            }


            dgvLivros.ResumeLayout();
        }

        /// <summary>
        /// Escapa caracteres especiais para uso em consultas LIKE
        /// </summary>
        /// <param name="value">Valor a ser escapado</param>
        /// <returns>Valor com caracteres especiais escapados</returns>
        private string EscapeLikeValue(string value)
        {
            return value.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("\\", "[\\]");
        }

        private void dgvLivros_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvLivros.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();

                if (status.Equals("Disponível", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font(dgvLivros.Font, FontStyle.Bold);
                }
                else if (status.Equals("Indisponível", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgvLivros.Font, FontStyle.Bold);
                }
            }
        }

        #endregion

        #region Eventos do Formulário

        /// <summary>
        /// Evento executado ao carregar o formulário
        /// </summary>
        private void LivrosForm_Load(object sender, EventArgs e)
        {

            ConfigurarGridLivros();
            CarregarLivros();
            cbDisponibilidade.SelectedIndex = 0;
            cbFiltro.SelectedIndex = 0;
        }

        #endregion

        #region Funcionalidades de Busca e Filtros

        // Executa busca de livros com filtros dinâmicos
        public void btnProcurar_Click(object sender, EventArgs e)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                try
                {
                    conexao.Open();

                    string campo = "nome"; // padrão
                    if (cbFiltro.SelectedItem != null)
                    {
                        string selecionado = cbFiltro.SelectedItem.ToString();
                        if (selecionado == "Autor") campo = "autor";
                        else if (selecionado == "Gênero") campo = "genero";
                        else if (selecionado == "Nome") campo = "nome";
                    }

                    string query = @"
                SELECT 
                    Id,
                    Nome,
                    Autor,
                    Genero,
                    Quantidade,
                    CodigoBarras,
                    CASE
                        WHEN Quantidade = 0 THEN 'Indisponível'
                        ELSE 'Disponível'
                    END AS Status
                FROM Livros
                WHERE 1=1";

                    // 🔍 Filtro por nome, autor ou gênero
                    if (!string.IsNullOrWhiteSpace(txtNome.Text))
                        query += $" AND {campo} LIKE @termo";

                    // 🔍 Filtro por código de barras (busca por prefixo)
                    if (!string.IsNullOrWhiteSpace(ObterCodigoDeBarrasFormatado()))
                        query += " AND CodigoBarras LIKE @codigo";

                    // 🔍 Filtro por disponibilidade
                    if (cbDisponibilidade.SelectedItem != null)
                    {
                        string status = cbDisponibilidade.SelectedItem.ToString();
                        if (status == "Disponíveis")
                            query += " AND Quantidade > 0";
                        else if (status == "Indisponíveis")
                            query += " AND Quantidade = 0";
                    }

                    query += " ORDER BY Nome ASC";

                    using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                    {
                        if (!string.IsNullOrWhiteSpace(txtNome.Text))
                            comando.Parameters.AddWithValue("@termo", "%" + txtNome.Text.Trim() + "%");

                        if (!string.IsNullOrWhiteSpace(ObterCodigoDeBarrasFormatado()))
                            comando.Parameters.AddWithValue("@codigo", ObterCodigoDeBarrasFormatado() + "%");

                        SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                        DataTable tabela = new DataTable();
                        adaptador.Fill(tabela);

                        dgvLivros.AutoGenerateColumns = false;
                        dgvLivros.DataSource = tabela;

                        lblTitulos.Text = $"Títulos encontrados: {tabela.Rows.Count}";

                        // Soma todas as unidades (coluna "Quantidade")
                        int totalUnidades = 0;
                        foreach (DataRow r in tabela.Rows)
                        {
                            int q = 0;
                            int.TryParse(r["Quantidade"]?.ToString() ?? "0", out q);
                            totalUnidades += q;
                        }
                        lblUnidades.Text = $"Unidades encontradas: {totalUnidades}";

                        // Oculta o campo "disponibilidade" caso ainda exista no banco
                        if (dgvLivros.Columns.Contains("disponibilidade"))
                            dgvLivros.Columns["disponibilidade"].Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    dgvLivros.DataSource = null;
                    MessageBox.Show("Erro ao procurar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    lblTitulos.Text = "Títulos encontrados: 0";
                    lblUnidades.Text = "Unidades encontradas: 0";
                }
            }
        }

        private const int LIMITE_CODIGO_BARRAS = 13;


        private string ObterCodigoDeBarrasFormatado()
        {
            return new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray());
        }

        private void mtxCodigoBarras_KeyPressLimiter(object sender, KeyPressEventArgs e)
        {
            // Bloqueia entrada quando atingir o limite (permitindo teclas de controle e substituição de seleção)
            if (!char.IsControl(e.KeyChar))
            {
                int textoAtual = mtxCodigoBarras.Text?.Length ?? 0;
                int selecao = mtxCodigoBarras.SelectionLength;
                int novoTamanho = textoAtual - selecao + 1; // +1 pelo novo char
                if (novoTamanho > LIMITE_CODIGO_BARRAS)
                    e.Handled = true;
            }
        }

        private void mtxCodigoBarras_TextChangedLimiter(object sender, EventArgs e)
        {
            // Trunca conteúdo excedente (cobre colagens, entrada do leitor, etc.)
            var texto = mtxCodigoBarras.Text ?? string.Empty;
            if (texto.Length > LIMITE_CODIGO_BARRAS)
            {
                int caret = mtxCodigoBarras.SelectionStart;
                mtxCodigoBarras.Text = texto.Substring(0, LIMITE_CODIGO_BARRAS);
                mtxCodigoBarras.SelectionStart = Math.Min(caret, LIMITE_CODIGO_BARRAS);
            }
        }



        // Carrega lista de livros com filtros opcionais
        // Torna o método público para permitir atualização externa
        public void CarregarLivros(string nomeFiltro = "", string generoFiltro = "Todos", string disponibilidadeFiltro = "Todos")
        {
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sql = @"
                SELECT
                    Id,
                    Nome,
                    Autor,
                    Genero,
                    Quantidade,
                    CodigoBarras,
                    CASE
    WHEN Quantidade = 0 THEN 'Indisponível'
    WHEN Disponibilidade = 1 THEN 'Disponível'
    ELSE 'Indisponível'
END AS Status
                FROM Livros
                WHERE 1 = 1";

                    if (!string.IsNullOrWhiteSpace(nomeFiltro))
                        sql += " AND Nome LIKE @nome ESCAPE '\\'";

                    if (!string.Equals(generoFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                        sql += " AND Genero LIKE @genero";

                    if (!string.Equals(disponibilidadeFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                    {
                        switch (disponibilidadeFiltro)
                        {
                            case "Disponível":
                                sql += " AND Disponibilidade = 1 AND Quantidade > 0";
                                break;
                            case "Indisponível":
                                sql += " AND Quantidade = 0";
                                break;
                            
                        }
                    }

                    sql += " ORDER BY Nome ASC";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        if (!string.IsNullOrWhiteSpace(nomeFiltro))
                        {
                            var seguro = EscapeLikeValue(nomeFiltro.Trim());
                            cmd.Parameters.AddWithValue("@nome", seguro + "%");
                        }

                        if (!string.Equals(generoFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                            cmd.Parameters.AddWithValue("@genero", "%" + generoFiltro + "%");

                        var tabela = new DataTable();
                        using (var adapter = new SqlCeDataAdapter(cmd))
                        {
                            adapter.Fill(tabela);
                        }

                        dgvLivros.DataSource = tabela;



                        // Atualiza os contadores
                        lblTitulos.Text = $"Títulos encontrados: {tabela.Rows.Count}";

                        int totalUnidades = 0;
                        foreach (DataRow r in tabela.Rows)
                        {
                            int q = 0;
                            int.TryParse(r["Quantidade"]?.ToString() ?? "0", out q);
                            totalUnidades += q;
                        }
                        lblUnidades.Text = $"Unidades encontradas: {totalUnidades}";

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar livros: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lblTitulos.Text = "Títulos encontrados: 0";
                lblUnidades.Text = "Unidades encontradas: 0";
            }
        }

        #endregion

        #region Configuração e Estilização do DataGridView



        // Formata células baseado na disponibilidade do livro
        private void Lista_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvLivros.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string valor = e.Value.ToString();

                if (valor == "Indisponível")
                {
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(dgvLivros.Font, FontStyle.Italic);
                }
                else if (valor == "Disponível")
                {
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgvLivros.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }


        // Desenha botões personalizados no DataGridView

        private void dgvLivros_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLivros.Columns[e.ColumnIndex].Name == "Editar")
            {
                e.PaintBackground(e.CellBounds, true);

                // Cores do tema
                Color corFundo = Color.FromArgb(30, 61, 88);
                Color corTexto = Color.White;

                // Desenha botão arredondado
                int borderRadius = 8;
                Rectangle rect = new Rectangle(e.CellBounds.X + 6, e.CellBounds.Y + 6,
                                               e.CellBounds.Width - 12, e.CellBounds.Height - 12);

                using (SolidBrush brush = new SolidBrush(corFundo))
                using (Pen pen = new Pen(corFundo, 1))
                {
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddArc(rect.X, rect.Y, borderRadius, borderRadius, 180, 90);
                    path.AddArc(rect.Right - borderRadius, rect.Y, borderRadius, borderRadius, 270, 90);
                    path.AddArc(rect.Right - borderRadius, rect.Bottom - borderRadius, borderRadius, borderRadius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - borderRadius, borderRadius, borderRadius, 90, 90);
                    path.CloseFigure();

                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }

                // Texto centralizado
                TextRenderer.DrawText(e.Graphics, "Editar",
                    new Font("Segoe UI Semibold", 9F),
                    rect,
                    corTexto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        #endregion

        #region Eventos do DataGridView

        // Manipula cliques em células do DataGridView
        private void dgvLivros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLivros.Columns[e.ColumnIndex].Name == "Editar")
            {
                var row = dgvLivros.Rows[e.RowIndex];
                var nomeLivro = row.Cells["Nome"].Value?.ToString();

                var confirm = MessageBox.Show($"Deseja editar o livro \"{nomeLivro}\"?", "Editar Livro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    var livroEdit = new AlterarCadLivroForm();

                    livroEdit.PreencherLivro(new Livro
                    {
                        Id = Convert.ToInt32(row.Cells["Id"].Value),
                        Nome = row.Cells["Nome"].Value?.ToString(),
                        Autor = row.Cells["Autor"].Value?.ToString(),
                        Genero = row.Cells["Genero"].Value?.ToString(),
                        Quantidade = Convert.ToInt32(row.Cells["Quantidade"].Value),
                        CodigoDeBarras = row.Cells["CodigoBarras"].Value?.ToString(),

                    });

                    // Conecta o evento para atualizar o grid após edição
                    livroEdit.LivroAtualizado += (s, args) => CarregarLivros();

                    livroEdit.MdiParent = this.MdiParent;
                    livroEdit.Dock = DockStyle.Fill;
                    livroEdit.FormClosed += (s, args) => { livroEdit.Dispose(); };
                    livroEdit.Show();
                }
            }
        }





        #endregion

        private void TxtNome_TextChanged(object sender, EventArgs e)
        {
            // Reinicia o timer a cada tecla
            filtroTimer.Stop();
            filtroTimer.Start();
        }

        private void FiltroTimer_Tick(object sender, EventArgs e)
        {
            // O timer disparou, pare-o e execute a busca
            filtroTimer.Stop();

            btnProcurar_Click(sender, e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Verifica se a tecla pressionada foi o ENTER e se o controle ativo (com foco)
            // é o nosso campo de código de barras.
            if (keyData == Keys.Enter && this.ActiveControl == mtxCodigoBarras)
            {
                // --- Início da sua lógica de busca ---

                string codigo = new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray()).Trim();

                if (!string.IsNullOrWhiteSpace(codigo))
                {
                    // Executa a busca
                    btnProcurar.PerformClick();

                    // Agenda a limpeza e o foco para o próximo escaneamento
                    Task.Delay(1000).ContinueWith(_ =>
                    {
                        try
                        {
                            Invoke((MethodInvoker)(() =>
                            {
                                mtxCodigoBarras.Text = "";
                                mtxCodigoBarras.Focus();
                            }));
                        }
                        catch { /* Ignora erro se o form fechar */ }
                    });
                }

                // --- Fim da sua lógica de busca ---

                // Retorna 'true' para dizer ao Windows Forms:
                // "Eu já processei esta tecla, não faça mais nada com ela."
                return true;
            }

            // Para qualquer outra tecla ou qualquer outro controle,
            // deixa o Windows Forms continuar com seu comportamento padrão.
            return base.ProcessCmdKey(ref msg, keyData);
        }

       
    }
}
