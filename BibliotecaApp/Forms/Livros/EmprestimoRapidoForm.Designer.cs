// EmprestimoRapidoForm.Designer.cs
using System;

namespace BibliotecaApp.Forms.Livros
{
    partial class EmprestimoRapidoForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvRapidos = new System.Windows.Forms.DataGridView();
            this.labelTitle = new System.Windows.Forms.Label();
            this.lblProfessor = new System.Windows.Forms.Label();
            this.lblLivro = new System.Windows.Forms.Label();
            this.lblTurma = new System.Windows.Forms.Label();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.lblBibliotecaria = new System.Windows.Forms.Label();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.lblHoraEmprestimo = new System.Windows.Forms.Label();
            this.lstSugestoesLivro = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstSugestoesProfessor = new System.Windows.Forms.ListBox();
            this.lstSugestoesTurma = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtBarcode = new RoundedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnMenos = new System.Windows.Forms.Button();
            this.numQuantidade = new RoundedTextBox();
            this.btnMais = new System.Windows.Forms.Button();
            this.cbBibliotecaria = new RoundedComboBox();
            this.txtTurma = new RoundedTextBox();
            this.txtLivro = new RoundedTextBox();
            this.txtProfessor = new RoundedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRapidos)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvRapidos
            // 
            this.dgvRapidos.AllowUserToAddRows = false;
            this.dgvRapidos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dgvRapidos.Location = new System.Drawing.Point(22, 340);
            this.dgvRapidos.Name = "dgvRapidos";
            this.dgvRapidos.ReadOnly = true;
            this.dgvRapidos.RowTemplate.Height = 40;
            this.dgvRapidos.Size = new System.Drawing.Size(1236, 502);
            this.dgvRapidos.TabIndex = 17;
            this.dgvRapidos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRapidos_CellContentClick);
            this.dgvRapidos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvRapidos_CellFormatting);
            this.dgvRapidos.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvRapidos_CellPainting);
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.labelTitle.Location = new System.Drawing.Point(451, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(379, 41);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "EMPRÉSTIMO RÁPIDO";
            // 
            // lblProfessor
            // 
            this.lblProfessor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblProfessor.AutoSize = true;
            this.lblProfessor.BackColor = System.Drawing.Color.Transparent;
            this.lblProfessor.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfessor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblProfessor.Location = new System.Drawing.Point(41, 100);
            this.lblProfessor.Name = "lblProfessor";
            this.lblProfessor.Size = new System.Drawing.Size(98, 25);
            this.lblProfessor.TabIndex = 84;
            this.lblProfessor.Text = "Professor:";
            // 
            // lblLivro
            // 
            this.lblLivro.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblLivro.AutoSize = true;
            this.lblLivro.BackColor = System.Drawing.Color.Transparent;
            this.lblLivro.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLivro.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblLivro.Location = new System.Drawing.Point(451, 100);
            this.lblLivro.Name = "lblLivro";
            this.lblLivro.Size = new System.Drawing.Size(59, 25);
            this.lblLivro.TabIndex = 85;
            this.lblLivro.Text = "Livro:";
            // 
            // lblTurma
            // 
            this.lblTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTurma.AutoSize = true;
            this.lblTurma.BackColor = System.Drawing.Color.Transparent;
            this.lblTurma.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblTurma.Location = new System.Drawing.Point(41, 184);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Size = new System.Drawing.Size(71, 25);
            this.lblTurma.TabIndex = 87;
            this.lblTurma.Text = "Turma:";
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblQuantidade.AutoSize = true;
            this.lblQuantidade.BackColor = System.Drawing.Color.Transparent;
            this.lblQuantidade.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblQuantidade.Location = new System.Drawing.Point(862, 184);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(49, 25);
            this.lblQuantidade.TabIndex = 88;
            this.lblQuantidade.Text = "Qtd:";
            // 
            // lblBibliotecaria
            // 
            this.lblBibliotecaria.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBibliotecaria.AutoSize = true;
            this.lblBibliotecaria.BackColor = System.Drawing.Color.Transparent;
            this.lblBibliotecaria.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBibliotecaria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblBibliotecaria.Location = new System.Drawing.Point(451, 187);
            this.lblBibliotecaria.Name = "lblBibliotecaria";
            this.lblBibliotecaria.Size = new System.Drawing.Size(123, 25);
            this.lblBibliotecaria.TabIndex = 90;
            this.lblBibliotecaria.Text = "Bibliotecária:";
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRegistrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnRegistrar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnRegistrar.ForeColor = System.Drawing.Color.White;
            this.btnRegistrar.Location = new System.Drawing.Point(1007, 198);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(188, 68);
            this.btnRegistrar.TabIndex = 9;
            this.btnRegistrar.Text = "REGISTRAR";
            this.btnRegistrar.UseVisualStyleBackColor = false;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // lblHoraEmprestimo
            // 
            this.lblHoraEmprestimo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblHoraEmprestimo.AutoSize = true;
            this.lblHoraEmprestimo.BackColor = System.Drawing.Color.Transparent;
            this.lblHoraEmprestimo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoraEmprestimo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblHoraEmprestimo.Location = new System.Drawing.Point(541, 267);
            this.lblHoraEmprestimo.Name = "lblHoraEmprestimo";
            this.lblHoraEmprestimo.Size = new System.Drawing.Size(198, 21);
            this.lblHoraEmprestimo.TabIndex = 94;
            this.lblHoraEmprestimo.Text = "Hora do Empréstimo: --:--";
            // 
            // lstSugestoesLivro
            // 
            this.lstSugestoesLivro.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstSugestoesLivro.ItemHeight = 21;
            this.lstSugestoesLivro.Location = new System.Drawing.Point(456, 168);
            this.lstSugestoesLivro.Name = "lstSugestoesLivro";
            this.lstSugestoesLivro.Size = new System.Drawing.Size(368, 151);
            this.lstSugestoesLivro.TabIndex = 4;
            this.lstSugestoesLivro.Visible = false;
            this.lstSugestoesLivro.Click += new System.EventHandler(this.lstSugestoesLivro_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label1.Location = new System.Drawing.Point(17, 305);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 25);
            this.label1.TabIndex = 95;
            this.label1.Text = "Emprestimos Efetuados:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lstSugestoesProfessor
            // 
            this.lstSugestoesProfessor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstSugestoesProfessor.ItemHeight = 21;
            this.lstSugestoesProfessor.Location = new System.Drawing.Point(46, 168);
            this.lstSugestoesProfessor.Name = "lstSugestoesProfessor";
            this.lstSugestoesProfessor.Size = new System.Drawing.Size(372, 109);
            this.lstSugestoesProfessor.TabIndex = 2;
            this.lstSugestoesProfessor.Visible = false;
            this.lstSugestoesProfessor.Click += new System.EventHandler(this.lstSugestoesProfessor_Click);
            // 
            // lstSugestoesTurma
            // 
            this.lstSugestoesTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstSugestoesTurma.ItemHeight = 21;
            this.lstSugestoesTurma.Location = new System.Drawing.Point(46, 252);
            this.lstSugestoesTurma.Name = "lstSugestoesTurma";
            this.lstSugestoesTurma.Size = new System.Drawing.Size(372, 130);
            this.lstSugestoesTurma.TabIndex = 7;
            this.lstSugestoesTurma.Visible = false;
            this.lstSugestoesTurma.Click += new System.EventHandler(this.lstSugestoesTurma_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txtBarcode);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnMenos);
            this.panel1.Controls.Add(this.numQuantidade);
            this.panel1.Controls.Add(this.btnMais);
            this.panel1.Controls.Add(this.lstSugestoesTurma);
            this.panel1.Controls.Add(this.lstSugestoesProfessor);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lstSugestoesLivro);
            this.panel1.Controls.Add(this.lblHoraEmprestimo);
            this.panel1.Controls.Add(this.cbBibliotecaria);
            this.panel1.Controls.Add(this.txtTurma);
            this.panel1.Controls.Add(this.btnRegistrar);
            this.panel1.Controls.Add(this.lblBibliotecaria);
            this.panel1.Controls.Add(this.lblQuantidade);
            this.panel1.Controls.Add(this.lblTurma);
            this.panel1.Controls.Add(this.txtLivro);
            this.panel1.Controls.Add(this.lblLivro);
            this.panel1.Controls.Add(this.lblProfessor);
            this.panel1.Controls.Add(this.txtProfessor);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Controls.Add(this.dgvRapidos);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 845);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
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
            this.txtBarcode.Location = new System.Drawing.Point(867, 128);
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
            this.txtBarcode.Size = new System.Drawing.Size(351, 40);
            this.txtBarcode.TabIndex = 103;
            this.txtBarcode.TextColor = System.Drawing.Color.Black;
            this.txtBarcode.UseSystemPasswordChar = false;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label7.Location = new System.Drawing.Point(862, 99);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(162, 25);
            this.label7.TabIndex = 102;
            this.label7.Text = "Codigo de barras:";
            // 
            // btnMenos
            // 
            this.btnMenos.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMenos.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnMenos.Location = new System.Drawing.Point(924, 232);
            this.btnMenos.Margin = new System.Windows.Forms.Padding(0);
            this.btnMenos.Name = "btnMenos";
            this.btnMenos.Size = new System.Drawing.Size(18, 21);
            this.btnMenos.TabIndex = 98;
            this.btnMenos.Text = "▼";
            this.btnMenos.UseVisualStyleBackColor = true;
            this.btnMenos.Click += new System.EventHandler(this.btnMenos_Click);
            // 
            // numQuantidade
            // 
            this.numQuantidade.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numQuantidade.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numQuantidade.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numQuantidade.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numQuantidade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.numQuantidade.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.numQuantidade.BorderRadius = 10;
            this.numQuantidade.BorderThickness = 1;
            this.numQuantidade.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.numQuantidade.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numQuantidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.numQuantidade.HoverBackColor = System.Drawing.Color.LightGray;
            this.numQuantidade.Location = new System.Drawing.Point(867, 212);
            this.numQuantidade.Name = "numQuantidade";
            this.numQuantidade.Padding = new System.Windows.Forms.Padding(7);
            this.numQuantidade.PlaceholderColor = System.Drawing.Color.Gray;
            this.numQuantidade.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numQuantidade.PlaceholderMarginLeft = 12;
            this.numQuantidade.PlaceholderText = "";
            this.numQuantidade.SelectedText = "";
            this.numQuantidade.SelectionLength = 0;
            this.numQuantidade.SelectionStart = 0;
            this.numQuantidade.Size = new System.Drawing.Size(54, 40);
            this.numQuantidade.TabIndex = 5;
            this.numQuantidade.TextColor = System.Drawing.Color.Black;
            this.numQuantidade.UseSystemPasswordChar = false;
            this.numQuantidade.Load += new System.EventHandler(this.numQuantidade_Load);
            this.numQuantidade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numQuantidade_KeyPress);
            // 
            // btnMais
            // 
            this.btnMais.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMais.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnMais.Location = new System.Drawing.Point(924, 210);
            this.btnMais.Margin = new System.Windows.Forms.Padding(0);
            this.btnMais.Name = "btnMais";
            this.btnMais.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnMais.Size = new System.Drawing.Size(18, 21);
            this.btnMais.TabIndex = 96;
            this.btnMais.Text = "▲";
            this.btnMais.UseVisualStyleBackColor = true;
            this.btnMais.Click += new System.EventHandler(this.btnMais_Click);
            // 
            // cbBibliotecaria
            // 
            this.cbBibliotecaria.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbBibliotecaria.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbBibliotecaria.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbBibliotecaria.BorderRadius = 8;
            this.cbBibliotecaria.BorderThickness = 1;
            this.cbBibliotecaria.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbBibliotecaria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBibliotecaria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBibliotecaria.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBibliotecaria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.cbBibliotecaria.FormattingEnabled = true;
            this.cbBibliotecaria.ItemsFont = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBibliotecaria.Location = new System.Drawing.Point(456, 215);
            this.cbBibliotecaria.Name = "cbBibliotecaria";
            this.cbBibliotecaria.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBibliotecaria.PlaceholderMargin = 10;
            this.cbBibliotecaria.PlaceholderText = "Selecione a bibliotecária ...";
            this.cbBibliotecaria.Size = new System.Drawing.Size(368, 34);
            this.cbBibliotecaria.TabIndex = 8;
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
            this.txtTurma.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTurma.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtTurma.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtTurma.Location = new System.Drawing.Point(46, 212);
            this.txtTurma.Name = "txtTurma";
            this.txtTurma.Padding = new System.Windows.Forms.Padding(7);
            this.txtTurma.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtTurma.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTurma.PlaceholderMarginLeft = 12;
            this.txtTurma.PlaceholderText = "Digite aqui a turma...";
            this.txtTurma.SelectedText = "";
            this.txtTurma.SelectionLength = 0;
            this.txtTurma.SelectionStart = 0;
            this.txtTurma.Size = new System.Drawing.Size(372, 40);
            this.txtTurma.TabIndex = 6;
            this.txtTurma.TextColor = System.Drawing.Color.Black;
            this.txtTurma.UseSystemPasswordChar = false;
            this.txtTurma.TextChanged += new System.EventHandler(this.txtTurma_TextChanged);
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
            this.txtLivro.Location = new System.Drawing.Point(456, 128);
            this.txtLivro.Name = "txtLivro";
            this.txtLivro.Padding = new System.Windows.Forms.Padding(7);
            this.txtLivro.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLivro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLivro.PlaceholderMarginLeft = 12;
            this.txtLivro.PlaceholderText = "Digite aqui o livro...";
            this.txtLivro.SelectedText = "";
            this.txtLivro.SelectionLength = 0;
            this.txtLivro.SelectionStart = 0;
            this.txtLivro.Size = new System.Drawing.Size(368, 40);
            this.txtLivro.TabIndex = 3;
            this.txtLivro.TextColor = System.Drawing.Color.Black;
            this.txtLivro.UseSystemPasswordChar = false;
            this.txtLivro.TextChanged += new System.EventHandler(this.txtLivro_TextChanged);
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
            this.txtProfessor.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProfessor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtProfessor.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtProfessor.Location = new System.Drawing.Point(46, 128);
            this.txtProfessor.Name = "txtProfessor";
            this.txtProfessor.Padding = new System.Windows.Forms.Padding(7);
            this.txtProfessor.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtProfessor.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProfessor.PlaceholderMarginLeft = 12;
            this.txtProfessor.PlaceholderText = "Digite aqui o professor...";
            this.txtProfessor.SelectedText = "";
            this.txtProfessor.SelectionLength = 0;
            this.txtProfessor.SelectionStart = 0;
            this.txtProfessor.Size = new System.Drawing.Size(372, 40);
            this.txtProfessor.TabIndex = 1;
            this.txtProfessor.TextColor = System.Drawing.Color.Black;
            this.txtProfessor.UseSystemPasswordChar = false;
            this.txtProfessor.TextChanged += new System.EventHandler(this.txtProfessor_TextChanged);
            // 
            // EmprestimoRapidoForm
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1280, 845);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EmprestimoRapidoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.EmprestimoRapidoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRapidos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.DataGridView dgvRapidos;
        private System.Windows.Forms.Label labelTitle;
        private RoundedTextBox txtProfessor;
        public System.Windows.Forms.Label lblProfessor;
        public System.Windows.Forms.Label lblLivro;
        private RoundedTextBox txtLivro;
        public System.Windows.Forms.Label lblTurma;
        public System.Windows.Forms.Label lblQuantidade;
        public System.Windows.Forms.Label lblBibliotecaria;
        private System.Windows.Forms.Button btnRegistrar;
        private RoundedTextBox txtTurma;
        private RoundedComboBox cbBibliotecaria;
        public System.Windows.Forms.Label lblHoraEmprestimo;
        private System.Windows.Forms.ListBox lstSugestoesLivro;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstSugestoesProfessor;
        private System.Windows.Forms.ListBox lstSugestoesTurma;
        private System.Windows.Forms.Panel panel1;
        private RoundedTextBox numQuantidade;
        private System.Windows.Forms.Button btnMais;
        private System.Windows.Forms.Button btnMenos;
        private RoundedTextBox txtBarcode;
        private System.Windows.Forms.Label label7;
    }
}
