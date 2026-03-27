using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Usuarios;

namespace BibliotecaApp.Forms.Livros
{
    public partial class FichaAlunoForm : Form
    {
        public FichaAlunoForm()
        {
            InitializeComponent();
        }
        public void PreencherAluno(Aluno aluno)
        {
            txtNomeAluno.Text = aluno.Nome;
            txtEmail.Text = aluno.Email;
            txtTurma.Text = aluno.Turma;
            mtxTelefone.Text = aluno.Telefone;
            mtxCPF.Text = aluno.CPF;
            dtpDataNasc.Value = aluno.DataNascimento;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
