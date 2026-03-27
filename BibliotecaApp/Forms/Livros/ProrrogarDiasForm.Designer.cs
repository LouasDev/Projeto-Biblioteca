namespace BibliotecaApp.Forms.Livros
{
    partial class ProrrogarDiasForm
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblErro = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.btnMenos = new System.Windows.Forms.Button();
            this.numQuantidade = new RoundedTextBox();
            this.btnMais = new System.Windows.Forms.Button();
            this.lblPreviewData = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(384, 50);
            this.panelHeader.TabIndex = 65;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(384, 50);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Prorrogar Empréstimo";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblErro
            // 
            this.lblErro.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblErro.ForeColor = System.Drawing.Color.Red;
            this.lblErro.Location = new System.Drawing.Point(20, 148);
            this.lblErro.Name = "lblErro";
            this.lblErro.Size = new System.Drawing.Size(360, 15);
            this.lblErro.TabIndex = 69;
            this.lblErro.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblErro.Visible = false;
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Gray;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(23, 175);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 35);
            this.btnCancelar.TabIndex = 68;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnConfirmar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirmar.FlatAppearance.BorderSize = 0;
            this.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmar.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnConfirmar.ForeColor = System.Drawing.Color.White;
            this.btnConfirmar.Location = new System.Drawing.Point(237, 175);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(120, 35);
            this.btnConfirmar.TabIndex = 66;
            this.btnConfirmar.Text = "Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // lblMensagem
            // 
            this.lblMensagem.Font = new System.Drawing.Font("Segoe UI Semibold", 10.25F, System.Drawing.FontStyle.Bold);
            this.lblMensagem.Location = new System.Drawing.Point(20, 59);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(360, 40);
            this.lblMensagem.TabIndex = 67;
            this.lblMensagem.Text = "Quantos dias você quer prorrogar:";
            this.lblMensagem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnMenos
            // 
            this.btnMenos.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMenos.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnMenos.Location = new System.Drawing.Point(85, 119);
            this.btnMenos.Margin = new System.Windows.Forms.Padding(0);
            this.btnMenos.Name = "btnMenos";
            this.btnMenos.Size = new System.Drawing.Size(18, 21);
            this.btnMenos.TabIndex = 102;
            this.btnMenos.Text = "▼";
            this.btnMenos.UseVisualStyleBackColor = true;
            this.btnMenos.Click += new System.EventHandler(this.btnMenos_Click);
            // 
            // numQuantidade
            // 
            this.numQuantidade.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numQuantidade.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numQuantidade.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numQuantidade.BackColor = System.Drawing.Color.White;
            this.numQuantidade.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.numQuantidade.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.numQuantidade.BorderRadius = 10;
            this.numQuantidade.BorderThickness = 1;
            this.numQuantidade.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.numQuantidade.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numQuantidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.numQuantidade.HoverBackColor = System.Drawing.Color.LightGray;
            this.numQuantidade.Location = new System.Drawing.Point(28, 100);
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
            this.numQuantidade.TabIndex = 99;
            this.numQuantidade.TextColor = System.Drawing.Color.Black;
            this.numQuantidade.UseSystemPasswordChar = false;
            // 
            // btnMais
            // 
            this.btnMais.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMais.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnMais.Location = new System.Drawing.Point(85, 97);
            this.btnMais.Margin = new System.Windows.Forms.Padding(0);
            this.btnMais.Name = "btnMais";
            this.btnMais.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnMais.Size = new System.Drawing.Size(18, 21);
            this.btnMais.TabIndex = 101;
            this.btnMais.Text = "▲";
            this.btnMais.UseVisualStyleBackColor = true;
            this.btnMais.Click += new System.EventHandler(this.btnMais_Click);
            // 
            // lblPreviewData
            // 
            this.lblPreviewData.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreviewData.Location = new System.Drawing.Point(126, 111);
            this.lblPreviewData.Name = "lblPreviewData";
            this.lblPreviewData.Size = new System.Drawing.Size(228, 18);
            this.lblPreviewData.TabIndex = 103;
            this.lblPreviewData.Text = "Nova data de devolução:";
            // 
            // ProrrogarDiasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(384, 225);
            this.Controls.Add(this.lblPreviewData);
            this.Controls.Add(this.btnMenos);
            this.Controls.Add(this.numQuantidade);
            this.Controls.Add(this.btnMais);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.lblErro);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.lblMensagem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProrrogarDiasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BibliotecaApp";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ProrrogarForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblErro;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Label lblMensagem;
        private System.Windows.Forms.Button btnMenos;
        private System.Windows.Forms.Button btnMais;
        public RoundedTextBox numQuantidade;
        private System.Windows.Forms.Label lblPreviewData;
    }
}