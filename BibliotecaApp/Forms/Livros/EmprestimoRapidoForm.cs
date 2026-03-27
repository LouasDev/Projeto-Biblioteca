using BibliotecaApp.Models;
using BibliotecaApp.Services;
using BibliotecaApp.Forms.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BibliotecaApp.Utils;

namespace BibliotecaApp.Forms.Livros
{
    public partial class EmprestimoRapidoForm : Form
    {
        private List<string> turmasCadastradas = new List<string>();
        private List<string> livrosCadastrados = new List<string>();
        private List<string> professoresCadastrados = new List<string>();

        public event EventHandler LivroAtualizado;

        private List<string> todasTurmasPadrao;

        // Limite de caracteres do código de barras (igual ao EmprestimoForm)
        private const int LIMITE_CODIGO_BARRAS = 13;

        private static bool IsAdminLogado()
            => string.Equals(Sessao.NomeBibliotecariaLogada, "Administrador", StringComparison.OrdinalIgnoreCase);

        public EmprestimoRapidoForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += EmprestimoRapidoForm_KeyDown;

            BibliotecaApp.Utils.EventosGlobais.BibliotecariaCadastrada += (s, e) => CarregarSugestoesECombo();
            BibliotecaApp.Utils.EventosGlobais.ProfessorCadastrado += (s, e) => CarregarSugestoesECombo();
            BibliotecaApp.Utils.EventosGlobais.LivroDidaticoCadastrado += (s, e) => CarregarSugestoesECombo();
        }

        private Timer horaTimer;
        private void AlinharEIniciarTimerMinuto(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            int msUntilNextMinute = 60000 - (now.Second * 1000 + now.Millisecond);
            horaTimer.Stop();
            horaTimer.Interval = msUntilNextMinute;
            horaTimer.Tick -= AlinharEIniciarTimerMinuto;
            horaTimer.Tick += HoraTimer_Minuto;
            horaTimer.Start();
        }

        private void HoraTimer_Minuto(object sender, EventArgs e)
        {
            lblHoraEmprestimo.Text = $"Hora do Empréstimo: {DateTime.Now:HH:mm}";
            // após o primeiro disparo alinhado, seta p/ 60s fixos
            horaTimer.Interval = 60000;
        }

        private void EmprestimoRapidoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (horaTimer != null)
            {
                horaTimer.Stop();
                horaTimer.Tick -= HoraTimer_Minuto; // ok remover qualquer um; não causa erro se não estiver inscrito
                horaTimer.Dispose();
                horaTimer = null;
            }
        }


        private void EmprestimoRapidoForm_Load(object sender, EventArgs e)
        {
            AppPaths.EnsureFolders();

            //Limpeza Automatica Semanal
            LimparEmprestimosSemana();
            // Carrega dados para sugestões (turmas, livros, professors) e bibliotecárias
            CarregarSugestoesECombo();

            txtProfessor.Focus();


            lblHoraEmprestimo.Text = $"Hora do Empréstimo: {DateTime.Now:HH:mm}";

            horaTimer = new System.Windows.Forms.Timer();
            horaTimer.Interval = 1000; // curto para alinhamento inicial
            horaTimer.Tick += AlinharEIniciarTimerMinuto;
            horaTimer.Start();

            this.FormClosing += EmprestimoRapidoForm_FormClosing;

            // Configura DataGrid
            ConfigurarGridRapidos();
            CarregarGridRapidos();

            numQuantidade.Text = "1"; // valor inicial
            numQuantidade.KeyPress += numQuantidade_KeyPress;
            numQuantidade.TextChanged += numQuantidade_TextChanged;
            numQuantidade.Leave += numQuantidade_Leave; // NOVO

            // Eventos para o autocomplete de Turma
            txtTurma.KeyDown += txtTurma_KeyDown;
            txtTurma.Leave += txtTurma_Leave;

            lstSugestoesTurma.Click += lstSugestoesTurma_Click;
            lstSugestoesTurma.KeyDown += lstSugestoesTurma_KeyDown;
            lstSugestoesTurma.Leave += lstSugestoesTurma_Leave;

            // NOVO: Eventos para Professor
            txtProfessor.KeyDown += txtProfessor_KeyDown;
            lstSugestoesProfessor.KeyDown += lstSugestoesProfessor_KeyDown;
            lstSugestoesProfessor.Leave += lstSugestoesProfessor_Leave;

            // NOVO: Eventos para Livro
            txtLivro.KeyDown += txtLivro_KeyDown;
            lstSugestoesLivro.KeyDown += lstSugestoesLivro_KeyDown;
            lstSugestoesLivro.Leave += lstSugestoesLivro_Leave;

            // Esconde listboxes inicialmente
            lstSugestoesProfessor.Visible = false;
            lstSugestoesLivro.Visible = false;
            lstSugestoesTurma.Visible = false;

            EstilizarListBoxSugestao(lstSugestoesProfessor);
            EstilizarListBoxSugestao(lstSugestoesLivro);
            EstilizarListBoxSugestao(lstSugestoesTurma);

            // === NOVO: Eventos do txtBarcode (igual EmprestimoForm) ===
            txtBarcode.Leave += txtBarcode_Leave;
            txtBarcode.KeyPress += txtBarcode_KeyPressLimiter;
            txtBarcode.TextChanged += txtBarcode_TextChangedLimiter;
        }

        private void EmprestimoRapidoForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            // Se houver alguma lista visível, confirma a seleção correspondente
            if (lstSugestoesProfessor.Visible || lstSugestoesLivro.Visible || lstSugestoesTurma.Visible)
            {
                e.SuppressKeyPress = true;

                // Prioriza a lista focada; se não houver, prioriza a relacionada ao campo focado
                if (lstSugestoesProfessor.Focused || (txtProfessor.Focused && lstSugestoesProfessor.Visible))
                    if (ConfirmarSugestao(lstSugestoesProfessor, txtProfessor)) return;

                if (lstSugestoesLivro.Focused || (txtLivro.Focused && lstSugestoesLivro.Visible))
                    if (ConfirmarSugestao(lstSugestoesLivro, txtLivro)) return;

                if (lstSugestoesTurma.Focused || (txtTurma.Focused && lstSugestoesTurma.Visible))
                    if (ConfirmarSugestao(lstSugestoesTurma, txtTurma)) return;

                // Fallback: se alguma está visível, confirma na ordem
                if (ConfirmarSugestao(lstSugestoesProfessor, txtProfessor)) return;
                if (ConfirmarSugestao(lstSugestoesLivro, txtLivro)) return;
                if (ConfirmarSugestao(lstSugestoesTurma, txtTurma)) return;

                return;
            }

            // Fluxo normal de Enter no formulário
            e.SuppressKeyPress = true;
            if (this.ActiveControl == btnRegistrar)
                btnRegistrar.PerformClick();
            else
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
        }

        // Confirma a seleção do listbox (seleciona o primeiro se nada estiver selecionado)
        private bool ConfirmarSugestao(ListBox listBox, RoundedTextBox target)
        {
            if (!listBox.Visible || listBox.Items.Count == 0) return false;

            if (listBox.SelectedIndex < 0) listBox.SelectedIndex = 0;

            var valor = listBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(valor)) return false;

            target.Text = valor;

            // === NOVO: ao confirmar livro pelo nome, preencher o barcode como no EmprestimoForm ===
            if (ReferenceEquals(target, txtLivro))
                AtualizarBarcodePorLivro(valor);

            listBox.Visible = false;
            target.Focus();
            this.SelectNextControl(target, true, true, true, true);
            return true;
        }

        private void LimparEmprestimosSemana()
        {
            try
            {
                AppPaths.EnsureFolders();
                string arquivoControle = Path.Combine(AppPaths.AppDataFolder, "limpeza.txt");

                DateTime ultimaLimpeza = DateTime.MinValue;
                if (File.Exists(arquivoControle))
                {
                    string conteudo = File.ReadAllText(arquivoControle);
                    if (DateTime.TryParseExact(conteudo, "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out ultimaLimpeza))
                    {
                        // Data lida com sucesso
                    }
                    else
                    {
                        ultimaLimpeza = DateTime.MinValue;
                    }
                }

                // Calcular o número da semana do ano para a data atual e última limpeza
                int semanaAtual = ObterNumeroSemana(DateTime.Now);
                int semanaUltimaLimpeza = ObterNumeroSemana(ultimaLimpeza);

                // Se for um ano diferente ou semana diferente, faz a limpeza
                if (DateTime.Now.Year != ultimaLimpeza.Year || semanaAtual != semanaUltimaLimpeza)
                {
                    using (var conexao = Conexao.ObterConexao())
                    {
                        conexao.Open();

                        using (var cmd = new SqlCeCommand("DELETE FROM EmprestimoRapido", conexao))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // Reseta o ID
                        using (var cmdReset = new SqlCeCommand(
                            "ALTER TABLE EmprestimoRapido ALTER COLUMN Id IDENTITY (1,1)", conexao))
                        {
                            cmdReset.ExecuteNonQuery();
                        }
                    }

                    // Atualiza data da última limpeza
                    File.WriteAllText(arquivoControle, DateTime.Now.ToString("yyyy-MM-dd"));

                    MessageBox.Show("Histórico de Empréstimos foi limpo. Novo ciclo semanal iniciado.",
                                    "Limpeza Semanal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao limpar empréstimos rápidos: " + ex.Message);
            }
        }

        // Método auxiliar para obter o número da semana no ano
        private int ObterNumeroSemana(DateTime data)
        {
            System.Globalization.CultureInfo cultura = System.Globalization.CultureInfo.CurrentCulture;
            return cultura.Calendar.GetWeekOfYear(data,
                System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }

        private void CarregarSugestoesECombo()
        {
            turmasCadastradas.Clear();
            livrosCadastrados.Clear();
            professoresCadastrados.Clear();
            cbBibliotecaria.Items.Clear();

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Turmas (distinct)
                    using (var cmd = new SqlCeCommand("SELECT DISTINCT Turma FROM Usuarios WHERE Turma IS NOT NULL AND Turma <> ''", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            turmasCadastradas.Add(reader.GetString(0));
                        }
                    }

                    // Livros disponíveis SOMENTE do gênero Didático
                    using (var cmd = new SqlCeCommand("SELECT Nome FROM Livros WHERE Nome IS NOT NULL AND Genero = 'Didático'", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            livrosCadastrados.Add(reader.GetString(0));
                        }
                    }

                    // Professores
                    using (var cmd = new SqlCeCommand("SELECT Nome FROM Usuarios WHERE TipoUsuario LIKE '%Professor%'", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            professoresCadastrados.Add(reader.GetString(0));
                        }
                    }

                    // Bibliotecárias (para combo)
                    using (var cmd = new SqlCeCommand("SELECT Nome FROM Usuarios WHERE TipoUsuario = 'Bibliotecário(a)'", conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cbBibliotecaria.Items.Add(reader.GetString(0));
                        }
                    }

                    // Seleciona automaticamente a logada se existir na lista (não adiciona "Administrador")
                    if (!string.IsNullOrWhiteSpace(Sessao.NomeBibliotecariaLogada))
                    {
                        int idx = cbBibliotecaria.Items.IndexOf(Sessao.NomeBibliotecariaLogada);
                        if (idx >= 0)
                            cbBibliotecaria.SelectedIndex = idx;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados iniciais: " + ex.Message);
            }
        }

        #region Métodos de Turma
        private void txtTurma_TextChanged(object sender, EventArgs e)
        {
            string texto = txtTurma.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                lstSugestoesTurma.Visible = false;
                return;
            }

            // Buscar sugestões inteligentes nas turmas permitidas
            var sugestoes = BibliotecaApp.Utils.TurmasUtil.BuscarSugestoes(texto);

            lstSugestoesTurma.Items.Clear();

            if (sugestoes.Count > 0)
            {
                foreach (var s in sugestoes)
                    lstSugestoesTurma.Items.Add(s);

                // Seleciona o primeiro item por padrão
                lstSugestoesTurma.SelectedIndex = 0;

                int visibleItems = Math.Min(5, sugestoes.Count);
                int extraPadding = 8;
                lstSugestoesTurma.Height = visibleItems * lstSugestoesTurma.ItemHeight + extraPadding;
                lstSugestoesTurma.Width = txtTurma.Width;

                // Posição exata abaixo do txtTurma, considerando o Form
                var pt = txtTurma.Parent.PointToScreen(txtTurma.Location);
                pt = this.PointToClient(pt);
                lstSugestoesTurma.Left = pt.X;
                lstSugestoesTurma.Top = pt.Y + txtTurma.Height;
                lstSugestoesTurma.BringToFront();
                lstSugestoesTurma.Visible = true;
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
                if (lstSugestoesTurma.Items.Count > 0)
                    lstSugestoesTurma.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (lstSugestoesTurma.SelectedItem != null)
                    txtTurma.Text = lstSugestoesTurma.SelectedItem.ToString();
                else if (lstSugestoesTurma.Items.Count > 0)
                    txtTurma.Text = lstSugestoesTurma.Items[0].ToString();

                lstSugestoesTurma.Visible = false;
                this.SelectNextControl((Control)sender, true, true, true, true);
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

                // Garante selecionar a primeira opção se nada estiver selecionado
                if (lstSugestoesTurma.SelectedIndex < 0 && lstSugestoesTurma.Items.Count > 0)
                    lstSugestoesTurma.SelectedIndex = 0;

                if (lstSugestoesTurma.SelectedItem != null)
                {
                    txtTurma.Text = lstSugestoesTurma.SelectedItem.ToString();
                    lstSugestoesTurma.Visible = false;
                    txtTurma.Focus();
                    this.SelectNextControl(txtTurma, true, true, true, true);
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesTurma.Visible = false;
                txtTurma.Focus();
            }
        }

        private void txtProfessor_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesProfessor.Visible || lstSugestoesProfessor.Items.Count == 0)
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
                lstSugestoesProfessor.Focus();
                if (lstSugestoesProfessor.Items.Count > 0)
                    lstSugestoesProfessor.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (lstSugestoesProfessor.SelectedItem != null)
                    txtProfessor.Text = lstSugestoesProfessor.SelectedItem.ToString();
                else if (lstSugestoesProfessor.Items.Count > 0)
                    txtProfessor.Text = lstSugestoesProfessor.Items[0].ToString();

                lstSugestoesProfessor.Visible = false;
                this.SelectNextControl((Control)sender, true, true, true, true);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesProfessor.Visible = false;
            }
        }

        private void lstSugestoesProfessor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (lstSugestoesProfessor.SelectedIndex < 0 && lstSugestoesProfessor.Items.Count > 0)
                    lstSugestoesProfessor.SelectedIndex = 0;

                if (lstSugestoesProfessor.SelectedItem != null)
                {
                    txtProfessor.Text = lstSugestoesProfessor.SelectedItem.ToString();
                    lstSugestoesProfessor.Visible = false;
                    txtProfessor.Focus();
                    this.SelectNextControl(txtProfessor, true, true, true, true);
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesProfessor.Visible = false;
                txtProfessor.Focus();
            }
        }

        private void txtLivro_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugestoesLivro.Visible || lstSugestoesLivro.Items.Count == 0)
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
                lstSugestoesLivro.Focus();
                if (lstSugestoesLivro.Items.Count > 0)
                    lstSugestoesLivro.SelectedIndex = 0;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (lstSugestoesLivro.SelectedItem != null)
                {
                    SetLivroTextProgrammatic(lstSugestoesLivro.SelectedItem.ToString(), origemBarcode: false);
                    AtualizarBarcodePorLivro(txtLivro.Text);
                    lstSugestoesLivro.Visible = false;
                    txtLivro.Focus();
                    this.SelectNextControl(txtLivro, true, true, true, true);
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesLivro.Visible = false;
            }
        }

        private void lstSugestoesLivro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (lstSugestoesLivro.SelectedIndex < 0 && lstSugestoesLivro.Items.Count > 0)
                    lstSugestoesLivro.SelectedIndex = 0;

                if (lstSugestoesLivro.SelectedItem != null)
                {
                    txtLivro.Text = lstSugestoesLivro.SelectedItem.ToString();

                    // === NOVO: ao escolher o livro pelo nome, preencher o barcode ===
                    AtualizarBarcodePorLivro(txtLivro.Text);

                    lstSugestoesLivro.Visible = false;
                    txtLivro.Focus();
                    this.SelectNextControl(txtLivro, true, true, true, true);
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                lstSugestoesLivro.Visible = false;
                txtLivro.Focus();
            }
        }

        private void lstSugestoesLivro_Leave(object sender, EventArgs e)
        {
            lstSugestoesLivro.Visible = false;
        }

        private void lstSugestoesProfessor_Leave(object sender, EventArgs e)
        {
            lstSugestoesProfessor.Visible = false;
        }

        private void lstSugestoesTurma_Leave(object sender, EventArgs e)
        {
            lstSugestoesTurma.Visible = false;
        }
        #endregion

        #region Autocomplete listboxes (Professor / Livro)


        private bool NomeContemTodosTokens(string nome, string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return true;
            if (string.IsNullOrWhiteSpace(nome)) return false;

            var tokens = query
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var t in tokens)
            {
                // busca "contains" case-insensitive para cada token
                if (nome.IndexOf(t, StringComparison.CurrentCultureIgnoreCase) < 0)
                    return false;
            }
            return true;
        }

        private void txtProfessor_TextChanged(object sender, EventArgs e)
        {
            var texto = txtProfessor.Text.Trim();
            lstSugestoesProfessor.Items.Clear();
            if (string.IsNullOrWhiteSpace(texto))
            {
                lstSugestoesProfessor.Visible = false;
                return;
            }

            var sugest = professoresCadastrados
                .Where(x => x != null && NomeContemTodosTokens(x, texto))
                .ToArray();

            if (sugest.Length > 0)
            {
                lstSugestoesProfessor.Items.AddRange(sugest);
                lstSugestoesProfessor.Visible = true;
                lstSugestoesProfessor.SelectedIndex = 0; // seleciona a primeira opção
            }
            else
            {
                lstSugestoesProfessor.Visible = false;
            }
        }

        private void lstSugestoesProfessor_Click(object sender, EventArgs e)
        {
            if (lstSugestoesProfessor.SelectedItem != null)
            {
                txtProfessor.Text = lstSugestoesProfessor.SelectedItem.ToString();
                lstSugestoesProfessor.Visible = false;
            }
        }

        private void txtLivro_TextChanged(object sender, EventArgs e)
        {
            var prefixo = txtLivro.Text.Trim();
            lstSugestoesLivro.Items.Clear();

            // Se veio do scanner, não exibe listbox e só aplica limites
            if (_preenchendoPorBarcode)
            {
                lstSugestoesLivro.Visible = false;
                LimitarQuantidadeDisponivel(prefixo);
                _preenchendoPorBarcode = false; // consome o estado do scanner
                return;
            }

            // Se o usuário alterou o nome que foi preenchido pelo código de barras, limpar o código
            if (!_alterandoTxtLivroProgramaticamente && !string.IsNullOrEmpty(_nomePreenchidoPorBarcode))
            {
                if (!string.Equals(prefixo, _nomePreenchidoPorBarcode, StringComparison.CurrentCulture))
                {
                    txtBarcode.Text = "";
                    _nomePreenchidoPorBarcode = null; // não considerar mais “nome vindo do barcode”
                }
            }

            if (string.IsNullOrWhiteSpace(prefixo))
            {
                lstSugestoesLivro.Visible = false;
                LimitarQuantidadeDisponivel(prefixo);
                return;
            }

            var sugest = livrosCadastrados
                .Where(x => x != null && NomeContemTodosTokens(x, prefixo))
                .ToArray();

            if (sugest.Length > 0)
            {
                lstSugestoesLivro.Items.AddRange(sugest);
                lstSugestoesLivro.Visible = true;
                lstSugestoesLivro.SelectedIndex = 0; // seleciona a primeira opção
            }
            else
            {
                lstSugestoesLivro.Visible = false;
            }

            // Limitar quantidade disponível
            LimitarQuantidadeDisponivel(prefixo);
        }

        private int quantidadeMaximaDisponivel = 1;

        private void LimitarQuantidadeDisponivel(string nomeLivro)
        {
            quantidadeMaximaDisponivel = 1; // valor padrão
            if (string.IsNullOrWhiteSpace(nomeLivro))
            {
                // Só define 1 se o campo estiver vazio (não force durante digitação)
                if (!numQuantidade.Focused && string.IsNullOrWhiteSpace(numQuantidade.Text))
                    numQuantidade.Text = "1";
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    using (var cmd = new SqlCeCommand("SELECT Quantidade FROM Livros WHERE Nome = @nome", conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", nomeLivro);
                        var obj = cmd.ExecuteScalar();
                        int disponivel = 1;
                        if (obj != null && int.TryParse(obj.ToString(), out disponivel) && disponivel > 0)
                        {
                            quantidadeMaximaDisponivel = disponivel;
                        }
                    }
                }
            }
            catch
            {
                quantidadeMaximaDisponivel = 1;
            }

            // Se o usuário está digitando, não altere o texto agora; valida ao sair
            if (numQuantidade.Focused) return;

            // Fora de foco, normalize o valor para o intervalo permitido
            if (!int.TryParse(numQuantidade.Text, out var valorAtual) || valorAtual < 1)
                numQuantidade.Text = "1";
            else if (valorAtual > quantidadeMaximaDisponivel)
                numQuantidade.Text = quantidadeMaximaDisponivel.ToString();
        }

        private void lstSugestoesLivro_Click(object sender, EventArgs e)
        {
            if (lstSugestoesLivro.SelectedItem != null)
            {
                txtLivro.Text = lstSugestoesLivro.SelectedItem.ToString();

                // === NOVO: ao escolher o livro pelo nome, preencher o barcode ===
                AtualizarBarcodePorLivro(txtLivro.Text);

                lstSugestoesLivro.Visible = false;
            }
        }
        #endregion

        #region Registrar Empréstimo Rápido
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Bloqueia ação por administrador
            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode realizar empréstimos rápidos.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // validações
            if (string.IsNullOrWhiteSpace(txtProfessor.Text) ||
                string.IsNullOrWhiteSpace(txtLivro.Text) ||
                string.IsNullOrWhiteSpace(txtTurma.Text) ||
                !int.TryParse(numQuantidade.Text, out int qtd) || qtd <= 0 ||
                cbBibliotecaria.SelectedItem == null)
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // BLOQUEIO: verifica se o professor já atingiu o limite de 2 "Faltando"
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    using (var cmd = new SqlCeCommand(
                        "SELECT COUNT(*) FROM EmprestimoRapido r " +
                        "INNER JOIN Usuarios u ON r.ProfessorId = u.Id " +
                        "WHERE u.Nome = @professor AND r.Status = 'Faltando'", conexao))
                    {
                        cmd.Parameters.AddWithValue("@professor", txtProfessor.Text.Trim());
                        int faltandoCount = (int)cmd.ExecuteScalar();
                        if (faltandoCount >= 2)
                        {
                            MessageBox.Show("Este professor já atingiu o limite de 2 devoluções faltando. Ele não pode fazer novos empréstimos rápidos até regularizar pelo menos um livro.", "Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar limite de faltando: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // checar livro, quantidade disponível e gênero
                    int livroId;
                    int quantidadeDisponivel;
                    string generoLivro = null;
                    using (var cmd = new SqlCeCommand("SELECT Id, Quantidade, Genero FROM Livros WHERE Nome = @nome", conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtLivro.Text.Trim());
                        using (var r = cmd.ExecuteReader())
                        {
                            if (!r.Read())
                            {
                                MessageBox.Show("Livro não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            livroId = Convert.ToInt32(r["Id"]);
                            quantidadeDisponivel = Convert.ToInt32(r["Quantidade"]);
                            generoLivro = r["Genero"].ToString();
                        }
                    }

                    // Permitir empréstimo SOMENTE de livros com gênero "Didático"
                    if (!string.Equals(generoLivro, "Didático", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Somente livros do gênero 'Didático' podem ser emprestados por este formulário.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (qtd > quantidadeDisponivel)
                    {
                        MessageBox.Show($"Quantidade indisponível. Estoque atual: {quantidadeDisponivel}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // obter professor id
                    int professorId;
                    using (var cmd = new SqlCeCommand("SELECT Id FROM Usuarios WHERE Nome = @nome AND TipoUsuario LIKE @tipo", conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtProfessor.Text.Trim());
                        cmd.Parameters.AddWithValue("@tipo", "%Professor%");
                        var obj = cmd.ExecuteScalar();
                        if (obj == null)
                        {
                            MessageBox.Show("Professor não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        professorId = Convert.ToInt32(obj);
                    }

                    // Inserir EmprestimoRapido (Bibliotecaria é string; admin não chegará aqui pois já bloqueamos)
                    string insertSql = @"INSERT INTO EmprestimoRapido
(ProfessorId, LivroId, LivroNome, Turma, Quantidade, DataHoraEmprestimo, DataHoraDevolucaoReal, Bibliotecaria, Status)
VALUES (@prof, @livro, @livroNome, @turma, @qt, @dataEmp, NULL, @bibli, 'Ativo')";

                    using (var cmd = new SqlCeCommand(insertSql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@prof", professorId);
                        cmd.Parameters.AddWithValue("@livro", livroId);
                        cmd.Parameters.AddWithValue("@livroNome", txtLivro.Text.Trim());
                        cmd.Parameters.AddWithValue("@turma", txtTurma.Text.Trim());
                        cmd.Parameters.AddWithValue("@qt", qtd);
                        cmd.Parameters.AddWithValue("@dataEmp", DateTime.Now);
                        cmd.Parameters.AddWithValue("@bibli", cbBibliotecaria.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                    }

                    // Atualizar quantidade do livro
                    int novaQuantidade = quantidadeDisponivel - qtd;
                    bool disponivel = novaQuantidade > 0;
                    using (var cmd = new SqlCeCommand("UPDATE Livros SET Quantidade = @qt, Disponibilidade = @disp WHERE Id = @id", conexao))
                    {
                        cmd.Parameters.AddWithValue("@qt", novaQuantidade);
                        cmd.Parameters.AddWithValue("@disp", disponivel);
                        cmd.Parameters.AddWithValue("@id", livroId);
                        cmd.ExecuteNonQuery();
                    }
                } // using conexao

                MessageBox.Show("Empréstimo rápido registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparCampos();
                CarregarSugestoesECombo(); // recarrega quantidades / listas
                CarregarGridRapidos();
                LivroAtualizado?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao registrar empréstimo rápido: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparCampos()
        {
            txtProfessor.Text = "";
            txtLivro.Text = "";
            txtTurma.Text = "";
            numQuantidade.Text = "1";
            // === NOVO: limpar o campo de código de barras (igual EmprestimoForm) ===
            txtBarcode.Text = "";
        }
        #endregion

        #region Grid: configurar / carregar / finalizar
        private void ConfigurarGridRapidos()
        {
            dgvRapidos.SuspendLayout();
            dgvRapidos.AutoGenerateColumns = false;
            dgvRapidos.Columns.Clear();

            DataGridViewTextBoxColumn AddTextCol(string prop, string hdr, int minw, DataGridViewContentAlignment align)
            {
                var c = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = prop,
                    Name = prop,
                    HeaderText = hdr,
                    ReadOnly = true,
                    MinimumWidth = minw,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = align }
                };
                dgvRapidos.Columns.Add(c);
                return c;
            }

            var colId = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "Id",
                HeaderText = "ID",
                Visible = false,
            };
            dgvRapidos.Columns.Add(colId);

            var ColPoint = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Point",
                Name = "Point",
                HeaderText = "•",
                ReadOnly = true,
                Width = 30,  // largura fixa
                MinimumWidth = 30,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None, // não cresce
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            };
            dgvRapidos.Columns.Add(ColPoint);

            AddTextCol("Professor", "Professor", 180, DataGridViewContentAlignment.MiddleLeft);
            AddTextCol("Livro", "Livro", 200, DataGridViewContentAlignment.MiddleLeft);
            AddTextCol("Turma", "Turma", 120, DataGridViewContentAlignment.MiddleLeft);
            AddTextCol("Quantidade", "Qtd", 60, DataGridViewContentAlignment.MiddleCenter);

            var colEmp = AddTextCol("DataHoraEmprestimo", "Emprestado em", 130, DataGridViewContentAlignment.MiddleLeft);
            colEmp.DefaultCellStyle.Format = "dd/MM HH:mm";
            var colDev = AddTextCol("DataHoraDevolucaoReal", "Devolução", 130, DataGridViewContentAlignment.MiddleLeft);
            colDev.DefaultCellStyle.Format = "dd/MM HH:mm";

            AddTextCol("Bibliotecaria", "Bibliotecaria", 120, DataGridViewContentAlignment.MiddleLeft);
            AddTextCol("Status", "Status", 80, DataGridViewContentAlignment.MiddleLeft);

            var btnFinalizar = new DataGridViewButtonColumn
            {
                Name = "Finalizar",
                HeaderText = "",
                Text = "",
                UseColumnTextForButtonValue = true,
                Width = 90,                   // largura fixa
                MinimumWidth = 70,            // impede encolher
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None, // impede crescer
                FlatStyle = FlatStyle.Flat
            };
            dgvRapidos.Columns.Add(btnFinalizar);

            // styling (copiado do UsuarioForm)
            dgvRapidos.BackgroundColor = Color.White;
            dgvRapidos.BorderStyle = BorderStyle.None;
            dgvRapidos.GridColor = Color.FromArgb(235, 239, 244);
            dgvRapidos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRapidos.RowHeadersVisible = false;
            dgvRapidos.ReadOnly = true;
            dgvRapidos.MultiSelect = false;
            dgvRapidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRapidos.AllowUserToAddRows = false;
            dgvRapidos.AllowUserToDeleteRows = false;
            dgvRapidos.AllowUserToResizeRows = false;


            dgvRapidos.DefaultCellStyle.BackColor = Color.White;
            dgvRapidos.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgvRapidos.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgvRapidos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgvRapidos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvRapidos.RowTemplate.Height = 40;
            dgvRapidos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);

            dgvRapidos.EnableHeadersVisualStyles = false;
            dgvRapidos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvRapidos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgvRapidos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRapidos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgvRapidos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvRapidos.ColumnHeadersHeight = 44;
            dgvRapidos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvRapidos.AllowUserToResizeColumns = false;
            dgvRapidos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // double buffer
            typeof(DataGridView).InvokeMember(
       "DoubleBuffered",
       System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
       null,
       dgvRapidos,
       new object[] { true });

            dgvRapidos.ResumeLayout();
        }

        private void CarregarGridRapidos()
        {
            try
            {
                var tabela = new DataTable();
                tabela.Columns.Add("Id", typeof(int));
                tabela.Columns.Add("Professor", typeof(string));
                tabela.Columns.Add("Livro", typeof(string));
                tabela.Columns.Add("Turma", typeof(string));
                tabela.Columns.Add("Quantidade", typeof(int));
                tabela.Columns.Add("DataHoraEmprestimo", typeof(DateTime));
                tabela.Columns.Add("DataHoraDevolucaoReal", typeof(DateTime));

                tabela.Columns.Add("Bibliotecaria", typeof(string));
                tabela.Columns.Add("Status", typeof(string));
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = @"
    SELECT r.Id, u.Nome as Professor, 
       COALESCE(l.Nome, r.LivroNome) as Livro,
       r.Turma, r.Quantidade,
       r.DataHoraEmprestimo, r.DataHoraDevolucaoReal, 
       r.Bibliotecaria, r.Status, r.LivroId
FROM EmprestimoRapido r
INNER JOIN Usuarios u ON r.ProfessorId = u.Id
LEFT JOIN Livros l ON r.LivroId = l.Id
ORDER BY r.Id DESC";

                    using (var cmd = new SqlCeCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = tabela.NewRow();
                            row["Id"] = reader.GetInt32(0);
                            row["Professor"] = reader.GetString(1);
                            row["Livro"] = reader.GetString(2);
                            row["Turma"] = reader.GetString(3);
                            row["Quantidade"] = reader.GetInt32(4);
                            row["DataHoraEmprestimo"] = reader.GetDateTime(5);

                            if (!reader.IsDBNull(6))
                                row["DataHoraDevolucaoReal"] = reader.GetDateTime(6); // coluna do DataTable (ver nota abaixo)
                            else
                                row["DataHoraDevolucaoReal"] = DBNull.Value;

                            row["Bibliotecaria"] = reader.GetString(7);
                            row["Status"] = reader.GetString(8);
                            tabela.Rows.Add(row);
                        }
                    }

                }

                dgvRapidos.DataSource = tabela;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar empréstimos rápidos: " + ex.Message);
            }
        }

        private void dgvRapidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvRapidos.Columns[e.ColumnIndex].Name != "Finalizar") return;

            // Impede devolução pelo admin
            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode finalizar devoluções.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var row = dgvRapidos.Rows[e.RowIndex];
            var status = row.Cells["Status"].Value?.ToString();
            if (string.Equals(status, "Devolvido", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Empréstimo já finalizado.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            AbrirDevolucaoRapidaForm(row);
        }

        // CellPainting para desenhar botão arredondado (igual UsuarioForm)
        private void dgvRapidos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRapidos.Columns[e.ColumnIndex].Name == "Finalizar")
            {
                e.PaintBackground(e.CellBounds, true);

                Color corFundo = Color.FromArgb(30, 61, 88);
                Color corTexto = Color.White;

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

                TextRenderer.DrawText(e.Graphics, "Finalizar",
                    new Font("Segoe UI Semibold", 9F),
                    rect,
                    corTexto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        private void dgvRapidos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // --- Status ---
            if (dgvRapidos.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                var status = e.Value.ToString();
                if (status.Equals("Atrasado", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor = Color.FromArgb(178, 34, 34);
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                else if (status.Equals("Ativo", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor = Color.FromArgb(34, 139, 34);
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                else if (status.Equals("Devolvido", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor = Color.Gray;
                }
                else if (status.Equals("Faltando", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
            }

            // --- Livro Excluído ---
            if (dgvRapidos.Columns[e.ColumnIndex].Name == "Livro")
            {
                var nomeLivro = e.Value?.ToString();
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
                                e.CellStyle.ForeColor = Color.Red;
                                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                            }
                        }
                    }
                }
            }
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

        // Helpers centralizados
        private int ObterQuantidadeAtual()
        {
            int valor;
            return int.TryParse(numQuantidade.Text, out valor) ? valor : 0;
        }

        private void DefinirQuantidade(int valor)
        {
            if (valor < 1) valor = 1;
            if (valor > quantidadeMaximaDisponivel) valor = quantidadeMaximaDisponivel;
            numQuantidade.Text = valor.ToString();
        }

        private void numQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite teclas de controle (Delete, Backspace, Ctrl+C/V etc.) e dígitos
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void numQuantidade_TextChanged(object sender, EventArgs e)
        {
            // Não faz clamp durante a digitação. Valida no Leave e nos botões.
            // Mantemos este handler leve para permitir múltiplos dígitos.
        }

        private void numQuantidade_Leave(object sender, EventArgs e)
        {
            // Normaliza ao sair do campo
            if (string.IsNullOrWhiteSpace(numQuantidade.Text))
            {
                numQuantidade.Text = "1";
                return;
            }

            DefinirQuantidade(ObterQuantidadeAtual());
        }

        private void btnMais_Click(object sender, EventArgs e)
        {
            var atual = ObterQuantidadeAtual();
            DefinirQuantidade(atual + 1);
        }

        private void btnMenos_Click(object sender, EventArgs e)
        {
            var atual = ObterQuantidadeAtual();
            DefinirQuantidade(atual - 1);
        }

        private void AbrirDevolucaoRapidaForm(DataGridViewRow row)
        {
            int emprestimoId = Convert.ToInt32(row.Cells["Id"].Value);
            string professor = row.Cells["Professor"].Value?.ToString();
            string livro = row.Cells["Livro"].Value?.ToString();
            string turma = row.Cells["Turma"].Value?.ToString();
            int quantidadeEmprestada = Convert.ToInt32(row.Cells["Quantidade"].Value);
            DateTime dataEmprestimo = (DateTime)row.Cells["DataHoraEmprestimo"].Value;

            string codigoBarras = "";
            // Buscar o código de barras do livro no banco
            using (var conexao = Conexao.ObterConexao())
            {
                conexao.Open();
                using (var cmd = new SqlCeCommand("SELECT CodigoBarras FROM Livros WHERE Nome = @nome", conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", livro);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null)
                        codigoBarras = obj.ToString();
                }
            }

            using (var form = new DevoluçãoRapidaForm(emprestimoId, professor, livro, turma, quantidadeEmprestada, dataEmprestimo, codigoBarras))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    int quantidadeDevolvida = form.QuantidadeDevolvida;
                    ProcessarDevolucaoRapida(emprestimoId, quantidadeEmprestada, quantidadeDevolvida, professor);
                }
            }
        }
        private void ProcessarDevolucaoRapida(int emprestimoId, int quantidadeEmprestada, int quantidadeDevolvida, string professor)
        {
            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();

                    // Verifica limite de devoluções faltando
                    if (quantidadeDevolvida < quantidadeEmprestada)
                    {
                        using (var cmd = new SqlCeCommand(
                            "SELECT COUNT(*) FROM EmprestimoRapido r " +
                            "INNER JOIN Usuarios u ON r.ProfessorId = u.Id " +
                            "WHERE u.Nome = @professor AND r.Status = 'Faltando'", conexao))
                        {
                            cmd.Parameters.AddWithValue("@professor", professor);
                            int faltandoCount = (int)cmd.ExecuteScalar();
                            if (faltandoCount >= 2)
                            {
                                MessageBox.Show("Limite de 2 empréstimos devolvidos faltando por professor atingido.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    // Atualiza status do empréstimo
                    string novoStatus = quantidadeDevolvida == quantidadeEmprestada ? "Devolvido" : "Faltando";
                    using (var cmd = new SqlCeCommand(
                        "UPDATE EmprestimoRapido SET Status = @status, DataHoraDevolucaoReal = @data, Quantidade = @qtd WHERE Id = @id", conexao))
                    {
                        cmd.Parameters.AddWithValue("@status", novoStatus);
                        cmd.Parameters.AddWithValue("@data", DateTime.Now);
                        cmd.Parameters.AddWithValue("@qtd", quantidadeEmprestada - quantidadeDevolvida == 0 ? 0 : quantidadeEmprestada - quantidadeDevolvida);
                        cmd.Parameters.AddWithValue("@id", emprestimoId);
                        cmd.ExecuteNonQuery();
                    }

                    // Atualiza quantidade do livro
                    int livroId;
                    using (var cmd = new SqlCeCommand("SELECT LivroId FROM EmprestimoRapido WHERE Id = @id", conexao))
                    {
                        cmd.Parameters.AddWithValue("@id", emprestimoId);
                        livroId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    using (var cmd = new SqlCeCommand("UPDATE Livros SET Quantidade = Quantidade + @qtd WHERE Id = @id", conexao))
                    {
                        cmd.Parameters.AddWithValue("@qtd", quantidadeDevolvida);
                        cmd.Parameters.AddWithValue("@id", livroId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Devolução registrada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarSugestoesECombo();
                CarregarGridRapidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar devolução: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numQuantidade_Load(object sender, EventArgs e)
        {

        }

        // =========================
        // NOVO: Métodos de Código de Barras (igual EmprestimoForm)
        // =========================
        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            // Só busca se o campo estiver preenchido
            if (!string.IsNullOrEmpty(txtBarcode.Text))
            {
                BuscarEPreencherLivroPorCodigo();
            }
        }

        private void BuscarEPreencherLivroPorCodigo()
        {
            string codigo = (txtBarcode.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(codigo)) return;

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    // Busca apenas livros do gênero Didático
                    string sql = "SELECT TOP 1 Nome FROM Livros WHERE CodigoBarras = @codigo AND Genero = 'Didático'";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Define o nome sem mostrar listbox e sem acionar autocompletar visual
                                SetLivroTextProgrammatic(reader.GetString(0), origemBarcode: true);

                                // Oculta qualquer lista e aplica limitações de quantidade
                                lstSugestoesLivro.Visible = false;
                                LimitarQuantidadeDisponivel(txtLivro.Text);
                            }
                            else
                            {
                                MessageBox.Show("Livro (Didático) não encontrado pelo código de barras. Escaneie novamente.",
                                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtBarcode.Focus();
                                txtBarcode.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar o livro por código de barras: " + ex.Message,
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        // Preenche o código de barras com base no nome do livro selecionado (igual EmprestimoForm)
        private void AtualizarBarcodePorLivro(string nomeLivro)
        {
            if (string.IsNullOrWhiteSpace(nomeLivro))
            {
                txtBarcode.Enabled = true;
                txtBarcode.Text = "";
                txtBarcode.Enabled = false; // segue o mesmo comportamento
                return;
            }

            try
            {
                using (var conexao = Conexao.ObterConexao())
                {
                    conexao.Open();
                    string sql = "SELECT CodigoBarras FROM Livros WHERE Nome = @nome";
                    using (var cmd = new SqlCeCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", nomeLivro);
                        var obj = cmd.ExecuteScalar();

                        string codigo = obj == null ? "" : obj.ToString();

                        // Igual ao EmprestimoForm: habilita, seta e desabilita o campo
                        txtBarcode.Enabled = true;
                        txtBarcode.Text = codigo;
                        txtBarcode.Enabled = false;
                    }
                }
            }
            catch
            {
                // Em caso de erro, mantemos o comportamento simples
                txtBarcode.Enabled = true;
                txtBarcode.Text = "";
                txtBarcode.Enabled = false;
            }
        }

        // Flags de controle para diferenciar alterações programáticas x usuário
        private bool _alterandoTxtLivroProgramaticamente = false;
        private bool _preenchendoPorBarcode = false;
        private string _nomePreenchidoPorBarcode = null;

        // Helper para definir o texto do livro de forma programática
        private void SetLivroTextProgrammatic(string value, bool origemBarcode)
        {
            _alterandoTxtLivroProgramaticamente = true;

            if (origemBarcode)
            {
                _preenchendoPorBarcode = true;           // sinaliza que veio do scanner
                _nomePreenchidoPorBarcode = value;       // guarda o nome encontrado pelo código
            }

            txtLivro.Text = value;

            _alterandoTxtLivroProgramaticamente = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}