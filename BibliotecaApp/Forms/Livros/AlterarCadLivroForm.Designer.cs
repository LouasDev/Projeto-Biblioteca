namespace BibliotecaApp
{
    partial class AlterarCadLivroForm
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
            this.btnGerarEtiqueta = new System.Windows.Forms.Button();
            this.lstSugestoesGenero = new System.Windows.Forms.ListBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.mtxCodigoBarras = new RoundedMaskedTextBox();
            this.txtAutor = new RoundedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtQuantidade = new RoundedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGenero = new RoundedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNome = new RoundedTextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.Titulo = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnGerarEtiqueta);
            this.panel1.Controls.Add(this.lstSugestoesGenero);
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnExcluir);
            this.panel1.Controls.Add(this.btnSalvar);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.mtxCodigoBarras);
            this.panel1.Controls.Add(this.txtAutor);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtQuantidade);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtGenero);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtNome);
            this.panel1.Controls.Add(this.lblNome);
            this.panel1.Controls.Add(this.Titulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 905);
            this.panel1.TabIndex = 94;
            // 
            // btnGerarEtiqueta
            // 
            this.btnGerarEtiqueta.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnGerarEtiqueta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnGerarEtiqueta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGerarEtiqueta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGerarEtiqueta.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F, System.Drawing.FontStyle.Bold);
            this.btnGerarEtiqueta.ForeColor = System.Drawing.Color.White;
            this.btnGerarEtiqueta.Image = global::BibliotecaApp.Properties.Resources.icons8_refresh_barcode_25;
            this.btnGerarEtiqueta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGerarEtiqueta.Location = new System.Drawing.Point(823, 140);
            this.btnGerarEtiqueta.Name = "btnGerarEtiqueta";
            this.btnGerarEtiqueta.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
            this.btnGerarEtiqueta.Size = new System.Drawing.Size(126, 53);
            this.btnGerarEtiqueta.TabIndex = 133;
            this.btnGerarEtiqueta.Text = "     Etiqueta";
            this.btnGerarEtiqueta.UseVisualStyleBackColor = false;
            this.btnGerarEtiqueta.Click += new System.EventHandler(this.btnGerarEtiqueta_Click);
            // 
            // lstSugestoesGenero
            // 
            this.lstSugestoesGenero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lstSugestoesGenero.FormattingEnabled = true;
            this.lstSugestoesGenero.ItemHeight = 25;
            this.lstSugestoesGenero.Location = new System.Drawing.Point(332, 341);
            this.lstSugestoesGenero.Name = "lstSugestoesGenero";
            this.lstSugestoesGenero.Size = new System.Drawing.Size(617, 79);
            this.lstSugestoesGenero.TabIndex = 132;
            this.lstSugestoesGenero.Visible = false;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancelar.BackColor = System.Drawing.Color.DimGray;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(566, 701);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(149, 57);
            this.btnCancelar.TabIndex = 131;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnExcluir
            // 
            this.btnExcluir.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExcluir.BackColor = System.Drawing.Color.DarkRed;
            this.btnExcluir.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnExcluir.ForeColor = System.Drawing.Color.White;
            this.btnExcluir.Location = new System.Drawing.Point(332, 701);
            this.btnExcluir.Margin = new System.Windows.Forms.Padding(5);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Size = new System.Drawing.Size(149, 57);
            this.btnExcluir.TabIndex = 130;
            this.btnExcluir.Text = "Excluir ";
            this.btnExcluir.UseVisualStyleBackColor = false;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSalvar.BackColor = System.Drawing.Color.DarkGreen;
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(800, 701);
            this.btnSalvar.Margin = new System.Windows.Forms.Padding(5);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(149, 57);
            this.btnSalvar.TabIndex = 129;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label4.Location = new System.Drawing.Point(335, 525);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 25);
            this.label4.TabIndex = 103;
            this.label4.Text = "Codigo de Barras";
            // 
            // mtxCodigoBarras
            // 
            this.mtxCodigoBarras.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mtxCodigoBarras.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mtxCodigoBarras.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.mtxCodigoBarras.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.mtxCodigoBarras.BorderRadius = 10;
            this.mtxCodigoBarras.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mtxCodigoBarras.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxCodigoBarras.ForeColor = System.Drawing.Color.Gray;
            this.mtxCodigoBarras.HoverBackColor = System.Drawing.Color.LightGray;
            this.mtxCodigoBarras.HoverBorderColor = System.Drawing.Color.DarkGray;
            this.mtxCodigoBarras.LeftMargin = 0;
            this.mtxCodigoBarras.Location = new System.Drawing.Point(332, 553);
            this.mtxCodigoBarras.Mask = "0 000000 000000";
            this.mtxCodigoBarras.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxCodigoBarras.Name = "mtxCodigoBarras";
            this.mtxCodigoBarras.Padding = new System.Windows.Forms.Padding(13, 7, 0, 0);
            this.mtxCodigoBarras.Size = new System.Drawing.Size(617, 40);
            this.mtxCodigoBarras.TabIndex = 102;
            // 
            // txtAutor
            // 
            this.txtAutor.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtAutor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtAutor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtAutor.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtAutor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtAutor.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtAutor.BorderRadius = 10;
            this.txtAutor.BorderThickness = 1;
            this.txtAutor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAutor.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAutor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtAutor.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtAutor.Location = new System.Drawing.Point(332, 466);
            this.txtAutor.Name = "txtAutor";
            this.txtAutor.Padding = new System.Windows.Forms.Padding(7);
            this.txtAutor.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtAutor.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.txtAutor.PlaceholderMarginLeft = 12;
            this.txtAutor.PlaceholderText = "Digite aqui a quantidade...";
            this.txtAutor.SelectedText = "";
            this.txtAutor.SelectionLength = 0;
            this.txtAutor.SelectionStart = 0;
            this.txtAutor.Size = new System.Drawing.Size(617, 40);
            this.txtAutor.TabIndex = 101;
            this.txtAutor.TextColor = System.Drawing.Color.Black;
            this.txtAutor.UseSystemPasswordChar = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label3.Location = new System.Drawing.Point(335, 438);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 25);
            this.label3.TabIndex = 100;
            this.label3.Text = "Autor";
            // 
            // txtQuantidade
            // 
            this.txtQuantidade.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtQuantidade.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtQuantidade.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtQuantidade.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtQuantidade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtQuantidade.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtQuantidade.BorderRadius = 10;
            this.txtQuantidade.BorderThickness = 1;
            this.txtQuantidade.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtQuantidade.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuantidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtQuantidade.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtQuantidade.Location = new System.Drawing.Point(332, 382);
            this.txtQuantidade.Name = "txtQuantidade";
            this.txtQuantidade.Padding = new System.Windows.Forms.Padding(7);
            this.txtQuantidade.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtQuantidade.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.txtQuantidade.PlaceholderMarginLeft = 12;
            this.txtQuantidade.PlaceholderText = "Digite aqui a quantidade...";
            this.txtQuantidade.SelectedText = "";
            this.txtQuantidade.SelectionLength = 0;
            this.txtQuantidade.SelectionStart = 0;
            this.txtQuantidade.Size = new System.Drawing.Size(617, 40);
            this.txtQuantidade.TabIndex = 99;
            this.txtQuantidade.TextColor = System.Drawing.Color.Black;
            this.txtQuantidade.UseSystemPasswordChar = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label2.Location = new System.Drawing.Point(335, 354);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 25);
            this.label2.TabIndex = 98;
            this.label2.Text = "Quantidade";
            // 
            // txtGenero
            // 
            this.txtGenero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtGenero.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtGenero.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtGenero.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtGenero.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtGenero.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtGenero.BorderRadius = 10;
            this.txtGenero.BorderThickness = 1;
            this.txtGenero.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGenero.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGenero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtGenero.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtGenero.Location = new System.Drawing.Point(332, 301);
            this.txtGenero.Name = "txtGenero";
            this.txtGenero.Padding = new System.Windows.Forms.Padding(7);
            this.txtGenero.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtGenero.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.txtGenero.PlaceholderMarginLeft = 12;
            this.txtGenero.PlaceholderText = "Digite aqui o gênero...";
            this.txtGenero.SelectedText = "";
            this.txtGenero.SelectionLength = 0;
            this.txtGenero.SelectionStart = 0;
            this.txtGenero.Size = new System.Drawing.Size(617, 40);
            this.txtGenero.TabIndex = 97;
            this.txtGenero.TextColor = System.Drawing.Color.Black;
            this.txtGenero.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label1.Location = new System.Drawing.Point(335, 273);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 25);
            this.label1.TabIndex = 96;
            this.label1.Text = "Gênero";
            // 
            // txtNome
            // 
            this.txtNome.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtNome.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtNome.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtNome.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNome.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtNome.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtNome.BorderRadius = 10;
            this.txtNome.BorderThickness = 1;
            this.txtNome.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtNome.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtNome.Location = new System.Drawing.Point(332, 219);
            this.txtNome.Name = "txtNome";
            this.txtNome.Padding = new System.Windows.Forms.Padding(7);
            this.txtNome.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNome.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.txtNome.PlaceholderMarginLeft = 12;
            this.txtNome.PlaceholderText = "Digite aqui o nome...";
            this.txtNome.SelectedText = "";
            this.txtNome.SelectionLength = 0;
            this.txtNome.SelectionStart = 0;
            this.txtNome.Size = new System.Drawing.Size(617, 40);
            this.txtNome.TabIndex = 95;
            this.txtNome.TextColor = System.Drawing.Color.Black;
            this.txtNome.UseSystemPasswordChar = false;
            // 
            // lblNome
            // 
            this.lblNome.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNome.AutoSize = true;
            this.lblNome.BackColor = System.Drawing.Color.Transparent;
            this.lblNome.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblNome.Location = new System.Drawing.Point(335, 191);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(142, 25);
            this.lblNome.TabIndex = 94;
            this.lblNome.Text = "Nome Do Livro";
            // 
            // Titulo
            // 
            this.Titulo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Titulo.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.Titulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.Titulo.Location = new System.Drawing.Point(490, 53);
            this.Titulo.Name = "Titulo";
            this.Titulo.Size = new System.Drawing.Size(301, 46);
            this.Titulo.TabIndex = 93;
            this.Titulo.Text = "EDITOR DE LIVRO";
            // 
            // AlterarCadLivroForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScrollMargin = new System.Drawing.Size(2, 2);
            this.ClientSize = new System.Drawing.Size(1280, 905);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AlterarCadLivroForm";
            this.Text = "AlterarCadLivroForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label label4;
        private RoundedMaskedTextBox mtxCodigoBarras;
        private RoundedTextBox txtAutor;
        public System.Windows.Forms.Label label3;
        private RoundedTextBox txtQuantidade;
        public System.Windows.Forms.Label label2;
        private RoundedTextBox txtGenero;
        public System.Windows.Forms.Label label1;
        private RoundedTextBox txtNome;
        public System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.Label Titulo;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.ListBox lstSugestoesGenero;
        private System.Windows.Forms.Button btnGerarEtiqueta;
    }
}