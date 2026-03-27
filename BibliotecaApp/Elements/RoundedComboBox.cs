using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;



    public class RoundedComboBox : ComboBox
    {
        private string _placeholder = "Selecione...";
        private Font _placeholderFont = new Font("Segoe UI", 12F, FontStyle.Italic);
        private int _borderRadius = 8;
        private int _borderThickness = 2;
        private Color _borderColor = Color.Black;
        private int _placeholderMargin = 10;
        private Font _itemsFont = new Font("Segoe UI", 10F);

        [Category("Aparência")]
        public string PlaceholderText
        {
            get => _placeholder;
            set { _placeholder = value; Invalidate(); }
        }

        [Category("Aparência")]
        public Font PlaceholderFont
        {
            get => _placeholderFont;
            set { _placeholderFont = value; Invalidate(); }
        }

        [Category("Aparência")]
        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        [Category("Aparência")]
        public int BorderThickness
        {
            get => _borderThickness;
            set { _borderThickness = value; Invalidate(); }
        }

        [Category("Aparência")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Aparência")]
        public int PlaceholderMargin
        {
            get => _placeholderMargin;
            set { _placeholderMargin = value; Invalidate(); }
        }

        [Category("Aparência")]
        public Font ItemsFont
        {
            get => _itemsFont;
            set { _itemsFont = value; Invalidate(); }
        }

        public RoundedComboBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.FlatStyle = FlatStyle.Flat;
            this.Font = new Font("Segoe UI", 10F);
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using (GraphicsPath path = GetRoundedPath(rect, BorderRadius))
            using (Pen pen = new Pen(BorderColor, BorderThickness))
            {
                e.Graphics.DrawPath(pen, path);
            }

            // Texto do item selecionado ou placeholder
            string textToDraw = this.SelectedIndex >= 0 ? this.GetItemText(this.SelectedItem) : PlaceholderText;
            Font textFont = this.SelectedIndex >= 0 ? this.ItemsFont : this.PlaceholderFont;
            Color textColor = this.SelectedIndex >= 0 ? this.ForeColor : Color.Gray;

            using (Brush textBrush = new SolidBrush(textColor))
            {
                Rectangle textRect = new Rectangle(PlaceholderMargin, 0, this.Width - PlaceholderMargin - 20, this.Height);
                StringFormat format = new StringFormat { LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(textToDraw, textFont, textBrush, textRect, format);
            }

            // Desenha a seta nativa
            ComboBoxRenderer.DrawDropDownButton(
                e.Graphics,
                new Rectangle(this.Width - 20, 0, 20, this.Height),
                System.Windows.Forms.VisualStyles.ComboBoxState.Normal
            );
        }

        private GraphicsPath GetRoundedPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0 && e.Index < Items.Count)
            {
                string itemText = this.GetItemText(Items[e.Index]);
                using (Brush textBrush = new SolidBrush(ForeColor))
                {
                    e.Graphics.DrawString(itemText, ItemsFont, textBrush, e.Bounds);
                }
            }

            e.DrawFocusRectangle();
        }
    }

