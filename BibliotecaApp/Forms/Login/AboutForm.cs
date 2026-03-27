using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Login
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void picExit_Click(object sender, EventArgs e)
        {
            const string msg = "Tem certeza de que quer fechar a Aplicação?";
            const string box = "Confirmação de Encerramento";
            var confirma = MessageBox.Show(msg, box, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void picExit_MouseEnter(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Gainsboro;
        }

        private void picExit_MouseLeave(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Transparent;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Form loginForm = Application.OpenForms["LoginForm"];

            if (loginForm != null)
            {
                this.Hide();
                loginForm.Show();
                loginForm.BringToFront();
            }
            else
            {
                this.Hide();
                LoginForm novoLogin = new LoginForm();
                novoLogin.Show();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void gradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GitRenato_Click(object sender, EventArgs e)
        {
            OpenUrl("https://github.com/renato0x"); // substitua pela URL correta
        }

        private void GitLuis_Click(object sender, EventArgs e)
        {
            OpenUrl("https://github.com/ManoLouas"); // substitua pela URL correta
        }

        private void GitIthalo_Click(object sender, EventArgs e)
        {
            OpenUrl("https://github.com/Ithaloluzdepanela"); // substitua pela URL correta
        }

        private void GitMatheus_Click(object sender, EventArgs e)
        {
            OpenUrl("https://github.com/MatheusAlmeida10"); // substitua pela URL correta
        }

        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url); // .NET Framework 4.8: abre no navegador padrão
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível abrir o link: " + ex.Message,
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
