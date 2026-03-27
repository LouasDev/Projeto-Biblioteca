using BibliotecaApp.Forms.Livros;
using BibliotecaApp.Forms.Login;
using BibliotecaApp.Forms.Relatorio;
using BibliotecaApp.Forms.Usuario;
using BibliotecaApp.Froms.Usuario;
using BibliotecaApp.Models;
using BibliotecaApp.Properties;
using BibliotecaApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToggleSwitch;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BibliotecaApp.Forms.Inicio
{
    public partial class MainForm : Form
    {

        private bool menuAnimating = false;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip notifyMenu;

        public MainForm()
        {
            InitializeComponent();
            mdiProp();

            // Inicializa NotifyIcon e menu de contexto
            notifyMenu = new ContextMenuStrip();
            var abrirItem = new ToolStripMenuItem("Abrir");
            var sairItem = new ToolStripMenuItem("Sair");

            abrirItem.Click += (s, e) => RestoreFromTray();
            sairItem.Click += (s, e) =>
            {
                // remove o ícone antes de sair para não deixar resíduo
                try { notifyIcon.Visible = false; notifyIcon.Dispose(); } catch { }
                Application.Exit();
            };

            notifyMenu.Items.Add(abrirItem);
            notifyMenu.Items.Add(new ToolStripSeparator());
            notifyMenu.Items.Add(sairItem);

            notifyIcon = new NotifyIcon
            {
                Icon =  Resources.icon_gastao_valle_zoomed_v2, 
                Visible = false,
                Text = "BibliotecaApp",
                ContextMenuStrip = notifyMenu
            };
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            
        }

        #region Componentes de inicialização

        private void tamanho()
        {
            this.Width = 1440; this.Height = 800;
        }
        private Size tamanhoOriginal;
        private Point localOriginal;
        private bool maximizado = false;
        

        //Funções da API para movimentar a aba
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        //Nome dos Forms
        InicioForm inicio;
        UsuarioForm usuario;
        LivrosForm livros;
        RelForm rel;
        EmprestimoForm emprestimo;
        EmprestimoRapidoForm emprestimoRap;
        CadastroLivroForm cadastroLivro;
        DevoluçãoForm devolução;
        CadUsuario usuarioCad; 
        public EditarUsuarioForm usuarioEdit;

        //Proporção dos Forms
        private void mdiProp()
        {
            this.SetBevel(false);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(232, 234, 237);
        }

        //Variável criada para inicialização do Form de inicio no MDI
        private void btnInicio_Click(object sender, EventArgs e)
        {

            btnIn();

            btnInicio.Enabled = false;
            btnLivros.Enabled = true;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;
        }

        //Função de maximizar/restaurar o Form
        private void AlternarMaximizado()
        {
            
            if (!maximizado)
            {
                tamanhoOriginal = this.Size;
                localOriginal = this.Location;

                Rectangle areaTrabalho = Screen.FromHandle(this.Handle).WorkingArea; // exclui barra de tarefas
                this.Location = areaTrabalho.Location;
                this.Size = areaTrabalho.Size;

                maximizado = true;
            }
            else
            {
                this.Size = tamanhoOriginal;
                this.Location = localOriginal;

                maximizado = false;
            }
        }

        private void PanelControl_MouseDown(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ChecarMapeamentoPendenteAoInicializar()
        {
            try
            {
                AppPaths.EnsureFolders();
                string pasta = AppPaths.MappingFolder;
                string arquivoMap = Path.Combine(pasta, $"mapeamento_{DateTime.Now.Year}.txt");
                string marcador = Path.Combine(pasta, "app_instalado_em.txt");

                // primeira execução: criar marcador e não abrir mapeamento
                if (!File.Exists(marcador))
                {
                    File.WriteAllText(marcador, DateTime.Now.Year.ToString());
                    return;
                }

                // se existe arquivo de mapeamento e está pendente -> perguntar
                if (File.Exists(arquivoMap))
                {
                    try
                    {
                        var json = File.ReadAllText(arquivoMap);
                        using (var doc = JsonDocument.Parse(json))
                        {
                            if (doc.RootElement.TryGetProperty("Status", out var s) &&
                                string.Equals(s.GetString(), "pendente", StringComparison.OrdinalIgnoreCase))
                            {
                                var msg = $"Existe um mapeamento pendente para o ano {DateTime.Now.Year}.\nDeseja executar o mapeamento agora (Sim) ou lembrar depois (Não)?";
                                var resp = MessageBox.Show(msg, "Mapeamento pendente", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                                if (resp == DialogResult.Yes)
                                {
                                    AbrirMapeamentoModal();
                                }
                                // se escolher "Não", nada acontece; aparecerá novamente no próximo startup
                            }
                        }
                    }
                    catch
                    {
                        // ignora erro de leitura para não bloquear app
                    }
                    return;
                }

                // se não existe arquivo de mapeamento e app_instalado_em existe com ano < anoAtual -> primeiro ano após instalação: abrir e gerar automaticamente
                var instaladoStr = File.ReadAllText(marcador).Trim();
                if (int.TryParse(instaladoStr, out int anoInstalado))
                {
                    var anoAtual = DateTime.Now.Year;
                    if (anoInstalado < anoAtual)
                    {
                        var msg = $"É necessário realizar o mapeamento de turmas para o ano {anoAtual}.\nDeseja executar agora (Sim) ou mais tarde (Não)?";
                        var resp = MessageBox.Show(msg, "Mapeamento Obrigatório", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        if (resp == DialogResult.Yes)
                        {   
                            AbrirMapeamentoModal();
                        }
                        // se escolher "Não", aparecerá novamente no próximo startup
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro checar mapeamento: {ex.Message}");
            }
        }
        private void AbrirMapeamentoModal()
        {
            // --- INÍCIO DA MODIFICAÇÃO ---
            // Verificamos se o usuário logado é o "Administrador"
            // Conforme LoginForm.cs, apenas o admin terá esse nome exato na sessão.
            if (string.Equals(Sessao.NomeBibliotecariaLogada, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "O Mapeamento de Turmas é uma função restrita a usuários do tipo 'Bibliotecário(a)'.",
                    "Acesso Negado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return; // Impede a abertura do formulário de mapeamento
            }
            // --- FIM DA MODIFICAÇÃO ---


            var mapeamentoForm = new BibliotecaApp.Forms.Usuario.MapeamentoDeTurmasWizardForm();

            // Configurar como modal dialog
            mapeamentoForm.StartPosition = FormStartPosition.CenterParent;
            mapeamentoForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            mapeamentoForm.MaximizeBox = false;
            mapeamentoForm.MinimizeBox = false;
            mapeamentoForm.ShowInTaskbar = false;

            // Abrir como modal - não permite clicar fora
            var resultado = mapeamentoForm.ShowDialog(this);

            if (resultado == DialogResult.OK)
            {
                
                MessageBox.Show(
                    "Mapeamento de turmas concluído com sucesso!",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (resultado == DialogResult.Cancel)
            {
                MessageBox.Show(
                    "Mapeamento cancelado. Será solicitado novamente na próxima inicialização.",
                    "Mapeamento Cancelado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Botões
        //Form Inicio
        public void btnIn()
        {
            // evita criar múltiplas instâncias do Inicio
            if (inicio == null || inicio.IsDisposed)
            {
                inicio = new InicioForm();
                inicio.FormClosed += Inicio_FormClosed;
            }

            OpenChild(inicio, keepPreviousHidden: false);
            btnInicio.Enabled = false;
        }

        private void Inicio_FormClosed(object sender, FormClosedEventArgs e)
        {
            inicio = null;
            if (activeChild == null) activeChild = null;
        }

        #region Form do Usuários

        
        //Form usuário
        private async void btnUsuario_Click(object sender, EventArgs e)
        {

            if (menuAnimating) return;      // já estamos animando -> ignora
            menuAnimating = true;
            btnUsuario.Enabled = false;
            btnLivro.Enabled = true;

            if (livroContainer.Height > 60)
            {
                livroTransition.Start();
                await Task.Delay(610);
            }

            userTransition.Start();

            await Task.Delay(400);
            menuAnimating = false;
            btnUsuario.Enabled = true;
            // após a animação, garanta que o menu receba foco para não ativar outro MDI child
            menu.Focus();


        }

        // MainForm.cs
        public void SetUserButtonsEnabled(bool userEnabled, bool userEditEnabled)
        {
            btnUser.Enabled = userEnabled;
            btnUserEdit.Enabled = userEditEnabled;
        }

       

        //Botão de cadastro do usuário
        public void btnUser_Click(object sender, EventArgs e)
        {
            btnUser.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            if (usuario == null || usuario.IsDisposed)
            {
                usuario = new UsuarioForm();
                usuario.FormClosed += Usuario_FormClosed;
            }
            OpenChild(usuario, keepPreviousHidden: true); // keepPreviousHidden se quiser manter estado do anterior (opcional)

        }

        //Botão de cadastro do usuário
        private void btnUserCad_Click(object sender, EventArgs e)
        {
            btnUserCad.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserEdit.Enabled = true;

            if (usuarioCad == null || usuarioCad.IsDisposed)
            {
                usuarioCad = new CadUsuario();

                // evitar múltiplos +=
                usuarioCad.UsuarioCriado -= UsuarioCad_UsuarioCriado;
                usuarioCad.UsuarioCriado += UsuarioCad_UsuarioCriado;

                usuarioCad.FormClosed += UsuarioCad_FormClosed;
            }

            OpenChild(usuarioCad, keepPreviousHidden: true);
        }

        private void UsuarioCad_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (usuarioCad != null)
            {
                usuarioCad.UsuarioCriado -= UsuarioCad_UsuarioCriado;
            }
            usuarioCad = null;
        }

        private void UsuarioCad_UsuarioCriado(object sender, EventArgs e)
        {
            // Procura se o UsuarioForm já está aberto como MDI child
            var usuarioForm = this.MdiChildren.OfType<BibliotecaApp.Forms.Usuario.UsuarioForm>().FirstOrDefault();
            if (usuarioForm != null)
            {
                // chama método público para recarregar a grid
                usuarioForm.RefreshGrid();
            }
        }

        //Botão de edição do usuário
        public void btnUserEdit_Click(object sender, EventArgs e)
        {
            btnUserEdit.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;

            if (usuarioEdit == null || usuarioEdit.IsDisposed)
            {
                usuarioEdit = new EditarUsuarioForm();
                usuarioEdit.FormClosed += UsuarioEdit_FormClosed;
            }
            OpenChild(usuarioEdit, keepPreviousHidden: true);
        }

        private void UsuarioEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            ResetarUsuarioEdit();
        }


        public void ResetarUsuarioEdit()
        {
            usuarioEdit = null;
        }

        //Botão Usuário(Biblioteca)

        #endregion

        private void Usuario_FormClosed(object sender, FormClosedEventArgs e)
        {
            usuario = null;
        }

        #region Form dos Livros
        //Botão Expansão do Livro
        public async void btnLivro_Click(object sender, EventArgs e)
        {
            if (menuAnimating) return;
            btnLivro.Enabled = false;
            menuAnimating = true;
            
            btnUsuario.Enabled = true;

            if (userContainer.Height > 60)
            {
                userTransition.Start();
                await Task.Delay(400);
            }

            livroTransition.Start();
            await Task.Delay(500);
            
            menuAnimating = false;
            btnLivro.Enabled = true;
            menu.Focus();
        }

        //Botão Livro(Bilbioteca)
        private void btnLivros_Click(object sender, EventArgs e)
        {
            btnLivros.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            if (livros == null || livros.IsDisposed)
            {
                livros = new LivrosForm();
                livros.FormClosed += Livros_FormClosed;
            }
            OpenChild(livros, keepPreviousHidden: true);
           livros.btnProcurar_Click(null, null); // Carrega lista inicial
        }
        private void Livros_FormClosed(object sender, FormClosedEventArgs e)
        {
            livros = null;
        }

        //Botão de empréstimo do livro
        private void btnEmprestimo_Click(object sender, EventArgs e)
        {
            btnEmprestimo.Enabled = false;
            btnRel.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            if (emprestimo == null || emprestimo.IsDisposed)
            {
                emprestimo = new EmprestimoForm();
                emprestimo.FormClosed += Emprestimo_FormClosed;
            }
            OpenChild(emprestimo, keepPreviousHidden: true);
        }

        private void Emprestimo_FormClosed(object sender, FormClosedEventArgs e)
        {
            emprestimo = null;
        }

        //Botão Empréstimo Rápido
        public void btnEmprestimoRap_Click(object sender, EventArgs e)
        {
            btnEmprestimoRap.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            if (emprestimoRap == null || emprestimoRap.IsDisposed)
            {
                emprestimoRap = new EmprestimoRapidoForm();
                emprestimoRap.FormClosed += EmprestimoRap_FormClosed;
            }
            OpenChild(emprestimoRap, keepPreviousHidden: true);


        }

        //Botão de empréstimo rápido
        private void EmprestimoRap_FormClosed(object sender, FormClosedEventArgs e)
        {
            emprestimoRap = null; // corrigido
        }
        private void btnLivroCad_Click(object sender, EventArgs e)
        {
            btnLivroCad.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            if (cadastroLivro == null || cadastroLivro.IsDisposed)
            {
                cadastroLivro = new CadastroLivroForm();
                cadastroLivro.FormClosed += CadastroLivro_FormClosed;
            }
            OpenChild(cadastroLivro, keepPreviousHidden: true);


        }

        private void CadastroLivro_FormClosed(object sender, FormClosedEventArgs e)
        {
            cadastroLivro = null;
        }

        //Botão Devolução
        private void btnDev_Click(object sender, EventArgs e)
        {
            btnDev.Enabled = false;
            btnRel.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            if (devolução == null || devolução.IsDisposed)
            {
                devolução = new DevoluçãoForm();
                devolução.FormClosed += Devolução_FormClosed;
            }
            OpenChild(devolução, keepPreviousHidden: true);
        }

        private void Devolução_FormClosed(object sender, FormClosedEventArgs e)
        {
            devolução = null;
        }


        #endregion

        //Form rel
        private void btnRel_Click(object sender, EventArgs e)
        {
            btnRel.Enabled = false;
            btnEmprestimo.Enabled = true;
            btnLivros.Enabled = true;
            btnInicio.Enabled = true;
            btnDev.Enabled = true;
            btnLivroCad.Enabled = true;
            btnEmprestimoRap.Enabled = true;
            btnEmprestimo.Enabled = true;
            btnUser.Enabled = true;
            btnUserCad.Enabled = true;
            btnUserEdit.Enabled = true;

            if (rel == null || rel.IsDisposed)
            {
                rel = new RelForm();
                rel.FormClosed += Relatorios_FormClosed;
            }
            OpenChild(rel, keepPreviousHidden: true);

        }

        private void Relatorios_FormClosed(object sender, FormClosedEventArgs e)
        {
            rel = null;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            const string msg = "Tem certeza de que quer finalizar a sessão?";
            const string box = "Confirmação de logout";
            var confirma = MessageBox.Show(msg, box, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirma == DialogResult.Yes)
            {
                // sinaliza logout para permitir fechamento real do MainForm
                Program.RequestLogout = true;

                // garante liberar notifyIcon
                try { notifyIcon.Visible = false; notifyIcon.Dispose(); } catch { }

                // fecha o MainForm (OnFormClosing permitirá o fechamento por causa da flag)
                this.Close();
            }
        }



        #endregion

        #region Interações/Funcionalidades do Form

        #region Control box
        private void picExit_Click(object sender, EventArgs e)
        {
            var confirma = MessageBox.Show(
                "Tem certeza de que quer sair da aplicação?",
                "Confirmação de saída",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirma == DialogResult.Yes)
            {
                try { notifyIcon.Visible = false; notifyIcon.Dispose(); } catch { }
                Application.Exit(); // Fecha completamente o app
            }
        }




        //Funcionalidade dos botões
        private void picMax_Click(object sender, EventArgs e)
        {
            AlternarMaximizado();

            if (maximizado == false)
            {
               picMax.BackgroundImage = Resources.icons8_quadrado_arredondado_20;
            }
            else
            {
               picMax.BackgroundImage = Resources.icons8_verificar_todos_os_20;
            }
        }
        private void picMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Animação de fundo
        private void picExit_MouseEnter(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Gainsboro;
        }

        private void picExit_MouseLeave(object sender, EventArgs e)
        {
            picExit.BackColor = Color.Transparent;
        }

        private void picMax_MouseEnter(object sender, EventArgs e)
        {
            picMax.BackColor = Color.Gainsboro;
        }

        private void picMax_MouseLeave(object sender, EventArgs e)
        {
            picMax.BackColor = Color.Transparent;
        }

        private void picMin_MouseEnter(object sender, EventArgs e)
        {
            picMin.BackColor = Color.Gainsboro;
        }

        private void picMin_MouseLeave(object sender, EventArgs e)
        {
            picMin.BackColor = Color.Transparent;
        }
        #endregion

        //Load para fechar o Login
        private void MainForm_Load(object sender, EventArgs e)
        {


    

            btnIn();

            if (LoginForm.cancelar == false)
            {
                Application.Exit();
            }

            //Inicializar com tela cheia
            if (LoginForm.cancelar == true)
            {
                AlternarMaximizado();
            }
           

            ChecarMapeamentoPendenteAoInicializar();
        }

        //Locomoção do painel
        private void panelControl_MouseDown(object sender, MouseEventArgs e)
        {
            maximizado = false;
            picMax.BackgroundImage = Resources.icons8_quadrado_arredondado_20;
            tamanho();
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        //Transição de expansão do livro e usuário
        bool userExpand = false;
        bool livroExpand = false;

        // expor leitura do estado para outros forms
        public bool IsLivroExpanded => livroExpand;
        public bool IsMenuAnimating => menuAnimating;

        private void userTransition_Tick(object sender, EventArgs e)
        {
            if (userExpand == false)
            {
                userContainer.Height += 10;
                if (userContainer.Height >= 240)
                {
                    userTransition.Stop();
                    userExpand = true;
                }
            }
            else
            {
                userContainer.Height -= 10;
                if (userContainer.Height <= 60)
                {
                    userTransition.Stop();
                    userExpand = false;
                }
            }
        }
        private void livroTransition_Tick(object sender, EventArgs e)
        {

            if (livroExpand == false)
            {
                livroContainer.Height += 10;
                if (livroContainer.Height >= 360)
                {
                    livroTransition.Stop();
                    livroExpand = true;
                }
            }
            else
            {
                livroContainer.Height -= 10;
                if (livroContainer.Height <= 60)
                {
                    livroTransition.Stop();
                    livroExpand = false;
                }
            }
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
                // Se já existe um filho ativo diferente, esconde/fecha ele
                if (activeChild != null && activeChild != child && !activeChild.IsDisposed)
                {
                    if (keepPreviousHidden) activeChild.Hide();
                    else activeChild.Close(); // fecha para liberar recursos (troque por Hide se preferir manter estado)
                }

                // se a instância atual do child foi descartada, tente recriar? (faça fora se necessário)
                child.MdiParent = this;
                child.Dock = DockStyle.Fill;

                // Se o form já está criado mas escondido, apenas BringToFront
                if (!child.Visible)
                    child.Show();

                child.BringToFront();
                child.Activate();

                activeChild = child;
            }
            catch { /*silent para não quebrar UI*/ }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void RestoreFromTray()
        {
            try
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
                this.Activate();
                this.BringToFront();
            }
            catch { }
        }




        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (Program.RequestLogout)
            {
                try { notifyIcon.Visible = false; notifyIcon.Dispose(); } catch { }
                base.OnFormClosing(e);
                return;
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Fecha normalmente
                try { notifyIcon.Visible = false; notifyIcon.Dispose(); } catch { }
                base.OnFormClosing(e);
            }
            else
            {
                try { notifyIcon.Visible = false; notifyIcon.Dispose(); } catch { }
                base.OnFormClosing(e);
            }
        }




        #endregion

    }
}
