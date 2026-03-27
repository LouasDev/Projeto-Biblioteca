using BibliotecaApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Livros
{
    public partial class DevoluçãoRapidaForm : Form
    {
        public int QuantidadeDevolvida => int.Parse(numQuantidadeDevolvidos.Text);
        public event EventHandler LivroAtualizado;

        private static bool IsAdminLogado()
            => string.Equals(Sessao.NomeBibliotecariaLogada, "Administrador", StringComparison.OrdinalIgnoreCase);

        public DevoluçãoRapidaForm(int emprestimoId, string professor, string livro, string turma, int quantidadeEmprestada, DateTime dataEmprestimo, string codigoBarras)
        {
            InitializeComponent();

            // Nome do Professor
            txtProfessor.Text = professor;

            //Livro
            txtLivro.Text = livro;

            // Turma
            txtTurma.Text = turma;
            txtTurma.Enabled = false;

            // Quantidade emprestada
            numQuantidadeEmprestado.Text = quantidadeEmprestada.ToString();
            numQuantidadeEmprestado.Enabled = false;

            // Hora do empréstimo
            roundedTextBox2.Text = dataEmprestimo.ToString("HH:mm");
            roundedTextBox2.Enabled = false;

            // Hora da devolução (agora)
            roundedTextBox3.Text = DateTime.Now.ToString("HH:mm");
            roundedTextBox3.Enabled = false;

            // Quantidade devolvida (editável)
            numQuantidadeDevolvidos.Text = quantidadeEmprestada.ToString();
            numQuantidadeDevolvidos.Enabled = true;


            // Preencher o campo de código de barras
            mtxCodigoBarras.Text = codigoBarras;
            mtxCodigoBarras.Enabled = false;

            // Botão de confirmar
            btnConfirmarDevolucao.Click += btnConfirmar_Click;

            // Demais inicializações...
        }

        public DevoluçãoRapidaForm()
        {
            InitializeComponent();

            numQuantidadeDevolvidos.Text = "1"; // valor inicial
            numQuantidadeDevolvidos.KeyPress += numQuantidade_KeyPress;
            numQuantidadeDevolvidos.TextChanged += numQuantidade_TextChanged;
        }


        private void numQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                e.Handled = true;// cancela caracteres não numéricos
        }

        private void numQuantidade_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(numQuantidadeDevolvidos.Text, out int valor) || valor < 1)
            {

                numQuantidadeDevolvidos.SelectionStart = numQuantidadeDevolvidos.Text.Length; // cursor no final
            }
        }

        private void btnMais_Click(object sender, EventArgs e)
        {
            int valor = int.Parse(numQuantidadeDevolvidos.Text);
            valor++;
            numQuantidadeDevolvidos.Text = valor.ToString();

        }

        private void btnMenos_Click(object sender, EventArgs e)
        {
            int valor = int.Parse(numQuantidadeDevolvidos.Text);
            if (valor > 1)
                valor--;
            numQuantidadeDevolvidos.Text = valor.ToString();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (IsAdminLogado())
            {
                MessageBox.Show("Administrador não pode registrar devoluções.", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            int qtdDevolvida = int.Parse(numQuantidadeDevolvidos.Text);
            int qtdEmprestada = int.Parse(numQuantidadeEmprestado.Text);

            if (qtdDevolvida < qtdEmprestada)
            {
                var result = MessageBox.Show(
                    "A quantidade devolvida é menor que a emprestada. Deseja realmente registrar como devolução faltando?",
                    "Confirmação de devolução faltando",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;
            }

            LivroAtualizado?.Invoke(this, EventArgs.Empty);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

       
        
    }
}
