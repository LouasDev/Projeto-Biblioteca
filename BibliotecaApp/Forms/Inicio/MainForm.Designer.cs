using System.Security.Cryptography.X509Certificates;

namespace BibliotecaApp.Forms.Inicio
{
    partial class MainForm
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelControl = new System.Windows.Forms.Panel();
            this.ControlPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.picExit = new System.Windows.Forms.PictureBox();
            this.picMax = new System.Windows.Forms.PictureBox();
            this.picMin = new System.Windows.Forms.PictureBox();
            this.sairContainer = new System.Windows.Forms.Panel();
            this.btnSair = new System.Windows.Forms.Button();
            this.relContainer = new System.Windows.Forms.Panel();
            this.btnRel = new System.Windows.Forms.Button();
            this.userContainer = new System.Windows.Forms.Panel();
            this.btnUser = new System.Windows.Forms.Button();
            this.btnUserCad = new System.Windows.Forms.Button();
            this.btnUsuario = new System.Windows.Forms.Button();
            this.btnUserEdit = new System.Windows.Forms.Button();
            this.incioContainer = new System.Windows.Forms.Panel();
            this.btnInicio = new System.Windows.Forms.Button();
            this.menu = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.livroContainer = new System.Windows.Forms.Panel();
            this.btnDev = new System.Windows.Forms.Button();
            this.btnLivros = new System.Windows.Forms.Button();
            this.btnLivroCad = new System.Windows.Forms.Button();
            this.btnEmprestimoRap = new System.Windows.Forms.Button();
            this.btnLivro = new System.Windows.Forms.Button();
            this.btnEmprestimo = new System.Windows.Forms.Button();
            this.livroTransition = new System.Windows.Forms.Timer(this.components);
            this.userTransition = new System.Windows.Forms.Timer(this.components);
            this.panelControl.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMin)).BeginInit();
            this.sairContainer.SuspendLayout();
            this.relContainer.SuspendLayout();
            this.userContainer.SuspendLayout();
            this.incioContainer.SuspendLayout();
            this.menu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.livroContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.AllowDrop = true;
            this.panelControl.BackColor = System.Drawing.Color.White;
            this.panelControl.Controls.Add(this.ControlPanel);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl.ForeColor = System.Drawing.SystemColors.Control;
            this.panelControl.Location = new System.Drawing.Point(205, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(1235, 30);
            this.panelControl.TabIndex = 0;
            this.panelControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelControl_MouseDown);
            // 
            // ControlPanel
            // 
            this.ControlPanel.BackColor = System.Drawing.Color.Transparent;
            this.ControlPanel.Controls.Add(this.picExit);
            this.ControlPanel.Controls.Add(this.picMax);
            this.ControlPanel.Controls.Add(this.picMin);
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ControlPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ControlPanel.Location = new System.Drawing.Point(1127, 0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(108, 30);
            this.ControlPanel.TabIndex = 4;
            // 
            // picExit
            // 
            this.picExit.BackColor = System.Drawing.Color.Transparent;
            this.picExit.BackgroundImage = global::BibliotecaApp.Properties.Resources.icons8_x_20;
            this.picExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picExit.Location = new System.Drawing.Point(80, 3);
            this.picExit.Name = "picExit";
            this.picExit.Size = new System.Drawing.Size(25, 25);
            this.picExit.TabIndex = 3;
            this.picExit.TabStop = false;
            this.picExit.Click += new System.EventHandler(this.picExit_Click);
            this.picExit.MouseEnter += new System.EventHandler(this.picExit_MouseEnter);
            this.picExit.MouseLeave += new System.EventHandler(this.picExit_MouseLeave);
            // 
            // picMax
            // 
            this.picMax.BackColor = System.Drawing.Color.Transparent;
            this.picMax.BackgroundImage = global::BibliotecaApp.Properties.Resources.icons8_verificar_todos_os_20;
            this.picMax.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picMax.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMax.Location = new System.Drawing.Point(49, 3);
            this.picMax.Name = "picMax";
            this.picMax.Size = new System.Drawing.Size(25, 25);
            this.picMax.TabIndex = 6;
            this.picMax.TabStop = false;
            this.picMax.Click += new System.EventHandler(this.picMax_Click);
            this.picMax.MouseEnter += new System.EventHandler(this.picMax_MouseEnter);
            this.picMax.MouseLeave += new System.EventHandler(this.picMax_MouseLeave);
            // 
            // picMin
            // 
            this.picMin.BackColor = System.Drawing.Color.Transparent;
            this.picMin.BackgroundImage = global::BibliotecaApp.Properties.Resources.icons8_menos_20;
            this.picMin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picMin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picMin.Location = new System.Drawing.Point(18, 3);
            this.picMin.Name = "picMin";
            this.picMin.Size = new System.Drawing.Size(25, 25);
            this.picMin.TabIndex = 5;
            this.picMin.TabStop = false;
            this.picMin.Click += new System.EventHandler(this.picMin_Click);
            this.picMin.MouseEnter += new System.EventHandler(this.picMin_MouseEnter);
            this.picMin.MouseLeave += new System.EventHandler(this.picMin_MouseLeave);
            // 
            // sairContainer
            // 
            this.sairContainer.Controls.Add(this.btnSair);
            this.sairContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sairContainer.Location = new System.Drawing.Point(3, 343);
            this.sairContainer.Name = "sairContainer";
            this.sairContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.sairContainer.Size = new System.Drawing.Size(200, 60);
            this.sairContainer.TabIndex = 7;
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(87)))), ((int)(((byte)(174)))));
            this.btnSair.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(78)))), ((int)(((byte)(157)))));
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnSair.ForeColor = System.Drawing.Color.White;
            this.btnSair.Image = global::BibliotecaApp.Properties.Resources.icons8_sair_25;
            this.btnSair.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSair.Location = new System.Drawing.Point(0, -3);
            this.btnSair.Name = "btnSair";
            this.btnSair.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnSair.Size = new System.Drawing.Size(223, 60);
            this.btnSair.TabIndex = 3;
            this.btnSair.TabStop = false;
            this.btnSair.Text = "Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // relContainer
            // 
            this.relContainer.Controls.Add(this.btnRel);
            this.relContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.relContainer.Location = new System.Drawing.Point(3, 277);
            this.relContainer.Name = "relContainer";
            this.relContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.relContainer.Size = new System.Drawing.Size(200, 60);
            this.relContainer.TabIndex = 6;
            // 
            // btnRel
            // 
            this.btnRel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(87)))), ((int)(((byte)(174)))));
            this.btnRel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(78)))), ((int)(((byte)(157)))));
            this.btnRel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRel.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnRel.ForeColor = System.Drawing.Color.White;
            this.btnRel.Image = global::BibliotecaApp.Properties.Resources.icons8_relatório_25;
            this.btnRel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRel.Location = new System.Drawing.Point(0, -3);
            this.btnRel.Name = "btnRel";
            this.btnRel.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnRel.Size = new System.Drawing.Size(223, 60);
            this.btnRel.TabIndex = 3;
            this.btnRel.TabStop = false;
            this.btnRel.Text = "Relatório";
            this.btnRel.UseVisualStyleBackColor = false;
            this.btnRel.Click += new System.EventHandler(this.btnRel_Click);
            // 
            // userContainer
            // 
            this.userContainer.Controls.Add(this.btnUser);
            this.userContainer.Controls.Add(this.btnUserCad);
            this.userContainer.Controls.Add(this.btnUsuario);
            this.userContainer.Controls.Add(this.btnUserEdit);
            this.userContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.userContainer.Location = new System.Drawing.Point(3, 145);
            this.userContainer.Name = "userContainer";
            this.userContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.userContainer.Size = new System.Drawing.Size(200, 60);
            this.userContainer.TabIndex = 4;
            // 
            // btnUser
            // 
            this.btnUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(98)))), ((int)(((byte)(144)))));
            this.btnUser.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(88)))), ((int)(((byte)(130)))));
            this.btnUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUser.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnUser.ForeColor = System.Drawing.Color.Transparent;
            this.btnUser.Image = global::BibliotecaApp.Properties.Resources.icons8_usuário_masculino_25;
            this.btnUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUser.Location = new System.Drawing.Point(0, 59);
            this.btnUser.Name = "btnUser";
            this.btnUser.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnUser.Size = new System.Drawing.Size(200, 60);
            this.btnUser.TabIndex = 15;
            this.btnUser.TabStop = false;
            this.btnUser.Text = "Cadastros";
            this.btnUser.UseVisualStyleBackColor = false;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // btnUserCad
            // 
            this.btnUserCad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(98)))), ((int)(((byte)(144)))));
            this.btnUserCad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(88)))), ((int)(((byte)(130)))));
            this.btnUserCad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserCad.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnUserCad.ForeColor = System.Drawing.Color.Transparent;
            this.btnUserCad.Image = global::BibliotecaApp.Properties.Resources.icons8_adicionar_usuário_masculino_25;
            this.btnUserCad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUserCad.Location = new System.Drawing.Point(0, 119);
            this.btnUserCad.Name = "btnUserCad";
            this.btnUserCad.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnUserCad.Size = new System.Drawing.Size(200, 60);
            this.btnUserCad.TabIndex = 13;
            this.btnUserCad.TabStop = false;
            this.btnUserCad.Text = "Cadastrar";
            this.btnUserCad.UseVisualStyleBackColor = false;
            this.btnUserCad.Click += new System.EventHandler(this.btnUserCad_Click);
            // 
            // btnUsuario
            // 
            this.btnUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(87)))), ((int)(((byte)(174)))));
            this.btnUsuario.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(78)))), ((int)(((byte)(157)))));
            this.btnUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsuario.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnUsuario.ForeColor = System.Drawing.Color.White;
            this.btnUsuario.Image = global::BibliotecaApp.Properties.Resources.icons8_chamada_em_conferência_25;
            this.btnUsuario.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUsuario.Location = new System.Drawing.Point(0, -3);
            this.btnUsuario.Name = "btnUsuario";
            this.btnUsuario.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnUsuario.Size = new System.Drawing.Size(223, 60);
            this.btnUsuario.TabIndex = 3;
            this.btnUsuario.TabStop = false;
            this.btnUsuario.Text = "Usuários";
            this.btnUsuario.UseVisualStyleBackColor = false;
            this.btnUsuario.Click += new System.EventHandler(this.btnUsuario_Click);
            // 
            // btnUserEdit
            // 
            this.btnUserEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(98)))), ((int)(((byte)(144)))));
            this.btnUserEdit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(88)))), ((int)(((byte)(130)))));
            this.btnUserEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnUserEdit.ForeColor = System.Drawing.Color.White;
            this.btnUserEdit.Image = global::BibliotecaApp.Properties.Resources.icons8_registration_25;
            this.btnUserEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUserEdit.Location = new System.Drawing.Point(0, 179);
            this.btnUserEdit.Name = "btnUserEdit";
            this.btnUserEdit.Padding = new System.Windows.Forms.Padding(6, 0, 0, 2);
            this.btnUserEdit.Size = new System.Drawing.Size(200, 60);
            this.btnUserEdit.TabIndex = 14;
            this.btnUserEdit.TabStop = false;
            this.btnUserEdit.Text = "Editar";
            this.btnUserEdit.UseVisualStyleBackColor = false;
            this.btnUserEdit.Click += new System.EventHandler(this.btnUserEdit_Click);
            // 
            // incioContainer
            // 
            this.incioContainer.Controls.Add(this.btnInicio);
            this.incioContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.incioContainer.Location = new System.Drawing.Point(3, 79);
            this.incioContainer.Name = "incioContainer";
            this.incioContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.incioContainer.Size = new System.Drawing.Size(200, 60);
            this.incioContainer.TabIndex = 2;
            // 
            // btnInicio
            // 
            this.btnInicio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(87)))), ((int)(((byte)(174)))));
            this.btnInicio.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(78)))), ((int)(((byte)(157)))));
            this.btnInicio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInicio.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnInicio.ForeColor = System.Drawing.Color.White;
            this.btnInicio.Image = global::BibliotecaApp.Properties.Resources.icons8_página_inicial_25;
            this.btnInicio.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInicio.Location = new System.Drawing.Point(0, -3);
            this.btnInicio.Name = "btnInicio";
            this.btnInicio.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnInicio.Size = new System.Drawing.Size(223, 60);
            this.btnInicio.TabIndex = 3;
            this.btnInicio.TabStop = false;
            this.btnInicio.Text = "Início";
            this.btnInicio.UseVisualStyleBackColor = false;
            this.btnInicio.Click += new System.EventHandler(this.btnInicio_Click);
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(87)))), ((int)(((byte)(174)))));
            this.menu.Controls.Add(this.panel1);
            this.menu.Controls.Add(this.incioContainer);
            this.menu.Controls.Add(this.userContainer);
            this.menu.Controls.Add(this.livroContainer);
            this.menu.Controls.Add(this.relContainer);
            this.menu.Controls.Add(this.sairContainer);
            this.menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(205, 800);
            this.menu.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(196, 70);
            this.panel1.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::BibliotecaApp.Properties.Resources.gast_removebg_preview_100x100_1_;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Location = new System.Drawing.Point(43, -16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(118, 96);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // livroContainer
            // 
            this.livroContainer.Controls.Add(this.btnDev);
            this.livroContainer.Controls.Add(this.btnLivros);
            this.livroContainer.Controls.Add(this.btnLivroCad);
            this.livroContainer.Controls.Add(this.btnEmprestimoRap);
            this.livroContainer.Controls.Add(this.btnLivro);
            this.livroContainer.Controls.Add(this.btnEmprestimo);
            this.livroContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.livroContainer.Location = new System.Drawing.Point(3, 211);
            this.livroContainer.Name = "livroContainer";
            this.livroContainer.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.livroContainer.Size = new System.Drawing.Size(200, 60);
            this.livroContainer.TabIndex = 8;
            // 
            // btnDev
            // 
            this.btnDev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(98)))), ((int)(((byte)(144)))));
            this.btnDev.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(88)))), ((int)(((byte)(130)))));
            this.btnDev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDev.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnDev.ForeColor = System.Drawing.Color.White;
            this.btnDev.Image = ((System.Drawing.Image)(resources.GetObject("btnDev.Image")));
            this.btnDev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDev.Location = new System.Drawing.Point(0, 299);
            this.btnDev.Name = "btnDev";
            this.btnDev.Padding = new System.Windows.Forms.Padding(10, 0, 0, 2);
            this.btnDev.Size = new System.Drawing.Size(200, 60);
            this.btnDev.TabIndex = 7;
            this.btnDev.TabStop = false;
            this.btnDev.Text = "Devolução";
            this.btnDev.UseVisualStyleBackColor = false;
            this.btnDev.Click += new System.EventHandler(this.btnDev_Click);
            // 
            // btnLivros
            // 
            this.btnLivros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(98)))), ((int)(((byte)(144)))));
            this.btnLivros.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(88)))), ((int)(((byte)(130)))));
            this.btnLivros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLivros.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnLivros.ForeColor = System.Drawing.Color.Transparent;
            this.btnLivros.Image = global::BibliotecaApp.Properties.Resources.icons8_livro_25;
            this.btnLivros.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLivros.Location = new System.Drawing.Point(0, 59);
            this.btnLivros.Name = "btnLivros";
            this.btnLivros.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnLivros.Size = new System.Drawing.Size(200, 60);
            this.btnLivros.TabIndex = 9;
            this.btnLivros.TabStop = false;
            this.btnLivros.Text = "Biblioteca";
            this.btnLivros.UseVisualStyleBackColor = false;
            this.btnLivros.Click += new System.EventHandler(this.btnLivros_Click);
            // 
            // btnLivroCad
            // 
            this.btnLivroCad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(98)))), ((int)(((byte)(144)))));
            this.btnLivroCad.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(88)))), ((int)(((byte)(130)))));
            this.btnLivroCad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLivroCad.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnLivroCad.ForeColor = System.Drawing.Color.Transparent;
            this.btnLivroCad.Image = ((System.Drawing.Image)(resources.GetObject("btnLivroCad.Image")));
            this.btnLivroCad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLivroCad.Location = new System.Drawing.Point(0, 239);
            this.btnLivroCad.Name = "btnLivroCad";
            this.btnLivroCad.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnLivroCad.Size = new System.Drawing.Size(200, 60);
            this.btnLivroCad.TabIndex = 6;
            this.btnLivroCad.TabStop = false;
            this.btnLivroCad.Text = "   Cadastrar Livro";
            this.btnLivroCad.UseVisualStyleBackColor = false;
            this.btnLivroCad.Click += new System.EventHandler(this.btnLivroCad_Click);
            // 
            // btnEmprestimoRap
            // 
            this.btnEmprestimoRap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(98)))), ((int)(((byte)(144)))));
            this.btnEmprestimoRap.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(88)))), ((int)(((byte)(130)))));
            this.btnEmprestimoRap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmprestimoRap.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnEmprestimoRap.ForeColor = System.Drawing.Color.White;
            this.btnEmprestimoRap.Image = ((System.Drawing.Image)(resources.GetObject("btnEmprestimoRap.Image")));
            this.btnEmprestimoRap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmprestimoRap.Location = new System.Drawing.Point(0, 179);
            this.btnEmprestimoRap.Name = "btnEmprestimoRap";
            this.btnEmprestimoRap.Padding = new System.Windows.Forms.Padding(6, 0, 0, 2);
            this.btnEmprestimoRap.Size = new System.Drawing.Size(200, 60);
            this.btnEmprestimoRap.TabIndex = 5;
            this.btnEmprestimoRap.TabStop = false;
            this.btnEmprestimoRap.Text = "      Empréstimo rápido";
            this.btnEmprestimoRap.UseVisualStyleBackColor = false;
            this.btnEmprestimoRap.Click += new System.EventHandler(this.btnEmprestimoRap_Click);
            // 
            // btnLivro
            // 
            this.btnLivro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(87)))), ((int)(((byte)(174)))));
            this.btnLivro.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(78)))), ((int)(((byte)(157)))));
            this.btnLivro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLivro.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnLivro.ForeColor = System.Drawing.Color.White;
            this.btnLivro.Image = ((System.Drawing.Image)(resources.GetObject("btnLivro.Image")));
            this.btnLivro.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLivro.Location = new System.Drawing.Point(0, -3);
            this.btnLivro.Name = "btnLivro";
            this.btnLivro.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnLivro.Size = new System.Drawing.Size(223, 60);
            this.btnLivro.TabIndex = 3;
            this.btnLivro.TabStop = false;
            this.btnLivro.Text = "Livros";
            this.btnLivro.UseVisualStyleBackColor = false;
            this.btnLivro.Click += new System.EventHandler(this.btnLivro_Click);
            // 
            // btnEmprestimo
            // 
            this.btnEmprestimo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(98)))), ((int)(((byte)(144)))));
            this.btnEmprestimo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(88)))), ((int)(((byte)(130)))));
            this.btnEmprestimo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmprestimo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.btnEmprestimo.ForeColor = System.Drawing.Color.Transparent;
            this.btnEmprestimo.Image = ((System.Drawing.Image)(resources.GetObject("btnEmprestimo.Image")));
            this.btnEmprestimo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmprestimo.Location = new System.Drawing.Point(0, 119);
            this.btnEmprestimo.Name = "btnEmprestimo";
            this.btnEmprestimo.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnEmprestimo.Size = new System.Drawing.Size(200, 60);
            this.btnEmprestimo.TabIndex = 4;
            this.btnEmprestimo.TabStop = false;
            this.btnEmprestimo.Text = "Empréstimo";
            this.btnEmprestimo.UseVisualStyleBackColor = false;
            this.btnEmprestimo.Click += new System.EventHandler(this.btnEmprestimo_Click);
            // 
            // livroTransition
            // 
            this.livroTransition.Interval = 5;
            this.livroTransition.Tick += new System.EventHandler(this.livroTransition_Tick);
            // 
            // userTransition
            // 
            this.userTransition.Interval = 5;
            this.userTransition.Tick += new System.EventHandler(this.userTransition_Tick);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1440, 800);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.menu);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.Text = "BibliotecaApp";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelControl.ResumeLayout(false);
            this.ControlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMin)).EndInit();
            this.sairContainer.ResumeLayout(false);
            this.relContainer.ResumeLayout(false);
            this.userContainer.ResumeLayout(false);
            this.incioContainer.ResumeLayout(false);
            this.menu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.livroContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.PictureBox picExit;
        private System.Windows.Forms.FlowLayoutPanel ControlPanel;
        private System.Windows.Forms.PictureBox picMin;
        private System.Windows.Forms.PictureBox picMax;
        private System.Windows.Forms.Panel sairContainer;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Panel relContainer;
        private System.Windows.Forms.Button btnRel;
        private System.Windows.Forms.Button btnUsuario;
        private System.Windows.Forms.Panel incioContainer;
        private System.Windows.Forms.FlowLayoutPanel menu;
        private System.Windows.Forms.Panel livroContainer;
        private System.Windows.Forms.Button btnDev;
        private System.Windows.Forms.Button btnLivroCad;
        private System.Windows.Forms.Button btnEmprestimoRap;
        private System.Windows.Forms.Button btnEmprestimo;
        private System.Windows.Forms.Button btnLivro;
        private System.Windows.Forms.Timer livroTransition;
        private System.Windows.Forms.Button btnInicio;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnLivros;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUserCad;
        private System.Windows.Forms.Timer userTransition;
        public System.Windows.Forms.Panel userContainer;
        public System.Windows.Forms.Button btnUser;
        public System.Windows.Forms.Button btnUserEdit;
    }
}

