using System;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class ProrrogarDiasForm : Form
    {

        public event EventHandler LivroAtualizado;

        private const int MAX_DIAS = 14; // Limite máximo de dias para prorrogação

        public ProrrogarDiasForm()
        {
            InitializeComponent();

            numQuantidade.KeyPress += numQuantidade_KeyPress;
            numQuantidade.TextChanged += numQuantidade_TextChanged;

            btnConfirmar.Click += btnConfirmar_Click;
            btnCancelar.Click += btnCancelar_Click;

            AtualizarPreviewData();
        }

        public int DiasSelecionados { get; private set; } = 0;
        public DateTime DataDevolucaoAtual { get; set; } = DateTime.Now.Date;

        private void ProrrogarForm_Load(object sender, EventArgs e)
        {
            numQuantidade.Text = "7";
            lblErro.Visible = false;
            AtualizarPreviewData();
        }

        private void numQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true; // cancela caracteres não numéricos
        }

        private void numQuantidade_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(numQuantidade.Text, out int valor) || valor < 1)
            {
                lblErro.Text = "Informe um número válido de dias (mínimo 1).";
                lblErro.Visible = true;
                btnConfirmar.Enabled = false;
            }
            else if (valor > MAX_DIAS)
            {
                numQuantidade.Text = MAX_DIAS.ToString();
                numQuantidade.SelectionStart = numQuantidade.Text.Length;
                lblErro.Text = $"O máximo permitido é {MAX_DIAS} dias.";
                lblErro.Visible = true;
                btnConfirmar.Enabled = false;
            }
            else
            {
                lblErro.Visible = false;
                btnConfirmar.Enabled = true;
            }

            AtualizarPreviewData();
        }

        private void btnMais_Click(object sender, EventArgs e)
        {
            int valor = 1;
            int.TryParse(numQuantidade.Text, out valor);
            if (valor < MAX_DIAS)
            {
                valor++;
                numQuantidade.Text = valor.ToString();
            }
        }

        private void btnMenos_Click(object sender, EventArgs e)
        {
            int valor = 1;
            int.TryParse(numQuantidade.Text, out valor);
            if (valor > 1)
                valor--;
            numQuantidade.Text = valor.ToString();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(numQuantidade.Text, out int valor) || valor < 1 || valor > MAX_DIAS)
            {
                lblErro.Text = $"Informe um número válido de dias (1 a {MAX_DIAS}).";
                lblErro.Visible = true;
                return;
            }

            DiasSelecionados = valor;
            this.DialogResult = DialogResult.OK;
            LivroAtualizado?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AtualizarPreviewData()
        {
            if (int.TryParse(numQuantidade.Text, out int dias) && dias > 0)
            {
                var novaData = DataDevolucaoAtual.AddDays(dias);
                lblPreviewData.Text = $"Devolução: {DataDevolucaoAtual:dd/MM/yyyy} → {novaData:dd/MM/yyyy}";
            }
            else
            {
                lblPreviewData.Text = "Devolução: -";
            }
        }

        
    }
}
