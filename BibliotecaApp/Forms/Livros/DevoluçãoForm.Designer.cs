namespace BibliotecaApp.Forms.Livros
{
    partial class DevoluçãoForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnConfirmarDevolucao = new System.Windows.Forms.Button();
            this.btnProrrogar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.Titulo = new System.Windows.Forms.Label();
            this.dgvEmprestimos = new System.Windows.Forms.DataGridView();
            this.lblDadosLivro = new System.Windows.Forms.Label();
            this.btnBuscarEmprestimo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mtxCodigoBarras = new RoundedTextBox();
            this.lstSugestoesUsuario = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsuario = new RoundedTextBox();
            this.cbFiltroEmprestimo = new RoundedComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLivro = new RoundedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmprestimos)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConfirmarDevolucao
            // 
            this.btnConfirmarDevolucao.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConfirmarDevolucao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnConfirmarDevolucao.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnConfirmarDevolucao.ForeColor = System.Drawing.Color.White;
            this.btnConfirmarDevolucao.Location = new System.Drawing.Point(745, 734);
            this.btnConfirmarDevolucao.Name = "btnConfirmarDevolucao";
            this.btnConfirmarDevolucao.Size = new System.Drawing.Size(150, 60);
            this.btnConfirmarDevolucao.TabIndex = 118;
            this.btnConfirmarDevolucao.Text = "DEVOLVER";
            this.btnConfirmarDevolucao.UseVisualStyleBackColor = false;
            this.btnConfirmarDevolucao.Click += new System.EventHandler(this.btnConfirmarDevolucao_Click);
            // 
            // btnProrrogar
            // 
            this.btnProrrogar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnProrrogar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnProrrogar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnProrrogar.ForeColor = System.Drawing.Color.White;
            this.btnProrrogar.Location = new System.Drawing.Point(389, 734);
            this.btnProrrogar.Name = "btnProrrogar";
            this.btnProrrogar.Size = new System.Drawing.Size(150, 60);
            this.btnProrrogar.TabIndex = 117;
            this.btnProrrogar.Text = "PRORROGAR";
            this.btnProrrogar.UseVisualStyleBackColor = false;
            this.btnProrrogar.Click += new System.EventHandler(this.btnProrrogar_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label4.Location = new System.Drawing.Point(611, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 25);
            this.label4.TabIndex = 116;
            this.label4.Text = "Código de Barras:";
            // 
            // lblNome
            // 
            this.lblNome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNome.AutoSize = true;
            this.lblNome.BackColor = System.Drawing.Color.Transparent;
            this.lblNome.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblNome.Location = new System.Drawing.Point(319, 117);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(144, 25);
            this.lblNome.TabIndex = 107;
            this.lblNome.Text = "Nome do Livro:";
            // 
            // Titulo
            // 
            this.Titulo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Titulo.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.Titulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.Titulo.Location = new System.Drawing.Point(448, 16);
            this.Titulo.Name = "Titulo";
            this.Titulo.Size = new System.Drawing.Size(384, 46);
            this.Titulo.TabIndex = 106;
            this.Titulo.Text = "DEVOLUÇÃO DE LIVRO";
            // 
            // dgvEmprestimos
            // 
            this.dgvEmprestimos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dgvEmprestimos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEmprestimos.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEmprestimos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvEmprestimos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmprestimos.Location = new System.Drawing.Point(2, 245);
            this.dgvEmprestimos.Name = "dgvEmprestimos";
            this.dgvEmprestimos.ReadOnly = true;
            this.dgvEmprestimos.RowHeadersWidth = 51;
            this.dgvEmprestimos.RowTemplate.Height = 24;
            this.dgvEmprestimos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEmprestimos.Size = new System.Drawing.Size(1276, 452);
            this.dgvEmprestimos.TabIndex = 119;
            this.dgvEmprestimos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEmprestimos_CellContentClick);
            this.dgvEmprestimos.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvEmprestimos_CellFormatting);
            this.dgvEmprestimos.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvEmprestimos_CellPainting);
            // 
            // lblDadosLivro
            // 
            this.lblDadosLivro.AutoSize = true;
            this.lblDadosLivro.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDadosLivro.Location = new System.Drawing.Point(22, 321);
            this.lblDadosLivro.Name = "lblDadosLivro";
            this.lblDadosLivro.Size = new System.Drawing.Size(0, 19);
            this.lblDadosLivro.TabIndex = 121;
            // 
            // btnBuscarEmprestimo
            // 
            this.btnBuscarEmprestimo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBuscarEmprestimo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnBuscarEmprestimo.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnBuscarEmprestimo.ForeColor = System.Drawing.Color.White;
            this.btnBuscarEmprestimo.Location = new System.Drawing.Point(1128, 135);
            this.btnBuscarEmprestimo.Name = "btnBuscarEmprestimo";
            this.btnBuscarEmprestimo.Size = new System.Drawing.Size(150, 60);
            this.btnBuscarEmprestimo.TabIndex = 123;
            this.btnBuscarEmprestimo.Text = "Procurar";
            this.btnBuscarEmprestimo.UseVisualStyleBackColor = false;
            this.btnBuscarEmprestimo.Click += new System.EventHandler(this.btnBuscarEmprestimo_Click);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.mtxCodigoBarras);
            this.panel1.Controls.Add(this.lstSugestoesUsuario);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtUsuario);
            this.panel1.Controls.Add(this.cbFiltroEmprestimo);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.Titulo);
            this.panel1.Controls.Add(this.btnBuscarEmprestimo);
            this.panel1.Controls.Add(this.lblNome);
            this.panel1.Controls.Add(this.txtLivro);
            this.panel1.Controls.Add(this.dgvEmprestimos);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnConfirmarDevolucao);
            this.panel1.Controls.Add(this.btnProrrogar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 845);
            this.panel1.TabIndex = 0;
            // 
            // mtxCodigoBarras
            // 
            this.mtxCodigoBarras.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtxCodigoBarras.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.mtxCodigoBarras.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.mtxCodigoBarras.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mtxCodigoBarras.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.mtxCodigoBarras.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.mtxCodigoBarras.BorderRadius = 10;
            this.mtxCodigoBarras.BorderThickness = 1;
            this.mtxCodigoBarras.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mtxCodigoBarras.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxCodigoBarras.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxCodigoBarras.HoverBackColor = System.Drawing.Color.LightGray;
            this.mtxCodigoBarras.Location = new System.Drawing.Point(616, 146);
            this.mtxCodigoBarras.Margin = new System.Windows.Forms.Padding(4);
            this.mtxCodigoBarras.Name = "mtxCodigoBarras";
            this.mtxCodigoBarras.Padding = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.mtxCodigoBarras.PlaceholderColor = System.Drawing.Color.Gray;
            this.mtxCodigoBarras.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxCodigoBarras.PlaceholderMarginLeft = 12;
            this.mtxCodigoBarras.PlaceholderText = "Escaneei para buscar...";
            this.mtxCodigoBarras.SelectedText = "";
            this.mtxCodigoBarras.SelectionLength = 0;
            this.mtxCodigoBarras.SelectionStart = 0;
            this.mtxCodigoBarras.Size = new System.Drawing.Size(221, 40);
            this.mtxCodigoBarras.TabIndex = 136;
            this.mtxCodigoBarras.TextColor = System.Drawing.Color.Black;
            this.mtxCodigoBarras.UseSystemPasswordChar = false;
            // 
            // lstSugestoesUsuario
            // 
            this.lstSugestoesUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstSugestoesUsuario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSugestoesUsuario.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.lstSugestoesUsuario.FormattingEnabled = true;
            this.lstSugestoesUsuario.ItemHeight = 25;
            this.lstSugestoesUsuario.Location = new System.Drawing.Point(3, 184);
            this.lstSugestoesUsuario.Margin = new System.Windows.Forms.Padding(4);
            this.lstSugestoesUsuario.Name = "lstSugestoesUsuario";
            this.lstSugestoesUsuario.ScrollAlwaysVisible = true;
            this.lstSugestoesUsuario.Size = new System.Drawing.Size(299, 102);
            this.lstSugestoesUsuario.TabIndex = 128;
            this.lstSugestoesUsuario.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label1.Location = new System.Drawing.Point(-2, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 25);
            this.label1.TabIndex = 126;
            this.label1.Text = "Nome do Usuario:";
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
            this.txtUsuario.Location = new System.Drawing.Point(3, 145);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Padding = new System.Windows.Forms.Padding(7);
            this.txtUsuario.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtUsuario.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.txtUsuario.PlaceholderMarginLeft = 12;
            this.txtUsuario.PlaceholderText = "Digite o nome do livro...";
            this.txtUsuario.SelectedText = "";
            this.txtUsuario.SelectionLength = 0;
            this.txtUsuario.SelectionStart = 0;
            this.txtUsuario.Size = new System.Drawing.Size(299, 40);
            this.txtUsuario.TabIndex = 1;
            this.txtUsuario.TextColor = System.Drawing.Color.Black;
            this.txtUsuario.UseSystemPasswordChar = false;
            // 
            // cbFiltroEmprestimo
            // 
            this.cbFiltroEmprestimo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbFiltroEmprestimo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbFiltroEmprestimo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbFiltroEmprestimo.BorderRadius = 8;
            this.cbFiltroEmprestimo.BorderThickness = 1;
            this.cbFiltroEmprestimo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbFiltroEmprestimo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltroEmprestimo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFiltroEmprestimo.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbFiltroEmprestimo.FormattingEnabled = true;
            this.cbFiltroEmprestimo.Items.AddRange(new object[] {
            "Todos",
            "Devolvido",
            "Atrasado",
            "Ativo"});
            this.cbFiltroEmprestimo.ItemsFont = new System.Drawing.Font("Segoe UI", 14.25F);
            this.cbFiltroEmprestimo.Location = new System.Drawing.Point(859, 150);
            this.cbFiltroEmprestimo.Name = "cbFiltroEmprestimo";
            this.cbFiltroEmprestimo.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14F);
            this.cbFiltroEmprestimo.PlaceholderMargin = 10;
            this.cbFiltroEmprestimo.PlaceholderText = "Selecione uma situação...";
            this.cbFiltroEmprestimo.Size = new System.Drawing.Size(248, 34);
            this.cbFiltroEmprestimo.TabIndex = 125;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label6.Location = new System.Drawing.Point(854, 119);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 25);
            this.label6.TabIndex = 124;
            this.label6.Text = "Situação:";
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
            this.txtLivro.Location = new System.Drawing.Point(324, 145);
            this.txtLivro.Name = "txtLivro";
            this.txtLivro.Padding = new System.Windows.Forms.Padding(7);
            this.txtLivro.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtLivro.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.txtLivro.PlaceholderMarginLeft = 12;
            this.txtLivro.PlaceholderText = "Digite o nome do livro...";
            this.txtLivro.SelectedText = "";
            this.txtLivro.SelectionLength = 0;
            this.txtLivro.SelectionStart = 0;
            this.txtLivro.Size = new System.Drawing.Size(270, 40);
            this.txtLivro.TabIndex = 2;
            this.txtLivro.TextColor = System.Drawing.Color.Black;
            this.txtLivro.UseSystemPasswordChar = false;
            // 
            // DevoluçãoForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1280, 845);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblDadosLivro);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevoluçãoForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.DevoluçãoForm_Activated);
            this.Load += new System.EventHandler(this.DevoluçãoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmprestimos)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConfirmarDevolucao;
        private System.Windows.Forms.Button btnProrrogar;
        public System.Windows.Forms.Label label4;
        private RoundedTextBox txtLivro;
        public System.Windows.Forms.Label lblNome;
        public System.Windows.Forms.Label Titulo;
        private System.Windows.Forms.DataGridView dgvEmprestimos;
        private System.Windows.Forms.Label lblDadosLivro;
        private System.Windows.Forms.Button btnBuscarEmprestimo;
        private System.Windows.Forms.Panel panel1;
        private RoundedComboBox cbFiltroEmprestimo;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label label1;
        private RoundedTextBox txtUsuario;
        private System.Windows.Forms.ListBox lstSugestoesUsuario;
        private RoundedTextBox mtxCodigoBarras;
    }
}