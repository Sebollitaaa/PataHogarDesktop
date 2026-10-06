using SistemaAdministracionPataHogar.Modelos;
using SistemaAdministracionPataHogar.Estilos;

namespace SistemaAdministracionPataHogar.Vistas
{
    // Sub-vista: la "tarjetita" que representa UNA solicitud dentro de la
    // lista. Es un componente visual simple: recibe una solicitud, la dibuja y
    // avisa cuando le hacen clic. No tiene controlador propio; el que escucha
    // su aviso es VistaSolicitudesVerificacion.
    public partial class VistaTarjetaSolicitud : UserControl
    {
        // La solicitud que esta tarjeta representa.
        public SolicitudVerificacion Solicitud { get; private set; }

        // Evento propio: avisa a quien este escuchando que el usuario hizo
        // clic en esta tarjeta. Es la misma idea que el evento Click de un
        // boton, pero lo definimos nosotros para esta tarjeta.
        public event EventHandler Seleccionada;

        public VistaTarjetaSolicitud()
        {
            InitializeComponent();

            // La tarjeta tiene varias etiquetas superpuestas (nombre, ciudad,
            // fecha, etc). Para que hacer clic en CUALQUIERA de ellas
            // funcione igual que hacer clic en la tarjeta, conectamos el
            // mismo metodo al evento Click de todos los controles.
            pnlTarjeta.Click += ControlDeLaTarjeta_Click;
            lblNombre.Click += ControlDeLaTarjeta_Click;
            lblTipoCiudad.Click += ControlDeLaTarjeta_Click;
            lblFecha.Click += ControlDeLaTarjeta_Click;
            pnlEstado.Click += ControlDeLaTarjeta_Click;
            lblEstado.Click += ControlDeLaTarjeta_Click;
        }

        private void ControlDeLaTarjeta_Click(object sender, EventArgs e)
        {
            if (Seleccionada != null)
            {
                Seleccionada(this, EventArgs.Empty);
            }
        }

        public void CargarDatos(SolicitudVerificacion solicitud)
        {
            Solicitud = solicitud;

            lblNombre.Text = solicitud.NombreOrganizacion;
            lblTipoCiudad.Text = TipoOrganizacionAyudante.ObtenerIcono(solicitud.Tipo) + " " +
                                 TipoOrganizacionAyudante.ObtenerTexto(solicitud.Tipo) + " · " +
                                 solicitud.Ciudad + ", " + solicitud.Provincia;
            lblFecha.Text = FormatearFechaRelativa(solicitud.FechaSolicitud);

            lblEstado.Text = EstadoSolicitudAyudante.ObtenerTexto(solicitud.Estado);
            lblEstado.ForeColor = TemaApp.ObtenerColorDeEstado(solicitud.Estado);
            pnlEstado.BackColor = TemaApp.ObtenerColorDeFondoDeEstado(solicitud.Estado);

            MarcarSeleccion(false);
        }

        public void MarcarSeleccion(bool seleccionada)
        {
            if (seleccionada)
            {
                pnlTarjeta.ColorBorde = TemaApp.Principal;
                pnlTarjeta.GrosorBorde = 2;
                pnlTarjeta.BackColor = TemaApp.PrincipalClaro;
            }
            else
            {
                pnlTarjeta.ColorBorde = TemaApp.Borde;
                pnlTarjeta.GrosorBorde = 1;
                pnlTarjeta.BackColor = TemaApp.FondoTarjeta;
            }

            pnlTarjeta.Invalidate();
        }

        private static string FormatearFechaRelativa(DateTime fecha)
        {
            TimeSpan diferencia = DateTime.Now - fecha;

            if (diferencia.TotalHours < 24) return "Hoy";
            if (diferencia.TotalHours < 48) return "Ayer";

            int dias = (int)diferencia.TotalDays;
            return "Hace " + dias + " días · " + fecha.ToString("dd/MM/yyyy");
        }
    }
}
