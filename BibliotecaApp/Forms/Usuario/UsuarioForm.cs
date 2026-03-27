using BibliotecaApp.Forms.Inicio;
using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Utils; // ADICIONE ESTA LINHA
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;


namespace BibliotecaApp.Forms.Usuario
{
    public partial class UsuarioForm : Form
    {

        private Timer filtroTimer;
        public UsuarioForm()
        {
            InitializeComponent();
            this.Load += UsuarioForm_Load;
        }

        private EventHandler _empProrrogadoHandler;

        private void EventosGlobais_EmprestimoProrrogado(object sender, EventArgs e)
        {
            // Garantir execução na UI thread
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try
                {
                    this.BeginInvoke(new Action(() => CarregarUsuarios()));
                }
                catch
                {
                    // se BeginInvoke falhar por algum motivo (form fechando), ignore
                }
            }
        }


        private void UsuarioForm_Load(object sender, EventArgs e)
        {
            BibliotecaApp.Utils.EventosGlobais.EmprestimoRealizado -= EventosGlobais_EmprestimoRealizado;
            BibliotecaApp.Utils.EventosGlobais.EmprestimoRealizado += EventosGlobais_EmprestimoRealizado;
            BibliotecaApp.Utils.EventosGlobais.EmprestimoProrrogado -= EventosGlobais_EmprestimoProrrogado;
            BibliotecaApp.Utils.EventosGlobais.EmprestimoProrrogado += EventosGlobais_EmprestimoProrrogado;


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
                        // Opcional para depurar:
                        // MessageBox.Show("Registros em 'usuarios': " + count);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao abrir o banco .sdf ou acessar a tabela 'usuarios': " + ex.Message,
                                "Erro de conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Itens e seleção padrão
            cmbTipoUsuario.Items.Clear();
            cmbTipoUsuario.Items.AddRange(new object[] { "Todos", "Aluno(a)", "Professor(a)", "Bibliotecário(a)", "Outros" });
            cmbTipoUsuario.SelectedItem = "Todos";
            cmbEmprestimo.SelectedItem = "Todos";
            if (cmbTipoUsuario.SelectedIndex < 0 && cmbTipoUsuario.Items.Count > 0)
                cmbTipoUsuario.SelectedIndex = 0;

            

            // Estilo e colunas do grid antes de carregar
            ConfigurarGrid();
            CarregarUsuarios();

            filtroTimer = new Timer();
            filtroTimer.Interval = 200; // 200ms de espera
            filtroTimer.Tick += FiltroTimer_Tick;

            // Assina o evento TextChanged
            txtNome.TextChanged += TxtNome_TextChanged;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text?.Trim() ?? string.Empty;
            string tipo = cmbTipoUsuario.SelectedItem?.ToString() ?? "Todos";
            string emprestimo = cmbEmprestimo?.SelectedItem?.ToString() ?? "Todos";
            CarregarUsuarios(nome, tipo, emprestimo);
        }

        private void EventosGlobais_EmprestimoRealizado(object sender, EventArgs e)
        {
            // Atualiza a lista de usuários após empréstimo
            this.BeginInvoke(new Action(() =>
            {
                CarregarUsuarios();
            }));
        }

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

            // Reutiliza a lógica de filtragem (igual ao btnFiltrar_Click)
            string nome = txtNome.Text?.Trim() ?? string.Empty;
            string tipo = cmbTipoUsuario.SelectedItem?.ToString() ?? "Todos";
            string emprestimo = cmbEmprestimo?.SelectedItem?.ToString() ?? "Todos";
            CarregarUsuarios(nome, tipo, emprestimo);
        }


        private void CarregarUsuarios(string nomeFiltro = "", string tipoFiltro = "Todos", string emprestimoFiltro = "Todos")
        {
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    string sql = @"
                SELECT
                    u.id              AS Id,
                    u.nome            AS Nome,
                    u.email           AS Email,
                    u.tipousuario     AS TipoUsuario,
                    u.cpf             AS CPF,
                    u.telefone        AS Telefone,
                    u.turma           AS Turma,
                    u.datanascimento  AS DataNascimento,
                    CASE
                        WHEN EXISTS (
                            SELECT 1
                            FROM Emprestimo e
                            WHERE e.Alocador = u.id
                              AND e.Status = 'Atrasado'
                        ) THEN 'Atrasado'
                        WHEN EXISTS (
                            SELECT 1
                            FROM Emprestimo e
                            WHERE e.Alocador = u.id
                              AND e.Status <> 'Devolvido'
                        ) THEN 'Ativo'
                        ELSE 'Sem empréstimo'
                    END AS EmprestimoStatus
                FROM Usuarios u
                WHERE 1 = 1";

                    // Mantemos filtros por tipo/emprestimo no SQL — mas **NÃO** aplicamos filtro por nome aqui.
                    if (!string.Equals(tipoFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                        sql += " AND u.tipousuario LIKE @tipo";

                    if (!string.Equals(emprestimoFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                    {
                        switch (emprestimoFiltro)
                        {
                            case "Sem empréstimo":
                                sql += @"
                            AND NOT EXISTS (
                                SELECT 1
                                FROM Emprestimo e
                                WHERE e.Alocador = u.id
                                  AND e.Status <> 'Devolvido'
                            )";
                                break;

                            case "Ativo":
                                sql += @"
                            AND EXISTS (
                                SELECT 1
                                FROM Emprestimo e
                                WHERE e.Alocador = u.id
                                  AND e.Status = 'Ativo'
                            )";
                                break;

                            case "Atrasado":
                                sql += @"
                            AND EXISTS (
                                SELECT 1
                                FROM Emprestimo e
                                WHERE e.Alocador = u.id
                                  AND e.Status = 'Atrasado'
                            )";
                                break;
                        }
                    }

                    sql += " ORDER BY u.nome ASC";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        if (!string.Equals(tipoFiltro, "Todos", StringComparison.OrdinalIgnoreCase))
                            cmd.Parameters.AddWithValue("@tipo", "%" + tipoFiltro + "%");

                        var tabela = new DataTable();
                        using (var adapter = new SqlCeDataAdapter(cmd))
                        {
                            adapter.Fill(tabela);
                        }

                        // --- AQUI: filtro por nome no C# sem acentuação ---
                        if (!string.IsNullOrWhiteSpace(nomeFiltro))
                        {
                            string nomeSemAcentoFiltro = RemoverAcentos(nomeFiltro).ToLowerInvariant();

                            var linhas = tabela.AsEnumerable()
                                .Where(r =>
                                {
                                    string nomeDb = r.Field<string>("Nome") ?? "";
                                    string nomeDbSemAcento = RemoverAcentos(nomeDb).ToLowerInvariant();
                                    return nomeDbSemAcento.Contains(nomeSemAcentoFiltro);
                                });

                            dgvUsuarios.DataSource = linhas.Any() ? linhas.CopyToDataTable() : tabela.Clone();
                        }
                        else
                        {
                            dgvUsuarios.DataSource = tabela;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void ConfigurarGrid()
        {
            dgvUsuarios.SuspendLayout();

            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.Columns.Clear();

            // Alinha o conteúdo padrão à esquerda (caso alguma coluna seja criada sem alinhamento explícito)
            dgvUsuarios.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

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
                dgvUsuarios.Columns.Add(col);
                return col;
            }

            // Se quiser ID à esquerda também, troque para MiddleLeft.
            AddTextCol("Id", "ID", 50, DataGridViewContentAlignment.MiddleCenter, 40);

            // Todas as colunas de texto alinhadas à esquerda
            AddTextCol("Nome", "Nome", 180, DataGridViewContentAlignment.MiddleLeft, 120);
            AddTextCol("Email", "E-mail", 200, DataGridViewContentAlignment.MiddleLeft, 140);
            AddTextCol("TipoUsuario", "Tipo", 110, DataGridViewContentAlignment.MiddleLeft, 90);
            AddTextCol("CPF", "CPF", 110, DataGridViewContentAlignment.MiddleLeft, 90);
            AddTextCol("Telefone", "Telefone", 120, DataGridViewContentAlignment.MiddleLeft, 90);
            AddTextCol("Turma", "Turma", 180, DataGridViewContentAlignment.MiddleLeft, 140);

            var nasc = AddTextCol("DataNascimento", "Nascimento", 130, DataGridViewContentAlignment.MiddleLeft, 110);
            nasc.DefaultCellStyle.Format = "dd/MM/yyyy";
            nasc.DefaultCellStyle.NullValue = "";

            // Coluna de empréstimo também à esquerda
            AddTextCol("EmprestimoStatus", "Empréstimo", 140, DataGridViewContentAlignment.MiddleLeft, 110);

            // Botão Editar (mantém)
            var btnEditar = new DataGridViewButtonColumn
            {
                Name = "Editar",
                HeaderText = "",
                Text = "",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FillWeight = 60,
                FlatStyle = FlatStyle.Flat
            };
            dgvUsuarios.Columns.Add(btnEditar);

            // Aparência
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.BorderStyle = BorderStyle.None;
            dgvUsuarios.GridColor = Color.FromArgb(235, 239, 244);
            dgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;

            dgvUsuarios.DefaultCellStyle.BackColor = Color.White;
            dgvUsuarios.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvUsuarios.RowTemplate.Height = 40;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            // Cabeçalho alinhado à esquerda
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvUsuarios.ColumnHeadersHeight = 44;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Largura
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn col in dgvUsuarios.Columns)
                col.SortMode = DataGridViewColumnSortMode.Automatic;

            // Suavizar rolagem
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgvUsuarios,
                new object[] { true }
            );


            if (dgvUsuarios.Columns.Contains("Id"))
            {
                var colId = dgvUsuarios.Columns["ID"];

                colId.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                var headerStyle = (DataGridViewCellStyle)dgvUsuarios.ColumnHeadersDefaultCellStyle.Clone();
                headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colId.HeaderCell.Style = headerStyle;
                colId.HeaderCell.Style.Padding = new Padding(14, 0, 0, 0);

                colId.MinimumWidth = 60;   // largura mínima em pixels (ajuste conforme desejar)
                colId.FillWeight = 30f;    // peso relativo (aumente para dar mais espaço proporcional)
            }

            dgvUsuarios.ResumeLayout();
            dgvUsuarios.CellPainting += DgvUsuarios_CellPainting;

            // Mantém a coloração da coluna de empréstimo, se você adicionou antes
            dgvUsuarios.CellFormatting -= DgvUsuarios_CellFormatting;
            dgvUsuarios.CellFormatting += DgvUsuarios_CellFormatting;

            foreach (DataGridViewColumn col in dgvUsuarios.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.Resizable = DataGridViewTriState.False;
            }
        }

        // NOVO: coloração condicional da coluna de empréstimo
        private void DgvUsuarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvUsuarios.Columns[e.ColumnIndex].Name != "EmprestimoStatus") return;
            if (e.Value == null) return;

            var status = e.Value.ToString();
            if (status.Equals("Atrasado", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.FromArgb(178, 34, 34); // vermelho
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else if (status.Equals("Ativo", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.ForeColor = Color.FromArgb(34, 139, 34); // verde
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
            }
            else
            {
                e.CellStyle.ForeColor = Color.FromArgb(100, 100, 100); // cinza
            }
        }

        private void DgvUsuarios_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsuarios.Columns[e.ColumnIndex].Name == "Editar")
            {
                e.PaintBackground(e.CellBounds, true);

                // Cores do seu tema
                Color corFundo = Color.FromArgb(30, 61, 88);  // igual ao cabeçalho
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

        // dentro da classe UsuarioForm
        private EditarUsuarioForm usuarioEdit;
        private int? _restoredUserId = null;
        private bool _needsRefreshAfterClose = false;



        private void UsuarioEdit_UsuarioAtualizado(object sender, EventArgs e)
        {
            // Recarrega a grid com os dados mais recentes
            CarregarUsuarios();
            _needsRefreshAfterClose = true;
        }

        private void CadUsuario_UsuarioCriado(object sender, EventArgs e)
        {
            CarregarUsuarios();
        }

        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se clicou na coluna de botão "Editar" e se não é header
            if (e.RowIndex < 0 || dgvUsuarios.Columns[e.ColumnIndex].Name != "Editar")
                return;

            var row = dgvUsuarios.Rows[e.RowIndex];
            var nome = row.Cells["Nome"].Value?.ToString();

            var confirm = MessageBox.Show($"Deseja editar o usuário \"{nome}\"?", "Editar Usuário", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            // Reutiliza a instância de campo (não criar uma local!)
            // Reutiliza a instância de campo
            if (usuarioEdit == null || usuarioEdit.IsDisposed)
            {
                usuarioEdit = new EditarUsuarioForm();
                usuarioEdit.FormClosed += UsuarioEdit_FormClosed;
                // assinaturas de evento...
                usuarioEdit.UsuarioAtualizado -= UsuarioEdit_UsuarioAtualizado;
                usuarioEdit.UsuarioAtualizado += UsuarioEdit_UsuarioAtualizado;
            }

            usuarioEdit.FecharAoSalvar = true;

            // Ajusta botões no MainForm (se MdiParent existir)
            var main = this.MdiParent as MainForm;
            if (main != null)
            {
                main.btnUserEdit.Enabled = false;
                main.btnUser.Enabled = true;
            }

            if (row.Cells["Id"].Value != null && int.TryParse(row.Cells["Id"].Value.ToString(), out int selectedId))
            {
                _restoredUserId = selectedId;
            }
            else
            {
                _restoredUserId = null;
            }

            // Abre o editor primeiro (assim OnLoad/Shown já executam)
            OpenChild(usuarioEdit, keepPreviousHidden: true);

            // Preenche o formulário APÓS o form ser exibido
            usuarioEdit.BeginInvoke(new Action(() =>
            {
                usuarioEdit.PreencherUsuario(new Usuarios
                {
                    Id = Convert.ToInt32(row.Cells["Id"].Value),
                    Nome = row.Cells["Nome"].Value?.ToString(),
                    Email = row.Cells["Email"].Value?.ToString(),
                    TipoUsuario = row.Cells["TipoUsuario"].Value?.ToString(),
                    CPF = row.Cells["CPF"].Value?.ToString(),
                    Telefone = row.Cells["Telefone"].Value?.ToString(),
                    Turma = row.Cells["Turma"].Value?.ToString(),
                    DataNascimento = row.Cells["DataNascimento"].Value != DBNull.Value
                        ? Convert.ToDateTime(row.Cells["DataNascimento"].Value)
                        : DateTime.MinValue
                });
            }));
        }



        private void UsuarioEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (usuarioEdit != null)
            {
                usuarioEdit.UsuarioAtualizado -= UsuarioEdit_UsuarioAtualizado;
            }

            var main = this.MdiParent as MainForm;
            if (main != null)
                main.ResetarUsuarioEdit();

            // Se o editor sinalizou que houve alteração -> recarrega e tenta restaurar a seleção/posição
            if (_needsRefreshAfterClose)
            {
                try
                {
                    CarregarUsuarios();
                    RestoreSelectionAfterRefresh();
                }
                catch (Exception ex)
                {
                    // opcional: Debug.WriteLine(ex);
                }
                finally
                {
                    _needsRefreshAfterClose = false;
                }
            }

            usuarioEdit = null;
        }


        private void RestoreSelectionAfterRefresh()
        {
            if (!_restoredUserId.HasValue) return;

            int idParaRestaurar = _restoredUserId.Value;

            // Executa depois que o binding for aplicado à grid (garante que as linhas já existam)
            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    for (int i = 0; i < dgvUsuarios.Rows.Count; i++)
                    {
                        var cell = dgvUsuarios.Rows[i].Cells["Id"];
                        if (cell?.Value == null) continue;

                        if (int.TryParse(cell.Value.ToString(), out int rowId) && rowId == idParaRestaurar)
                        {
                            // Seleciona a linha encontrada
                            dgvUsuarios.ClearSelection();
                            dgvUsuarios.Rows[i].Selected = true;

                            // Ajusta a célula corrente
                            dgvUsuarios.CurrentCell = dgvUsuarios.Rows[i].Cells[0];

                            // Centraliza a linha na visualização
                            int visible = Math.Max(1, dgvUsuarios.DisplayedRowCount(false));
                            int targetFirst = Math.Max(0, i - visible / 2);
                            targetFirst = Math.Min(targetFirst, Math.Max(0, dgvUsuarios.RowCount - 1));
                            try
                            {
                                dgvUsuarios.FirstDisplayedScrollingRowIndex = targetFirst;
                            }
                            catch
                            {
                                // ignore
                            }

                            break;
                        }
                    }
                }
                catch
                {
                    // ignore
                }
                finally
                {
                    // limpa para não tentar restaurar novamente
                    _restoredUserId = null;
                }
            }));
        }


        public void RefreshGrid()
        {
            // chama o método que já existe; se quiser, pode passar filtros padrão
            CarregarUsuarios();
        }

        private static string RemoverAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            var normalized = texto.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (var c in normalized)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }




        Form activeChild = null;

        /// <summary>
        /// Mostra o form child como único MDI child visível (esconde o anterior),
        /// garante Dock, MdiParent, BringToFront e Activate.
        /// </summary>
        private void OpenChild(Form child, bool keepPreviousHidden = false)
        {
            if (child == null) return;

            try
            {
                if (activeChild != null && activeChild != child && !activeChild.IsDisposed)
                {
                    activeChild.Close(); // sempre fecha o anterior
                }

                child.MdiParent = this.MdiParent;
                child.Dock = DockStyle.Fill;

                if (!child.Visible)
                    child.Show();

                child.BringToFront();
                child.Activate();

                activeChild = child;
            }
            catch (Exception ex)
            {
                // opcional: Debug.WriteLine(ex);
            }
        }

    }
}
