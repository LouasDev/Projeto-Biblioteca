namespace BibliotecaApp.Forms.Livros
{
    partial class EmprestimoForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpDataDevolucao = new RoundedDatePicker();
            this.dtpDataEmprestimo = new RoundedDatePicker();
            this.lstSugestoesUsuario = new System.Windows.Forms.ListBox();
            this.lstLivros = new System.Windows.Forms.ListBox();
            this.cbBibliotecaria = new RoundedComboBox();
            this.txtBarcode = new RoundedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkDevolucaoPersonalizada = new System.Windows.Forms.CheckBox();
            this.txtLivro = new RoundedTextBox();
            this.txtNomeUsuario = new RoundedTextBox();
            this.btnEmprestar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.dtpDataDevolucao);
            this.panel1.Controls.Add(this.dtpDataEmprestimo);
            this.panel1.Controls.Add(this.lstSugestoesUsuario);
            this.panel1.Controls.Add(this.lstLivros);
            this.panel1.Controls.Add(this.cbBibliotecaria);
            this.panel1.Controls.Add(this.txtBarcode);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.chkDevolucaoPersonalizada);
            this.panel1.Controls.Add(this.txtLivro);
            this.panel1.Controls.Add(this.txtNomeUsuario);
            this.panel1.Controls.Add(this.btnEmprestar);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 845);
            this.panel1.TabIndex = 720;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // dtpDataDevolucao
            // 
            this.dtpDataDevolucao.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpDataDevolucao.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpDataDevolucao.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dtpDataDevolucao.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.dtpDataDevolucao.BorderRadius = 10;
            this.dtpDataDevolucao.BorderThickness = 1;
            this.dtpDataDevolucao.Enabled = false;
            this.dtpDataDevolucao.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.dtpDataDevolucao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataDevolucao.HoverBackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpDataDevolucao.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataDevolucao.IconHoverAreaColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dtpDataDevolucao.IconHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataDevolucao.Location = new System.Drawing.Point(332, 573);
            this.dtpDataDevolucao.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDataDevolucao.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDataDevolucao.Name = "dtpDataDevolucao";
            this.dtpDataDevolucao.PlaceholderColor = System.Drawing.Color.Gray;
            this.dtpDataDevolucao.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.dtpDataDevolucao.PlaceholderText = "";
            this.dtpDataDevolucao.SelectedDate = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            this.dtpDataDevolucao.Size = new System.Drawing.Size(245, 40);
            this.dtpDataDevolucao.TabIndex = 128;
            this.dtpDataDevolucao.TabStop = false;
            this.dtpDataDevolucao.Value = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            // 
            // dtpDataEmprestimo
            // 
            this.dtpDataEmprestimo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpDataEmprestimo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpDataEmprestimo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dtpDataEmprestimo.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.dtpDataEmprestimo.BorderRadius = 10;
            this.dtpDataEmprestimo.BorderThickness = 1;
            this.dtpDataEmprestimo.Enabled = false;
            this.dtpDataEmprestimo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.dtpDataEmprestimo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataEmprestimo.HoverBackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpDataEmprestimo.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataEmprestimo.IconHoverAreaColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dtpDataEmprestimo.IconHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataEmprestimo.Location = new System.Drawing.Point(332, 496);
            this.dtpDataEmprestimo.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDataEmprestimo.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDataEmprestimo.Name = "dtpDataEmprestimo";
            this.dtpDataEmprestimo.PlaceholderColor = System.Drawing.Color.Gray;
            this.dtpDataEmprestimo.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.dtpDataEmprestimo.PlaceholderText = "";
            this.dtpDataEmprestimo.SelectedDate = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            this.dtpDataEmprestimo.Size = new System.Drawing.Size(245, 40);
            this.dtpDataEmprestimo.TabIndex = 127;
            this.dtpDataEmprestimo.TabStop = false;
            this.dtpDataEmprestimo.Value = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            // 
            // lstSugestoesUsuario
            // 
            this.lstSugestoesUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstSugestoesUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSugestoesUsuario.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.lstSugestoesUsuario.FormattingEnabled = true;
            this.lstSugestoesUsuario.ItemHeight = 25;
            this.lstSugestoesUsuario.Location = new System.Drawing.Point(333, 210);
            this.lstSugestoesUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.lstSugestoesUsuario.Name = "lstSugestoesUsuario";
            this.lstSugestoesUsuario.ScrollAlwaysVisible = true;
            this.lstSugestoesUsuario.Size = new System.Drawing.Size(616, 127);
            this.lstSugestoesUsuario.TabIndex = 98;
            this.lstSugestoesUsuario.Visible = false;
            this.lstSugestoesUsuario.Click += new System.EventHandler(this.lstSugestoesUsuario_Click);
            this.lstSugestoesUsuario.SelectedIndexChanged += new System.EventHandler(this.lstSugestoesUsuario_SelectedIndexChanged);
            this.lstSugestoesUsuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstSugestoesUsuario_KeyDown);
            // 
            // lstLivros
            // 
            this.lstLivros.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstLivros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLivros.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.lstLivros.FormattingEnabled = true;
            this.lstLivros.ItemHeight = 25;
            this.lstLivros.Location = new System.Drawing.Point(333, 291);
            this.lstLivros.Margin = new System.Windows.Forms.Padding(4);
            this.lstLivros.Name = "lstLivros";
            this.lstLivros.ScrollAlwaysVisible = true;
            this.lstLivros.Size = new System.Drawing.Size(616, 127);
            this.lstLivros.TabIndex = 102;
            this.lstLivros.Visible = false;
            this.lstLivros.Click += new System.EventHandler(this.lstLivros_Click);
            this.lstLivros.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstLivros_KeyDown);
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
            this.cbBibliotecaria.Location = new System.Drawing.Point(332, 418);
            this.cbBibliotecaria.Name = "cbBibliotecaria";
            this.cbBibliotecaria.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14F);
            this.cbBibliotecaria.PlaceholderMargin = 10;
            this.cbBibliotecaria.PlaceholderText = "Selecione a Bibliotecária...";
            this.cbBibliotecaria.Size = new System.Drawing.Size(617, 34);
            this.cbBibliotecaria.TabIndex = 103;
            this.cbBibliotecaria.SelectedIndexChanged += new System.EventHandler(this.cbBibliotecaria_SelectedIndexChanged);
            // 
            // txtBarcode
            // 
            this.txtBarcode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtBarcode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtBarcode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtBarcode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtBarcode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtBarcode.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtBarcode.BorderRadius = 10;
            this.txtBarcode.BorderThickness = 1;
            this.txtBarcode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBarcode.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtBarcode.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtBarcode.Location = new System.Drawing.Point(332, 333);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.txtBarcode.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtBarcode.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.PlaceholderMarginLeft = 12;
            this.txtBarcode.PlaceholderText = "Clique e Escaneei para buscar informacoes...";
            this.txtBarcode.SelectedText = "";
            this.txtBarcode.SelectionLength = 0;
            this.txtBarcode.SelectionStart = 0;
            this.txtBarcode.Size = new System.Drawing.Size(617, 40);
            this.txtBarcode.TabIndex = 101;
            this.txtBarcode.TextColor = System.Drawing.Color.Black;
            this.txtBarcode.UseSystemPasswordChar = false;
            this.txtBarcode.Load += new System.EventHandler(this.txtBarcode_Load);
            this.txtBarcode.Leave += new System.EventHandler(this.txtBarcode_Leave);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label7.Location = new System.Drawing.Point(327, 304);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(162, 25);
            this.label7.TabIndex = 100;
            this.label7.Text = "Codigo de barras:";
            // 
            // chkDevolucaoPersonalizada
            // 
            this.chkDevolucaoPersonalizada.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkDevolucaoPersonalizada.AutoSize = true;
            this.chkDevolucaoPersonalizada.BackColor = System.Drawing.Color.White;
            this.chkDevolucaoPersonalizada.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.chkDevolucaoPersonalizada.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.chkDevolucaoPersonalizada.Location = new System.Drawing.Point(332, 619);
            this.chkDevolucaoPersonalizada.Margin = new System.Windows.Forms.Padding(4);
            this.chkDevolucaoPersonalizada.Name = "chkDevolucaoPersonalizada";
            this.chkDevolucaoPersonalizada.Size = new System.Drawing.Size(162, 24);
            this.chkDevolucaoPersonalizada.TabIndex = 99;
            this.chkDevolucaoPersonalizada.Text = "Estender devolução";
            this.chkDevolucaoPersonalizada.UseVisualStyleBackColor = false;
            this.chkDevolucaoPersonalizada.CheckedChanged += new System.EventHandler(this.chkDevolucaoPersonalizada_CheckedChanged);
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
            this.txtLivro.Location = new System.Drawing.Point(332, 251);
            this.txtLivro.Margin = new System.Windows.Forms.Padding(4);
            this.txtLivro.Name = "txtLivro";
            this.txtLivro.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.txtLivro.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLivro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.PlaceholderMarginLeft = 12;
            this.txtLivro.PlaceholderText = "Busque aqui o livro...";
            this.txtLivro.SelectedText = "";
            this.txtLivro.SelectionLength = 0;
            this.txtLivro.SelectionStart = 0;
            this.txtLivro.Size = new System.Drawing.Size(616, 40);
            this.txtLivro.TabIndex = 95;
            this.txtLivro.TextColor = System.Drawing.Color.Black;
            this.txtLivro.UseSystemPasswordChar = false;
            this.txtLivro.Load += new System.EventHandler(this.txtLivro_Load);
            this.txtLivro.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLivro_KeyDown);
            // 
            // txtNomeUsuario
            // 
            this.txtNomeUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtNomeUsuario.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtNomeUsuario.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtNomeUsuario.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNomeUsuario.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtNomeUsuario.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtNomeUsuario.BorderRadius = 10;
            this.txtNomeUsuario.BorderThickness = 1;
            this.txtNomeUsuario.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNomeUsuario.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtNomeUsuario.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtNomeUsuario.Location = new System.Drawing.Point(332, 170);
            this.txtNomeUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.txtNomeUsuario.Name = "txtNomeUsuario";
            this.txtNomeUsuario.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.txtNomeUsuario.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNomeUsuario.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeUsuario.PlaceholderMarginLeft = 12;
            this.txtNomeUsuario.PlaceholderText = "Busque aqui o Nome do Usuario ...";
            this.txtNomeUsuario.SelectedText = "";
            this.txtNomeUsuario.SelectionLength = 0;
            this.txtNomeUsuario.SelectionStart = 0;
            this.txtNomeUsuario.Size = new System.Drawing.Size(617, 40);
            this.txtNomeUsuario.TabIndex = 1;
            this.txtNomeUsuario.TextColor = System.Drawing.Color.Black;
            this.txtNomeUsuario.UseSystemPasswordChar = false;
            this.txtNomeUsuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNomeUsuario_KeyDown);
            // 
            // btnEmprestar
            // 
            this.btnEmprestar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEmprestar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnEmprestar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnEmprestar.ForeColor = System.Drawing.Color.White;
            this.btnEmprestar.Location = new System.Drawing.Point(794, 676);
            this.btnEmprestar.Margin = new System.Windows.Forms.Padding(4);
            this.btnEmprestar.Name = "btnEmprestar";
            this.btnEmprestar.Size = new System.Drawing.Size(155, 70);
            this.btnEmprestar.TabIndex = 91;
            this.btnEmprestar.Text = "EMPRESTRAR";
            this.btnEmprestar.UseVisualStyleBackColor = false;
            this.btnEmprestar.Click += new System.EventHandler(this.btnEmprestar_Click);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label6.Location = new System.Drawing.Point(327, 390);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(213, 25);
            this.label6.TabIndex = 21;
            this.label6.Text = "Bliotecária responsável:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label5.Location = new System.Drawing.Point(328, 546);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 25);
            this.label5.TabIndex = 20;
            this.label5.Text = "Devolução Prevista:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label4.Location = new System.Drawing.Point(328, 468);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 25);
            this.label4.TabIndex = 19;
            this.label4.Text = "Data de Empréstimo:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label3.Location = new System.Drawing.Point(328, 224);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 25);
            this.label3.TabIndex = 18;
            this.label3.Text = "Livro:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label2.Location = new System.Drawing.Point(328, 143);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 25);
            this.label2.TabIndex = 17;
            this.label2.Text = "Nome do Usuario:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.label1.Location = new System.Drawing.Point(440, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(401, 46);
            this.label1.TabIndex = 16;
            this.label1.Text = "EMPRÉSTIMO DE LIVRO";
            // 
            // EmprestimoForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1280, 845);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmprestimoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmprestimoForm";
            this.Load += new System.EventHandler(this.EmprestimoForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEmprestar;
        private RoundedTextBox txtNomeUsuario;
        private System.Windows.Forms.ListBox lstSugestoesUsuario;
        private System.Windows.Forms.CheckBox chkDevolucaoPersonalizada;
        private RoundedTextBox txtBarcode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox lstLivros;
        private RoundedComboBox cbBibliotecaria;
        public RoundedTextBox txtLivro;
        private RoundedDatePicker dtpDataDevolucao;
        private RoundedDatePicker dtpDataEmprestimo;
    }
}