using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ToggleSwitch
{
    public partial class AnimatedToggle : UserControl
    {
        private bool isChecked = false;
        private Timer animationTimer;
        private int circleX;
        private int animationTarget;

        [Category("Appearance")]
        public bool Checked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                animationTarget = isChecked ? this.Width - this.Height + 2 : 2;
                animationTimer.Start();
                Invalidate();
                OnCheckedChanged(EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        public Color OnBackColor { get; set; } = Color.MediumSeaGreen;

        [Category("Appearance")]
        public Color OffBackColor { get; set; } = Color.LightGray;

        [Category("Appearance")]
        public Color ToggleColor { get; set; } = Color.White;

        public event EventHandler CheckedChanged;

        public AnimatedToggle()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Size = new Size(60, 30); // Tamanho redondo
            circleX = 2;

            animationTimer = new Timer();
            animationTimer.Interval = 10;
            animationTimer.Tick += AnimateToggle;

            this.Cursor = Cursors.Hand;
            this.Click += (s, e) => Checked = !Checked;
        }

        private void AnimateToggle(object sender, EventArgs e)
        {
            if (circleX < animationTarget)
            {
                circleX += 2;
                if (circleX >= animationTarget)
                {
                    circleX = animationTarget;
                    animationTimer.Stop();
                }
            }
            else if (circleX > animationTarget)
            {
                circleX -= 2;
                if (circleX <= animationTarget)
                {
                    circleX = animationTarget;
                    animationTimer.Stop();
                }
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int radius = this.Height;
            Rectangle backgroundRect = new Rectangle(0, 0, this.Width, this.Height);

            // Desenhar fundo arredondado (pílula)
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, radius, radius, 90, 180);
                path.AddArc(this.Width - radius, 0, radius, radius, 270, 180);
                path.CloseAllFigures();

                using (SolidBrush backgroundBrush = new SolidBrush(isChecked ? OnBackColor : OffBackColor))
                {
                    g.FillPath(backgroundBrush, path);
                }
            }

            // Desenhar o círculo deslizante
            int circleSize = this.Height - 4;
            using (SolidBrush toggleBrush = new SolidBrush(ToggleColor))
            {
                g.FillEllipse(toggleBrush, circleX, 2, circleSize, circleSize);
            }

            base.OnPaint(e);
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            CheckedChanged?.Invoke(this, e);
        }
    }
}
