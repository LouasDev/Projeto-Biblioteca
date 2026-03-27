using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;


namespace BibliotecaApp.Forms.Relatorio
    {
        public partial class RelForm : Form
        {
            

            // nomes detectados das tabelas (serão preenchidos no Load)
            private string tblUsuarios = "Usuarios";
            private string tblLivros = "Livros";
            private string tblEmprestimo = "Emprestimo";
            private string tblEmprestimoRapido = "EmprestimoRapido";
           

            public RelForm()
            {
                InitializeComponent();
                this.Load += RelForm_Load;

            // garantir que btnFiltrar acione CarregarLog (caso designer não tenha feito)
            btnFiltrar.Click -= btnFiltrar_Click; 
            btnFiltrar.Click += btnFiltrar_Click;


            txtUsuario.TextChanged += TxtUsuario_TextChanged;
                txtUsuario.KeyDown += TxtUsuario_KeyDown;
                lstSugestoesUsuario.Click += LstSugestoesUsuario_Click;

                txtLivro.TextChanged += TxtLivro_TextChanged;
                txtLivro.KeyDown += TxtLivro_KeyDown;
                lstLivros.Click += LstLivros_Click;

                BibliotecaApp.Utils.EventosGlobais.BibliotecariaCadastrada += (s, e) => PopularCbBibliotecaria();
            BibliotecaApp.Utils.EventosGlobais.EmprestimoProrrogado += (s, e) => CarregarLog();

        }

        private void TxtLivro_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtLivro.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                lstLivros.Visible = false;
                return;
            }

            using (var conn = Conexao.ObterConexao())
            {
                conn.Open();
                var cmd = new SqlCeCommand($"SELECT Nome FROM [{tblLivros}] WHERE Nome LIKE @filtro", conn);
                cmd.Parameters.AddWithValue("@filtro", filtro + "%");


                var lista = new List<string>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(reader.GetString(0));
                }

                if (lista.Any())
                {
                    lstLivros.Items.Clear();
                    lstLivros.Items.AddRange(lista.ToArray());
                    lstLivros.Visible = true;
                }
                else
                {
                    lstLivros.Visible = false;
                }
            }
        }
           


            private void TxtLivro_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Down && lstLivros.Visible && lstLivros.Items.Count > 0)
                {
                    lstLivros.Focus();
                    lstLivros.SelectedIndex = 0;
                }
                else if (e.KeyCode == Keys.Enter && lstLivros.Visible && lstLivros.SelectedItem != null)
                {
                    txtLivro.Text = lstLivros.SelectedItem.ToString();
                    lstLivros.Visible = false;
                    e.SuppressKeyPress = true;
                }
            }

            private void LstLivros_Click(object sender, EventArgs e)
            {
                if (lstLivros.SelectedItem != null)
                {
                    txtLivro.Text = lstLivros.SelectedItem.ToString();
                    lstLivros.Visible = false;
                }
            }






            private void RelForm_Load(object sender, EventArgs e)
            {



                EstilizarListBoxSugestao(lstSugestoesUsuario);
                EstilizarListBoxSugestao(lstLivros);

            dtpInicio.Value = DateTime.Today.AddDays(-30);
            dtpFim.Value = DateTime.Today;

            dtpFim.MinDate = dtpInicio.Value;


            var caminho = Conexao.CaminhoBanco;
                if (!File.Exists(caminho))
                {
                    MessageBox.Show("Arquivo .sdf NÃO encontrado em: " + caminho +
                                    "\nDefina o arquivo como 'Copy to Output Directory'.",
                                    "Banco não encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Teste rápido de conexão e existência de tabela/dados
                try
                {
                    using (var c = Conexao.ObterConexao())
                    {
                        c.Open();
                        using (var cmd = new SqlCeCommand("SELECT COUNT(*) FROM usuarios", c))
                        {
                            var count = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Falha ao abrir o banco .sdf ou acessar a tabela 'usuarios': " + ex.Message,
                                    "Erro de conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // detecta nomes reais das tabelas no banco (aceita plural/singular)
                try
                {
                    using (var c = Conexao.ObterConexao())
                    {
                        c.Open();
                        tblUsuarios = GetExistingTableName(c, new[] { "Usuarios", "usuarios", "Usuario", "usuario" }) ?? tblUsuarios;
                        tblLivros = GetExistingTableName(c, new[] { "Livros", "livros", "Livro", "livro" }) ?? tblLivros;
                        tblEmprestimo = GetExistingTableName(c, new[] { "Emprestimo", "Emprestimos", "emprestimo", "emprestimos" }) ?? tblEmprestimo;
                        tblEmprestimoRapido = GetExistingTableName(c, new[] { "EmprestimoRapido", "EmprestimoRapidos", "emprestimoRapido", "emprestimorapido" }) ?? tblEmprestimoRapido;
                        
                    }
                }
                catch
                {
                    // se falhar na detecção, presuma os nomes padrão que já definimos
                }

            // popula o combobox de ação (garantir ordem esperada)
            cmbAcao.Items.Clear();
            cmbAcao.Items.Add("Todas");               // index 0
            cmbAcao.Items.Add("Empréstimo");          // index 1
            cmbAcao.Items.Add("Devolução");           // index 2
            cmbAcao.Items.Add("Empréstimo Rápido");   // index 3
            cmbAcao.Items.Add("Prorrogação");         // index 4 
            cmbAcao.SelectedIndex = 0;


            // popula combobox de bibliotecárias
            PopularCbBibliotecaria();

                CarregarLog();

                // Configurações iniciais do DataGridView
                ConfigurarGrid();
            }

            /// <summary>
            /// Retorna o primeiro nome de tabela existente entre os candidatos, ou null se nenhum existir.
            /// </summary>
            private string GetExistingTableName(SqlCeConnection conexao, IEnumerable<string> candidatos)
            {
                foreach (var cand in candidatos)
                {
                    // consulta INFORMATION_SCHEMA.TABLES para validar existência (case-insensitive)
                    string q = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @t";
                    using (var cmd = new SqlCeCommand(q, conexao))
                    {
                        cmd.Parameters.AddWithValue("@t", cand);
                        try
                        {
                            var c = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                            if (c > 0)
                                return cand;
                        }
                        catch
                        {
                            // ignorar e testar próximo
                        }
                    }
                }
                return null;
            }


        private void TxtUsuario_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtUsuario.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                lstSugestoesUsuario.Visible = false;
                return;
            }

            using (var conn = Conexao.ObterConexao())
            {
                conn.Open();

                // agora também selecionamos o TipoUsuario
                var cmd = new SqlCeCommand(
                    $"SELECT Nome, Turma, TipoUsuario FROM [{tblUsuarios}] WHERE Nome LIKE @filtro ORDER BY Nome", conn);
                cmd.Parameters.AddWithValue("@filtro", filtro + "%"); // só nomes começando com o filtro

                var lista = new List<string>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nome = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        string turma = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string tipo = reader.FieldCount > 2 && !reader.IsDBNull(2) ? reader.GetString(2) : "";

                        // Se houver turma mostra "Nome - Turma", senão mostra "Nome - TipoUsuario" (se existir)
                        string sufixo = !string.IsNullOrWhiteSpace(turma) ? turma :
                                        (!string.IsNullOrWhiteSpace(tipo) ? tipo : "");
                        if (!string.IsNullOrWhiteSpace(sufixo))
                            lista.Add($"{nome} - {sufixo}");
                        else
                            lista.Add(nome);
                    }
                }

                if (lista.Any())
                {
                    lstLivros.Visible = false; // caso esteja visível
                    lstSugestoesUsuario.Items.Clear();
                    lstSugestoesUsuario.Items.AddRange(lista.ToArray());
                    lstSugestoesUsuario.Visible = true;
                }
                else
                {
                    lstSugestoesUsuario.Visible = false;
                }
            }
        }






        private void TxtUsuario_KeyDown(object sender, KeyEventArgs e)
            {
            if (e.KeyCode == Keys.Enter && lstSugestoesUsuario.Visible && lstSugestoesUsuario.SelectedItem != null)
            {
                string texto = lstSugestoesUsuario.SelectedItem.ToString();
                int idx = texto.IndexOf(" - ");
                txtUsuario.Text = (idx >= 0) ? texto.Substring(0, idx).Trim() : texto.Trim();
                lstSugestoesUsuario.Visible = false;
                e.SuppressKeyPress = true;
            }

        }

        private void LstSugestoesUsuario_Click(object sender, EventArgs e)
            {
            if (lstSugestoesUsuario.SelectedItem != null)
            {
                string texto = lstSugestoesUsuario.SelectedItem.ToString();
                int idx = texto.IndexOf(" - ");
                txtUsuario.Text = (idx >= 0) ? texto.Substring(0, idx).Trim() : texto.Trim();
                lstSugestoesUsuario.Visible = false;
            }

        }




        private void PopularCbBibliotecaria()
            {
                cbBibliotecaria.Items.Clear();
                cbBibliotecaria.Items.Add("Todas");
                try
                {
                    using (var c = Conexao.ObterConexao())
                    {
                        c.Open();
                        string sql = $"SELECT Nome FROM [{tblUsuarios}] WHERE TipoUsuario LIKE @tipo ORDER BY Nome";
                        using (var cmd = new SqlCeCommand(sql, c))
                        {
                            cmd.Parameters.AddWithValue("@tipo", "%Bibliotec%");
                            using (var r = cmd.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    var nome = r.IsDBNull(0) ? "" : r.GetString(0);
                                    if (!string.IsNullOrWhiteSpace(nome) && !cbBibliotecaria.Items.Contains(nome))
                                        cbBibliotecaria.Items.Add(nome);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Erro ao popular cbBibliotecaria: " + ex.Message);
                }

                cbBibliotecaria.SelectedIndex = 0;
            }

            private void btnFiltrar_Click(object sender, EventArgs e)
            {

            if (!ValidarDatasPeriodo())
            {
                return;
            }

            CarregarLog();
            }

            private void CarregarLog()
            {
                try
                {
                    using (var conexao = Conexao.ObterConexao())
                    {
                        conexao.Open();

                        List<string> selects = new List<string>();

                    // 1) Empréstimo (registro inicial)
                    if (!string.IsNullOrEmpty(tblEmprestimo))
                    {
                        selects.Add($@"
        SELECT 
            e.Id,
            u.Id AS UsuarioId,
            COALESCE(u.Nome, 'Excluído') AS NomeU,
            CASE WHEN l.Id IS NULL THEN e.LivroNome + ' (Excluído)' ELSE l.Nome END AS NomeL,
            'Empréstimo' AS Acao,
            e.DataEmprestimo AS DataAcao,
            b.Nome AS Bibliotecaria
        FROM [{tblEmprestimo}] e
        LEFT JOIN [{tblUsuarios}] u ON e.Alocador = u.Id
        LEFT JOIN [{tblLivros}] l ON e.Livro = l.Id
        LEFT JOIN [{tblUsuarios}] b ON e.Responsavel = b.Id
        WHERE e.DataEmprestimo IS NOT NULL
    ");
                    }

                    // 2) Devolução (quando DataRealDevolucao preenchida)
                    if (!string.IsNullOrEmpty(tblEmprestimo))
                    {
                        selects.Add($@"
        SELECT 
            e.Id,
            u.Id AS UsuarioId,
            COALESCE(u.Nome, 'Excluído') AS NomeU,
            CASE WHEN l.Id IS NULL THEN e.LivroNome + ' (Excluído)' ELSE l.Nome END AS NomeL,
            'Devolução' AS Acao,
            e.DataRealDevolucao AS DataAcao,
            b.Nome AS Bibliotecaria
        FROM [{tblEmprestimo}] e
        LEFT JOIN [{tblUsuarios}] u ON e.Alocador = u.Id
        LEFT JOIN [{tblLivros}] l ON e.Livro = l.Id
        LEFT JOIN [{tblUsuarios}] b ON e.Responsavel = b.Id
        WHERE e.DataRealDevolucao IS NOT NULL
    ");
                    }

                    string tblLogProrrogacoes = GetExistingTableName(conexao, new[] { "LogProrrogacoes", "LogProrrogacao", "logprorrogacoes" });

                    // 2.5) Prorrogação (quando DataProrrogacao preenchida)
                    if (!string.IsNullOrEmpty(tblLogProrrogacoes))
                    {
                        selects.Add($@"
        SELECT
            lp.LogId AS Id,
            u.Id AS UsuarioId,
            COALESCE(u.Nome, 'Excluído') AS NomeU,
            CASE WHEN l.Id IS NULL THEN e.LivroNome + ' (Excluído)' ELSE l.Nome END AS NomeL,
            'Prorrogação' AS Acao,
            lp.DataDaAcao AS DataAcao,
            lp.BibliotecariaNome AS Bibliotecaria
        FROM [{tblLogProrrogacoes}] lp
        LEFT JOIN [{tblEmprestimo}] e ON lp.EmprestimoId = e.Id
        LEFT JOIN [{tblUsuarios}] u ON e.Alocador = u.Id
        LEFT JOIN [{tblLivros}] l ON e.Livro = l.Id
        WHERE lp.DataDaAcao IS NOT NULL
    ");
                    
                }

                    // 3) Empréstimo Rápido (empréstimo / devolução)
                    if (!string.IsNullOrEmpty(tblEmprestimoRapido))
                    {
                        selects.Add($@"
        SELECT
            r.Id,
            u.Id AS UsuarioId,
            COALESCE(u.Nome, 'Excluído') AS NomeU,
            CASE WHEN l.Id IS NULL THEN r.LivroNome + ' (Excluído)' ELSE l.Nome END AS NomeL,
            'Empréstimo Rápido' AS Acao,
            r.DataHoraEmprestimo AS DataAcao,
            r.Bibliotecaria AS Bibliotecaria
        FROM [{tblEmprestimoRapido}] r
        LEFT JOIN [{tblUsuarios}] u ON r.ProfessorId = u.Id
        LEFT JOIN [{tblLivros}] l ON r.LivroId = l.Id
        WHERE r.DataHoraEmprestimo IS NOT NULL
    ");

                        selects.Add($@"
        SELECT
            r.Id,
            u.Id AS UsuarioId,
            COALESCE(u.Nome, 'Excluído') AS NomeU,
            CASE WHEN l.Id IS NULL THEN r.LivroNome + ' (Excluído)' ELSE l.Nome END AS NomeL,
            'Devolução' AS Acao,
            r.DataHoraDevolucaoReal AS DataAcao,
            r.Bibliotecaria AS Bibliotecaria
        FROM [{tblEmprestimoRapido}] r
        LEFT JOIN [{tblUsuarios}] u ON r.ProfessorId = u.Id
        LEFT JOIN [{tblLivros}] l ON r.LivroId = l.Id
        WHERE r.DataHoraDevolucaoReal IS NOT NULL
    ");
                    



                }

                        if (selects.Count == 0)
                        {
                            MessageBox.Show("Nenhuma tabela de origem encontrada para gerar o relatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // monta a query final com UNION ALL
                        var sb = new StringBuilder();
                        for (int i = 0; i < selects.Count; i++)
                        {
                            if (i > 0) sb.AppendLine("UNION ALL");
                            sb.AppendLine(selects[i]);
                        }

                        // agora envolvemos e aplicamos filtros externos
                        var final = new StringBuilder();
                        final.AppendLine("SELECT * FROM (");
                        final.AppendLine(sb.ToString());
                        final.AppendLine(") AS Log");
                        final.AppendLine("WHERE 1=1");

                        bool usaUsuario = !string.IsNullOrWhiteSpace(txtUsuario.Text);
                        bool usaLivro = !string.IsNullOrWhiteSpace(txtLivro.Text);
                        bool usaCbBibl = cbBibliotecaria.SelectedIndex > 0 && cbBibliotecaria.SelectedItem != null && cbBibliotecaria.SelectedItem.ToString() != "Todas";

                        if (usaUsuario) final.AppendLine(" AND NomeU LIKE @usuario");
                        if (usaLivro) final.AppendLine(" AND NomeL LIKE @livro");


                    var acaoSelecionada = cmbAcao.SelectedItem?.ToString();
                    if (!string.IsNullOrWhiteSpace(acaoSelecionada) && acaoSelecionada != "Todas")
                    {
                        final.AppendLine(" AND Acao = @acaoSelecionada");
                    }



                    if (usaCbBibl) final.AppendLine(" AND Bibliotecaria LIKE @bibliotecaria");

                        final.AppendLine(" AND DataAcao >= @inicio AND DataAcao <= @fim");
                        final.AppendLine(" ORDER BY DataAcao DESC");

                        string sqlFinal = final.ToString();

                    using (var cmd = new SqlCeCommand(sqlFinal, conexao))
                    {
                        if (usaUsuario) cmd.Parameters.AddWithValue("@usuario", "%" + txtUsuario.Text.Trim() + "%");
                        if (usaLivro) cmd.Parameters.AddWithValue("@livro", "%" + txtLivro.Text.Trim() + "%");
                        if (usaCbBibl) cmd.Parameters.AddWithValue("@bibliotecaria", "%" + cbBibliotecaria.SelectedItem.ToString() + "%");

                        
                        if (!string.IsNullOrWhiteSpace(acaoSelecionada) && acaoSelecionada != "Todas")
                            cmd.Parameters.AddWithValue("@acaoSelecionada", acaoSelecionada);


                        // intervalo: inicio 00:00:00, fim 23:59:59
                        cmd.Parameters.AddWithValue("@inicio", dtpInicio.Value.Date);
                        cmd.Parameters.AddWithValue("@fim", dtpFim.Value.Date.AddDays(1).AddSeconds(-1));

                        var tabela = new DataTable();
                        using (var adapter = new SqlCeDataAdapter(cmd))
                        {
                            adapter.Fill(tabela);
                        }

                        dgvHistorico.DataSource = tabela;
                        dgvHistorico.Refresh();
                    }

                }
            }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar relatório: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void ConfigurarGrid()
            {
                dgvHistorico.SuspendLayout();

                dgvHistorico.AutoGenerateColumns = false;
                dgvHistorico.Columns.Clear();

                dgvHistorico.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                DataGridViewTextBoxColumn AddTextCol(string dataProp, string header, float fillWeight, DataGridViewContentAlignment align, int minWidth = 60, string format = null)
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
                    if (!string.IsNullOrEmpty(format))
                        col.DefaultCellStyle.Format = format;
                    dgvHistorico.Columns.Add(col);
                    return col;
                }

            AddTextCol("UsuarioId", "ID", 30,DataGridViewContentAlignment.MiddleCenter, 40);
            AddTextCol("NomeU", "Nome do Usuário", 180, DataGridViewContentAlignment.MiddleLeft, 120);
            AddTextCol("NomeL", "Nome do Livro", 180, DataGridViewContentAlignment.MiddleLeft, 120);
            AddTextCol("Acao", "Ação", 110, DataGridViewContentAlignment.MiddleLeft, 90);
            AddTextCol("Bibliotecaria", "Bibliotecária", 140, DataGridViewContentAlignment.MiddleLeft, 110);
            AddTextCol("DataAcao", "Data da Ação", 130, DataGridViewContentAlignment.MiddleLeft, 110, "dd/MM/yyyy HH:mm");
            

            dgvHistorico.BackgroundColor = Color.White;
                dgvHistorico.BorderStyle = BorderStyle.None;
                dgvHistorico.GridColor = Color.FromArgb(235, 239, 244);
                dgvHistorico.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvHistorico.RowHeadersVisible = false;
                dgvHistorico.ReadOnly = true;
                dgvHistorico.MultiSelect = false;
                dgvHistorico.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvHistorico.AllowUserToAddRows = false;
                dgvHistorico.AllowUserToDeleteRows = false;
                dgvHistorico.AllowUserToResizeRows = false;

                dgvHistorico.DefaultCellStyle.BackColor = Color.White;
                dgvHistorico.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
                dgvHistorico.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
                dgvHistorico.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
                dgvHistorico.DefaultCellStyle.SelectionForeColor = Color.Black;
                dgvHistorico.RowTemplate.Height = 40;
                dgvHistorico.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

                dgvHistorico.EnableHeadersVisualStyles = false;
                dgvHistorico.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgvHistorico.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
                dgvHistorico.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvHistorico.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
                dgvHistorico.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvHistorico.ColumnHeadersHeight = 44;
                dgvHistorico.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                dgvHistorico.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

           

            foreach (DataGridViewColumn col in dgvHistorico.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
                    col.Resizable = DataGridViewTriState.False;
                }

                typeof(DataGridView).InvokeMember(
                    "DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                    null,
                    dgvHistorico,
                    new object[] { true }
                );

            if (dgvHistorico.Columns.Contains("UsuarioId"))
            {
                var colId = dgvHistorico.Columns["UsuarioId"];

                colId.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                
                var headerStyle = (DataGridViewCellStyle)dgvHistorico.ColumnHeadersDefaultCellStyle.Clone();
                headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colId.HeaderCell.Style = headerStyle;
                colId.HeaderCell.Style.Padding = new Padding(14, 0, 0, 0);

                colId.MinimumWidth = 60;   // largura mínima em pixels (ajuste conforme desejar)
                colId.FillWeight = 30f;    // peso relativo (aumente para dar mais espaço proporcional)
            }


            dgvHistorico.ResumeLayout();

            dgvHistorico.CellFormatting -= DgvHistorico_CellFormatting;
            dgvHistorico.CellFormatting += DgvHistorico_CellFormatting;
        }

        private void DgvHistorico_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvHistorico.Columns[e.ColumnIndex].Name == "NomeU")
            {
                var v = e.Value?.ToString();
                if (string.Equals(v, "Excluído", StringComparison.Ordinal))
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
            }

            if (dgvHistorico.Columns[e.ColumnIndex].Name == "NomeL")
            {
                var v = e.Value?.ToString() ?? "";
                if (v.EndsWith("(Excluído)", StringComparison.Ordinal))
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
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

        private bool ValidarDatasPeriodo()
        {
            if (dtpFim.Value < dtpInicio.Value)
            {
                MessageBox.Show("A data final não pode ser anterior à data inicial.",
                              "Período Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFim.Focus();
                return false;
            }
            return true;
        }

        private void dtpInicio_ValueChanged(object sender, EventArgs e)
        {
            // Ajusta automaticamente a data final se for anterior à data inicial
            if (dtpFim.Value < dtpInicio.Value)
            {
                dtpFim.Value = dtpInicio.Value;
            }

            // Define a data mínima permitida para a data final
            dtpFim.MinDate = dtpInicio.Value;
        }

        private void dtpFim_ValueChanged(object sender, EventArgs e)
        {
            if (dtpFim.Value < dtpInicio.Value)
            {
                MessageBox.Show("A data final não pode ser anterior à data inicial.",
                              "Período Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFim.Value = dtpInicio.Value;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtUsuario_Load(object sender, EventArgs e)
        {

        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            CarregarLog(); // garante dados atualizados
            if (dgvHistorico.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.", "Exportação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nomeArquivo = $"Relatorio_Biblioteca_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            string pastaDownloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            bool downloadsExiste = Directory.Exists(pastaDownloads);

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Salvar relatório";
                sfd.Filter = "Planilha do Excel (*.xlsx)|*.xlsx";
                sfd.FileName = nomeArquivo;
                sfd.OverwritePrompt = true;
                sfd.InitialDirectory = downloadsExiste
                    ? pastaDownloads
                    : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                var dr = sfd.ShowDialog(this);
                if (dr != DialogResult.OK) return;

                string caminhoCompleto = sfd.FileName;

                try
                {
                    // Garante que a pasta existe
                    string dir = Path.GetDirectoryName(caminhoCompleto);
                    if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Relatório");

                        // Cabeçalhos
                        int colIndex = 1;
                        foreach (DataGridViewColumn col in dgvHistorico.Columns)
                        {
                            if (col.Visible && !(col is DataGridViewButtonColumn))
                            {
                                var headerCell = ws.Cell(1, colIndex);
                                headerCell.Value = col.HeaderText;
                                headerCell.Style.Font.Bold = true;
                                headerCell.Style.Font.FontSize = 13;
                                headerCell.Style.Fill.BackgroundColor = XLColor.FromArgb(30, 61, 88);
                                headerCell.Style.Font.FontColor = XLColor.White;
                                headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                headerCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                                ws.Column(colIndex).Width = 30;
                                colIndex++;
                            }
                        }

                        // Dados
                        for (int r = 0; r < dgvHistorico.Rows.Count; r++)
                        {
                            if (dgvHistorico.Rows[r].IsNewRow) continue;

                            int cIndex = 1;
                            foreach (DataGridViewColumn col in dgvHistorico.Columns)
                            {
                                if (col.Visible && !(col is DataGridViewButtonColumn))
                                {
                                    var valor = dgvHistorico.Rows[r].Cells[col.Name].Value;
                                    var cell = ws.Cell(r + 2, cIndex);

                                    if (valor == null || valor == DBNull.Value)
                                    {
                                        cell.Value = "";
                                    }
                                    else if (col.HeaderText.ToLower().Contains("data") && valor is DateTime dt)
                                    {
                                        cell.Value = dt;
                                        cell.Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                                    }
                                    else
                                    {
                                        cell.Value = valor.ToString();
                                    }

                                    cell.Style.Font.FontSize = 12;
                                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                    cIndex++;
                                }
                            }
                        }

                        var used = ws.RangeUsed();
                        if (used != null)
                        {
                            used.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            used.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        }

                        // Opcional: autoajuste das colunas para melhor leitura
                        ws.Columns().AdjustToContents();

                        wb.SaveAs(caminhoCompleto);
                    }

                    MessageBox.Show($"Exportação concluída!\nArquivo salvo em:\n{caminhoCompleto}", "Exportação",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    try
                    {
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{caminhoCompleto}\"");
                    }
                    catch
                    {
                        // Ignora se o Explorer não puder ser aberto
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(
                        "Sem permissão para gravar no local escolhido.\n\n" +
                        "Dica: salve em uma pasta com permissão (ex.: Documentos, Desktop) " +
                        "ou desative o 'Acesso a pastas controladas' para este aplicativo.\n\n" +
                        $"Detalhes: {ex.Message}",
                        "Permissão negada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao exportar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

      
    }
    }
