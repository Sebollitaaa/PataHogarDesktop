using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace SistemaAdministracionPataHogar.Estilos
{
    // Un boton con esquinas redondeadas. Funciona igual que un Button comun
    // (tiene Text, Click, etc.) pero se dibuja distinto: en vez del boton
    // gris de siempre, dibujamos un rectangulo redondeado de color, y ese
    // color cambia cuando el mouse pasa por encima (efecto de resaltado).
    public class BotonRedondeado : Button
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RadioEsquinas { get; set; } = 8;

        // Color del boton en su estado normal.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorNormal { get; set; } = TemaApp.Principal;

        // Color del boton cuando el mouse esta encima.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorResaltado { get; set; } = TemaApp.PrincipalOscuro;

        // Guardamos si el mouse esta encima o no, para saber que color pintar.
        private bool _mouseEncima = false;

        public BotonRedondeado()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            ForeColor = Color.White;
            Font = TemaApp.FuenteCuerpoNegrita;
            Cursor = Cursors.Hand;
            Height = 38;

            MouseEnter += BotonRedondeado_MouseEnter;
            MouseLeave += BotonRedondeado_MouseLeave;
        }

        private void BotonRedondeado_MouseEnter(object sender, EventArgs e)
        {
            _mouseEncima = true;
            Invalidate(); // le pide al control que se vuelva a dibujar
        }

        private void BotonRedondeado_MouseLeave(object sender, EventArgs e)
        {
            _mouseEncima = false;
            Invalidate();
        }

        private GraphicsPath ArmarContornoRedondeado(Rectangle area, int radio)
        {
            GraphicsPath contorno = new GraphicsPath();

            if (radio <= 0)
            {
                contorno.AddRectangle(area);
                return contorno;
            }

            int diametro = radio * 2;

            contorno.StartFigure();
            contorno.AddArc(area.X, area.Y, diametro, diametro, 180, 90);
            contorno.AddArc(area.Right - diametro, area.Y, diametro, diametro, 270, 90);
            contorno.AddArc(area.Right - diametro, area.Bottom - diametro, diametro, diametro, 0, 90);
            contorno.AddArc(area.X, area.Bottom - diametro, diametro, diametro, 90, 90);
            contorno.CloseFigure();

            return contorno;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle area = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath contorno = ArmarContornoRedondeado(area, RadioEsquinas);
            Region = new Region(contorno);

            Color colorAPintar;
            if (!Enabled)
            {
                colorAPintar = Color.FromArgb(200, 200, 200);
            }
            else if (_mouseEncima)
            {
                colorAPintar = ColorResaltado;
            }
            else
            {
                colorAPintar = ColorNormal;
            }

            SolidBrush pincel = new SolidBrush(colorAPintar);
            pevent.Graphics.FillPath(pincel, contorno);

            TextRenderer.DrawText(pevent.Graphics, Text, Font, area, ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
