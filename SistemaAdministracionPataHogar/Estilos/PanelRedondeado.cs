using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace SistemaAdministracionPataHogar.Estilos
{
    // Un Panel comun de Windows Forms es siempre un rectangulo con esquinas
    // rectas. Esta clase HEREDA de Panel y le agrega esquinas redondeadas,
    // dibujandolas nosotros mismos con Graphics (lo que se llama "dibujo GDI+").
    //
    // No hace falta entender cada linea de matematica de las esquinas: lo
    // importante es la idea general -> "armamos un contorno redondeado y
    // pintamos el panel con ese contorno en vez de con un rectangulo comun".
    public class PanelRedondeado : Panel
    {
        // Que tan redondeadas son las esquinas (en pixeles).
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RadioEsquinas { get; set; } = 10;

        // Color del borde del panel.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ColorBorde { get; set; } = TemaApp.Borde;

        // Grosor del borde. Si es 0, no se dibuja borde.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int GrosorBorde { get; set; } = 1;

        public PanelRedondeado()
        {
            // Estas lineas le piden a Windows que nos deje dibujar nosotros
            // el panel (en vez de dibujar el rectangulo gris de siempre).
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            BackColor = TemaApp.FondoTarjeta;
        }

        // Arma el contorno redondeado: un rectangulo al que le "cortamos"
        // las cuatro esquinas con un arco de circulo.
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
            contorno.AddArc(area.X, area.Y, diametro, diametro, 180, 90);                         // esquina superior izquierda
            contorno.AddArc(area.Right - diametro, area.Y, diametro, diametro, 270, 90);          // esquina superior derecha
            contorno.AddArc(area.Right - diametro, area.Bottom - diametro, diametro, diametro, 0, 90); // esquina inferior derecha
            contorno.AddArc(area.X, area.Bottom - diametro, diametro, diametro, 90, 90);          // esquina inferior izquierda
            contorno.CloseFigure();

            return contorno;
        }

        // Este metodo se ejecuta automaticamente cada vez que el panel se
        // tiene que redibujar (por ejemplo, al abrir la pantalla).
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle area = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath contorno = ArmarContornoRedondeado(area, RadioEsquinas);

            // Recorta el panel para que solo se vea lo que esta dentro del contorno redondeado.
            Region = new Region(contorno);

            SolidBrush pincelDeFondo = new SolidBrush(BackColor);
            e.Graphics.FillPath(pincelDeFondo, contorno);

            if (GrosorBorde > 0)
            {
                Pen lapizDeBorde = new Pen(ColorBorde, GrosorBorde);
                e.Graphics.DrawPath(lapizDeBorde, contorno);
            }
        }
    }
}
