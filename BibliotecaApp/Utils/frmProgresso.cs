using System;
using System.Windows.Forms;

namespace BibliotecaApp.Forms.Utils
{
    public partial class frmProgresso : Form
    {
        private readonly Timer _closeTimer;
        private bool _operationComplete;

        public frmProgresso()
        {
            InitializeComponent();

            // Configurações iniciais
            _operationComplete = false;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            // Configura o timer para fechar automaticamente
            _closeTimer = new Timer
            {
                Interval = 1500 // 1.5 segundos após concluir
            };
            _closeTimer.Tick += CloseTimer_Tick;
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            _closeTimer.Stop();
            this.Close();
        }

        public void AtualizarProgresso(int valor, string mensagem)
        {
            if (this.IsDisposed)
                return;

            // Garante que o valor esteja dentro dos limites
            valor = Math.Max(0, Math.Min(valor, 100));

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    UpdateProgressInternal(valor, mensagem);
                }));
            }
            else
            {
                UpdateProgressInternal(valor, mensagem);
            }
        }

        private void UpdateProgressInternal(int valor, string mensagem)
        {
            try
            {
                if (this.IsDisposed)
                    return;

                progressBar1.Value = valor;
                lblStatus.Text = mensagem;

                // Se completou 100% e ainda não foi iniciado o timer
                if (valor >= 100 && !_operationComplete)
                {
                    _operationComplete = true;
                    _closeTimer.Start();
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignora se o formulário já foi descartado
            }
        }



        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _closeTimer?.Stop();
            _closeTimer?.Dispose();
        }

        private void frmProgresso_Load(object sender, EventArgs e)
        {
            // Foca na barra de progresso para melhor visualização
            progressBar1.Focus();
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
    }
}