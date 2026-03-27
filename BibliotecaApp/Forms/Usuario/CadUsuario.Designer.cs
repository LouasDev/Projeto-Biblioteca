 namespace BibliotecaApp.Froms.Usuario
{
    partial class CadUsuario
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
            this.lblAvisoEmail = new System.Windows.Forms.Label();
            this.DataNascAst = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.Titulo = new System.Windows.Forms.Label();
            this.TurmaAst = new System.Windows.Forms.Label();
            this.lblSenha = new System.Windows.Forms.Label();
            this.TelefoneAst = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblConfirmSenha = new System.Windows.Forms.Label();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.NomeAst = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCPF = new System.Windows.Forms.Label();
            this.lblTurma = new System.Windows.Forms.Label();
            this.chkMostrarSenha = new System.Windows.Forms.CheckBox();
            this.aviso = new System.Windows.Forms.Label();
            this.ConfirmSenhaAst = new System.Windows.Forms.Label();
            this.lblDataNasc = new System.Windows.Forms.Label();
            this.SenhaAst = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.btnCadastrar = new System.Windows.Forms.Button();
            this.EmailAst = new System.Windows.Forms.Label();
            this.lstSugestoesTurma = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtNome = new RoundedTextBox();
            this.txtEmail = new RoundedTextBox();
            this.txtTurma = new RoundedTextBox();
            this.cbUsuario = new RoundedComboBox();
            this.dtpDataNasc = new RoundedDatePicker();
            this.txtConfirmSenha = new RoundedTextBox();
            this.mtxCPF = new RoundedMaskedTextBox();
            this.mtxTelefone = new RoundedMaskedTextBox();
            this.txtSenha = new RoundedTextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAvisoEmail
            // 
            this.lblAvisoEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblAvisoEmail.AutoSize = true;
            this.lblAvisoEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvisoEmail.ForeColor = System.Drawing.Color.Gray;
            this.lblAvisoEmail.Location = new System.Drawing.Point(311, 350);
            this.lblAvisoEmail.Name = "lblAvisoEmail";
            this.lblAvisoEmail.Size = new System.Drawing.Size(301, 13);
            this.lblAvisoEmail.TabIndex = 100;
            this.lblAvisoEmail.Text = "*Caso não cadastrar um e-mail, não receberá notificações...";
            // 
            // DataNascAst
            // 
            this.DataNascAst.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DataNascAst.AutoSize = true;
            this.DataNascAst.BackColor = System.Drawing.Color.Transparent;
            this.DataNascAst.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DataNascAst.ForeColor = System.Drawing.Color.Red;
            this.DataNascAst.Location = new System.Drawing.Point(210, 179);
            this.DataNascAst.Name = "DataNascAst";
            this.DataNascAst.Size = new System.Drawing.Size(13, 17);
            this.DataNascAst.TabIndex = 80;
            this.DataNascAst.Text = "*";
            // 
            // lblNome
            // 
            this.lblNome.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNome.AutoSize = true;
            this.lblNome.BackColor = System.Drawing.Color.Transparent;
            this.lblNome.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNome.ForeColor = System.Drawing.Color.LightGray;
            this.lblNome.Location = new System.Drawing.Point(306, 194);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(159, 25);
            this.lblNome.TabIndex = 62;
            this.lblNome.Text = "Nome Completo:";
            // 
            // Titulo
            // 
            this.Titulo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Titulo.AutoSize = true;
            this.Titulo.Font = new System.Drawing.Font("Segoe UI", 25.25F, System.Drawing.FontStyle.Bold);
            this.Titulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.Titulo.Location = new System.Drawing.Point(425, 9);
            this.Titulo.Name = "Titulo";
            this.Titulo.Size = new System.Drawing.Size(431, 46);
            this.Titulo.TabIndex = 60;
            this.Titulo.Text = "CADASTRO DE USUARIOS";
            // 
            // TurmaAst
            // 
            this.TurmaAst.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TurmaAst.AutoSize = true;
            this.TurmaAst.BackColor = System.Drawing.Color.Transparent;
            this.TurmaAst.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TurmaAst.ForeColor = System.Drawing.Color.Red;
            this.TurmaAst.Location = new System.Drawing.Point(377, 370);
            this.TurmaAst.Name = "TurmaAst";
            this.TurmaAst.Size = new System.Drawing.Size(13, 17);
            this.TurmaAst.TabIndex = 81;
            this.TurmaAst.Text = "*";
            // 
            // lblSenha
            // 
            this.lblSenha.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSenha.AutoSize = true;
            this.lblSenha.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSenha.ForeColor = System.Drawing.Color.LightGray;
            this.lblSenha.Location = new System.Drawing.Point(18, 262);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(69, 25);
            this.lblSenha.TabIndex = 75;
            this.lblSenha.Text = "Senha:";
            this.lblSenha.Visible = false;
            // 
            // TelefoneAst
            // 
            this.TelefoneAst.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TelefoneAst.AutoSize = true;
            this.TelefoneAst.BackColor = System.Drawing.Color.Transparent;
            this.TelefoneAst.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TelefoneAst.ForeColor = System.Drawing.Color.Red;
            this.TelefoneAst.Location = new System.Drawing.Point(105, 17);
            this.TelefoneAst.Name = "TelefoneAst";
            this.TelefoneAst.Size = new System.Drawing.Size(13, 17);
            this.TelefoneAst.TabIndex = 82;
            this.TelefoneAst.Text = "*";
            this.TelefoneAst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEmail
            // 
            this.lblEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.Color.LightGray;
            this.lblEmail.Location = new System.Drawing.Point(306, 279);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(64, 25);
            this.lblEmail.TabIndex = 65;
            this.lblEmail.Text = "Email:";
            // 
            // lblConfirmSenha
            // 
            this.lblConfirmSenha.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblConfirmSenha.AutoSize = true;
            this.lblConfirmSenha.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmSenha.ForeColor = System.Drawing.Color.LightGray;
            this.lblConfirmSenha.Location = new System.Drawing.Point(18, 367);
            this.lblConfirmSenha.Name = "lblConfirmSenha";
            this.lblConfirmSenha.Size = new System.Drawing.Size(159, 25);
            this.lblConfirmSenha.TabIndex = 76;
            this.lblConfirmSenha.Text = "Confirmar senha:";
            this.lblConfirmSenha.Visible = false;
            // 
            // lblTelefone
            // 
            this.lblTelefone.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefone.ForeColor = System.Drawing.Color.LightGray;
            this.lblTelefone.Location = new System.Drawing.Point(19, 17);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Size = new System.Drawing.Size(89, 25);
            this.lblTelefone.TabIndex = 74;
            this.lblTelefone.Text = "Telefone:";
            // 
            // NomeAst
            // 
            this.NomeAst.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.NomeAst.AutoSize = true;
            this.NomeAst.BackColor = System.Drawing.Color.Transparent;
            this.NomeAst.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NomeAst.ForeColor = System.Drawing.Color.Red;
            this.NomeAst.Location = new System.Drawing.Point(457, 194);
            this.NomeAst.Name = "NomeAst";
            this.NomeAst.Size = new System.Drawing.Size(13, 17);
            this.NomeAst.TabIndex = 77;
            this.NomeAst.Text = "*";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(512, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 40);
            this.label3.TabIndex = 86;
            this.label3.Text = "*";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.label2.Location = new System.Drawing.Point(306, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 25);
            this.label2.TabIndex = 83;
            this.label2.Text = "Tipo usuário:";
            // 
            // lblCPF
            // 
            this.lblCPF.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCPF.AutoSize = true;
            this.lblCPF.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPF.ForeColor = System.Drawing.Color.LightGray;
            this.lblCPF.Location = new System.Drawing.Point(18, 95);
            this.lblCPF.Name = "lblCPF";
            this.lblCPF.Size = new System.Drawing.Size(50, 25);
            this.lblCPF.TabIndex = 70;
            this.lblCPF.Text = "CPF:";
            // 
            // lblTurma
            // 
            this.lblTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTurma.AutoSize = true;
            this.lblTurma.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTurma.ForeColor = System.Drawing.Color.LightGray;
            this.lblTurma.Location = new System.Drawing.Point(309, 370);
            this.lblTurma.Name = "lblTurma";
            this.lblTurma.Size = new System.Drawing.Size(71, 25);
            this.lblTurma.TabIndex = 73;
            this.lblTurma.Text = "Turma:";
            // 
            // chkMostrarSenha
            // 
            this.chkMostrarSenha.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkMostrarSenha.AutoSize = true;
            this.chkMostrarSenha.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMostrarSenha.ForeColor = System.Drawing.Color.LightGray;
            this.chkMostrarSenha.Location = new System.Drawing.Point(26, 342);
            this.chkMostrarSenha.Name = "chkMostrarSenha";
            this.chkMostrarSenha.Size = new System.Drawing.Size(101, 19);
            this.chkMostrarSenha.TabIndex = 87;
            this.chkMostrarSenha.Text = "Mostrar senha";
            this.chkMostrarSenha.UseVisualStyleBackColor = true;
            this.chkMostrarSenha.Visible = false;
            this.chkMostrarSenha.CheckedChanged += new System.EventHandler(this.chkMostrarSenha_CheckedChanged);
            // 
            // aviso
            // 
            this.aviso.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.aviso.AutoSize = true;
            this.aviso.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aviso.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(54)))), ((int)(((byte)(77)))));
            this.aviso.Location = new System.Drawing.Point(530, 55);
            this.aviso.Name = "aviso";
            this.aviso.Size = new System.Drawing.Size(220, 25);
            this.aviso.TabIndex = 85;
            this.aviso.Text = "CAMPOS OBRIGATÓRIOS";
            // 
            // ConfirmSenhaAst
            // 
            this.ConfirmSenhaAst.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ConfirmSenhaAst.AutoSize = true;
            this.ConfirmSenhaAst.BackColor = System.Drawing.Color.Transparent;
            this.ConfirmSenhaAst.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConfirmSenhaAst.ForeColor = System.Drawing.Color.Red;
            this.ConfirmSenhaAst.Location = new System.Drawing.Point(174, 367);
            this.ConfirmSenhaAst.Name = "ConfirmSenhaAst";
            this.ConfirmSenhaAst.Size = new System.Drawing.Size(13, 17);
            this.ConfirmSenhaAst.TabIndex = 79;
            this.ConfirmSenhaAst.Text = "*";
            // 
            // lblDataNasc
            // 
            this.lblDataNasc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDataNasc.AutoSize = true;
            this.lblDataNasc.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataNasc.ForeColor = System.Drawing.Color.LightGray;
            this.lblDataNasc.Location = new System.Drawing.Point(21, 180);
            this.lblDataNasc.Name = "lblDataNasc";
            this.lblDataNasc.Size = new System.Drawing.Size(192, 25);
            this.lblDataNasc.TabIndex = 72;
            this.lblDataNasc.Text = "Data de Nascimento:";
            // 
            // SenhaAst
            // 
            this.SenhaAst.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SenhaAst.AutoSize = true;
            this.SenhaAst.BackColor = System.Drawing.Color.Transparent;
            this.SenhaAst.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SenhaAst.ForeColor = System.Drawing.Color.Red;
            this.SenhaAst.Location = new System.Drawing.Point(84, 262);
            this.SenhaAst.Name = "SenhaAst";
            this.SenhaAst.Size = new System.Drawing.Size(13, 17);
            this.SenhaAst.TabIndex = 78;
            this.SenhaAst.Text = "*";
            // 
            // lblUsuario
            // 
            this.lblUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.Color.Transparent;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.Red;
            this.lblUsuario.Location = new System.Drawing.Point(420, 115);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(13, 17);
            this.lblUsuario.TabIndex = 84;
            this.lblUsuario.Text = "*";
            // 
            // btnLimpar
            // 
            this.btnLimpar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnLimpar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnLimpar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnLimpar.ForeColor = System.Drawing.Color.White;
            this.btnLimpar.Location = new System.Drawing.Point(23, 304);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(149, 57);
            this.btnLimpar.TabIndex = 89;
            this.btnLimpar.Text = "LIMPAR";
            this.btnLimpar.UseVisualStyleBackColor = false;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnCadastrar
            // 
            this.btnCadastrar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCadastrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnCadastrar.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnCadastrar.ForeColor = System.Drawing.Color.White;
            this.btnCadastrar.Location = new System.Drawing.Point(491, 304);
            this.btnCadastrar.Name = "btnCadastrar";
            this.btnCadastrar.Size = new System.Drawing.Size(149, 57);
            this.btnCadastrar.TabIndex = 10;
            this.btnCadastrar.Text = "CADASTRAR";
            this.btnCadastrar.UseVisualStyleBackColor = false;
            this.btnCadastrar.Click += new System.EventHandler(this.btnCadastrar_Click);
            // 
            // EmailAst
            // 
            this.EmailAst.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.EmailAst.AutoSize = true;
            this.EmailAst.BackColor = System.Drawing.Color.Transparent;
            this.EmailAst.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmailAst.ForeColor = System.Drawing.Color.Red;
            this.EmailAst.Location = new System.Drawing.Point(362, 280);
            this.EmailAst.Name = "EmailAst";
            this.EmailAst.Size = new System.Drawing.Size(13, 17);
            this.EmailAst.TabIndex = 91;
            this.EmailAst.Text = "*";
            // 
            // lstSugestoesTurma
            // 
            this.lstSugestoesTurma.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lstSugestoesTurma.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lstSugestoesTurma.FormattingEnabled = true;
            this.lstSugestoesTurma.ItemHeight = 25;
            this.lstSugestoesTurma.Location = new System.Drawing.Point(310, 438);
            this.lstSugestoesTurma.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lstSugestoesTurma.Name = "lstSugestoesTurma";
            this.lstSugestoesTurma.ScrollAlwaysVisible = true;
            this.lstSugestoesTurma.Size = new System.Drawing.Size(618, 79);
            this.lstSugestoesTurma.TabIndex = 131;
            this.lstSugestoesTurma.Visible = false;
            this.lstSugestoesTurma.Click += new System.EventHandler(this.lstSugestoesTurma_Click);
            this.lstSugestoesTurma.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstSugestoesTurma_KeyDown);
            this.lstSugestoesTurma.Leave += new System.EventHandler(this.lstSugestoesTurma_Leave);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMargin = new System.Drawing.Size(1, 1);
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lstSugestoesTurma);
            this.panel1.Controls.Add(this.EmailAst);
            this.panel1.Controls.Add(this.lblUsuario);
            this.panel1.Controls.Add(this.txtNome);
            this.panel1.Controls.Add(this.aviso);
            this.panel1.Controls.Add(this.lblTurma);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.NomeAst);
            this.panel1.Controls.Add(this.lblEmail);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.txtTurma);
            this.panel1.Controls.Add(this.cbUsuario);
            this.panel1.Controls.Add(this.TurmaAst);
            this.panel1.Controls.Add(this.Titulo);
            this.panel1.Controls.Add(this.lblNome);
            this.panel1.Controls.Add(this.lblAvisoEmail);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1280, 980);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.Controls.Add(this.dtpDataNasc);
            this.panel2.Controls.Add(this.txtConfirmSenha);
            this.panel2.Controls.Add(this.DataNascAst);
            this.panel2.Controls.Add(this.btnLimpar);
            this.panel2.Controls.Add(this.btnCadastrar);
            this.panel2.Controls.Add(this.mtxCPF);
            this.panel2.Controls.Add(this.lblSenha);
            this.panel2.Controls.Add(this.mtxTelefone);
            this.panel2.Controls.Add(this.txtSenha);
            this.panel2.Controls.Add(this.lblConfirmSenha);
            this.panel2.Controls.Add(this.SenhaAst);
            this.panel2.Controls.Add(this.lblTelefone);
            this.panel2.Controls.Add(this.lblDataNasc);
            this.panel2.Controls.Add(this.lblCPF);
            this.panel2.Controls.Add(this.ConfirmSenhaAst);
            this.panel2.Controls.Add(this.chkMostrarSenha);
            this.panel2.Controls.Add(this.TelefoneAst);
            this.panel2.Location = new System.Drawing.Point(288, 430);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(657, 541);
            this.panel2.TabIndex = 132;
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
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtNome.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtNome.Location = new System.Drawing.Point(311, 222);
            this.txtNome.Name = "txtNome";
            this.txtNome.Padding = new System.Windows.Forms.Padding(7);
            this.txtNome.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtNome.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.PlaceholderMarginLeft = 12;
            this.txtNome.PlaceholderText = "Digite aqui o nome...";
            this.txtNome.SelectedText = "";
            this.txtNome.SelectionLength = 0;
            this.txtNome.SelectionStart = 0;
            this.txtNome.Size = new System.Drawing.Size(617, 40);
            this.txtNome.TabIndex = 2;
            this.txtNome.TextColor = System.Drawing.Color.Black;
            this.txtNome.UseSystemPasswordChar = false;
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
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtEmail.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtEmail.Location = new System.Drawing.Point(311, 307);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Padding = new System.Windows.Forms.Padding(7);
            this.txtEmail.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtEmail.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.PlaceholderMarginLeft = 12;
            this.txtEmail.PlaceholderText = "Digite aqui o email...";
            this.txtEmail.SelectedText = "";
            this.txtEmail.SelectionLength = 0;
            this.txtEmail.SelectionStart = 0;
            this.txtEmail.Size = new System.Drawing.Size(617, 40);
            this.txtEmail.TabIndex = 3;
            this.txtEmail.TextColor = System.Drawing.Color.Black;
            this.txtEmail.UseSystemPasswordChar = false;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            this.txtEmail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEmail_KeyPress);
            this.txtEmail.Leave += new System.EventHandler(this.txtEmail_Leave);
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
            this.txtTurma.Location = new System.Drawing.Point(311, 398);
            this.txtTurma.Name = "txtTurma";
            this.txtTurma.Padding = new System.Windows.Forms.Padding(7);
            this.txtTurma.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtTurma.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTurma.PlaceholderMarginLeft = 12;
            this.txtTurma.PlaceholderText = "Digite aqui a turma...";
            this.txtTurma.SelectedText = "";
            this.txtTurma.SelectionLength = 0;
            this.txtTurma.SelectionStart = 0;
            this.txtTurma.Size = new System.Drawing.Size(617, 40);
            this.txtTurma.TabIndex = 4;
            this.txtTurma.TextColor = System.Drawing.Color.Black;
            this.txtTurma.UseSystemPasswordChar = false;
            this.txtTurma.TextChanged += new System.EventHandler(this.txtTurma_TextChanged);
            this.txtTurma.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTurma_KeyDown);
            this.txtTurma.Leave += new System.EventHandler(this.txtTurma_Leave);
            // 
            // cbUsuario
            // 
            this.cbUsuario.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cbUsuario.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cbUsuario.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbUsuario.BorderRadius = 8;
            this.cbUsuario.BorderThickness = 1;
            this.cbUsuario.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbUsuario.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.cbUsuario.FormattingEnabled = true;
            this.cbUsuario.Items.AddRange(new object[] {
            "Aluno(a)",
            "Bibliotecário(a)",
            "Professor(a)",
            "Outros"});
            this.cbUsuario.ItemsFont = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUsuario.Location = new System.Drawing.Point(311, 143);
            this.cbUsuario.Name = "cbUsuario";
            this.cbUsuario.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbUsuario.PlaceholderMargin = 10;
            this.cbUsuario.PlaceholderText = "Selecione o tipo de usuario...";
            this.cbUsuario.Size = new System.Drawing.Size(329, 34);
            this.cbUsuario.TabIndex = 1;
            this.cbUsuario.SelectedIndexChanged += new System.EventHandler(this.cbUsuario_SelectedIndexChanged);
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
            this.dtpDataNasc.Location = new System.Drawing.Point(23, 208);
            this.dtpDataNasc.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpDataNasc.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpDataNasc.Name = "dtpDataNasc";
            this.dtpDataNasc.PlaceholderColor = System.Drawing.Color.Gray;
            this.dtpDataNasc.PlaceholderFont = new System.Drawing.Font("Segoe UI", 12.2F);
            this.dtpDataNasc.PlaceholderText = "";
            this.dtpDataNasc.SelectedDate = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            this.dtpDataNasc.Size = new System.Drawing.Size(245, 40);
            this.dtpDataNasc.TabIndex = 128;
            this.dtpDataNasc.TabStop = false;
            this.dtpDataNasc.Value = new System.DateTime(2025, 11, 1, 0, 0, 0, 0);
            // 
            // txtConfirmSenha
            // 
            this.txtConfirmSenha.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtConfirmSenha.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtConfirmSenha.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtConfirmSenha.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtConfirmSenha.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtConfirmSenha.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtConfirmSenha.BorderRadius = 10;
            this.txtConfirmSenha.BorderThickness = 1;
            this.txtConfirmSenha.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtConfirmSenha.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmSenha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtConfirmSenha.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtConfirmSenha.Location = new System.Drawing.Point(23, 395);
            this.txtConfirmSenha.Name = "txtConfirmSenha";
            this.txtConfirmSenha.Padding = new System.Windows.Forms.Padding(7);
            this.txtConfirmSenha.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtConfirmSenha.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmSenha.PlaceholderMarginLeft = 12;
            this.txtConfirmSenha.PlaceholderText = "Confirme a senha...";
            this.txtConfirmSenha.SelectedText = "";
            this.txtConfirmSenha.SelectionLength = 0;
            this.txtConfirmSenha.SelectionStart = 0;
            this.txtConfirmSenha.Size = new System.Drawing.Size(617, 40);
            this.txtConfirmSenha.TabIndex = 9;
            this.txtConfirmSenha.TextColor = System.Drawing.Color.Black;
            this.txtConfirmSenha.UseSystemPasswordChar = true;
            this.txtConfirmSenha.Visible = false;
            // 
            // mtxCPF
            // 
            this.mtxCPF.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.mtxCPF.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mtxCPF.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.mtxCPF.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.mtxCPF.BorderRadius = 10;
            this.mtxCPF.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mtxCPF.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxCPF.ForeColor = System.Drawing.Color.Gray;
            this.mtxCPF.HoverBackColor = System.Drawing.Color.LightGray;
            this.mtxCPF.HoverBorderColor = System.Drawing.Color.DarkGray;
            this.mtxCPF.LeftMargin = 0;
            this.mtxCPF.Location = new System.Drawing.Point(23, 123);
            this.mtxCPF.Mask = "000,000,000-00";
            this.mtxCPF.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxCPF.Name = "mtxCPF";
            this.mtxCPF.Padding = new System.Windows.Forms.Padding(15, 5, 5, 5);
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
            this.mtxTelefone.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxTelefone.ForeColor = System.Drawing.Color.Gray;
            this.mtxTelefone.HoverBackColor = System.Drawing.Color.LightGray;
            this.mtxTelefone.HoverBorderColor = System.Drawing.Color.DarkGray;
            this.mtxTelefone.LeftMargin = 0;
            this.mtxTelefone.Location = new System.Drawing.Point(23, 45);
            this.mtxTelefone.Mask = "(00)00000-0000";
            this.mtxTelefone.MaskTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.mtxTelefone.Name = "mtxTelefone";
            this.mtxTelefone.Padding = new System.Windows.Forms.Padding(13, 5, 5, 5);
            this.mtxTelefone.Size = new System.Drawing.Size(329, 40);
            this.mtxTelefone.TabIndex = 5;
            // 
            // txtSenha
            // 
            this.txtSenha.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtSenha.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtSenha.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtSenha.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtSenha.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.txtSenha.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.txtSenha.BorderRadius = 10;
            this.txtSenha.BorderThickness = 1;
            this.txtSenha.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSenha.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(42)))), ((int)(((byte)(60)))));
            this.txtSenha.HoverBackColor = System.Drawing.Color.LightGray;
            this.txtSenha.Location = new System.Drawing.Point(23, 290);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Padding = new System.Windows.Forms.Padding(7);
            this.txtSenha.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtSenha.PlaceholderFont = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenha.PlaceholderMarginLeft = 12;
            this.txtSenha.PlaceholderText = "Digite aqui uma senha...";
            this.txtSenha.SelectedText = "";
            this.txtSenha.SelectionLength = 0;
            this.txtSenha.SelectionStart = 0;
            this.txtSenha.Size = new System.Drawing.Size(617, 40);
            this.txtSenha.TabIndex = 8;
            this.txtSenha.TextColor = System.Drawing.Color.Black;
            this.txtSenha.UseSystemPasswordChar = true;
            this.txtSenha.Visible = false;
            // 
            // CadUsuario
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1280, 980);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CadUsuario";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InicioForm";
            this.Load += new System.EventHandler(this.CadUsuario_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAvisoEmail;
        private System.Windows.Forms.Label DataNascAst;
        private RoundedMaskedTextBox mtxCPF;
        public System.Windows.Forms.Label lblNome;
        public System.Windows.Forms.Label Titulo;
        private System.Windows.Forms.Label TurmaAst;
        public System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.Label TelefoneAst;
        private RoundedMaskedTextBox mtxTelefone;
        private RoundedTextBox txtSenha;
        private RoundedComboBox cbUsuario;
        private RoundedTextBox txtTurma;
        private RoundedTextBox txtEmail;
        public System.Windows.Forms.Label lblEmail;
        public System.Windows.Forms.Label lblConfirmSenha;
        public System.Windows.Forms.Label lblTelefone;
        private System.Windows.Forms.Label NomeAst;
        private System.Windows.Forms.Label label3;
        private RoundedTextBox txtConfirmSenha;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblCPF;
        public System.Windows.Forms.Label lblTurma;
        private System.Windows.Forms.CheckBox chkMostrarSenha;
        private System.Windows.Forms.Label aviso;
        private System.Windows.Forms.Label ConfirmSenhaAst;
        public System.Windows.Forms.Label lblDataNasc;
        private System.Windows.Forms.Label SenhaAst;
        private RoundedTextBox txtNome;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.Button btnCadastrar;
        private System.Windows.Forms.Label EmailAst;
        private System.Windows.Forms.ListBox lstSugestoesTurma;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private RoundedDatePicker dtpDataNasc;
    }
}