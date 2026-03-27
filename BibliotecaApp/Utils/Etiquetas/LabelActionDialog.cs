using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaApp.Utils.Etiquetas
{
    public enum LabelActionChoice
    {
        None = 0,
        Pdf = 1,
        Print = 2
    }

    internal sealed partial class LabelActionDialog : Form
    {
        private Panel panelHeader;
        private Label lblTitulo;
        private Label lblMensagem;
        private Button btnPdf;
        private Button btnPrint;
        private Button btnCancelar;

        // UI de quantidade
        private Label lblQuantidade;
        private TextBox numQuantidade;
        private Button btnMais;
        private Button btnMenos;

        // Propriedades de quantidade
        public int MaxQuantity { get; set; } = 1;
        public int Quantity { get; private set; } = 1;

        // NOVO: controlar visibilidade do seletor de quantidade
        public bool AllowQuantitySelection { get; set; } = true;

        public LabelActionChoice Choice { get; private set; } = LabelActionChoice.None;

        public LabelActionDialog(string tituloLivro = null)
        {
            InitializeComponent();

            string tituloSafe = string.IsNullOrWhiteSpace(tituloLivro) ? "o livro" : $"\"{tituloLivro}\"";
            lblMensagem.Text = $"Como deseja gerar as etiquetas para {tituloSafe}?";

            // Default
            SetQuantity(1);
            ApplyQuantityVisibility();
        }

        // Overload com MaxQuantity
        public LabelActionDialog(string tituloLivro, int maxQuantity) : this(tituloLivro)
        {
            MaxQuantity = Math.Max(1, maxQuantity);
            SetQuantity(1);
        }

        // NOVO: configurar quantidade (fixa ou editável) a partir do chamador
        public void ConfigureQuantity(bool allowSelection, int fixedQuantity)
        {
            AllowQuantitySelection = allowSelection;
            MaxQuantity = Math.Max(1, fixedQuantity);
            SetQuantity(Math.Max(1, fixedQuantity));
            ApplyQuantityVisibility();
        }

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.btnPdf = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.numQuantidade = new System.Windows.Forms.TextBox();
            this.btnMais = new System.Windows.Forms.Button();
            this.btnMenos = new System.Windows.Forms.Button();
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
            this.panelHeader.Size = new System.Drawing.Size(460, 50);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(460, 50);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gerar Etiquetas";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMensagem
            // 
            this.lblMensagem.Font = new System.Drawing.Font("Segoe UI Semibold", 10.25F, System.Drawing.FontStyle.Bold);
            this.lblMensagem.Location = new System.Drawing.Point(20, 60);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(420, 40);
            this.lblMensagem.TabIndex = 1;
            this.lblMensagem.Text = "Como deseja gerar as etiquetas?";
            this.lblMensagem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPdf
            // 
            this.btnPdf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(61)))), ((int)(((byte)(88)))));
            this.btnPdf.FlatAppearance.BorderSize = 0;
            this.btnPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPdf.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnPdf.ForeColor = System.Drawing.Color.White;
            this.btnPdf.Location = new System.Drawing.Point(296, 150);
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.Size = new System.Drawing.Size(144, 35);
            this.btnPdf.TabIndex = 10;
            this.btnPdf.Text = "Salvar em PDF";
            this.btnPdf.UseVisualStyleBackColor = false;
            this.btnPdf.Click += new System.EventHandler(this.btnPdf_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(74)))), ((int)(((byte)(158)))));
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(172, 150);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(110, 35);
            this.btnPrint.TabIndex = 9;
            this.btnPrint.Text = "Imprimir";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Gray;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(40, 150);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(110, 35);
            this.btnCancelar.TabIndex = 8;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblQuantidade
            // 
            this.lblQuantidade.Font = new System.Drawing.Font("Segoe UI Semibold", 10.25F, System.Drawing.FontStyle.Bold);
            this.lblQuantidade.Location = new System.Drawing.Point(20, 105);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Size = new System.Drawing.Size(120, 25);
            this.lblQuantidade.TabIndex = 4;
            this.lblQuantidade.Text = "Quantidade:";
            this.lblQuantidade.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numQuantidade
            // 
            this.numQuantidade.BackColor = System.Drawing.Color.WhiteSmoke;
            this.numQuantidade.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.numQuantidade.Location = new System.Drawing.Point(125, 102);
            this.numQuantidade.MaxLength = 4;
            this.numQuantidade.Name = "numQuantidade";
            this.numQuantidade.Size = new System.Drawing.Size(56, 29);
            this.numQuantidade.TabIndex = 5;
            this.numQuantidade.Text = "1";
            this.numQuantidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numQuantidade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumQuantidade_KeyPress);
            this.numQuantidade.Leave += new System.EventHandler(this.NumQuantidade_Leave);
            // 
            // btnMais
            // 
            this.btnMais.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnMais.Location = new System.Drawing.Point(184, 95);
            this.btnMais.Margin = new System.Windows.Forms.Padding(0);
            this.btnMais.Name = "btnMais";
            this.btnMais.Size = new System.Drawing.Size(22, 22);
            this.btnMais.TabIndex = 6;
            this.btnMais.Text = "▲";
            this.btnMais.UseVisualStyleBackColor = true;
            this.btnMais.Click += new System.EventHandler(this.BtnMais_Click);
            // 
            // btnMenos
            // 
            this.btnMenos.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnMenos.Location = new System.Drawing.Point(184, 117);
            this.btnMenos.Margin = new System.Windows.Forms.Padding(0);
            this.btnMenos.Name = "btnMenos";
            this.btnMenos.Size = new System.Drawing.Size(22, 22);
            this.btnMenos.TabIndex = 7;
            this.btnMenos.Text = "▼";
            this.btnMenos.UseVisualStyleBackColor = true;
            this.btnMenos.Click += new System.EventHandler(this.BtnMenos_Click);
            // 
            // LabelActionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(460, 205);
            this.Controls.Add(this.btnMenos);
            this.Controls.Add(this.btnMais);
            this.Controls.Add(this.numQuantidade);
            this.Controls.Add(this.lblQuantidade);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnPdf);
            this.Controls.Add(this.lblMensagem);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LabelActionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gerador";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LabelActionDialog_KeyDown);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ApplyQuantityVisibility()
        {
            bool vis = AllowQuantitySelection;
            if (lblQuantidade != null) lblQuantidade.Visible = vis;
            if (numQuantidade != null) numQuantidade.Visible = vis;
            if (btnMais != null) btnMais.Visible = vis;
            if (btnMenos != null) btnMenos.Visible = vis;
        }

        private void SetQuantity(int val)
        {
            if (val < 1) val = 1;
            if (val > MaxQuantity) val = MaxQuantity;
            Quantity = val;
            if (numQuantidade != null) numQuantidade.Text = Quantity.ToString();
        }

        private int ParseQuantity() => int.TryParse(numQuantidade?.Text ?? "1", out var v) ? v : 1;

        private void NumQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void NumQuantidade_Leave(object sender, EventArgs e)
        {
            var v = ParseQuantity();
            SetQuantity(v);
        }

        private void BtnMais_Click(object sender, EventArgs e)
        {
            SetQuantity(ParseQuantity() + 1);
        }

        private void BtnMenos_Click(object sender, EventArgs e)
        {
            SetQuantity(ParseQuantity() - 1);
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            SetQuantity(ParseQuantity());
            Choice = LabelActionChoice.Pdf;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SetQuantity(ParseQuantity());
            Choice = LabelActionChoice.Print;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Choice = LabelActionChoice.None;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void LabelActionDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Choice = LabelActionChoice.None;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}