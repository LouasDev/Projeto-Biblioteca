namespace BibliotecaApp.Forms.Livros
{
    partial class FichaAlunoForm
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
            this.btnSair = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtpDataNasc = new RoundedDatePicker();
            this.mtxCPF = new RoundedMaskedTextBox();
            this.mtxTelefone = new RoundedMaskedTextBox();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.lblCPF = new System.Windows.Forms.Label();
            this.lblDataNasc = new System.Windows.Forms.Label();
            this.txtTurma = new RoundedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEmail = new RoundedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNomeAluno = new RoundedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSair);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.txtTurma);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtNomeAluno);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 845);
            this.panel1.TabIndex = 0;
            // 
            // btnSair
            // 
            this.btnSair.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSair.BackColor = System.Drawing.Color.DimGray;
            this.btnSair.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnSair.ForeColor = System.Drawing.Color.White;
            this.btnSair.Location = new System.Drawing.Point(771, 701);
            this.btnSair.Margin = new System.Windows.Forms.Padding(5);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(149, 57);
            this.btnSair.TabIndex = 101;
            this.btnSair.Text = "Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.dtpDataNasc);
            this.panel2.Controls.Add(this.mtxCPF);
            this.panel2.Controls.Add(this.mtxTelefone);
            this.panel2.Controls.Add(this.lblTelefone);
            this.panel2.Controls.Add(this.lblCPF);
            this.panel2.Controls.Add(this.lblDataNasc);
            this.panel2.Location = new System.Drawing.Point(291, 378);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(364, 251);
            this.panel2.TabIndex = 137;
            // 
            // dtpDataNasc
            // 
            this.dtpDataNasc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpDataNasc.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpDataNasc.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dtpDataNasc.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.dtpDataNasc.BorderRadius = 10;
            this.dtpDataNasc.BorderThickness = 1;
            this.dtpDataNasc.Enabled = false;
            this.dtpDataNasc.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.dtpDataNasc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataNasc.HoverBackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpDataNasc.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataNasc.IconHoverAreaColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dtpDataNasc.IconHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.dtpDataNasc.Location = new System.Drawing.Point(12, 203);
            this.dtpDataNasc.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDataNasc.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDataNasc.Name = "dtpDataNasc";
            this.dtpDataNasc.PlaceholderColor = System.Drawing.Color.Gray;
            this.dtpDataNasc.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.dtpDataNasc.PlaceholderText = "";
            this.dtpDataNasc.SelectedDate = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            this.dtpDataNasc.Size = new System.Drawing.Size(262, 40);
            this.dtpDataNasc.TabIndex = 129;
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
            this.mtxCPF.Location = new System.Drawing.Point(12, 121);
            this.mtxCPF.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mtxCPF.Mask = "000,000,000-00";
            this.mtxCPF.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxCPF.Name = "mtxCPF";
            this.mtxCPF.Padding = new System.Windows.Forms.Padding(18, 6, 6, 6);
            this.mtxCPF.Size = new System.Drawing.Size(262, 40);
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
            this.mtxTelefone.Location = new System.Drawing.Point(12, 38);
            this.mtxTelefone.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.mtxTelefone.Mask = "(00)00000-0000";
            this.mtxTelefone.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxTelefone.Name = "mtxTelefone";
            this.mtxTelefone.Padding = new System.Windows.Forms.Padding(15, 6, 6, 6);
            this.mtxTelefone.Size = new System.Drawing.Size(262, 40);
            this.mtxTelefone.TabIndex = 5;
            // 
            // lblTelefone
            // 
            this.lblTelefone.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.lblTelefone.Location = new System.Drawing.Point(7, 10);
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
            this.lblCPF.Location = new System.Drawing.Point(7, 93);
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
            this.lblDataNasc.Location = new System.Drawing.Point(7, 175);
            this.lblDataNasc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDataNasc.Name = "lblDataNasc";
            this.lblDataNasc.Size = new System.Drawing.Size(192, 25);
            this.lblDataNasc.TabIndex = 98;
            this.lblDataNasc.Text = "Data de Nascimento:";
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
            this.txtTurma.Location = new System.Drawing.Point(303, 333);
            this.txtTurma.Margin = new System.Windows.Forms.Padding(5);
            this.txtTurma.Name = "txtTurma";
            this.txtTurma.Padding = new System.Windows.Forms.Padding(9, 12, 9, 12);
            this.txtTurma.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtTurma.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTurma.PlaceholderMarginLeft = 12;
            this.txtTurma.PlaceholderText = "Indisponivel...";
            this.txtTurma.SelectedText = "";
            this.txtTurma.SelectionLength = 0;
            this.txtTurma.SelectionStart = 0;
            this.txtTurma.Size = new System.Drawing.Size(617, 40);
            this.txtTurma.TabIndex = 135;
            this.txtTurma.TextColor = System.Drawing.Color.Black;
            this.txtTurma.UseSystemPasswordChar = false;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label4.Location = new System.Drawing.Point(298, 303);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 25);
            this.label4.TabIndex = 136;
            this.label4.Text = "Turma:";
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
            this.txtEmail.Location = new System.Drawing.Point(303, 246);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(5);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Padding = new System.Windows.Forms.Padding(9, 12, 9, 12);
            this.txtEmail.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtEmail.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.PlaceholderMarginLeft = 12;
            this.txtEmail.PlaceholderText = "Indisponivel...";
            this.txtEmail.SelectedText = "";
            this.txtEmail.SelectionLength = 0;
            this.txtEmail.SelectionStart = 0;
            this.txtEmail.Size = new System.Drawing.Size(617, 40);
            this.txtEmail.TabIndex = 133;
            this.txtEmail.TextColor = System.Drawing.Color.Black;
            this.txtEmail.UseSystemPasswordChar = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label3.Location = new System.Drawing.Point(298, 216);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 25);
            this.label3.TabIndex = 134;
            this.label3.Text = "Email:";
            // 
            // txtNomeAluno
            // 
            this.txtNomeAluno.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtNomeAluno.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtNomeAluno.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtNomeAluno.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNomeAluno.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtNomeAluno.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtNomeAluno.BorderRadius = 10;
            this.txtNomeAluno.BorderThickness = 1;
            this.txtNomeAluno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNomeAluno.Enabled = false;
            this.txtNomeAluno.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeAluno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtNomeAluno.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtNomeAluno.Location = new System.Drawing.Point(303, 157);
            this.txtNomeAluno.Margin = new System.Windows.Forms.Padding(5);
            this.txtNomeAluno.Name = "txtNomeAluno";
            this.txtNomeAluno.Padding = new System.Windows.Forms.Padding(9, 12, 9, 12);
            this.txtNomeAluno.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNomeAluno.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeAluno.PlaceholderMarginLeft = 12;
            this.txtNomeAluno.PlaceholderText = "Indisponivel...";
            this.txtNomeAluno.SelectedText = "";
            this.txtNomeAluno.SelectionLength = 0;
            this.txtNomeAluno.SelectionStart = 0;
            this.txtNomeAluno.Size = new System.Drawing.Size(617, 40);
            this.txtNomeAluno.TabIndex = 131;
            this.txtNomeAluno.TextColor = System.Drawing.Color.Black;
            this.txtNomeAluno.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label1.Location = new System.Drawing.Point(298, 127);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 25);
            this.label1.TabIndex = 132;
            this.label1.Text = "Nome do Aluno:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.label2.Location = new System.Drawing.Point(487, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(307, 46);
            this.label2.TabIndex = 130;
            this.label2.Text = "FICHA DO ALUNO";
            // 
            // FichaAlunoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1280, 845);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FichaAlunoForm";
            this.Text = "FichaAluno";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label label2;
        private RoundedTextBox txtNomeAluno;
        private System.Windows.Forms.Label label1;
        private RoundedTextBox txtTurma;
        private System.Windows.Forms.Label label4;
        private RoundedTextBox txtEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private RoundedMaskedTextBox mtxCPF;
        private RoundedMaskedTextBox mtxTelefone;
        public System.Windows.Forms.Label lblTelefone;
        public System.Windows.Forms.Label lblCPF;
        public System.Windows.Forms.Label lblDataNasc;
        private System.Windows.Forms.Button btnSair;
        private RoundedDatePicker dtpDataNasc;
    }
}