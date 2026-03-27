using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp.ElementosPersonalizados
{
    public class GradientPanel : Panel
    {
        //Cria uma  Propriedades para defir a cor para a parte superior e inferior do gradiente gradiente
        public Color Gradientetop { get; set; }
        public Color Gradientebottom { get; set; }

        //Criar construtor do gradiente painel class 

        public GradientPanel(){
            this.Resize += GradientPanel_Resize;

        }

        private void GradientPanel_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush linear = new LinearGradientBrush(
                this.ClientRectangle,
                this.Gradientetop,
                this.Gradientebottom,
                90F
                );

            Graphics g = e.Graphics;

            g.FillRectangle(linear, this.ClientRectangle);



            base.OnPaint(e);
        }
    }
}
