using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


    public class RoundedMaskedTextBox : UserControl
    {
        private readonly MaskedTextBox maskedTextBox = new MaskedTextBox();

        private bool isMouseOver = false;
        private bool isFocused = false;

        private Color borderColor = Color.Gray;
        private int borderRadius = 10;
        private Color maskTextColor = Color.Black;
        private int leftMargin = 0;

        // ==== NOVAS PROPRIEDADES PARA PERSONALIZAÇÃO ====

        [Category("Aparência")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        [Category("Aparência")]
        public Color BorderFocusColor { get; set; } = Color.Blue;

        [Category("Aparência")]
        public Color HoverBackColor { get; set; } = Color.LightGray;

        [Category("Aparência")]
        public Color HoverBorderColor { get; set; } = Color.DarkGray;

        [Category("Aparência")]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = value; Invalidate(); }
        }

        [Category("Aparência")]
        public Color MaskTextColor
        {
            get => maskTextColor;
            set { maskTextColor = value; maskedTextBox.ForeColor = value; Invalidate(); }
        }

        [Category("Aparência")]
        public int LeftMargin
        {
            get => leftMargin;
            set
            {
                leftMargin = value;
                maskedTextBox.Location = new Point(Padding.Left + leftMargin, Padding.Top);
                Invalidate();
            }
        }

        [Category("Aparência")]
        public override string Text
        {
            get => maskedTextBox.Text;
            set => maskedTextBox.Text = value;
        }

        [Category("Aparência")]
        public string Mask
        {
            get => maskedTextBox.Mask;
            set => maskedTextBox.Mask = value;
        }

        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                maskedTextBox.BackColor = value;
                Invalidate();
            }
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                maskedTextBox.ForeColor = value;
            }
        }

        // ==== CONSTRUTOR ====

        public RoundedMaskedTextBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                     ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            this.Padding = new Padding(5);
            maskedTextBox.BorderStyle = BorderStyle.None;
            maskedTextBox.Dock = DockStyle.Fill;
            maskedTextBox.BackColor = this.BackColor;
            maskedTextBox.ForeColor = this.ForeColor;

            Controls.Add(maskedTextBox);

            Size = new Size(200, 30);

            // === EVENTOS PARA REDESENHO ===

            maskedTextBox.GotFocus += (s, e) => { isFocused = true; Invalidate(); };
            maskedTextBox.LostFocus += (s, e) => { isFocused = false; Invalidate(); };

            maskedTextBox.MouseEnter += (s, e) => { isMouseOver = true; Invalidate(); };
            maskedTextBox.MouseLeave += (s, e) => { isMouseOver = false; Invalidate(); };

            this.MouseEnter += (s, e) => { isMouseOver = true; Invalidate(); };
            this.MouseLeave += (s, e) => { isMouseOver = false; Invalidate(); };

            // Encaminha eventos de tecla do inner MaskedTextBox para o UserControl
            // Isso permite que handlers registrados em mtxCodigoBarras.KeyDown / KeyPress / KeyUp
            // sejam chamados (por exemplo, para detectar Enter de scanners).
            maskedTextBox.KeyDown += (s, e) =>
            {
                try { OnKeyDown(e); } catch { /* não propagar exceções */ }
            };
            maskedTextBox.KeyPress += (s, e) =>
            {
                try { OnKeyPress(e); } catch { /* não propagar exceções */ }
            };
            maskedTextBox.KeyUp += (s, e) =>
            {
                try { OnKeyUp(e); } catch { /* não propagar exceções */ }
            };
        }

        // ==== DESENHO PERSONALIZADO ====

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = ClientRectangle;
            rect.Width -= 1;
            rect.Height -= 1;

            Color back = isMouseOver ? HoverBackColor : BackColor;
            Color border = isFocused ? BorderFocusColor : (isMouseOver ? HoverBorderColor : BorderColor);

            using (GraphicsPath path = GetRoundedRectanglePath(rect, borderRadius))
            using (SolidBrush brush = new SolidBrush(back))
            using (Pen pen = new Pen(border, 1.5f))
            {
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawPath(pen, path);
            }

            // Atualizar o fundo da MaskedTextBox também
            maskedTextBox.BackColor = back;
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int r = radius * 2;

            if (radius <= 0)
            {
                path.AddRectangle(rect);
            }
            else
            {
                path.StartFigure();
                path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                path.CloseFigure();
            }

            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
    }

