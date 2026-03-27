using BibliotecaApp.Forms.Usuario;
using BibliotecaApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BibliotecaApp.Utils;

//Para Acessar o form de mapeamento altere o arquivo txt em AppData para um ano anterior ao ano atual

namespace BibliotecaApp.Forms.Usuario
{
    public partial class MapeamentoDeTurmasWizardForm : Form
    {
        #region Campos e Variáveis
        private readonly string _baseFolder;
        private MapeamentoAnualModel _model;
        private int _etapaAtual = 1;
        private Dictionary<string, string> _padroesTurma = new Dictionary<string, string>();
        private List<MapeamentoRegistro> _registrosOriginais = new List<MapeamentoRegistro>();
        #endregion

        #region Construtor e Inicialização
        public MapeamentoDeTurmasWizardForm()
        {
            InitializeComponent();
            AppPaths.EnsureFolders();
            _baseFolder = AppPaths.MappingFolder;

            // IMPORTANTE: garantir que os handlers não sejam adicionados duplicadamente
            // (muitos problemas surgem porque o designer também pode ter ligado os eventos).
            btnProximo.Click -= BtnProximo_Click;
            btnProximo.Click += BtnProximo_Click;

            btnAnterior.Click -= BtnAnterior_Click;
            btnAnterior.Click += BtnAnterior_Click;

            btnCancelar.Click -= BtnCancelar_Click;
            btnCancelar.Click += BtnCancelar_Click;

            this.Load -= MapeamentoDeTurmasWizardForm_Load;
            this.Load += MapeamentoDeTurmasWizardForm_Load;
        }

        private void MapeamentoDeTurmasWizardForm_Load(object sender, EventArgs e)
        {
            MostrarTutorialEtapa(1);

            try
            {
                int ano = DateTime.Now.Year;
                _model = LoadMapping(ano) ?? new MapeamentoAnualModel { Ano = ano, GeradoEm = DateTime.Now, Status = "pendente" };

                CarregarDadosIniciais();
                MostrarEtapa(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar o wizard: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Carregamento de Dados
        private void CarregarDadosIniciais()
        {
            _registrosOriginais.Clear();
            _padroesTurma.Clear();

            using (var cn = Conexao.ObterConexao())
            {
                cn.Open();
                using (var cmd = new SqlCeCommand("SELECT Id, Nome, Turma FROM usuarios WHERE TipoUsuario LIKE '%Aluno%' ORDER BY Turma, Nome", cn))
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var turmaAtual = r["Turma"]?.ToString() ?? "";
                        var sugestao = SugerirProximaTurmaInteligente(turmaAtual);

                        _registrosOriginais.Add(new MapeamentoRegistro
                        {
                            UsuarioId = Convert.ToInt32(r["Id"]),
                            Nome = r["Nome"]?.ToString() ?? "",
                            TurmaAtual = turmaAtual,
                            Sugestao = sugestao,
                            NovaTurma = sugestao,
                            Observacao = ""
                        });
                    }
                }
            }

            // Agrupar por turma para definir padrões
            var turmasDistintas = _registrosOriginais
                .Where(r => !string.IsNullOrWhiteSpace(r.TurmaAtual))
                .GroupBy(r => r.TurmaAtual)
                .Select(g => g.Key)
                .OrderBy(t => t)
                .ToList();

            foreach (var turma in turmasDistintas)
            {
                _padroesTurma[turma] = SugerirProximaTurmaInteligente(turma);
            }
        }

        /// <summary>
        /// Sugestão inteligente baseada nas turmas existentes e permitidas.
        /// Melhoria: 9º Ano (EF) agora sugere 1º EM (INT preferencialmente) e, se não houver, 1º Técnico (sempre bloqueando EP PRO).
        /// </summary>
        private string SugerirProximaTurmaInteligente(string turmaAtual)
        {
            if (string.IsNullOrWhiteSpace(turmaAtual))
                return "EGRESSO";

            var sugestoes = TurmasUtil.BuscarSugestoes(turmaAtual);
            var turmaBase = sugestoes.FirstOrDefault() ?? turmaAtual;

            var upper = turmaBase.ToUpperInvariant();
            if (upper == "EGRESSO" || upper == "TRANSFERIDO" || upper == "DESISTENTE")
                return upper;

            var m = Regex.Match(turmaBase, @"^(\d+)[°º]?\s*(.+)$", RegexOptions.IgnoreCase);
            if (!m.Success)
                return "EGRESSO";

            int serieAtual = int.Parse(m.Groups[1].Value);
            var turmasPermit = TurmasUtil.TurmasPermitidas;
            var cursoAtual = ExtrairCurso(turmaBase);

            // Substitua pela nova lógica:
            string padrao;
            string sufixo = null;

            // Regex para capturar: (1:Série) (2:Padrão Base) (3:Sufixo Numérico Opcional)
            var matchPadrao = Regex.Match(turmaBase, @"^\s*(\d+)[°º]?\s*(.+?)(?:\s+(\d+))?\s*$", RegexOptions.IgnoreCase);

            if (matchPadrao.Success)
            {
                padrao = matchPadrao.Groups[2].Value.Trim(); // ex: "EF AF REG"
                if (matchPadrao.Groups[3].Success)
                    sufixo = matchPadrao.Groups[3].Value; // ex: "2"
            }
            else
            {
                // Fallback caso o regex falhe (improvável)
                padrao = "EGRESSO";
            }

            // A função RemoverPrefixo ainda é necessária para os filtros 'Where', mantenha ela onde está.
            string RemoverPrefixo(string t)
            {
                var mm = Regex.Match(t, @"^\s*(\d+)[°º]?\s*(.+?)(?:\s+\d+)?\s*$", RegexOptions.IgnoreCase);
                return mm.Success ? mm.Groups[2].Value.Trim() : t.Trim();
            }

            // Bloquear EP PRO quando origem for EM ou Técnico
            bool bloquearEpPro = string.Equals(cursoAtual, "AnoEM", StringComparison.OrdinalIgnoreCase) || IsCursoTecnico(turmaBase);
            IEnumerable<string> FiltrarEpProSeNecessario(IEnumerable<string> lista)
                => bloquearEpPro ? lista.Where(t => !IsEpPro(t)) : lista;

            // ============================================================
            // ENSINO FUNDAMENTAL (6º ao 9º ano)
            // ============================================================
            if (string.Equals(cursoAtual, "Ano", StringComparison.OrdinalIgnoreCase))
            {
                // 9º Ano -> 1º EM (INT preferido, depois REG) ou 1º Técnico
                // Substitua pelo novo bloco (dentro de "ENSINO FUNDAMENTAL")
                if (serieAtual == 9)
                {
                    var dePrimeiraSerie = turmasPermit.Where(t => Regex.IsMatch(t, @"^1[°º]\b", RegexOptions.IgnoreCase)).ToList();

                    // 1. Prioridade: EM INT "Puro" (sem palavras técnicas)
                    var emIntPuro = FiltrarEpProSeNecessario(dePrimeiraSerie.Where(t =>
                        string.Equals(ExtrairCurso(t), "AnoEM", StringComparison.OrdinalIgnoreCase) &&
                        Regex.IsMatch(t, @"\bEM\s*INT\b", RegexOptions.IgnoreCase) &&
                        !ContemPalavraTecnica(t))); // <-- Nova verificação

                    // 2. Prioridade: EM REG "Puro" (sem palavras técnicas)
                    var emRegPuro = FiltrarEpProSeNecessario(dePrimeiraSerie.Where(t =>
                        string.Equals(ExtrairCurso(t), "AnoEM", StringComparison.OrdinalIgnoreCase) &&
                        Regex.IsMatch(t, @"\bEM\s*REG\b", RegexOptions.IgnoreCase) &&
                        !ContemPalavraTecnica(t))); // <-- Nova verificação

                    // 3. Prioridade: EM INT "Técnico" (ex: DESENV DE SISTEMAS EM INT)
                    var emIntTec = FiltrarEpProSeNecessario(dePrimeiraSerie.Where(t =>
                        string.Equals(ExtrairCurso(t), "AnoEM", StringComparison.OrdinalIgnoreCase) &&
                        Regex.IsMatch(t, @"\bEM\s*INT\b", RegexOptions.IgnoreCase) &&
                        ContemPalavraTecnica(t))); // <-- Nova verificação

                    // 4. Prioridade: Técnico Puro (que não é EM)
                    var tecn1 = FiltrarEpProSeNecessario(dePrimeiraSerie.Where(t => IsCursoTecnico(t)));

                    // Nova ordem de escolha:
                    var escolha = emIntPuro.FirstOrDefault() ??
                                  emRegPuro.FirstOrDefault() ??
                                  emIntTec.FirstOrDefault() ??
                                  tecn1.FirstOrDefault();

                    if (!string.IsNullOrEmpty(escolha))
                        return escolha;

                    return "EGRESSO";
                }

                // *** CORREÇÃO: 6º, 7º, 8º ano -> progressão normal (série + 1) no EF ***
                // Substitua pelo novo bloco:
                // *** CORREÇÃO: 6º, 7º, 8º ano -> progressão normal (série + 1) no EF ***
                int proximaSerie = serieAtual + 1;

                // 1. Tentar encontrar a turma com o MESMO sufixo (ex: 8º REG 2 -> 9º REG 2)
                if (sufixo != null)
                {
                    var candidatoSufixoExato = turmasPermit.FirstOrDefault(t =>
                    {
                        var mTarget = Regex.Match(t, @"^\s*(\d+)[°º]?\s*(.+?)(?:\s+(\d+))?\s*$", RegexOptions.IgnoreCase);
                        if (!mTarget.Success) return false;

                        var targetSerie = mTarget.Groups[1].Value;
                        var targetPadrao = mTarget.Groups[2].Value.Trim();
                        var targetSufixo = mTarget.Groups[3].Success ? mTarget.Groups[3].Value : null;

                        return targetSerie == proximaSerie.ToString() &&
                               string.Equals(ExtrairCurso(t), "Ano", StringComparison.OrdinalIgnoreCase) &&
                               string.Equals(targetPadrao, padrao, StringComparison.OrdinalIgnoreCase) &&
                               string.Equals(targetSufixo, sufixo, StringComparison.OrdinalIgnoreCase);
                    });

                    if (!string.IsNullOrEmpty(candidatoSufixoExato))
                        return candidatoSufixoExato; // Encontrou "9º EF AF REG 2"
                }

                // 2. Fallback: Tentar encontrar turma com mesmo padrão (pegar a primeira, ex: "REG 1")
                var candidatosProximaEF = turmasPermit.Where(t =>
                    Regex.IsMatch(t, @"^" + Regex.Escape(proximaSerie.ToString()) + @"[°º]", RegexOptions.IgnoreCase) &&
                    string.Equals(ExtrairCurso(t), "Ano", StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(RemoverPrefixo(t), padrao, StringComparison.OrdinalIgnoreCase)
                ).OrderBy(t => t).ToList(); // Ordenar para pegar "1"

                if (candidatosProximaEF.Any())
                    return candidatosProximaEF.First(); // Encontrou "9º EF AF REG 1"

                // 3. Fallback: qualquer turma da próxima série do EF (mesmo que com padrão diferente)
                var anyProximaEF = turmasPermit.Where(t =>
                    Regex.IsMatch(t, @"^" + Regex.Escape(proximaSerie.ToString()) + @"[°º]", RegexOptions.IgnoreCase) &&
                    string.Equals(ExtrairCurso(t), "Ano", StringComparison.OrdinalIgnoreCase)
                ).FirstOrDefault();

                return anyProximaEF ?? "EGRESSO";
            }

            // ============================================================
            // ENSINO MÉDIO E TÉCNICO (1º, 2º, 3º ano)
            // ============================================================


            // candidatos de 3º respeitando o padrão
            // [NOVO BLOCO DE CÓDIGO - INÍCIO]

            // Função auxiliar interna para aplicar a lógica de sufixo ao EM/Técnico
            Func<int, string> ObterProximaTurmaPorSufixo = (proximaSerie) =>
            {
                // 1. Tentar encontrar a turma com o MESMO sufixo (ex: 1º INT 2 -> 2º INT 2)
                if (sufixo != null)
                {
                    var candidatoSufixoExato = turmasPermit.FirstOrDefault(t =>
                    {
                        var mTarget = Regex.Match(t, @"^\s*(\d+)[°º]?\s*(.+?)(?:\s+(\d+))?\s*$", RegexOptions.IgnoreCase);
                        if (!mTarget.Success) return false;

                        var targetSerie = mTarget.Groups[1].Value;
                        var targetPadrao = mTarget.Groups[2].Value.Trim();
                        var targetSufixo = mTarget.Groups[3].Success ? mTarget.Groups[3].Value : null;

                        return targetSerie == proximaSerie.ToString() &&
                               string.Equals(targetPadrao, padrao, StringComparison.OrdinalIgnoreCase) && // Mesmo padrão base
                               string.Equals(targetSufixo, sufixo, StringComparison.OrdinalIgnoreCase); // Mesmo sufixo
                    });

                    // Verifica se a sugestão encontrada é válida (não é EP PRO bloqueado)
                    if (!string.IsNullOrEmpty(candidatoSufixoExato) &&
                        FiltrarEpProSeNecessario(new[] { candidatoSufixoExato }).Any())
                    {
                        return candidatoSufixoExato; // Encontrado! ex: 2º EM INT 2
                    }
                }

                // 2. Fallback: Tentar encontrar turma com mesmo padrão (pegar a primeira, ex: "INT 1")
                var candidatoMesmoPadrao = FiltrarEpProSeNecessario(
                    turmasPermit.Where(t =>
                        Regex.IsMatch(t, @"^" + Regex.Escape(proximaSerie.ToString()) + @"[°º]", RegexOptions.IgnoreCase) &&
                        string.Equals(RemoverPrefixo(t), padrao, StringComparison.OrdinalIgnoreCase)
                    ).OrderBy(t => t) // Ordenar para pegar "1" primeiro
                ).FirstOrDefault();

                if (!string.IsNullOrEmpty(candidatoMesmoPadrao))
                    return candidatoMesmoPadrao; // Encontrado! ex: 2º EM INT 1

                // 3. Fallback: Qualquer turma da próxima série (se nenhum padrão bater)
                var anyProxima = FiltrarEpProSeNecessario(
                    turmasPermit.Where(t =>
                        Regex.IsMatch(t, @"^" + Regex.Escape(proximaSerie.ToString()) + @"[°º]", RegexOptions.IgnoreCase)
                    ).OrderBy(t => t)
                ).FirstOrDefault();

                return anyProxima; // Pode ser null
            };

            // Aplicar a lógica
            if (serieAtual == 2) // 2º -> 3º
            {
                var sugestao = ObterProximaTurmaPorSufixo(3);
                return sugestao ?? "EGRESSO";
            }

            if (serieAtual == 1) // 1º -> 2º
            {
                var sugestao = ObterProximaTurmaPorSufixo(2);
                return sugestao ?? "EGRESSO";
            }

            return "EGRESSO"; // Padrão
                              
        }




        #endregion

        #region Controle do Wizard
        private void MostrarEtapa(int etapa)
        {
            _etapaAtual = etapa;

            // Esconder todos os painéis
            panelEtapa1.Visible = false;
            panelEtapa2.Visible = false;
            panelEtapa3.Visible = false;

            // Mostrar painel da etapa atual
            switch (etapa)
            {
                case 1:
                    panelEtapa1.Visible = true;
                    btnAnterior.Enabled = false;
                    btnProximo.Text = "Avançar";
                    ConfigurarEtapa1();
                    break;
                case 2:
                    panelEtapa2.Visible = true;
                    btnAnterior.Enabled = true;
                    btnProximo.Text = "Avançar";
                    ConfigurarEtapa2();
                    break;
                case 3:
                    panelEtapa3.Visible = true;
                    btnAnterior.Enabled = true;
                    btnProximo.Text = "Aplicar";
                    ConfigurarEtapa3();
                    break;
            }

            AtualizarProgressoWizard();
        }

        private void AtualizarProgressoWizard()
        {
            // garantir valores válidos no progress bar
            var val = Math.Max(1, Math.Min(3, _etapaAtual));
            progressBarWizard.Value = (val * 100) / 3;
            lblProgressoWizard.Text = $"Etapa {_etapaAtual} de 3";
        }

        private void BtnProximo_Click(object sender, EventArgs e)
        {
            // Observação: handler protegido contra dupla execução pela remoção no construtor.
            switch (_etapaAtual)
            {
                case 1 when ValidarEtapa1():
                    AplicarPadroesDefinidos();
                    MostrarEtapa(2);
                    MostrarTutorialEtapa(2);
                    break;
                case 2 when ValidarEtapa2():
                    MostrarEtapa(3);
                    MostrarTutorialEtapa(3);
                    break;
                case 3:
                    AplicarMapeamento();
                    break;
            }
        }

        private void MostrarTutorialEtapa(int etapa)
        {
            switch (etapa)
            {
                case 1:
                    lblTutorialTitulo.Text = "📘 ETAPA 1: DEFINIR TURMAS PADRÃO";
                    lblTutorialTexto.Text =
                        "Nesta etapa você define as **turmas padrão**.\n" +
                        "➡ Cada turma atual do sistema terá uma sugestão automática do próximo ano ou curso técnico.\n" +
                        "💡 Dica: revise as sugestões e ajuste apenas se necessário para evitar inconsistências.\n" +
                        "✅ O sistema aplicará automaticamente essas escolhas na Etapa 2.";
                    break;

                case 2:
                    lblTutorialTitulo.Text = "📝 ETAPA 2: AJUSTES INDIVIDUAIS";
                    lblTutorialTexto.Text =
                        "Aqui você pode **ajustar cada aluno individualmente**.\n" +
                        "➡ Alunos destacados possuem progressão ou status especial (EGRESSO, TRANSFERIDO, DESISTENTE).\n" +
                        "💡 Dica: selecione apenas opções válidas no combo para garantir a progressão correta.\n" +
                        "🔎 Use o filtro de *turma e nome* para facilitar a revisão.";
                    break;

                case 3:
                    lblTutorialTitulo.Text = "✅ ETAPA 3: CONFIRMAÇÃO E APLICAÇÃO";
                    lblTutorialTexto.Text =
                        "Revise o **resumo final do mapeamento** antes de aplicar.\n" +
                        "➡ Confirme que todas as alterações estão corretas.\n" +
                        "💡 Dica: o sistema salvará um registro de mapeamento para histórico e futuras consultas.\n" +
                        "🔒 Será necessário confirmar sua senha para aplicar as alterações.";
                    break;
            }
        }

        private void BtnAnterior_Click(object sender, EventArgs e)
        {
            if (_etapaAtual > 1)
            {
                _etapaAtual--;
                MostrarEtapa(_etapaAtual);
                MostrarTutorialEtapa(_etapaAtual);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Deseja realmente cancelar o mapeamento? Todas as alterações serão perdidas.",
                                       "Confirmar Cancelamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
        #endregion

        #region Etapa 1: Definir Padrões
        // Etapa 1: definir padrões – garantir ordenação estável por turma
        private void ConfigurarEtapa1()
        {
            // remover handlers antigos para evitar múltiplas inscrições
            dgvPadroes.CellValueChanged -= DgvPadroes_CellValueChanged;
            dgvPadroes.CurrentCellDirtyStateChanged -= DgvPadroes_CurrentCellDirtyStateChanged;
            dgvPadroes.CellEndEdit -= DgvPadroes_CellEndEdit;
            dgvPadroes.DataError -= DgvPadroes_DataError;

            dgvPadroes.Rows.Clear();
            dgvPadroes.Columns.Clear();

            dgvPadroes.Columns.Add("TurmaAtual", "Turma Atual");
            dgvPadroes.Columns.Add("QtdAlunos", "Qtd Alunos");
            dgvPadroes.Columns.Add("SugestaoSistema", "Sugestão do Sistema");

            var colNovoPadrao = new DataGridViewComboBoxColumn
            {
                Name = "NovoPadrao",
                HeaderText = "Novo Padrão",
                FlatStyle = FlatStyle.Flat
            };
            dgvPadroes.Columns.Add(colNovoPadrao);

            dgvPadroes.CellValueChanged += DgvPadroes_CellValueChanged;
            dgvPadroes.CurrentCellDirtyStateChanged += DgvPadroes_CurrentCellDirtyStateChanged;
            dgvPadroes.CellEndEdit += DgvPadroes_CellEndEdit;
            dgvPadroes.DataError += DgvPadroes_DataError;

            // NOVO: ordem alfabética por TurmaAtual para UX consistente
            foreach (var kvp in _padroesTurma.OrderBy(p => p.Key, StringComparer.OrdinalIgnoreCase))
            {
                var qtdAlunos = _registrosOriginais.Count(r => r.TurmaAtual == kvp.Key);
                var rowIndex = dgvPadroes.Rows.Add(kvp.Key, qtdAlunos, kvp.Value);

                ConfigurarOpcoesValidasPorTurma(rowIndex, kvp.Key);
                dgvPadroes.Rows[rowIndex].Cells["NovoPadrao"].Value = kvp.Value;
            }

            EstilizarDataGrid(dgvPadroes);
            GarantirApenasComboEditavel(dgvPadroes, new[] { "NovoPadrao" }, alturaLinhaFixa: 35);
        }

        // Torna o tratamento de erro do combo mais resiliente na Etapa 1
        private void DgvPadroes_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is ArgumentException && dgvPadroes.Columns[e.ColumnIndex].Name == "NovoPadrao")
            {
                var turmaAtual = dgvPadroes.Rows[e.RowIndex].Cells["TurmaAtual"].Value?.ToString();
                var combo = dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"] as DataGridViewComboBoxCell;

                // Tenta aplicar o padrão salvo, caso ele exista na lista; senão, usa a primeira opção válida
                if (!string.IsNullOrEmpty(turmaAtual) && _padroesTurma.ContainsKey(turmaAtual))
                {
                    var sugerido = _padroesTurma[turmaAtual];
                    if (combo != null && combo.Items.Contains(sugerido))
                        dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"].Value = sugerido;
                    else if (combo != null && combo.Items.Count > 0)
                        dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"].Value = combo.Items[0];
                    else
                        dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"].Value = null;
                }
                else if (combo != null && combo.Items.Count > 0)
                {
                    dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"].Value = combo.Items[0];
                }

                e.ThrowException = false;
            }
        }

        // Etapa 1: popula o combo de "Novo Padrão" SEM "TRANSFERIDO" / "DESISTENTE"
        private void ConfigurarOpcoesValidasPorTurma(int rowIndex, string turmaAtual)
        {
            var cell = (DataGridViewComboBoxCell)dgvPadroes.Rows[rowIndex].Cells["NovoPadrao"];
            var opcoesValidas = ObterOpcoesValidasParaTurmaInteligente(turmaAtual);

            // Remover status que não devem ser padrão de uma turma inteira (apenas Etapa 1)
            opcoesValidas = opcoesValidas
                .Where(o => !string.Equals(o, "TRANSFERIDO", StringComparison.OrdinalIgnoreCase)
                         && !string.Equals(o, "DESISTENTE", StringComparison.OrdinalIgnoreCase))
                .ToList();

            cell.Items.Clear();

            var itensUnicos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var opcao in opcoesValidas)
            {
                if (itensUnicos.Add(opcao))
                    cell.Items.Add(opcao);
            }

            var valorAtual = _padroesTurma.ContainsKey(turmaAtual) ? _padroesTurma[turmaAtual] : "";

            // Garante que o valor atual só é aplicado se existir na lista filtrada
            if (!string.IsNullOrEmpty(valorAtual) && cell.Items.Contains(valorAtual))
                cell.Value = valorAtual;
            else if (cell.Items.Count > 0)
                cell.Value = cell.Items[0];
            else
                cell.Value = null;
        }

        /// <summary>
        /// Opções válidas para turma, baseadas em TurmasUtil.TurmasPermitidas e regras de progressão.
        /// Inclui 9º Ano -> 1º EM/1º Técnico e bloqueia EP PRO para origem EM/Técnico.
        /// </summary>
        private List<string> ObterOpcoesValidasParaTurmaInteligente(string turmaAtual)
        {
            var opcoes = new List<string>();
            if (string.IsNullOrWhiteSpace(turmaAtual))
                return new List<string> { "EGRESSO" };

            var upper = turmaAtual.ToUpperInvariant();
            if (upper == "EGRESSO" || upper == "TRANSFERIDO" || upper == "DESISTENTE")
                return new List<string> { upper };

            var match = Regex.Match(turmaAtual, @"^(\d+)[°º]?\s*(.+)$", RegexOptions.IgnoreCase);
            if (!match.Success)
                return new List<string> { "EGRESSO" };

            int serieAtual = int.Parse(match.Groups[1].Value);
            var turmasPermitidas = TurmasUtil.TurmasPermitidas;
            var cursoAtual = ExtrairCurso(turmaAtual);

       
            bool bloquearEpPro = string.Equals(cursoAtual, "AnoEM", StringComparison.OrdinalIgnoreCase) ||
                                 IsCursoTecnico(turmaAtual) ||
                                 string.Equals(cursoAtual, "Ano", StringComparison.OrdinalIgnoreCase);
            IEnumerable<string> FiltrarEpProSeNecessario(IEnumerable<string> lista)
                => bloquearEpPro ? lista.Where(t => !IsEpPro(t)) : lista;

            Func<int, IEnumerable<string>> turmasPorSerie = (s) =>
                turmasPermitidas.Where(t => {
                    var mm = Regex.Match(t, @"^(\d+)[°º]");
                    return mm.Success && int.Parse(mm.Groups[1].Value) == s;
                });

            if (serieAtual == 3)
            {
                opcoes.Add("EGRESSO");
                opcoes.AddRange(FiltrarEpProSeNecessario(turmasPorSerie(3)));
                return opcoes.Distinct().OrderBy(o => o).ToList();
            }

            if (serieAtual == 2)
            {
                opcoes.AddRange(FiltrarEpProSeNecessario(turmasPorSerie(3)));
                opcoes.AddRange(FiltrarEpProSeNecessario(turmasPorSerie(2)));
                opcoes.Add("DESISTENTE");
                opcoes.Add("TRANSFERIDO");
                return opcoes.Distinct().OrderBy(o => o).ToList();
            }

            if (serieAtual == 1)
            {
                opcoes.AddRange(FiltrarEpProSeNecessario(turmasPorSerie(2)));
                opcoes.AddRange(FiltrarEpProSeNecessario(turmasPorSerie(1)));
                opcoes.Add("DESISTENTE");
                opcoes.Add("TRANSFERIDO");
                return opcoes.Distinct().OrderBy(o => o).ToList();
            }

            // EF (6º ao 9º)
            var ehEF = string.Equals(cursoAtual, "Ano", StringComparison.OrdinalIgnoreCase);
            if (ehEF)
            {
                var proxima = serieAtual + 1;

                // Progressão normal no EF
                opcoes.AddRange(turmasPermitidas.Where(t =>
                    Regex.IsMatch(t, @"^" + Regex.Escape(proxima.ToString()) + @"[°º]", RegexOptions.IgnoreCase) &&
                    string.Equals(ExtrairCurso(t), "Ano", StringComparison.OrdinalIgnoreCase)));

                // Reprovação
                opcoes.AddRange(turmasPermitidas.Where(t =>
                    Regex.IsMatch(t, @"^" + Regex.Escape(serieAtual.ToString()) + @"[°º]", RegexOptions.IgnoreCase) &&
                    string.Equals(ExtrairCurso(t), "Ano", StringComparison.OrdinalIgnoreCase)));

                // Transição 9º -> 1º EM (INT/REG) e 9º -> 1º Técnico
                if (serieAtual == 9)
                {
                    var turmas1Serie = turmasPorSerie(1).ToList();
                    var em1 = turmas1Serie.Where(t => string.Equals(ExtrairCurso(t), "AnoEM", StringComparison.OrdinalIgnoreCase));
                    var tecn1 = turmas1Serie.Where(t => IsCursoTecnico(t));

                    opcoes.AddRange(FiltrarEpProSeNecessario(em1));
                    opcoes.AddRange(FiltrarEpProSeNecessario(tecn1));
                }

                opcoes.Add("DESISTENTE");
                opcoes.Add("TRANSFERIDO");
            }

            if (!opcoes.Any())
                opcoes.Add("EGRESSO");

            // filtro final contra EP PRO quando origem é EM ou Técnico
            opcoes = FiltrarEpProSeNecessario(opcoes).ToList();

            return opcoes.Distinct().OrderBy(o => o).ToList();
        }

        /// <summary>
        /// Verifica se o nome da turma contém palavras-chave de cursos técnicos.
        /// Diferente de IsCursoTecnico, ESTA FUNÇÃO NÃO ignora turmas "EM".
        /// </summary>
        private bool ContemPalavraTecnica(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma)) return false;
            var t = turma.ToLowerInvariant();

            return t.Contains("desenv") || t.Contains("sistemas") ||
                   t.Contains("agroneg") || t.Contains("propedêutic") ||
                   t.Contains("propedeut") || t.Contains("proped") ||
                   t.Contains("admin") || t.Contains("eletromec");
        }

        // Função para identificar se a turma é técnica (apenas curso técnico puro; EM INT/REG não é técnico puro)
        private bool IsCursoTecnico(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma)) return false;
            var t = turma.ToLowerInvariant();

            // Qualquer turma com EM (EM INT/EM REG) deve ser tratada como Ensino Médio, não técnico puro
            if (Regex.IsMatch(t, @"\bem\b"))
                return false;

            return t.Contains("desenv") || t.Contains("sistemas") ||
                   t.Contains("agroneg") || t.Contains("propedêutic") ||
                   t.Contains("propedeut") || t.Contains("proped") ||
                   t.Contains("admin") || t.Contains("eletromec");
        }

        // NOVO: identifica turmas EP PRO
        private bool IsEpPro(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma)) return false;
            return Regex.IsMatch(turma, @"\bEP\s*PRO\b", RegexOptions.IgnoreCase);
        }

        // Extração de curso robusta (prioriza EM/EM INT/EM REG antes dos nomes de curso)
        private string ExtrairCurso(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma))
                return "";

            var t = turma.ToLowerInvariant();

            // Ens. Médio (INT/REG) primeiro, para não classificar EM INT como "Desenvolvimento", etc.
            if (t.Contains("em int") || t.Contains("em reg") || Regex.IsMatch(t, @"\bem\b"))
                return "AnoEM";

            // Técnicos puros
            if (t.Contains("desenv") || t.Contains("sistemas")) return "Desenvolvimento";
            if (t.Contains("agroneg")) return "Agronegócio";
            if (t.Contains("propedeut") || t.Contains("proped")) return "Propedeutico";
            if (t.Contains("admin")) return "Administração";
            if (t.Contains("eletromec")) return "Eletromecânica";

            // *** CORREÇÃO: Fundamental (6º ao 9º ano) - detectar "EF" ou "ano" ***
            if (t.Contains("ef") || t.Contains("ano")) return "Ano";

            return "";
        }





        // Validação de progressão corrigida (cobre EM INT lateral e 9º->1º EM)
        private bool IsProgressaoInvalida(string turmaAtual, string novaTurma)
        {
            if (string.IsNullOrWhiteSpace(turmaAtual) || string.IsNullOrWhiteSpace(novaTurma))
                return true;

            if (turmaAtual == "EGRESSO")
                return novaTurma != "EGRESSO";

            if (turmaAtual == "TRANSFERIDO" || turmaAtual == "DESISTENTE")
                return !(novaTurma == "EGRESSO" || novaTurma == "TRANSFERIDO" || novaTurma == "DESISTENTE");

            if (novaTurma == "DESISTENTE" || novaTurma == "TRANSFERIDO")
                return false;

            var serieAtual = ExtrairNumeroSerie(turmaAtual);
            var serieNova = ExtrairNumeroSerie(novaTurma);
            var cursoAtual = ExtrairCurso(turmaAtual);
            var cursoNovo = ExtrairCurso(novaTurma);

          
            if ((string.Equals(cursoAtual, "AnoEM", StringComparison.OrdinalIgnoreCase) || IsCursoTecnico(turmaAtual) || string.Equals(cursoAtual, "Ano", StringComparison.OrdinalIgnoreCase)) && IsEpPro(novaTurma))
                return true;

            // 3º ano pode trocar para qualquer turma do 3º ano (técnico ou EM)
            if (serieAtual == 3 && serieNova == 3)
                return false;

            // 3º ano (qualquer modalidade) pode ir para EGRESSO
            if (serieAtual == 3 && string.Equals(novaTurma, "EGRESSO", StringComparison.OrdinalIgnoreCase))
                return false;

            // 2º ano técnico: pode ir para qualquer 3º ano técnico
            if (IsCursoTecnico(turmaAtual) && serieAtual == 2 && serieNova == 3 && IsCursoTecnico(novaTurma))
                return false;

            // Reprovação no 2º ano: pode ir para qualquer 2º ano do mesmo curso/tipo
            if (serieAtual == 2 && serieNova == 2 && string.Equals(cursoAtual, cursoNovo, StringComparison.OrdinalIgnoreCase))
                return false;

            // Troca lateral entre modalidades do EM (REG ↔ INT) na mesma série (1º, 2º ou 3º)
            if (string.Equals(cursoAtual, "AnoEM", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(cursoNovo, "AnoEM", StringComparison.OrdinalIgnoreCase) &&
                serieAtual == serieNova)
                return false;

            // Troca entre cursos técnicos do mesmo ano (2º ou 3º)
            if (!string.Equals(cursoAtual, cursoNovo, StringComparison.OrdinalIgnoreCase) &&
                serieAtual == serieNova &&
                (serieNova == 2 || serieNova == 3) &&
                IsCursoTecnico(turmaAtual) && IsCursoTecnico(novaTurma))
                return false;

            // Progressão normal (mesmo curso, série +1)
            if (string.Equals(cursoAtual, cursoNovo, StringComparison.OrdinalIgnoreCase) && serieNova == serieAtual + 1)
                return false;

            // Progressão fundamental (6º ao 9º ano)
            if (string.Equals(cursoAtual, "Ano", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(cursoNovo, "Ano", StringComparison.OrdinalIgnoreCase) &&
                serieNova == serieAtual + 1)
                return false;

            // 9º ano pode ir para 1º EM (INT/REG) ou 1º técnico
            if (string.Equals(cursoAtual, "Ano", StringComparison.OrdinalIgnoreCase) &&
                serieAtual == 9 && serieNova == 1 &&
                (string.Equals(cursoNovo, "AnoEM", StringComparison.OrdinalIgnoreCase) || IsCursoTecnico(novaTurma)))
                return false;

            return true;
        }


        private int ExtrairNumeroSerie(string turma)
        {
            if (string.IsNullOrWhiteSpace(turma))
                return 0;

            var match = Regex.Match(turma, @"(\d+)[°º]");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int numero))
                return numero;

            // Se não há símbolo ordinal, tenta pegar número no começo
            match = Regex.Match(turma, @"^(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out numero))
                return numero;

            return 0;
        }


        private void DgvPadroes_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPadroes.IsCurrentCellDirty)
                dgvPadroes.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgvPadroes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvPadroes.Columns[e.ColumnIndex].Name == "NovoPadrao")
            {
                var turmaAtual = dgvPadroes.Rows[e.RowIndex].Cells["TurmaAtual"].Value?.ToString();
                var novaEscolha = dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"].Value?.ToString();

                if (!string.IsNullOrEmpty(turmaAtual) && !string.IsNullOrEmpty(novaEscolha) &&
                    IsEscolhaInvalidaParaPadrao(turmaAtual, novaEscolha))
                {
                    MessageBox.Show($"Escolha inválida: {turmaAtual} não pode ir para {novaEscolha}",
                                  "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvPadroes.Rows[e.RowIndex].Cells["NovoPadrao"].Value = _padroesTurma[turmaAtual];
                }
            }
        }

        private void DgvPadroes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvPadroes.Columns[e.ColumnIndex].Name == "NovoPadrao")
            {
                var cell = dgvPadroes.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var valor = cell.Value?.ToString();
                var opcoes = ((DataGridViewComboBoxCell)cell).Items.Cast<string>().ToList();
                if (!string.IsNullOrEmpty(valor) && !opcoes.Contains(valor))
                {
                    MessageBox.Show("Selecione uma turma válida da lista de turmas permitidas.", "Turma inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cell.Value = "";
                    dgvPadroes.CurrentCell = cell;
                    dgvPadroes.BeginEdit(true);
                }
            }
        }

        private bool IsEscolhaInvalidaParaPadrao(string turmaAtual, string novaEscolha)
        {
            if ((turmaAtual == "EGRESSO" || turmaAtual == "TRANSFERIDO" || turmaAtual == "DESISTENTE") &&
                novaEscolha != turmaAtual)
                return true;

            if ((novaEscolha == "DESISTENTE" || novaEscolha == "TRANSFERIDO") &&
                !(turmaAtual == "DESISTENTE" || turmaAtual == "TRANSFERIDO"))
                return true;

            return IsProgressaoInvalida(turmaAtual, novaEscolha);
        }
        #endregion

        #region Etapa 2: Ajustes Individuais
        private void ConfigurarEtapa2()
        {
            cmbFiltroTurmaEtapa2.SelectedIndexChanged -= CmbFiltroTurmaEtapa2_SelectedIndexChanged;

            cmbFiltroTurmaEtapa2.Items.Clear();
            cmbFiltroTurmaEtapa2.Items.Add("(Todas as Turmas)");

            var turmasComAlunos = _registrosOriginais
                .Where(r => !string.IsNullOrWhiteSpace(r.TurmaAtual))
                .GroupBy(r => r.TurmaAtual)
                .Select(g => g.Key)
                .OrderBy(t => t);

            foreach (var turma in turmasComAlunos)
                cmbFiltroTurmaEtapa2.Items.Add(turma);

            cmbFiltroTurmaEtapa2.SelectedIndex = 0;
            cmbFiltroTurmaEtapa2.SelectedIndexChanged += CmbFiltroTurmaEtapa2_SelectedIndexChanged;

            txtBuscaAlunoEtapa2.TextChanged -= TxtBuscaAlunoEtapa2_TextChanged;
            txtBuscaAlunoEtapa2.TextChanged += TxtBuscaAlunoEtapa2_TextChanged;


            ConfigurarDataGridEtapa2();
            AtualizarGridEtapa2();
            GarantirApenasComboEditavel(dgvAjustesIndividuais, new[] { "NovaEscolha", "Observacao" }, alturaLinhaFixa: 35);
        }

        // --- ADICIONE ESTE NOVO MÉTODO ---
        private void TxtBuscaAlunoEtapa2_TextChanged(object sender, EventArgs e)
        {
            // Apenas chama o método de atualização, que agora lerá ambos os campos
            AtualizarGridEtapa2();

            // É importante chamar isso de novo para manter o estilo/comportamento da grade
            GarantirApenasComboEditavel(dgvAjustesIndividuais, new[] { "NovaEscolha", "Observacao" }, alturaLinhaFixa: 35);
        }
        // --- FIM DO NOVO MÉTODO ---

        private void ConfigurarDataGridEtapa2()
        {
            dgvAjustesIndividuais.Columns.Clear();

            dgvAjustesIndividuais.Columns.Add("Nome", "Nome do Aluno");
            dgvAjustesIndividuais.Columns.Add("TurmaAtual", "Turma Atual");
            dgvAjustesIndividuais.Columns.Add("PadraoDefinido", "Padrão Definido");

            var colNovaEscolha = new DataGridViewComboBoxColumn
            {
                Name = "NovaEscolha",
                HeaderText = "Nova Escolha",
                FlatStyle = FlatStyle.Flat
            };

            dgvAjustesIndividuais.Columns.Add(colNovaEscolha);
            dgvAjustesIndividuais.Columns.Add("Observacao", "Observação");

            dgvAjustesIndividuais.CellValueChanged += DgvAjustesIndividuais_CellValueChanged;
            dgvAjustesIndividuais.CurrentCellDirtyStateChanged += DgvAjustesIndividuais_CurrentCellDirtyStateChanged;
            dgvAjustesIndividuais.CellEndEdit += DgvAjustesIndividuais_CellEndEdit;
            dgvAjustesIndividuais.DataError += DgvAjustesIndividuais_DataError;

            EstilizarDataGrid(dgvAjustesIndividuais);
        }

        private void DgvAjustesIndividuais_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is ArgumentException && dgvAjustesIndividuais.Columns[e.ColumnIndex].Name == "NovaEscolha")
            {
                var padraoDefinido = dgvAjustesIndividuais.Rows[e.RowIndex].Cells["PadraoDefinido"].Value?.ToString();
                if (!string.IsNullOrEmpty(padraoDefinido))
                {
                    dgvAjustesIndividuais.Rows[e.RowIndex].Cells["NovaEscolha"].Value = padraoDefinido;
                }
                e.ThrowException = false;
            }
        }

        private void DgvAjustesIndividuais_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvAjustesIndividuais.IsCurrentCellDirty)
                dgvAjustesIndividuais.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DgvAjustesIndividuais_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvAjustesIndividuais.Columns[e.ColumnIndex].Name == "NovaEscolha")
            {
                var turmaAtual = dgvAjustesIndividuais.Rows[e.RowIndex].Cells["TurmaAtual"].Value?.ToString();
                var novaEscolha = dgvAjustesIndividuais.Rows[e.RowIndex].Cells["NovaEscolha"].Value?.ToString();
                var nome = dgvAjustesIndividuais.Rows[e.RowIndex].Cells["Nome"].Value?.ToString();

                if (!string.IsNullOrEmpty(turmaAtual) && !string.IsNullOrEmpty(novaEscolha) &&
                    IsProgressaoInvalida(turmaAtual, novaEscolha))
                {
                    MessageBox.Show($"Progressão inválida detectada para {nome}: {turmaAtual} → {novaEscolha}",
                                  "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dgvAjustesIndividuais.Rows[e.RowIndex].Cells["NovaEscolha"].Value =
                        dgvAjustesIndividuais.Rows[e.RowIndex].Cells["PadraoDefinido"].Value;
                }
            }
        }

        private void DgvAjustesIndividuais_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvAjustesIndividuais.Columns[e.ColumnIndex].Name == "NovaEscolha")
            {
                var cell = dgvAjustesIndividuais.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var valor = cell.Value?.ToString();
                var opcoes = ((DataGridViewComboBoxCell)cell).Items.Cast<string>().ToList();
                if (!string.IsNullOrEmpty(valor) && !opcoes.Contains(valor))
                {
                    MessageBox.Show("Selecione uma turma válida da lista de turmas permitidas.", "Turma inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cell.Value = "";
                    dgvAjustesIndividuais.CurrentCell = cell;
                    dgvAjustesIndividuais.BeginEdit(true);
                }
            }
        }

        private void AtualizarGridEtapa2()
        {
            dgvAjustesIndividuais.Rows.Clear();

            // 1. Obter o filtro da turma (ComboBox)
            var filtroTurma = cmbFiltroTurmaEtapa2.SelectedItem?.ToString();

            // 2. Obter o termo de busca (TextBox) - flexível e case-insensitive
            var termoBusca = txtBuscaAlunoEtapa2.Text.Trim();

            // 3. Começar com a lista completa
            var registrosFiltrados = _registrosOriginais.AsEnumerable();

            // 4. Aplicar o filtro de TURMA
            if (!string.IsNullOrEmpty(filtroTurma) && filtroTurma != "(Todas as Turmas)")
                registrosFiltrados = registrosFiltrados.Where(r => r.TurmaAtual == filtroTurma);

            // 5. Aplicar o filtro de BUSCA (Nome)
            if (!string.IsNullOrEmpty(termoBusca))
            {
                registrosFiltrados = registrosFiltrados.Where(r =>
                    // StringComparison.OrdinalIgnoreCase torna a busca "flexível" (ignora maiúsculas/minúsculas)
                    r.Nome != null && r.Nome.IndexOf(termoBusca, StringComparison.OrdinalIgnoreCase) >= 0
                );
            }

            // 6. Popular a grade com os dados filtrados
            foreach (var registro in registrosFiltrados.OrderBy(r => r.Nome))
            {
                var rowIndex = dgvAjustesIndividuais.Rows.Add(
                    registro.Nome,
                    registro.TurmaAtual,
                    registro.NovaTurma, // Padrão Definido (vem da Etapa 1)
                    registro.NovaTurma, // Nova Escolha (valor inicial)
                    registro.Observacao
                );

                ConfigurarOpcoesValidasParaAlunoInteligente(rowIndex, registro.TurmaAtual);
            }
        }

        // Etapa 2: opções individuais – não exibir EGRESSO para séries ≠ 3º
        private void ConfigurarOpcoesValidasParaAlunoInteligente(int rowIndex, string turmaAtual)
        {
            var cell = (DataGridViewComboBoxCell)dgvAjustesIndividuais.Rows[rowIndex].Cells["NovaEscolha"];
            var opcoesValidas = ObterOpcoesValidasParaTurmaInteligente(turmaAtual);

            // NOVO: só incluir EGRESSO quando a série atual for 3º; manter DESISTENTE/TRANSFERIDO
            var serieAtual = ExtrairNumeroSerie(turmaAtual);
            var turmaUpper = (turmaAtual ?? "").ToUpperInvariant();
            var isEspecial = turmaUpper == "EGRESSO" || turmaUpper == "DESISTENTE" || turmaUpper == "TRANSFERIDO";

            // Em casos "especiais", manter lista original (ObterOpcoes... já devolve o status atual).
            if (!isEspecial)
            {
                // garantir DESISTENTE/TRANSFERIDO sempre disponíveis
                if (!opcoesValidas.Contains("DESISTENTE")) opcoesValidas.Add("DESISTENTE");
                if (!opcoesValidas.Contains("TRANSFERIDO")) opcoesValidas.Add("TRANSFERIDO");

                // EGRESSO só para 3º ano
                if (serieAtual == 3 && !opcoesValidas.Contains("EGRESSO"))
                    opcoesValidas.Add("EGRESSO");
                else
                    opcoesValidas.RemoveAll(o => string.Equals(o, "EGRESSO", StringComparison.OrdinalIgnoreCase));
            }

            cell.Items.Clear();
            foreach (var opcao in opcoesValidas.OrderBy(o => o, StringComparer.OrdinalIgnoreCase))
                cell.Items.Add(opcao);

            var valorAtual = dgvAjustesIndividuais.Rows[rowIndex].Cells["NovaEscolha"].Value?.ToString();
            if (!string.IsNullOrEmpty(valorAtual) && !cell.Items.Contains(valorAtual))
                cell.Items.Add(valorAtual);
        }

        private void CmbFiltroTurmaEtapa2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarGridEtapa2();
            GarantirApenasComboEditavel(dgvAjustesIndividuais, new[] { "NovaEscolha", "Observacao" }, alturaLinhaFixa: 35);
        }

        private void GarantirApenasComboEditavel(DataGridView dgv, string[] colunasComboEditaveis, int alturaLinhaFixa = 35)
        {
            if (dgv == null) return;

            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            var setEditaveis = new HashSet<string>(colunasComboEditaveis ?? new string[0], StringComparer.OrdinalIgnoreCase);

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.Resizable = DataGridViewTriState.False;
                col.ReadOnly = !setEditaveis.Contains(col.Name);
            }

            dgv.ReadOnly = false;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            if (alturaLinhaFixa > 0)
                dgv.RowTemplate.Height = alturaLinhaFixa;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Resizable = DataGridViewTriState.False;
                row.Height = alturaLinhaFixa;
            }

            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.CellBeginEdit -= Dgv_CellBeginEdit_BlockNonCombo;
            dgv.CellBeginEdit += Dgv_CellBeginEdit_BlockNonCombo;

            void Dgv_CellBeginEdit_BlockNonCombo(object sender, DataGridViewCellCancelEventArgs ev)
            {
                try
                {
                    var dv = sender as DataGridView;
                    if (dv == null) return;
                    var colName = dv.Columns[ev.ColumnIndex].Name;
                    if (!setEditaveis.Contains(colName))
                        ev.Cancel = true;
                }
                catch
                {
                    // Não propagar erro
                }
            }
        }
        #endregion

        #region Etapa 3: Confirmação
        private void ConfigurarEtapa3()
        {
            var alteracoes = _registrosOriginais.Where(r => r.TurmaAtual != r.NovaTurma).ToList();
            var egressos = _registrosOriginais.Where(r => r.NovaTurma == "EGRESSO" || r.NovaTurma == "TRANSFERIDO").ToList();

            txtResumoFinal.Text = $"RESUMO DO MAPEAMENTO:\n\n";
            txtResumoFinal.Text += $"• Total de alunos: {_registrosOriginais.Count}\n";
            txtResumoFinal.Text += $"• Alterações de turma: {alteracoes.Count}\n";
            txtResumoFinal.Text += $"• Egressos/Transferidos: {egressos.Count}\n\n";

            txtResumoFinal.Text += "ALTERAÇÕES DETALHADAS:\n";
            foreach (var alt in alteracoes.Take(20))
                txtResumoFinal.Text += $"• {alt.Nome}: {alt.TurmaAtual} → {alt.NovaTurma}\n";

            if (alteracoes.Count > 20)
                txtResumoFinal.Text += $"... e mais {alteracoes.Count - 20} alterações.\n";
        }

        private void AplicarMapeamento()
        {
            var result = MessageBox.Show("Confirma a aplicação do mapeamento ao banco de dados? Esta ação não pode ser desfeita.",
                                       "Confirmar Aplicação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            if (!SolicitarConfirmacaoSenha())
                return;

            try
            {
                _model.Registros = _registrosOriginais;
                _model.Status = "pendente";
                SaveMapping(_model);

                using (var cn = Conexao.ObterConexao())
                {
                    cn.Open();
                    using (var tx = cn.BeginTransaction())
                    {
                        foreach (var registro in _registrosOriginais.Where(r => r.TurmaAtual != r.NovaTurma))
                        {
                            using (var cmd = new SqlCeCommand("UPDATE usuarios SET Turma = @turma WHERE Id = @id", cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@turma", registro.NovaTurma ?? "");
                                cmd.Parameters.AddWithValue("@id", registro.UsuarioId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tx.Commit();
                    }
                }

                _model.Status = "concluido";
                SaveMapping(_model);

                MessageBox.Show("Mapeamento aplicado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar mapeamento: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SolicitarConfirmacaoSenha()
        {
            for (int tentativas = 0; tentativas < 3; tentativas++)
            {
                string primeiraSenha, segundaSenha;

                using (var pf = new PasswordForm())
                {
                    pf.Titulo = "Confirmação de Aplicação";
                    pf.Mensagem = $"Digite sua senha (tentativa {tentativas + 1}/3):";
                    if (pf.ShowDialog(this) != DialogResult.OK)
                        return false;

                    primeiraSenha = pf.SenhaDigitada;
                }

                using (var pf = new PasswordForm())
                {
                    pf.Titulo = "Confirmação de Aplicação";
                    pf.Mensagem = $"Confirme sua senha (tentativa {tentativas + 1}/3):";
                    if (pf.ShowDialog(this) != DialogResult.OK)
                        return false;

                    segundaSenha = pf.SenhaDigitada;
                }

                if (primeiraSenha != segundaSenha)
                {
                    MessageBox.Show("As senhas não coincidem. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                if (VerificarSenha(primeiraSenha))
                    return true;

                MessageBox.Show("Senha incorreta.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            MessageBox.Show("Falha na autenticação. Operação cancelada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private bool VerificarSenha(string senha)
        {
            var nome = Sessao.NomeBibliotecariaLogada;
            if (string.IsNullOrWhiteSpace(nome))
                return false;

            try
            {
                using (var cn = Conexao.ObterConexao())
                {
                    cn.Open();
                    using (var cmd = new SqlCeCommand("SELECT Senha_hash, Senha_salt FROM usuarios WHERE Nome = @nome AND TipoUsuario LIKE '%Bibliotec%'", cn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                var hash = r["Senha_hash"]?.ToString() ?? "";
                                var salt = r["Senha_salt"]?.ToString() ?? "";
                                return BibliotecaApp.Utils.CriptografiaSenha.VerificarSenha(senha, hash, salt);
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
        #endregion

        #region Arquivo de Mapeamento
        private string MappingFilePath(int ano) => Path.Combine(_baseFolder, $"mapeamento_{ano}.txt");

        private MapeamentoAnualModel LoadMapping(int ano)
        {
            var path = MappingFilePath(ano);
            if (!File.Exists(path))
                return null;

            try
            {
                var content = File.ReadAllText(path);
                var lines = content.Split('\n');
                var model = new MapeamentoAnualModel();

                foreach (var line in lines)
                {
                    if (line.StartsWith("ANO="))
                        model.Ano = int.Parse(line.Substring(4));
                    else if (line.StartsWith("STATUS="))
                        model.Status = line.Substring(7);
                    else if (line.StartsWith("GERADO_EM="))
                        model.GeradoEm = DateTime.Parse(line.Substring(10));
                }

                return model;
            }
            catch
            {
                return null;
            }
        }

        private bool SaveMapping(MapeamentoAnualModel model)
        {
            var path = MappingFilePath(model.Ano);

            try
            {
                var content = $"ANO={model.Ano}\n";
                content += $"STATUS={model.Status}\n";
                content += $"GERADO_EM={model.GeradoEm:O}\n";
                content += "REGISTROS_START\n";

                foreach (var reg in model.Registros)
                    content += $"{reg.UsuarioId}|{reg.Nome}|{reg.TurmaAtual}|{reg.NovaTurma}|{reg.Observacao}\n";

                content += "REGISTROS_END\n";

                File.WriteAllText(path, content);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Utilitários e Métodos Auxiliares
        private void EstilizarDataGrid(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(20, 42, 60);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 238, 247);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 35;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 61, 88);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;

            dgv.GridColor = Color.FromArgb(235, 239, 244);
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
        }

        // Validação adicional da Etapa 1: impede salvar padrão TRANSFERIDO/DESISTENTE (caso algo escape)
        private bool ValidarEtapa1()
        {
            foreach (DataGridViewRow row in dgvPadroes.Rows)
            {
                var valor = row.Cells["NovoPadrao"].Value?.ToString();

                if (row.Cells["NovoPadrao"].Value == null || string.IsNullOrWhiteSpace(valor))
                {
                    MessageBox.Show("Todos os padrões devem ser definidos antes de prosseguir.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.Equals(valor, "TRANSFERIDO", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(valor, "DESISTENTE", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Na Etapa 1 não é permitido definir 'TRANSFERIDO' ou 'DESISTENTE' como padrão de uma turma inteira.",
                                    "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private bool ValidarEtapa2()
        {
            foreach (DataGridViewRow row in dgvAjustesIndividuais.Rows)
            {
                var turmaAtual = row.Cells["TurmaAtual"].Value?.ToString();
                var novaEscolha = row.Cells["NovaEscolha"].Value?.ToString();

                if (!string.IsNullOrEmpty(turmaAtual) && !string.IsNullOrEmpty(novaEscolha) &&
                    IsProgressaoInvalida(turmaAtual, novaEscolha))
                {
                    var nome = row.Cells["Nome"].Value?.ToString();
                    MessageBox.Show($"Progressão inválida detectada para {nome}: {turmaAtual} → {novaEscolha}",
                                  "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private void AplicarPadroesDefinidos()
        {
            foreach (DataGridViewRow row in dgvPadroes.Rows)
            {
                var turmaAtual = row.Cells["TurmaAtual"].Value?.ToString();
                var novoPadrao = row.Cells["NovoPadrao"].Value?.ToString();

                if (!string.IsNullOrEmpty(turmaAtual) && !string.IsNullOrEmpty(novoPadrao))
                {
                    foreach (var registro in _registrosOriginais.Where(r => r.TurmaAtual == turmaAtual))
                        registro.NovaTurma = novoPadrao;

                    _padroesTurma[turmaAtual] = novoPadrao;
                }
            }
        }
        #endregion

        private void picExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Deseja realmente cancelar o mapeamento? Todas as alterações serão perdidas.",
                                       "Confirmar Cancelamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
