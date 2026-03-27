namespace BibliotecaApp.Forms.Usuario
{
    partial class EditarUsuarioForm
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
        /// Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstSugestoesUsuario = new System.Windows.Forms.ListBox();
            this.lstSugestoesTurma = new System.Windows.Forms.ListBox();
            this.lblTipoUsuario = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.txtNomeUsuario = new RoundedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNome = new RoundedTextBox();
            this.lblTurma = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new RoundedTextBox();
            this.txtTurma = new RoundedTextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpDataNasc = new RoundedDatePicker();
            this.mtxCPF = new RoundedMaskedTextBox();
            this.mtxTelefone = new RoundedMaskedTextBox();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.lblCPF = new System.Windows.Forms.Label();
            this.lblDataNasc = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.lstSugestoesUsuario);
            this.panel2.Controls.Add(this.lstSugestoesTurma);
            this.panel2.Controls.Add(this.lblTipoUsuario);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.btnCancelar);
            this.panel2.Controls.Add(this.btnExcluir);
            this.panel2.Controls.Add(this.btnSalvar);
            this.panel2.Controls.Add(this.txtNomeUsuario);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtNome);
            this.panel2.Controls.Add(this.lblTurma);
            this.panel2.Controls.Add(this.lblEmail);
            this.panel2.Controls.Add(this.txtEmail);
            this.panel2.Controls.Add(this.txtTurma);
            this.panel2.Controls.Add(this.lblNome);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1280, 905);
            this.panel2.TabIndex = 132;
            // 
            // lstSugestoesUsuario
            // 
            this.lstSugestoesUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstSugestoesUsuario.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lstSugestoesUsuario.FormattingEnabled = true;
            this.lstSugestoesUsuario.ItemHeight = 25;
            this.lstSugestoesUsuario.Location = new System.Drawing.Point(334, 192);
            this.lstSugestoesUsuario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lstSugestoesUsuario.Name = "lstSugestoesUsuario";
            this.lstSugestoesUsuario.ScrollAlwaysVisible = true;
            this.lstSugestoesUsuario.Size = new System.Drawing.Size(618, 104);
            this.lstSugestoesUsuario.TabIndex = 2;
            this.lstSugestoesUsuario.Visible = false;
            this.lstSugestoesUsuario.SelectedIndexChanged += new System.EventHandler(this.lstSugestoesUsuario_SelectedIndexChanged);
            this.lstSugestoesUsuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNomeUsuario_KeyDown);
            // 
            // lstSugestoesTurma
            // 
            this.lstSugestoesTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstSugestoesTurma.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lstSugestoesTurma.FormattingEnabled = true;
            this.lstSugestoesTurma.ItemHeight = 25;
            this.lstSugestoesTurma.Location = new System.Drawing.Point(334, 475);
            this.lstSugestoesTurma.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lstSugestoesTurma.Name = "lstSugestoesTurma";
            this.lstSugestoesTurma.ScrollAlwaysVisible = true;
            this.lstSugestoesTurma.Size = new System.Drawing.Size(618, 104);
            this.lstSugestoesTurma.TabIndex = 132;
            this.lstSugestoesTurma.Visible = false;
            // 
            // lblTipoUsuario
            // 
            this.lblTipoUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTipoUsuario.AutoSize = true;
            this.lblTipoUsuario.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTipoUsuario.Location = new System.Drawing.Point(331, 198);
            this.lblTipoUsuario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTipoUsuario.Name = "lblTipoUsuario";
            this.lblTipoUsuario.Size = new System.Drawing.Size(50, 19);
            this.lblTipoUsuario.TabIndex = 131;
            this.lblTipoUsuario.Text = "label3";
            this.lblTipoUsuario.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.label2.Location = new System.Drawing.Point(457, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(372, 46);
            this.label2.TabIndex = 129;
            this.label2.Text = "EDITOR DE USUARIOS";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancelar.BackColor = System.Drawing.Color.DimGray;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(569, 772);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(149, 57);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnExcluir
            // 
            this.btnExcluir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExcluir.BackColor = System.Drawing.Color.DarkRed;
            this.btnExcluir.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnExcluir.ForeColor = System.Drawing.Color.White;
            this.btnExcluir.Location = new System.Drawing.Point(334, 772);
            this.btnExcluir.Margin = new System.Windows.Forms.Padding(5);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(149, 57);
            this.btnExcluir.TabIndex = 127;
            this.btnExcluir.Text = "Excluir ";
            this.btnExcluir.UseVisualStyleBackColor = false;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSalvar.BackColor = System.Drawing.Color.DarkGreen;
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(803, 772);
            this.btnSalvar.Margin = new System.Windows.Forms.Padding(5);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(149, 57);
            this.btnSalvar.TabIndex = 8;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
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
            this.txtNomeUsuario.Location = new System.Drawing.Point(335, 152);
            this.txtNomeUsuario.Margin = new System.Windows.Forms.Padding(5);
            this.txtNomeUsuario.Name = "txtNomeUsuario";
            this.txtNomeUsuario.Padding = new System.Windows.Forms.Padding(9, 12, 9, 12);
            this.txtNomeUsuario.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNomeUsuario.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeUsuario.PlaceholderMarginLeft = 12;
            this.txtNomeUsuario.PlaceholderText = "Busque aqui o Nome do Usuário ...";
            this.txtNomeUsuario.SelectedText = "";
            this.txtNomeUsuario.SelectionLength = 0;
            this.txtNomeUsuario.SelectionStart = 0;
            this.txtNomeUsuario.Size = new System.Drawing.Size(617, 40);
            this.txtNomeUsuario.TabIndex = 1;
            this.txtNomeUsuario.TextColor = System.Drawing.Color.Black;
            this.txtNomeUsuario.UseSystemPasswordChar = false;
            this.txtNomeUsuario.TextChanged += new System.EventHandler(this.txtNomeUsuario_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label1.Location = new System.Drawing.Point(330, 122);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 25);
            this.label1.TabIndex = 107;
            this.label1.Text = "Nome do Usuário:";
            // 
            // txtNome
            // 
            this.txtNome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtNome.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtNome.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtNome.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNome.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtNome.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtNome.BorderRadius = 10;
            this.txtNome.BorderThickness = 1;
            this.txtNome.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNome.Enabled = false;
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtNome.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtNome.Location = new System.Drawing.Point(335, 270);
            this.txtNome.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtNome.Name = "txtNome";
            this.txtNome.Padding = new System.Windows.Forms.Padding(8);
            this.txtNome.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNome.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.PlaceholderMarginLeft = 12;
            this.txtNome.PlaceholderText = "";
            this.txtNome.SelectedText = "";
            this.txtNome.SelectionLength = 0;
            this.txtNome.SelectionStart = 0;
            this.txtNome.Size = new System.Drawing.Size(617, 40);
            this.txtNome.TabIndex = 3;
            this.txtNome.TextColor = System.Drawing.Color.Black;
            this.txtNome.UseSystemPasswordChar = false;
            // 
            // lblTurma
            // 
            this.lblTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTurma.AutoSize = true;
            this.lblTurma.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblTurma.Location = new System.Drawing.Point(330, 407);
            this.lblTurma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Size = new System.Drawing.Size(71, 25);
            this.lblTurma.TabIndex = 99;
            this.lblTurma.Text = "Turma:";
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblEmail.Location = new System.Drawing.Point(330, 325);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(64, 25);
            this.lblEmail.TabIndex = 93;
            this.lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtEmail.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtEmail.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtEmail.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtEmail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtEmail.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtEmail.BorderRadius = 10;
            this.txtEmail.BorderThickness = 1;
            this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmail.Enabled = false;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtEmail.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtEmail.Location = new System.Drawing.Point(335, 353);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Padding = new System.Windows.Forms.Padding(8);
            this.txtEmail.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtEmail.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.PlaceholderMarginLeft = 12;
            this.txtEmail.PlaceholderText = "";
            this.txtEmail.SelectedText = "";
            this.txtEmail.SelectionLength = 0;
            this.txtEmail.SelectionStart = 0;
            this.txtEmail.Size = new System.Drawing.Size(617, 40);
            this.txtEmail.TabIndex = 3;
            this.txtEmail.TextColor = System.Drawing.Color.Black;
            this.txtEmail.UseSystemPasswordChar = false;
            // 
            // txtTurma
            // 
            this.txtTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtTurma.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtTurma.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtTurma.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtTurma.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtTurma.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtTurma.BorderRadius = 10;
            this.txtTurma.BorderThickness = 1;
            this.txtTurma.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTurma.Enabled = false;
            this.txtTurma.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtTurma.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtTurma.Location = new System.Drawing.Point(335, 435);
            this.txtTurma.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtTurma.Name = "txtTurma";
            this.txtTurma.Padding = new System.Windows.Forms.Padding(8);
            this.txtTurma.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtTurma.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTurma.PlaceholderMarginLeft = 12;
            this.txtTurma.PlaceholderText = "";
            this.txtTurma.SelectedText = "";
            this.txtTurma.SelectionLength = 0;
            this.txtTurma.SelectionStart = 0;
            this.txtTurma.Size = new System.Drawing.Size(617, 40);
            this.txtTurma.TabIndex = 4;
            this.txtTurma.TextColor = System.Drawing.Color.Black;
            this.txtTurma.UseSystemPasswordChar = false;
            this.txtTurma.TextChanged += new System.EventHandler(this.txtTurma_TextChanged);
            // 
            // lblNome
            // 
            this.lblNome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNome.AutoSize = true;
            this.lblNome.BackColor = System.Drawing.Color.Transparent;
            this.lblNome.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblNome.Location = new System.Drawing.Point(330, 242);
            this.lblNome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(159, 25);
            this.lblNome.TabIndex = 90;
            this.lblNome.Text = "Nome Completo:";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.dtpDataNasc);
            this.panel1.Controls.Add(this.mtxCPF);
            this.panel1.Controls.Add(this.mtxTelefone);
            this.panel1.Controls.Add(this.lblTelefone);
            this.panel1.Controls.Add(this.lblCPF);
            this.panel1.Controls.Add(this.lblDataNasc);
            this.panel1.Location = new System.Drawing.Point(321, 478);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(364, 251);
            this.panel1.TabIndex = 133;
            // 
            // dtpDataNasc
            // 
            this.dtpDataNasc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpDataNasc.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpDataNasc.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dtpDataNasc.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.dtpDataNasc.BorderRadius = 10;
            this.dtpDataNasc.BorderThickness = 1;
            this.dtpDataNasc.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.dtpDataNasc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataNasc.HoverBackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpDataNasc.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataNasc.IconHoverAreaColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dtpDataNasc.IconHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataNasc.Location = new System.Drawing.Point(14, 203);
            this.dtpDataNasc.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDataNasc.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDataNasc.Name = "dtpDataNasc";
            this.dtpDataNasc.PlaceholderColor = System.Drawing.Color.Gray;
            this.dtpDataNasc.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.dtpDataNasc.PlaceholderText = "";
            this.dtpDataNasc.SelectedDate = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            this.dtpDataNasc.Size = new System.Drawing.Size(245, 40);
            this.dtpDataNasc.TabIndex = 7;
            this.dtpDataNasc.TabStop = false;
            this.dtpDataNasc.Value = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            // 
            // mtxCPF
            // 
            this.mtxCPF.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtxCPF.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mtxCPF.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.mtxCPF.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.mtxCPF.BorderRadius = 10;
            this.mtxCPF.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mtxCPF.Enabled = false;
            this.mtxCPF.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxCPF.ForeColor = System.Drawing.Color.Gray;
            this.mtxCPF.HoverBackColor = System.Drawing.Color.LightGray;
            this.mtxCPF.HoverBorderColor = System.Drawing.Color.DarkGray;
            this.mtxCPF.LeftMargin = 0;
            this.mtxCPF.Location = new System.Drawing.Point(14, 121);
            this.mtxCPF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mtxCPF.Mask = "000,000,000-00";
            this.mtxCPF.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxCPF.Name = "mtxCPF";
            this.mtxCPF.Padding = new System.Windows.Forms.Padding(18, 6, 6, 6);
            this.mtxCPF.Size = new System.Drawing.Size(329, 40);
            this.mtxCPF.TabIndex = 6;
            // 
            // mtxTelefone
            // 
            this.mtxTelefone.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtxTelefone.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mtxTelefone.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.mtxTelefone.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.mtxTelefone.BorderRadius = 10;
            this.mtxTelefone.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mtxTelefone.Enabled = false;
            this.mtxTelefone.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxTelefone.ForeColor = System.Drawing.Color.Gray;
            this.mtxTelefone.HoverBackColor = System.Drawing.Color.LightGray;
            this.mtxTelefone.HoverBorderColor = System.Drawing.Color.DarkGray;
            this.mtxTelefone.LeftMargin = 0;
            this.mtxTelefone.Location = new System.Drawing.Point(13, 38);
            this.mtxTelefone.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mtxTelefone.Mask = "(00)00000-0000";
            this.mtxTelefone.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxTelefone.Name = "mtxTelefone";
            this.mtxTelefone.Padding = new System.Windows.Forms.Padding(15, 6, 6, 6);
            this.mtxTelefone.Size = new System.Drawing.Size(329, 40);
            this.mtxTelefone.TabIndex = 5;
            // 
            // lblTelefone
            // 
            this.lblTelefone.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblTelefone.Location = new System.Drawing.Point(8, 10);
            this.lblTelefone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(89, 25);
            this.lblTelefone.TabIndex = 100;
            this.lblTelefone.Text = "Telefone:";
            // 
            // lblCPF
            // 
            this.lblCPF.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCPF.AutoSize = true;
            this.lblCPF.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPF.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblCPF.Location = new System.Drawing.Point(9, 93);
            this.lblCPF.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCPF.Name = "lblCPF";
            this.lblCPF.Size = new System.Drawing.Size(50, 25);
            this.lblCPF.TabIndex = 97;
            this.lblCPF.Text = "CPF:";
            // 
            // lblDataNasc
            // 
            this.lblDataNasc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDataNasc.AutoSize = true;
            this.lblDataNasc.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataNasc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblDataNasc.Location = new System.Drawing.Point(8, 175);
            this.lblDataNasc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDataNasc.Name = "lblDataNasc";
            this.lblDataNasc.Size = new System.Drawing.Size(192, 25);
            this.lblDataNasc.TabIndex = 98;
            this.lblDataNasc.Text = "Data de Nascimento:";
            // 
            // EditarUsuarioForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1280, 905);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditarUsuarioForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditarUsuario";
            this.Resize += new System.EventHandler(this.EditarUsuarioForm_Resize);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Button btnSalvar;
        private RoundedTextBox txtNomeUsuario;
        private System.Windows.Forms.Label label1;
        private RoundedTextBox txtNome;
        public System.Windows.Forms.Label lblDataNasc;
        public System.Windows.Forms.Label lblTurma;
        public System.Windows.Forms.Label lblCPF;
        public System.Windows.Forms.Label lblTelefone;
        public System.Windows.Forms.Label lblEmail;
        private RoundedTextBox txtEmail;
        private RoundedTextBox txtTurma;
        private RoundedMaskedTextBox mtxTelefone;
        public System.Windows.Forms.Label lblNome;
        private RoundedMaskedTextBox mtxCPF;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstSugestoesUsuario;
        private System.Windows.Forms.Label lblTipoUsuario;
        private System.Windows.Forms.ListBox lstSugestoesTurma;
        private System.Windows.Forms.Panel panel1;
        private RoundedDatePicker dtpDataNasc;
    }
}