using BibliotecaApp.Models;
using BibliotecaApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Usuarios;

namespace BibliotecaApp.Forms.Livros
{
    public partial class DevoluçãoForm : Form
    {
        #region Construtor

        private int hoveredIndex = -1;
        private List<Usuarios> _cacheUsuarios = new List<Usuarios>();
        private bool _suppressSuggestionOnSetText = false;

        public DevoluçãoForm()
        {
            InitializeComponent();

       
            InicializarSugestoesUsuario();

          
        }
        private void InicializarSugestoesUsuario()
        {
            // garante que o listbox tenha o mesmo estilo/behavior dos outros forms
            EstilizarListBoxSugestao(lstSugestoesUsuario);

            

            // eventos (se já estiverem ligados no designer, não tem problema — estamos só garantindo)
            txtUsuario.TextChanged += txtUsuario_TextChanged;
            txtUsuario.KeyDown += txtUsuario_KeyDown;
            lstSugestoesUsuario.Click += lstSugestoesUsuario_Click;
            lstSugestoesUsuario.KeyDown += lstSugestoesUsuario_KeyDown;
            lstSugestoesUsuario.Leave += lstSugestoesUsuario_Leave;

            mtxCodigoBarras.KeyPress += mtxCodigoBarras_KeyPressLimiter;
            mtxCodigoBarras.TextChanged += mtxCodigoBarras_TextChangedLimiter;
        }

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

            Color backColor = hovered ? Color.FromArgb(235, 235, 235) : Color.White;
            Color textColor = Color.FromArgb(60, 60, 60);

            using (SolidBrush b = new SolidBrush(backColor))
                e.Graphics.FillRectangle(b, e.Bounds);

            string text = listBox.Items[e.Index].ToString();
            Font font = listBox.Font;

            Rectangle textRect = new Rectangle(e.Bounds.Left + 12, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, text, font, textRect, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            // linha divisória suave entre itens
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

        #region Eventos do Formulário

        public event EventHandler LivroAtualizado;
        private void DevoluçãoForm_Load(object sender, EventArgs e)
        {
           

            InicializarFormulario();
            VerificarAtrasos(); // Atualiza status de atrasos antes de buscar
            BuscarEmprestimos(); // Exibe todos os empréstimos atualizados no grid
        }

        // ---- INÍCIO: handlers e helpers para sugestões de usuário ----

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {
            if (_suppressSuggestionOnSetText) return;

            lstSugestoesUsuario.Items.Clear();
            lstSugestoesUsuario.Visible = false;
            _cacheUsuarios.Clear();

            string termo = txtUsuario.Text.Trim();
            if (string.IsNullOrWhiteSpace(termo)) return;

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT Id, Nome, Turma, TipoUsuario FROM Usuarios WHERE Nome LIKE @nome ORDER BY Nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", termo + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var usuario = new Usuarios
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    Nome = reader["Nome"]?.ToString() ?? "",
                                    Turma = reader["Turma"]?.ToString() ?? "",
                                    TipoUsuario = reader["TipoUsuario"]?.ToString() ?? ""
                                };

                                _cacheUsuarios.Add(usuario);

                                string sufixo = !string.IsNullOrWhiteSpace(usuario.Turma)
                                                ? usuario.Turma
                                                : (!string.IsNullOrWhiteSpace(usuario.TipoUsuario) ? usuario.TipoUsuario : "");
                                string exibicao = !string.IsNullOrWhiteSpace(sufixo) ? $"{usuario.Nome} - {sufixo}" : usuario.Nome;
                                lstSugestoesUsuario.Items.Add(exibicao);
                            }
                        }
                    }
                }

                if (lstSugestoesUsuario.Items.Count > 0)
                {
                    lstSugestoesUsuario.Visible = true;
                    lstSugestoesUsuario.SelectedIndex = 0;

                    int visibleItems = Math.Min(6, lstSugestoesUsuario.Items.Count);
                    int extraPadding = 6;
                    lstSugestoesUsuario.ItemHeight = 40; // garante consistência com EstilizarListBoxSugestao
                    lstSugestoesUsuario.Height = visibleItems * lstSugestoesUsuario.ItemHeight + extraPadding;
                    lstSugestoesUsuario.Width = txtUsuario.Width;
                    lstSugestoesUsuario.Left = txtUsuario.Left;
                    lstSugestoesUsuario.Top = txtUsuario.Bottom;
                    lstSugestoesUsuario.BringToFront();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na busca de usuários: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesUsuario.Visible) return;

            if (e.KeyCode == Keys.Down)
            {
                if (lstSugestoesUsuario.SelectedIndex < lstSugestoesUsuario.Items.Count - 1)
                    lstSugestoesUsuario.SelectedIndex++;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (lstSugestoesUsuario.SelectedIndex > 0)
                    lstSugestoesUsuario.SelectedIndex--;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (lstSugestoesUsuario.SelectedItem != null)
                    SelecionarSugestaoUsuario(lstSugestoesUsuario.SelectedItem.ToString());
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                lstSugestoesUsuario.Visible = false;
            }
        }

        private void lstSugestoesUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (lstSugestoesUsuario.SelectedIndex < 0 && lstSugestoesUsuario.Items.Count > 0)
                    lstSugestoesUsuario.SelectedIndex = 0;
                SelecionarSugestaoUsuario(lstSugestoesUsuario.SelectedItem.ToString());
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesUsuario.Visible = false;
                txtUsuario.Focus();
            }
        }

        private void lstSugestoesUsuario_Click(object sender, EventArgs e)
        {
            if (lstSugestoesUsuario.SelectedItem != null)
                SelecionarSugestaoUsuario(lstSugestoesUsuario.SelectedItem.ToString());
        }

        private void lstSugestoesUsuario_Leave(object sender, EventArgs e)
        {
            // Fecha a lista quando o foco sair (comportamento padrão nos outros forms)
            lstSugestoesUsuario.Visible = false;
        }

        private void SelecionarSugestaoUsuario(string texto)
        {
            int idx = texto.IndexOf(" - ");
            string nome = (idx >= 0) ? texto.Substring(0, idx).Trim() : texto.Trim();

            _suppressSuggestionOnSetText = true;
            try
            {
                txtUsuario.Text = nome;
                lstSugestoesUsuario.Visible = false;
            }
            finally
            {
                _suppressSuggestionOnSetText = false;
            }
        }




        private void btnBuscarEmprestimo_Click(object sender, EventArgs e)
        {
            BuscarEmprestimos();
        }

        private void btnProrrogar_Click(object sender, EventArgs e)
        {

            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode prorrogar empréstimo.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (dgvEmprestimos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um empréstimo para prorrogar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvEmprestimos.SelectedRows[0];
            int idEmprestimo = Convert.ToInt32(row.Cells["ID do Empréstimo"].Value);
            string livro = row.Cells["Livro"].Value?.ToString();
            string alocador = row.Cells["Alocador"].Value?.ToString();
            DateTime dataDevolucaoAtual = row.Cells["Data de Devolução"].Value != null
                ? Convert.ToDateTime(row.Cells["Data de Devolução"].Value)
                : DateTime.Now;

            using (var frm = new ProrrogarDiasForm())
            {
                frm.DataDevolucaoAtual = dataDevolucaoAtual; // Passe a data atual aqui
                if (frm.ShowDialog(this) != DialogResult.OK)
                    return;

                // Validação do valor digitado no RoundedTextBox
                if (!int.TryParse(frm.numQuantidade.Text, out int dias) || dias < 1)
                {
                    MessageBox.Show("Informe um número válido de dias.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime novaData = dataDevolucaoAtual.AddDays(dias);

                // Verifica se a nova data é futura para definir o status
                string novoStatus = novaData.Date >= DateTime.Now.Date ? "Ativo" : "Atrasado";

                string msg = $"Confirma a prorrogação deste empréstimo?\n\n" +
                             $"Livro: {livro}\n" +
                             $"Alocador: {alocador}\n" +
                             $"Data de Devolução Atual: {dataDevolucaoAtual:dd/MM/yyyy}\n" +
                             $"Nova Data de Devolução: {novaData:dd/MM/yyyy}\n" +
                             $"Dias de prorrogação: {dias}";

                var confirm = MessageBox.Show(msg, "Confirmação de Prorrogação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes)
                    return;

                // Atualiza no banco
                // -- início do bloco de atualização + insert de log --
                using (SqlCeConnection conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    using (var trans = conexao.BeginTransaction())
                    {
                        try
                        {
                            // 1) atualiza o empréstimo (DataDevolucao / DataProrrogacao / Status)
                            string updateSql = @"
                UPDATE Emprestimo
                SET DataDevolucao = @NovaData,
                    DataProrrogacao = @NovaData,
                    Status = @Status
                WHERE Id = @Id";
                            using (var cmdUpd = new SqlCeCommand(updateSql, conexao, trans))
                            {
                                cmdUpd.Parameters.AddWithValue("@NovaData", novaData);
                                cmdUpd.Parameters.AddWithValue("@Status", novoStatus);
                                cmdUpd.Parameters.AddWithValue("@Id", idEmprestimo);
                                cmdUpd.ExecuteNonQuery();
                            }

                            // 2) insere no log de prorrogações (LogProrrogacoes)
                            string insertLog = @"
                INSERT INTO LogProrrogacoes (EmprestimoId, DataDaAcao, NovaDataDevolucao, BibliotecariaNome)
                VALUES (@EmprestimoId, @DataDaAcao, @NovaDataDevolucao, @BibliotecariaNome)";
                            using (var cmdLog = new SqlCeCommand(insertLog, conexao, trans))
                            {
                                cmdLog.Parameters.AddWithValue("@EmprestimoId", idEmprestimo);
                                cmdLog.Parameters.AddWithValue("@DataDaAcao", DateTime.Now);
                                cmdLog.Parameters.AddWithValue("@NovaDataDevolucao", novaData);
                                // ajuste aqui para o nome da bibliotecária logada no seu sistema (ex.: Sessao.NomeBibliotecariaLogada)
                                cmdLog.Parameters.AddWithValue("@BibliotecariaNome", Sessao.NomeBibliotecariaLogada ?? "");
                                cmdLog.ExecuteNonQuery();
                            }

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }

                // notificação UX
                MessageBox.Show("Empréstimo prorrogado com sucesso.", "Prorrogação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Atualiza grid local
                BuscarEmprestimos();

                // dispara evento global para que outros forms (ex.: InicioForm) atualizem automaticamente
                BibliotecaApp.Utils.EventosGlobais.OnEmprestimoProrrogado();

            }
        }

        private void btnConfirmarDevolucao_Click(object sender, EventArgs e)
        {
            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode registrar devoluções.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DevolverEmprestimo();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            BuscarEmprestimos();
        }

        private void dgvEmprestimos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvEmprestimos.Columns[e.ColumnIndex].Name != "Status" || e.Value == null)
                return;

            string status = e.Value.ToString().Trim();
            Color cor = ObterCorStatus(status);
            FontStyle estilo = ObterEstiloStatus(status);

            e.CellStyle.ForeColor = cor;
            e.CellStyle.Font = new Font(e.CellStyle.Font, estilo);
        }

        private void dgvEmprestimos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ProcessarCliqueBotaoFicha(e);
        }

        private void dgvEmprestimos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DesenharBotaoFicha(e);
        }

        #endregion

        #region Inicialização

        private void InicializarFormulario()
        {
            BuscarEmprestimos();
            ConfigurarGridEmprestimos();
            cbFiltroEmprestimo.SelectedIndex = 0;
            VerificarAtrasos();
        }

        #endregion

        #region Métodos de Interface

       

      

        private Color ObterCorStatus(string status)
        {
            switch (status)
            {
                case "Ativo":
                    return Color.Green;
                case "Atrasado":
                    return Color.Red;
                case "Devolvido":
                    return Color.DimGray;
                default:
                    return Color.Black;
            }
        }

        private FontStyle ObterEstiloStatus(string status)
        {
            switch (status)
            {
                case "Ativo":
                case "Atrasado":
                    return FontStyle.Bold;
                case "Devolvido":
                default:
                    return FontStyle.Regular;
            }
        }

        #endregion

        #region Busca e Consulta de Dados

        private void BuscarEmprestimos()
        {
            var filtros = ObterFiltrosBusca();

            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();
                string query = ConstruirQueryBusca(filtros);

                using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                {
                    AdicionarParametrosBusca(comando, filtros);

                    SqlCeDataAdapter adaptador = new SqlCeDataAdapter(comando);
                    DataTable tabela = new DataTable();
                    adaptador.Fill(tabela);

                    dgvEmprestimos.DataSource = tabela;
                }
            }
        }

        private FiltrosBusca ObterFiltrosBusca()
        {
            return new FiltrosBusca
            {
                NomeLivro = txtLivro.Text.Trim(),
                CodigoBarras = ObterCodigoDeBarrasFormatado(),
                StatusFiltro = cbFiltroEmprestimo.SelectedItem?.ToString(),
                NomeUsuario = txtUsuario.Text.Trim()
            };
        }



        private string ObterCodigoDeBarrasFormatado() { return new string(mtxCodigoBarras.Text.Where(char.IsDigit).ToArray()); }

        private string ConstruirQueryBusca(FiltrosBusca filtros)
        {
            string queryBase = @"
        SELECT 
            e.Id AS [ID do Empréstimo],
            uAlocador.Nome AS [Alocador],
            uResponsavel.Nome AS [Responsável],
            e.Alocador AS [IdResponsavel],
            COALESCE(l.Nome, e.LivroNome) AS [Livro],
            l.CodigoBarras AS [Código De Barras],
            e.DataEmprestimo AS [Data do Empréstimo],
            e.DataDevolucao AS [Data de Devolução],
            e.Status AS [Status]
        FROM Emprestimo e
        JOIN Usuarios uAlocador ON e.Alocador = uAlocador.Id
        JOIN Usuarios uResponsavel ON e.Responsavel = uResponsavel.Id
        JOIN Livros l ON e.Livro = l.Id
        WHERE l.Nome LIKE @LivroNome";

            if (filtros.FiltrarCodigoBarras)
                queryBase += " AND l.CodigoBarras LIKE @CodigoBarras";

            if (filtros.FiltrarStatus)
                queryBase += " AND e.Status = @Status";

            if (filtros.FiltrarUsuario)
                queryBase += " AND (uAlocador.Nome LIKE @UsuarioNome OR uResponsavel.Nome LIKE @UsuarioNome)";

            queryBase += @"
        ORDER BY 
            CASE e.Status
                WHEN 'Atrasado' THEN 1
                WHEN 'Ativo' THEN 2
                WHEN 'Devolvido' THEN 3
                ELSE 4
            END";

            return queryBase;
        }


        private void AdicionarParametrosBusca(SqlCeCommand comando, FiltrosBusca filtros)
        {
            comando.Parameters.AddWithValue("@LivroNome", "%" + filtros.NomeLivro + "%");

            if (filtros.FiltrarCodigoBarras)
                comando.Parameters.AddWithValue("@CodigoBarras", "%" + filtros.CodigoBarras + "%");

            if (filtros.FiltrarStatus)
                comando.Parameters.AddWithValue("@Status", filtros.StatusFiltro);

            if (filtros.FiltrarUsuario)
                comando.Parameters.AddWithValue("@UsuarioNome", "%" + filtros.NomeUsuario + "%");
        }


        #endregion

        #region Configuração do DataGridView

        private void ConfigurarGridEmprestimos()
        {
            dgvEmprestimos.SuspendLayout();

            ConfigurarColunasGrid();
            ConfigurarEstiloGrid();
            ConfigurarEventosGrid();
            AdicionarBotaoFicha();

            if (dgvEmprestimos.Columns.Contains("ID do Empréstimo"))
            {
                var colId = dgvEmprestimos.Columns["ID do Empréstimo"];

                colId.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                var headerStyle = (DataGridViewCellStyle)dgvEmprestimos.ColumnHeadersDefaultCellStyle.Clone();
                headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colId.HeaderCell.Style = headerStyle;
                colId.HeaderCell.Style.Padding = new Padding(14, 0, 0, 0);

                colId.MinimumWidth = 60;   // largura mínima em pixels (ajuste conforme desejar)
                colId.FillWeight = 30f;    // peso relativo (aumente para dar mais espaço proporcional)
            }


            dgvEmprestimos.ResumeLayout();
        }

        private void ConfigurarColunasGrid()
        {
            dgvEmprestimos.AutoGenerateColumns = false;
            dgvEmprestimos.Columns.Clear();
            dgvEmprestimos.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            var colunas = ObterDefinicoesColunas();
            foreach (var coluna in colunas)
            {
                AdicionarColuna(coluna);
            }

            dgvEmprestimos.Columns["IdResponsavel"].Visible = false;
        }

        private DefinicaoColuna[] ObterDefinicoesColunas()
        {
            return new[]
            {
                new DefinicaoColuna("ID do Empréstimo", "ID", 50, DataGridViewContentAlignment.MiddleCenter, 40),
                new DefinicaoColuna("Alocador", "Alocador", 160, DataGridViewContentAlignment.MiddleLeft, 100),
                new DefinicaoColuna("Responsável", "Responsável", 160, DataGridViewContentAlignment.MiddleLeft, 100),
                new DefinicaoColuna("Livro", "Nome do Livro", 180, DataGridViewContentAlignment.MiddleLeft, 120),
                new DefinicaoColuna("Código De Barras", "Código de Barras", 140, DataGridViewContentAlignment.MiddleLeft, 120),
                new DefinicaoColuna("Data do Empréstimo", "Data de Empréstimo", 150, DataGridViewContentAlignment.MiddleCenter, 110),
                new DefinicaoColuna("Data de Devolução", "Data de Devolução", 140, DataGridViewContentAlignment.MiddleCenter, 100),
                new DefinicaoColuna("Status", "Status", 100, DataGridViewContentAlignment.MiddleCenter, 80),
                new DefinicaoColuna("IdResponsavel", "IdResponsavel", 50, DataGridViewContentAlignment.MiddleCenter, 40)
            };
        }

        private void AdicionarColuna(DefinicaoColuna def)
        {
            var col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = def.DataProperty,
                Name = def.DataProperty,
                HeaderText = def.Header,
                ReadOnly = true,
                FillWeight = def.FillWeight,
                MinimumWidth = def.MinWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = def.Alignment,
                    WrapMode = DataGridViewTriState.False,
                    SelectionBackColor = Color.FromArgb(16, 87, 174),
                    SelectionForeColor = Color.White
                }
            };
            dgvEmprestimos.Columns.Add(col);
        }

        private void ConfigurarEstiloGrid()
        {
            ConfigurarEstilosBasicos();
            ConfigurarEstiloCelulas();
            ConfigurarEstiloCabecalho();
            ConfigurarColunas();
            HabilitarDoubleBuffering();
        }

        private void ConfigurarEstilosBasicos()
        {
            dgvEmprestimos.BackgroundColor = Color.White;
            dgvEmprestimos.BorderStyle = BorderStyle.None;
            dgvEmprestimos.GridColor = Color.FromArgb(235, 239, 244);
            dgvEmprestimos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEmprestimos.RowHeadersVisible = false;
            dgvEmprestimos.ReadOnly = true;
            dgvEmprestimos.MultiSelect = false;
            dgvEmprestimos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmprestimos.AllowUserToAddRows = false;
            dgvEmprestimos.AllowUserToDeleteRows = false;
            dgvEmprestimos.AllowUserToResizeRows = false;
            dgvEmprestimos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ConfigurarEstiloCelulas()
        {
            dgvEmprestimos.DefaultCellStyle.BackColor = Color.White;
            dgvEmprestimos.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvEmprestimos.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvEmprestimos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(16, 87, 174);
            dgvEmprestimos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvEmprestimos.RowTemplate.Height = 40;
            dgvEmprestimos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        }

        private void ConfigurarEstiloCabecalho()
        {
            dgvEmprestimos.EnableHeadersVisualStyles = false;
            dgvEmprestimos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvEmprestimos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvEmprestimos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmprestimos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgvEmprestimos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvEmprestimos.ColumnHeadersHeight = 44;
            dgvEmprestimos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        }

        private void ConfigurarColunas()
        {
            foreach (DataGridViewColumn col in dgvEmprestimos.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.Resizable = DataGridViewTriState.False;
            }
        }

        private void HabilitarDoubleBuffering()
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgvEmprestimos,
                new object[] { true }
            );
        }

        private void ConfigurarEventosGrid()
        {
            dgvEmprestimos.DataBindingComplete += (s, e) =>
            {
                foreach (DataGridViewRow row in dgvEmprestimos.Rows)
                {
                    AplicarCorStatus(row);
                }
            };

            dgvEmprestimos.DataBindingComplete += (s, e) =>
            {
                foreach (DataGridViewRow row in dgvEmprestimos.Rows)
                {
                    AplicarCorStatus(row);

                    // Verificar se o livro ainda existe
                    string nomeLivro = row.Cells["Livro"].Value?.ToString();
                    if (!string.IsNullOrEmpty(nomeLivro))
                    {
                        using (var conexao = Conexao.ObterConexao())
                        {
                            conexao.Open();
                            string sql = "SELECT COUNT(*) FROM Livros WHERE Nome = @nome";
                            using (var cmd = new SqlCeCommand(sql, conexao))
                            {
                                cmd.Parameters.AddWithValue("@nome", nomeLivro);
                                int count = (int)cmd.ExecuteScalar();

                                if (count == 0)
                                {
                                    row.Cells["Livro"].Style.ForeColor = Color.Red;
                                    row.Cells["Livro"].Style.SelectionForeColor = Color.Red;
                                    row.Cells["Livro"].Style.Font = new Font(dgvEmprestimos.DefaultCellStyle.Font, FontStyle.Bold);
                                }
                            }
                        }
                    }
                }
            };
        }

        private void AplicarCorStatus(DataGridViewRow row)
        {
            var status = row.Cells["Status"].Value?.ToString()?.Trim();
            Color foreColor = ObterCorForegroundStatus(status);
            Color selectionColor = ObterCorSelectionStatus(status);

            row.Cells["Status"].Style.ForeColor = foreColor;
            row.Cells["Status"].Style.SelectionForeColor = selectionColor;
        }

        private Color ObterCorForegroundStatus(string status)
        {
            switch (status)
            {
                case "Atrasado":
                    return Color.Red;
                case "Ativo":
                    return Color.Green;
                case "Finalizado":
                    return Color.DimGray;
                default:
                    return Color.Black;
            }
        }

        private Color ObterCorSelectionStatus(string status)
        {
            switch (status)
            {
                case "Atrasado":
                    return Color.Red;
                case "Ativo":
                    return Color.Green;
                case "Finalizado":
                    return Color.DimGray;
                default:
                    return Color.Black;
            }
        }

        #endregion

        #region Botão Ficha do Aluno

        private void AdicionarBotaoFicha()
        {
            var btnFicha = new DataGridViewButtonColumn
            {
                Name = "Ficha",
                HeaderText = "Ficha",
                Text = "Abrir",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(16, 87, 174),
                    SelectionBackColor = Color.FromArgb(16, 87, 174),
                    SelectionForeColor = Color.White
                }
            };

            dgvEmprestimos.Columns.Add(btnFicha);
        }

        private void ProcessarCliqueBotaoFicha(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvEmprestimos.Columns[e.ColumnIndex].Name != "Ficha")
                return;

            var idObj = dgvEmprestimos.Rows[e.RowIndex].Cells["IdResponsavel"].Value;
            if (idObj != null && int.TryParse(idObj.ToString(), out int idUsuario))
            {
                AbrirFichaAluno(idUsuario);
            }
        }

        private void AbrirFichaAluno(int idUsuario)
        {
            var aluno = BuscarAlunoPorId(idUsuario);

            if (aluno != null)
            {
                var fichaForm = new FichaAlunoForm();
                fichaForm.PreencherAluno(aluno);
                fichaForm.MdiParent = this.MdiParent;
                fichaForm.Dock = DockStyle.Fill;
                fichaForm.Show();
            }
            else
            {
                MessageBox.Show("Aluno não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DesenharBotaoFicha(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvEmprestimos.Columns[e.ColumnIndex].Name != "Ficha")
                return;

            e.PaintBackground(e.CellBounds, true);

            Color corFundo = Color.FromArgb(30, 61, 88);
            Color corTexto = Color.White;
            int borderRadius = 8;

            Rectangle rect = new Rectangle(
                e.CellBounds.X + 6,
                e.CellBounds.Y + 6,
                e.CellBounds.Width - 12,
                e.CellBounds.Height - 12);

            DesenharBotaoArredondado(e.Graphics, rect, corFundo, corTexto, borderRadius);
            e.Handled = true;
        }

        private void DesenharBotaoArredondado(Graphics graphics, Rectangle rect, Color corFundo, Color corTexto, int borderRadius)
        {
            using (SolidBrush brush = new SolidBrush(corFundo))
            using (Pen pen = new Pen(corFundo, 1))
            {
                var path = CriarCaminhoArredondado(rect, borderRadius);
                graphics.FillPath(brush, path);
                graphics.DrawPath(pen, path);
            }

            TextRenderer.DrawText(graphics, "Ficha",
                new Font("Segoe UI Semibold", 9F),
                rect,
                corTexto,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private System.Drawing.Drawing2D.GraphicsPath CriarCaminhoArredondado(Rectangle rect, int borderRadius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(rect.X, rect.Y, borderRadius, borderRadius, 180, 90);
            path.AddArc(rect.Right - borderRadius, rect.Y, borderRadius, borderRadius, 270, 90);
            path.AddArc(rect.Right - borderRadius, rect.Bottom - borderRadius, borderRadius, borderRadius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - borderRadius, borderRadius, borderRadius, 90, 90);
            path.CloseFigure();
            return path;
        }

        #endregion

        #region Operações de Devolução

        private void DevolverEmprestimo()
        {
            if (!ValidarSelecaoEmprestimo())
                return;

            var emprestimoInfo = ObterInformacoesEmprestimoSelecionado();

            if (emprestimoInfo.Status == "Devolvido")
            {
                MessageBox.Show("Este empréstimo já foi devolvido.");
                return;
            }

            // Obter dados adicionais da linha selecionada
            var row = dgvEmprestimos.SelectedRows[0];
            string livro = row.Cells["Livro"].Value?.ToString();
            string alocador = row.Cells["Alocador"].Value?.ToString();
           
            string dataEmprestimo = row.Cells["Data do Empréstimo"].Value != null
                ? Convert.ToDateTime(row.Cells["Data do Empréstimo"].Value).ToString("dd/MM/yyyy")
                : "";
 

            string msg = $"Confirma a devolução deste empréstimo?\n\n" +
                         $"Livro: {livro}\n" +
                         $"Alocador: {alocador}\n" +
                         $"Data do Empréstimo: {dataEmprestimo}\n";
                        

            var confirm = MessageBox.Show(msg, "Confirmação de Devolução", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            ProcessarDevolucaoNoBanco(emprestimoInfo.Id);
            MessageBox.Show("Livro devolvido com sucesso.");
            LivroAtualizado?.Invoke(this, EventArgs.Empty);

            // Gatilho global para atualização em outros formulários
            BibliotecaApp.Utils.EventosGlobais.OnLivroDevolvido();

            BuscarEmprestimos();
        }

        private bool ValidarSelecaoEmprestimo()
        {
            if (dgvEmprestimos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um empréstimo para devolver.");
                return false;
            }
            return true;
        }

        private EmprestimoInfo ObterInformacoesEmprestimoSelecionado()
        {
            var row = dgvEmprestimos.SelectedRows[0];
            return new EmprestimoInfo
            {
                Id = Convert.ToInt32(row.Cells["ID do Empréstimo"].Value),
                Status = row.Cells["Status"].Value?.ToString()
            };
        }

        private void ProcessarDevolucaoNoBanco(int idEmprestimo)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        int idLivro = ObterIdLivroDoEmprestimo(conexao, idEmprestimo, transacao);
                        AtualizarStatusEmprestimo(conexao, idEmprestimo, transacao);
                        AtualizarDisponibilidadeLivro(conexao, idLivro, transacao);

                        transacao.Commit();
                    }
                    catch
                    {
                        transacao.Rollback();
                        throw;
                    }
                }
            }
        }

        private int ObterIdLivroDoEmprestimo(SqlCeConnection conexao, int idEmprestimo, SqlCeTransaction transacao)
        {
            string query = "SELECT Livro FROM Emprestimo WHERE Id = @Id";
            using (SqlCeCommand cmd = new SqlCeCommand(query, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("@Id", idEmprestimo);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void AtualizarStatusEmprestimo(SqlCeConnection conexao, int idEmprestimo, SqlCeTransaction transacao)
        {
            string query = @"
                UPDATE Emprestimo 
                SET Status = @Status, DataRealDevolucao = @DataDevolucao 
                WHERE Id = @Id";

            using (SqlCeCommand cmd = new SqlCeCommand(query, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("@Status", "Devolvido");
                cmd.Parameters.AddWithValue("@DataDevolucao", DateTime.Now);
                cmd.Parameters.AddWithValue("@Id", idEmprestimo);
                cmd.ExecuteNonQuery();
            }
        }

        private void AtualizarDisponibilidadeLivro(SqlCeConnection conexao, int idLivro, SqlCeTransaction transacao)
        {
            string query = @"
                UPDATE Livros 
                SET Quantidade = Quantidade + 1, Disponibilidade = 1
                WHERE Id = @IdLivro";

            using (SqlCeCommand cmd = new SqlCeCommand(query, conexao, transacao))
            {
                cmd.Parameters.AddWithValue("@IdLivro", idLivro);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Verificação de Atrasos

        private void VerificarAtrasos()
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string query = @"
                    UPDATE Emprestimo
                    SET Status = 'Atrasado'
                    WHERE Status = 'Ativo'
                    AND (
                        (DataProrrogacao IS NOT NULL AND DataProrrogacao < @Hoje)
                        OR
                        (DataProrrogacao IS NULL AND DataDevolucao < @Hoje)
                    )";

                using (SqlCeCommand comando = new SqlCeCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@Hoje", DateTime.Now);
                    int afetados = comando.ExecuteNonQuery();

                    if (afetados > 0)
                    {
                        MessageBox.Show($"{afetados} empréstimo(s) foram marcados como atrasados.",
                                        "Verificação de Atrasos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        #endregion

        #region Consulta de Alunos

        public Aluno BuscarAlunoPorId(int id)
        {
            using (SqlCeConnection conexao = Conexao.ObterConexao())
            {
                conexao.Open();

                string query = @"
                    SELECT Nome, Email, Turma, Telefone, Cpf, DataNascimento 
                    FROM Usuarios WHERE Id = @Id";

                using (SqlCeCommand cmd = new SqlCeCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlCeDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Aluno
                            {
                                Nome = reader["Nome"].ToString(),
                                Email = reader["Email"].ToString(),
                                Turma = reader["Turma"].ToString(),
                                Telefone = reader["Telefone"].ToString(),
                                CPF = reader["Cpf"].ToString(),
                                DataNascimento = reader["DataNascimento"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["DataNascimento"])
                                    : DateTime.MinValue
                            };
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        private void DevoluçãoForm_Activated(object sender, EventArgs e)
        {
           BuscarEmprestimos();
            VerificarAtrasos();
        }


        private const int LIMITE_CODIGO_BARRAS = 13;
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 1. Verifica se a tecla pressionada é ENTER e se o foco está no campo mtxCodigoBarras
            if (keyData == Keys.Enter && this.ActiveControl == mtxCodigoBarras)
            {
                // Pega o código de barras formatado (apenas dígitos)
                string codigo = ObterCodigoDeBarrasFormatado();

                // Só executa a busca se houver algum código
                if (!string.IsNullOrWhiteSpace(codigo))
                {
                    // 2. Aciona o clique no botão de busca
                    btnBuscarEmprestimo.PerformClick();

                    // 3. Agenda a limpeza do campo para permitir o próximo escaneamento
                    Task.Delay(1000).ContinueWith(_ =>
                    {
                        try
                        {
                            // Usa Invoke para garantir que a atualização da UI seja feita na thread principal
                            Invoke((MethodInvoker)(() =>
                            {
                                mtxCodigoBarras.Text = "";
                                mtxCodigoBarras.Focus();
                            }));
                        }
                        catch
                        {
                            // Ignora erros caso o formulário seja fechado durante o delay
                        }
                    });
                }

                // 4. Retorna 'true' para indicar que a tecla já foi processada.
                // Isso impede que o som de "bip" do Windows toque.
                return true;
            }

            // 5. Para qualquer outra tecla, mantém o comportamento padrão do Windows Forms.
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static bool IsAdminLogado()
        => string.Equals(Sessao.NomeBibliotecariaLogada, "Administrador", StringComparison.OrdinalIgnoreCase);
    }

    #region Classes Auxiliares

    public class FiltrosBusca
    {
        public string NomeLivro { get; set; } = "";
        public string CodigoBarras { get; set; } = "";
        public string StatusFiltro { get; set; } = "";
        public string NomeUsuario { get; set; } = "";

        public bool FiltrarCodigoBarras => !string.IsNullOrEmpty(CodigoBarras);
        public bool FiltrarStatus => StatusFiltro != "Todos" && !string.IsNullOrEmpty(StatusFiltro);
        public bool FiltrarUsuario => !string.IsNullOrWhiteSpace(NomeUsuario);
    }



    public class DefinicaoColuna
    {
        public string DataProperty { get; }
        public string Header { get; }
        public float FillWeight { get; }
        public DataGridViewContentAlignment Alignment { get; }
        public int MinWidth { get; }

        public DefinicaoColuna(string dataProperty, string header, float fillWeight,
                              DataGridViewContentAlignment alignment, int minWidth)
        {
            DataProperty = dataProperty;
            Header = header;
            FillWeight = fillWeight;
            Alignment = alignment;
            MinWidth = minWidth;
        }
    }

    public class EmprestimoInfo
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }


    #endregion
}
