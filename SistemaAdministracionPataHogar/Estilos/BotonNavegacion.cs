using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace SistemaAdministracionPataHogar.Estilos
{
    // Boton del menu lateral (la barra lateral). Es rectangular, con el texto
    // pegado a la izquierda, y cuando esta "Seleccionado" se pinta de otro
    // color y le agrega una franja blanca a la izquierda.
    public class BotonNavegacion : Button
    {
        private bool _seleccionado = false;
        private bool _mouseEncima = false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Seleccionado
        {
            get { return _seleccionado; }
            set
            {
                _seleccionado = value;
                Invalidate();
            }
        }

        public BotonNavegacion()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = TemaApp.BarraLateral;
            ForeColor = TemaApp.BarraLateralTexto;
            Font = TemaApp.FuenteNavegacion;
            TextAlign = ContentAlignment.MiddleLeft;
            Padding = new Padding(24, 0, 0, 0);
            Cursor = Cursors.Hand;
            Height = 46;

            MouseEnter += BotonNavegacion_MouseEnter;
            MouseLeave += BotonNavegacion_MouseLeave;
        }

        private void BotonNavegacion_MouseEnter(object sender, EventArgs e)
        {
            _mouseEncima = true;
            Invalidate();
        }

        private void BotonNavegacion_MouseLeave(object sender, EventArgs e)
        {
            _mouseEncima = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color colorDeFondo;
            if (_seleccionado)
            {
                colorDeFondo = TemaApp.BarraLateralSeleccionada;
            }
            else if (_mouseEncima)
            {
                colorDeFondo = TemaApp.BarraLateralResaltada;
            }
            else
            {
                colorDeFondo = TemaApp.BarraLateral;
            }

            SolidBrush pincelDeFondo = new SolidBrush(colorDeFondo);
            pevent.Graphics.FillRectangle(pincelDeFondo, ClientRectangle);

            if (_seleccionado)
            {
                SolidBrush pincelDeFranja = new SolidBrush(Color.White);
                pevent.Graphics.FillRectangle(pincelDeFranja, 0, 0, 4, Height);
            }

            Color colorDeTexto = _seleccionado ? Color.White : ForeColor;
            Rectangle areaDeTexto = new Rectangle(Padding.Left, 0, Width - Padding.Left, Height);

            TextRenderer.DrawText(pevent.Graphics, Text, Font, areaDeTexto, colorDeTexto,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }
    }
}
