namespace BibliotecaApp.Forms.Relatorio
{
    partial class RelForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvHistorico = new System.Windows.Forms.DataGridView();
            this.NomeU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NomeL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Acao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bibliotecaria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataAcao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblLivro = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lstSugestoesUsuario = new System.Windows.Forms.ListBox();
            this.dtpFim = new RoundedDatePicker();
            this.dtpInicio = new RoundedDatePicker();
            this.btnExportar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstLivros = new System.Windows.Forms.ListBox();
            this.cbBibliotecaria = new RoundedComboBox();
            this.txtLivro = new RoundedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.cmbAcao = new RoundedComboBox();
            this.txtUsuario = new RoundedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorico)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvHistorico
            // 
            this.dgvHistorico.AllowUserToAddRows = false;
            this.dgvHistorico.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dgvHistorico.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistorico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvHistorico.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NomeU,
            this.NomeL,
            this.Acao,
            this.Bibliotecaria,
            this.DataAcao});
            this.dgvHistorico.Location = new System.Drawing.Point(33, 294);
            this.dgvHistorico.Name = "dgvHistorico";
            this.dgvHistorico.ReadOnly = true;
            this.dgvHistorico.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvHistorico.Size = new System.Drawing.Size(1214, 548);
            this.dgvHistorico.TabIndex = 23;
            // 
            // NomeU
            // 
            this.NomeU.HeaderText = "Nome do Usuário";
            this.NomeU.Name = "NomeU";
            this.NomeU.ReadOnly = true;
            // 
            // NomeL
            // 
            this.NomeL.HeaderText = "Nome do Livro";
            this.NomeL.Name = "NomeL";
            this.NomeL.ReadOnly = true;
            // 
            // Acao
            // 
            this.Acao.HeaderText = "Ação";
            this.Acao.Name = "Acao";
            this.Acao.ReadOnly = true;
            // 
            // Bibliotecaria
            // 
            this.Bibliotecaria.HeaderText = "Bibliotecaria";
            this.Bibliotecaria.Name = "Bibliotecaria";
            this.Bibliotecaria.ReadOnly = true;
            // 
            // DataAcao
            // 
            this.DataAcao.HeaderText = "Data da Ação";
            this.DataAcao.Name = "DataAcao";
            this.DataAcao.ReadOnly = true;
            // 
            // lblUsuario
            // 
            this.lblUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.Color.Transparent;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblUsuario.Location = new System.Drawing.Point(28, 101);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(167, 25);
            this.lblUsuario.TabIndex = 116;
            this.lblUsuario.Text = "Nome do Usuário:";
            // 
            // lblLivro
            // 
            this.lblLivro.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblLivro.AutoSize = true;
            this.lblLivro.BackColor = System.Drawing.Color.Transparent;
            this.lblLivro.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLivro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblLivro.Location = new System.Drawing.Point(463, 101);
            this.lblLivro.Name = "lblLivro";
            this.lblLivro.Size = new System.Drawing.Size(144, 25);
            this.lblLivro.TabIndex = 117;
            this.lblLivro.Text = "Nome do Livro:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label2.Location = new System.Drawing.Point(943, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 25);
            this.label2.TabIndex = 119;
            this.label2.Text = "Tipo de Ação:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label3.Location = new System.Drawing.Point(28, 178);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 25);
            this.label3.TabIndex = 120;
            this.label3.Text = "Inicio do Período:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label5.Location = new System.Drawing.Point(253, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 25);
            this.label5.TabIndex = 122;
            this.label5.Text = "Fim do Período:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lstSugestoesUsuario);
            this.panel1.Controls.Add(this.dtpFim);
            this.panel1.Controls.Add(this.dtpInicio);
            this.panel1.Controls.Add(this.btnExportar);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lstLivros);
            this.panel1.Controls.Add(this.cbBibliotecaria);
            this.panel1.Controls.Add(this.txtLivro);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnFiltrar);
            this.panel1.Controls.Add(this.dgvHistorico);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cmbAcao);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblUsuario);
            this.panel1.Controls.Add(this.lblLivro);
            this.panel1.Controls.Add(this.txtUsuario);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 845);
            this.panel1.TabIndex = 123;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lstSugestoesUsuario
            // 
            this.lstSugestoesUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstSugestoesUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSugestoesUsuario.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.lstSugestoesUsuario.FormattingEnabled = true;
            this.lstSugestoesUsuario.ItemHeight = 25;
            this.lstSugestoesUsuario.Location = new System.Drawing.Point(33, 169);
            this.lstSugestoesUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.lstSugestoesUsuario.Name = "lstSugestoesUsuario";
            this.lstSugestoesUsuario.ScrollAlwaysVisible = true;
            this.lstSugestoesUsuario.Size = new System.Drawing.Size(387, 102);
            this.lstSugestoesUsuario.TabIndex = 126;
            this.lstSugestoesUsuario.Visible = false;
            // 
            // dtpFim
            // 
            this.dtpFim.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpFim.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpFim.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dtpFim.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.dtpFim.BorderRadius = 10;
            this.dtpFim.BorderThickness = 1;
            this.dtpFim.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.dtpFim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpFim.HoverBackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpFim.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpFim.IconHoverAreaColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dtpFim.IconHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpFim.Location = new System.Drawing.Point(258, 207);
            this.dtpFim.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpFim.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpFim.Name = "dtpFim";
            this.dtpFim.PlaceholderColor = System.Drawing.Color.Gray;
            this.dtpFim.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.dtpFim.PlaceholderText = "";
            this.dtpFim.SelectedDate = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            this.dtpFim.Size = new System.Drawing.Size(162, 40);
            this.dtpFim.TabIndex = 131;
            this.dtpFim.TabStop = false;
            this.dtpFim.Value = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            // 
            // dtpInicio
            // 
            this.dtpInicio.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpInicio.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpInicio.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dtpInicio.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.dtpInicio.BorderRadius = 10;
            this.dtpInicio.BorderThickness = 1;
            this.dtpInicio.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.dtpInicio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpInicio.HoverBackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpInicio.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpInicio.IconHoverAreaColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dtpInicio.IconHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpInicio.Location = new System.Drawing.Point(33, 207);
            this.dtpInicio.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpInicio.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpInicio.Name = "dtpInicio";
            this.dtpInicio.PlaceholderColor = System.Drawing.Color.Gray;
            this.dtpInicio.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.dtpInicio.PlaceholderText = "";
            this.dtpInicio.SelectedDate = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            this.dtpInicio.Size = new System.Drawing.Size(162, 40);
            this.dtpInicio.TabIndex = 130;
            this.dtpInicio.TabStop = false;
            this.dtpInicio.Value = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            // 
            // btnExportar
            // 
            this.btnExportar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnExportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnExportar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportar.Font = new System.Drawing.Font("Segoe UI Semibold", 13.5F, System.Drawing.FontStyle.Bold);
            this.btnExportar.ForeColor = System.Drawing.Color.White;
            this.btnExportar.Image = global::BibliotecaApp.Properties.Resources.icons8_export_excel_25;
            this.btnExportar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportar.Location = new System.Drawing.Point(1113, 201);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
            this.btnExportar.Size = new System.Drawing.Size(134, 52);
            this.btnExportar.TabIndex = 129;
            this.btnExportar.Text = "      Exportar";
            this.btnExportar.UseVisualStyleBackColor = false;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.label1.Location = new System.Drawing.Point(538, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 46);
            this.label1.TabIndex = 128;
            this.label1.Text = "RELATÓRIO";
            // 
            // lstLivros
            // 
            this.lstLivros.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstLivros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLivros.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.lstLivros.FormattingEnabled = true;
            this.lstLivros.ItemHeight = 25;
            this.lstLivros.Location = new System.Drawing.Point(468, 169);
            this.lstLivros.Margin = new System.Windows.Forms.Padding(4);
            this.lstLivros.Name = "lstLivros";
            this.lstLivros.ScrollAlwaysVisible = true;
            this.lstLivros.Size = new System.Drawing.Size(429, 102);
            this.lstLivros.TabIndex = 127;
            this.lstLivros.Visible = false;
            // 
            // cbBibliotecaria
            // 
            this.cbBibliotecaria.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbBibliotecaria.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbBibliotecaria.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbBibliotecaria.BorderRadius = 8;
            this.cbBibliotecaria.BorderThickness = 1;
            this.cbBibliotecaria.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbBibliotecaria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBibliotecaria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBibliotecaria.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbBibliotecaria.FormattingEnabled = true;
            this.cbBibliotecaria.ItemsFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbBibliotecaria.Location = new System.Drawing.Point(468, 210);
            this.cbBibliotecaria.Name = "cbBibliotecaria";
            this.cbBibliotecaria.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14F);
            this.cbBibliotecaria.PlaceholderMargin = 10;
            this.cbBibliotecaria.PlaceholderText = "Selecione a Bliotecária...";
            this.cbBibliotecaria.Size = new System.Drawing.Size(429, 34);
            this.cbBibliotecaria.TabIndex = 125;
            // 
            // txtLivro
            // 
            this.txtLivro.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLivro.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtLivro.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtLivro.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtLivro.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtLivro.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtLivro.BorderRadius = 10;
            this.txtLivro.BorderThickness = 1;
            this.txtLivro.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLivro.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtLivro.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtLivro.Location = new System.Drawing.Point(468, 129);
            this.txtLivro.Name = "txtLivro";
            this.txtLivro.Padding = new System.Windows.Forms.Padding(7);
            this.txtLivro.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLivro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.PlaceholderMarginLeft = 12;
            this.txtLivro.PlaceholderText = "Digite aqui o nome do livro...";
            this.txtLivro.SelectedText = "";
            this.txtLivro.SelectionLength = 0;
            this.txtLivro.SelectionStart = 0;
            this.txtLivro.Size = new System.Drawing.Size(429, 40);
            this.txtLivro.TabIndex = 91;
            this.txtLivro.TextColor = System.Drawing.Color.Black;
            this.txtLivro.UseSystemPasswordChar = false;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label6.Location = new System.Drawing.Point(463, 182);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 25);
            this.label6.TabIndex = 124;
            this.label6.Text = "Bliotecária responsável:";
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnFiltrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnFiltrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnFiltrar.ForeColor = System.Drawing.Color.White;
            this.btnFiltrar.Image = global::BibliotecaApp.Properties.Resources.material_symbols___tab_search_rounded_25px;
            this.btnFiltrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFiltrar.Location = new System.Drawing.Point(948, 201);
            this.btnFiltrar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
            this.btnFiltrar.Size = new System.Drawing.Size(134, 52);
            this.btnFiltrar.TabIndex = 111;
            this.btnFiltrar.Text = "      Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // cmbAcao
            // 
            this.cmbAcao.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbAcao.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cmbAcao.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cmbAcao.BorderRadius = 8;
            this.cmbAcao.BorderThickness = 1;
            this.cmbAcao.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAcao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAcao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbAcao.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAcao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.cmbAcao.FormattingEnabled = true;
            this.cmbAcao.Items.AddRange(new object[] {
            "Todas",
            "Empréstimos",
            "Todas",
            "Empréstimos"});
            this.cmbAcao.ItemsFont = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAcao.Location = new System.Drawing.Point(948, 132);
            this.cmbAcao.Name = "cmbAcao";
            this.cmbAcao.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAcao.PlaceholderMargin = 10;
            this.cmbAcao.PlaceholderText = "Filtre por tipo da ação...";
            this.cmbAcao.Size = new System.Drawing.Size(299, 34);
            this.cmbAcao.TabIndex = 90;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtUsuario.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtUsuario.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtUsuario.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtUsuario.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtUsuario.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtUsuario.BorderRadius = 10;
            this.txtUsuario.BorderThickness = 1;
            this.txtUsuario.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtUsuario.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtUsuario.Location = new System.Drawing.Point(33, 129);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Padding = new System.Windows.Forms.Padding(7);
            this.txtUsuario.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtUsuario.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.PlaceholderMarginLeft = 12;
            this.txtUsuario.PlaceholderText = "Digite aqui o nome do usuário...";
            this.txtUsuario.SelectedText = "";
            this.txtUsuario.SelectionLength = 0;
            this.txtUsuario.SelectionStart = 0;
            this.txtUsuario.Size = new System.Drawing.Size(387, 40);
            this.txtUsuario.TabIndex = 1;
            this.txtUsuario.TextColor = System.Drawing.Color.Black;
            this.txtUsuario.UseSystemPasswordChar = false;
            this.txtUsuario.Load += new System.EventHandler(this.txtUsuario_Load);
            // 
            // RelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 845);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RelForm";
            this.Text = "InicioForm";
            this.Load += new System.EventHandler(this.RelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorico)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvHistorico;
        private RoundedComboBox cmbAcao;
        public RoundedTextBox txtUsuario;
        public RoundedTextBox txtLivro;
        private System.Windows.Forms.Button btnFiltrar;
        public System.Windows.Forms.Label lblUsuario;
        public System.Windows.Forms.Label lblLivro;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn NomeU;
        private System.Windows.Forms.DataGridViewTextBoxColumn NomeL;
        private System.Windows.Forms.DataGridViewTextBoxColumn Acao;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bibliotecaria;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataAcao;
        private System.Windows.Forms.Panel panel1;
        private RoundedComboBox cbBibliotecaria;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox lstSugestoesUsuario;
        private System.Windows.Forms.ListBox lstLivros;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExportar;
        private RoundedDatePicker dtpFim;
        private RoundedDatePicker dtpInicio;
    }
}