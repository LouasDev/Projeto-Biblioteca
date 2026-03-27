namespace BibliotecaApp.Forms.Livros
{
    partial class DevoluçãoRapidaForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevoluçãoRapidaForm));
            this.btnMenos = new System.Windows.Forms.Button();
            this.numQuantidadeDevolvidos = new RoundedTextBox();
            this.btnMais = new System.Windows.Forms.Button();
            this.txtLivro = new RoundedTextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.mtxCodigoBarras = new RoundedMaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTurma = new System.Windows.Forms.Label();
            this.txtTurma = new RoundedTextBox();
            this.numQuantidadeEmprestado = new RoundedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConfirmarDevolucao = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.roundedTextBox2 = new RoundedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.roundedTextBox3 = new RoundedTextBox();
            this.txtProfessor = new RoundedTextBox();
            this.lblProfessor = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMenos
            // 
            this.btnMenos.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMenos.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnMenos.Location = new System.Drawing.Point(128, 548);
            this.btnMenos.Margin = new System.Windows.Forms.Padding(0);
            this.btnMenos.Name = "btnMenos";
            this.btnMenos.Size = new System.Drawing.Size(18, 21);
            this.btnMenos.TabIndex = 105;
            this.btnMenos.Text = "▼";
            this.btnMenos.UseVisualStyleBackColor = true;
            this.btnMenos.Click += new System.EventHandler(this.btnMenos_Click);
            // 
            // numQuantidadeDevolvidos
            // 
            this.numQuantidadeDevolvidos.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numQuantidadeDevolvidos.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numQuantidadeDevolvidos.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numQuantidadeDevolvidos.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numQuantidadeDevolvidos.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.numQuantidadeDevolvidos.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.numQuantidadeDevolvidos.BorderRadius = 10;
            this.numQuantidadeDevolvidos.BorderThickness = 1;
            this.numQuantidadeDevolvidos.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.numQuantidadeDevolvidos.Font = new System.Drawing.Font("Segoe UI", 16.25F);
            this.numQuantidadeDevolvidos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.numQuantidadeDevolvidos.HoverBackColor = System.Drawing.Color.LightGray;
            this.numQuantidadeDevolvidos.Location = new System.Drawing.Point(46, 529);
            this.numQuantidadeDevolvidos.Name = "numQuantidadeDevolvidos";
            this.numQuantidadeDevolvidos.Padding = new System.Windows.Forms.Padding(7);
            this.numQuantidadeDevolvidos.PlaceholderColor = System.Drawing.Color.Gray;
            this.numQuantidadeDevolvidos.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numQuantidadeDevolvidos.PlaceholderMarginLeft = 12;
            this.numQuantidadeDevolvidos.PlaceholderText = "";
            this.numQuantidadeDevolvidos.SelectedText = "";
            this.numQuantidadeDevolvidos.SelectionLength = 0;
            this.numQuantidadeDevolvidos.SelectionStart = 0;
            this.numQuantidadeDevolvidos.Size = new System.Drawing.Size(75, 40);
            this.numQuantidadeDevolvidos.TabIndex = 103;
            this.numQuantidadeDevolvidos.TextColor = System.Drawing.Color.Black;
            this.numQuantidadeDevolvidos.UseSystemPasswordChar = false;
            // 
            // btnMais
            // 
            this.btnMais.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMais.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnMais.Location = new System.Drawing.Point(128, 526);
            this.btnMais.Margin = new System.Windows.Forms.Padding(0);
            this.btnMais.Name = "btnMais";
            this.btnMais.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnMais.Size = new System.Drawing.Size(18, 21);
            this.btnMais.TabIndex = 104;
            this.btnMais.Text = "▲";
            this.btnMais.UseVisualStyleBackColor = true;
            this.btnMais.Click += new System.EventHandler(this.btnMais_Click);
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
            this.txtLivro.Enabled = false;
            this.txtLivro.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtLivro.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtLivro.Location = new System.Drawing.Point(46, 184);
            this.txtLivro.Name = "txtLivro";
            this.txtLivro.Padding = new System.Windows.Forms.Padding(7);
            this.txtLivro.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLivro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.txtLivro.PlaceholderMarginLeft = 12;
            this.txtLivro.PlaceholderText = "";
            this.txtLivro.SelectedText = "";
            this.txtLivro.SelectionLength = 0;
            this.txtLivro.SelectionStart = 0;
            this.txtLivro.Size = new System.Drawing.Size(405, 39);
            this.txtLivro.TabIndex = 107;
            this.txtLivro.TextColor = System.Drawing.Color.Black;
            this.txtLivro.UseSystemPasswordChar = false;
            // 
            // lblNome
            // 
            this.lblNome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNome.AutoSize = true;
            this.lblNome.BackColor = System.Drawing.Color.Transparent;
            this.lblNome.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.lblNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblNome.Location = new System.Drawing.Point(42, 158);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(130, 23);
            this.lblNome.TabIndex = 106;
            this.lblNome.Text = "Nome Do Livro:";
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(497, 50);
            this.panelHeader.TabIndex = 108;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(497, 50);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Devolver Livro Didático";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mtxCodigoBarras
            // 
            this.mtxCodigoBarras.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtxCodigoBarras.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mtxCodigoBarras.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.mtxCodigoBarras.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.mtxCodigoBarras.BorderRadius = 10;
            this.mtxCodigoBarras.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mtxCodigoBarras.Enabled = false;
            this.mtxCodigoBarras.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxCodigoBarras.ForeColor = System.Drawing.Color.Gray;
            this.mtxCodigoBarras.HoverBackColor = System.Drawing.Color.LightGray;
            this.mtxCodigoBarras.HoverBorderColor = System.Drawing.Color.DarkGray;
            this.mtxCodigoBarras.LeftMargin = 0;
            this.mtxCodigoBarras.Location = new System.Drawing.Point(46, 261);
            this.mtxCodigoBarras.Mask = "0 000000 000000";
            this.mtxCodigoBarras.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxCodigoBarras.Name = "mtxCodigoBarras";
            this.mtxCodigoBarras.Padding = new System.Windows.Forms.Padding(13, 7, 0, 0);
            this.mtxCodigoBarras.Size = new System.Drawing.Size(188, 40);
            this.mtxCodigoBarras.TabIndex = 117;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label4.Location = new System.Drawing.Point(42, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 23);
            this.label4.TabIndex = 118;
            this.label4.Text = "Código de barras:";
            // 
            // lblTurma
            // 
            this.lblTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTurma.AutoSize = true;
            this.lblTurma.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.lblTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblTurma.Location = new System.Drawing.Point(42, 314);
            this.lblTurma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Size = new System.Drawing.Size(62, 23);
            this.lblTurma.TabIndex = 120;
            this.lblTurma.Text = "Turma:";
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
            this.txtTurma.Location = new System.Drawing.Point(46, 340);
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
            this.txtTurma.Size = new System.Drawing.Size(405, 40);
            this.txtTurma.TabIndex = 119;
            this.txtTurma.TextColor = System.Drawing.Color.Black;
            this.txtTurma.UseSystemPasswordChar = false;
            // 
            // numQuantidadeEmprestado
            // 
            this.numQuantidadeEmprestado.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numQuantidadeEmprestado.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numQuantidadeEmprestado.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numQuantidadeEmprestado.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numQuantidadeEmprestado.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.numQuantidadeEmprestado.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.numQuantidadeEmprestado.BorderRadius = 10;
            this.numQuantidadeEmprestado.BorderThickness = 1;
            this.numQuantidadeEmprestado.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.numQuantidadeEmprestado.Enabled = false;
            this.numQuantidadeEmprestado.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numQuantidadeEmprestado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.numQuantidadeEmprestado.HoverBackColor = System.Drawing.Color.LightGray;
            this.numQuantidadeEmprestado.Location = new System.Drawing.Point(301, 261);
            this.numQuantidadeEmprestado.Name = "numQuantidadeEmprestado";
            this.numQuantidadeEmprestado.Padding = new System.Windows.Forms.Padding(7);
            this.numQuantidadeEmprestado.PlaceholderColor = System.Drawing.Color.Gray;
            this.numQuantidadeEmprestado.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numQuantidadeEmprestado.PlaceholderMarginLeft = 12;
            this.numQuantidadeEmprestado.PlaceholderText = "";
            this.numQuantidadeEmprestado.SelectedText = "";
            this.numQuantidadeEmprestado.SelectionLength = 0;
            this.numQuantidadeEmprestado.SelectionStart = 0;
            this.numQuantidadeEmprestado.Size = new System.Drawing.Size(75, 40);
            this.numQuantidadeEmprestado.TabIndex = 121;
            this.numQuantidadeEmprestado.TextColor = System.Drawing.Color.Black;
            this.numQuantidadeEmprestado.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label1.Location = new System.Drawing.Point(297, 236);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 23);
            this.label1.TabIndex = 122;
            this.label1.Text = "Qtd Emprestada:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label2.Location = new System.Drawing.Point(41, 501);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 25);
            this.label2.TabIndex = 123;
            this.label2.Text = "Qtd Devolvida:";
            // 
            // btnConfirmarDevolucao
            // 
            this.btnConfirmarDevolucao.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConfirmarDevolucao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnConfirmarDevolucao.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnConfirmarDevolucao.ForeColor = System.Drawing.Color.White;
            this.btnConfirmarDevolucao.Location = new System.Drawing.Point(301, 519);
            this.btnConfirmarDevolucao.Name = "btnConfirmarDevolucao";
            this.btnConfirmarDevolucao.Size = new System.Drawing.Size(150, 60);
            this.btnConfirmarDevolucao.TabIndex = 124;
            this.btnConfirmarDevolucao.Text = "DEVOLVER";
            this.btnConfirmarDevolucao.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label3.Location = new System.Drawing.Point(42, 397);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 23);
            this.label3.TabIndex = 126;
            this.label3.Text = "Hora do Empréstimo:";
            // 
            // roundedTextBox2
            // 
            this.roundedTextBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.roundedTextBox2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.roundedTextBox2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.roundedTextBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.roundedTextBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.roundedTextBox2.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.roundedTextBox2.BorderRadius = 10;
            this.roundedTextBox2.BorderThickness = 1;
            this.roundedTextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.roundedTextBox2.Enabled = false;
            this.roundedTextBox2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundedTextBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.roundedTextBox2.HoverBackColor = System.Drawing.Color.LightGray;
            this.roundedTextBox2.Location = new System.Drawing.Point(46, 422);
            this.roundedTextBox2.Name = "roundedTextBox2";
            this.roundedTextBox2.Padding = new System.Windows.Forms.Padding(7);
            this.roundedTextBox2.PlaceholderColor = System.Drawing.Color.Gray;
            this.roundedTextBox2.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundedTextBox2.PlaceholderMarginLeft = 12;
            this.roundedTextBox2.PlaceholderText = "";
            this.roundedTextBox2.SelectedText = "";
            this.roundedTextBox2.SelectionLength = 0;
            this.roundedTextBox2.SelectionStart = 0;
            this.roundedTextBox2.Size = new System.Drawing.Size(120, 40);
            this.roundedTextBox2.TabIndex = 125;
            this.roundedTextBox2.TextColor = System.Drawing.Color.Black;
            this.roundedTextBox2.UseSystemPasswordChar = false;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label5.Location = new System.Drawing.Point(297, 397);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 23);
            this.label5.TabIndex = 128;
            this.label5.Text = "Hora da Devolução:";
            // 
            // roundedTextBox3
            // 
            this.roundedTextBox3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.roundedTextBox3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.roundedTextBox3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.roundedTextBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.roundedTextBox3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.roundedTextBox3.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.roundedTextBox3.BorderRadius = 10;
            this.roundedTextBox3.BorderThickness = 1;
            this.roundedTextBox3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.roundedTextBox3.Enabled = false;
            this.roundedTextBox3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundedTextBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.roundedTextBox3.HoverBackColor = System.Drawing.Color.LightGray;
            this.roundedTextBox3.Location = new System.Drawing.Point(301, 422);
            this.roundedTextBox3.Name = "roundedTextBox3";
            this.roundedTextBox3.Padding = new System.Windows.Forms.Padding(7);
            this.roundedTextBox3.PlaceholderColor = System.Drawing.Color.Gray;
            this.roundedTextBox3.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundedTextBox3.PlaceholderMarginLeft = 12;
            this.roundedTextBox3.PlaceholderText = "";
            this.roundedTextBox3.SelectedText = "";
            this.roundedTextBox3.SelectionLength = 0;
            this.roundedTextBox3.SelectionStart = 0;
            this.roundedTextBox3.Size = new System.Drawing.Size(120, 40);
            this.roundedTextBox3.TabIndex = 127;
            this.roundedTextBox3.TextColor = System.Drawing.Color.Black;
            this.roundedTextBox3.UseSystemPasswordChar = false;
            // 
            // txtProfessor
            // 
            this.txtProfessor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtProfessor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtProfessor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtProfessor.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtProfessor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtProfessor.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtProfessor.BorderRadius = 10;
            this.txtProfessor.BorderThickness = 1;
            this.txtProfessor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtProfessor.Enabled = false;
            this.txtProfessor.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProfessor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtProfessor.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtProfessor.Location = new System.Drawing.Point(46, 107);
            this.txtProfessor.Name = "txtProfessor";
            this.txtProfessor.Padding = new System.Windows.Forms.Padding(7);
            this.txtProfessor.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtProfessor.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.txtProfessor.PlaceholderMarginLeft = 12;
            this.txtProfessor.PlaceholderText = "";
            this.txtProfessor.SelectedText = "";
            this.txtProfessor.SelectionLength = 0;
            this.txtProfessor.SelectionStart = 0;
            this.txtProfessor.Size = new System.Drawing.Size(405, 39);
            this.txtProfessor.TabIndex = 130;
            this.txtProfessor.TextColor = System.Drawing.Color.Black;
            this.txtProfessor.UseSystemPasswordChar = false;
            // 
            // lblProfessor
            // 
            this.lblProfessor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblProfessor.AutoSize = true;
            this.lblProfessor.BackColor = System.Drawing.Color.Transparent;
            this.lblProfessor.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold);
            this.lblProfessor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblProfessor.Location = new System.Drawing.Point(42, 81);
            this.lblProfessor.Name = "lblProfessor";
            this.lblProfessor.Size = new System.Drawing.Size(86, 23);
            this.lblProfessor.TabIndex = 129;
            this.lblProfessor.Text = "Professor:";
            // 
            // DevoluçãoRapidaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(497, 630);
            this.Controls.Add(this.txtProfessor);
            this.Controls.Add(this.lblProfessor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.roundedTextBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.roundedTextBox2);
            this.Controls.Add(this.btnConfirmarDevolucao);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numQuantidadeEmprestado);
            this.Controls.Add(this.lblTurma);
            this.Controls.Add(this.txtTurma);
            this.Controls.Add(this.mtxCodigoBarras);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.txtLivro);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.btnMenos);
            this.Controls.Add(this.numQuantidadeDevolvidos);
            this.Controls.Add(this.btnMais);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevoluçãoRapidaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BibliotecaApp";
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMenos;
        public RoundedTextBox numQuantidadeDevolvidos;
        private System.Windows.Forms.Button btnMais;
        private RoundedTextBox txtLivro;
        public System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private RoundedMaskedTextBox mtxCodigoBarras;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lblTurma;
        private RoundedTextBox txtTurma;
        public RoundedTextBox numQuantidadeEmprestado;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConfirmarDevolucao;
        public System.Windows.Forms.Label label3;
        public RoundedTextBox roundedTextBox2;
        public System.Windows.Forms.Label label5;
        public RoundedTextBox roundedTextBox3;
        private RoundedTextBox txtProfessor;
        public System.Windows.Forms.Label lblProfessor;
    }
}