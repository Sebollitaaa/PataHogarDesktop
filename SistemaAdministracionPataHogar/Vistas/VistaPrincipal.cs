using SistemaAdministracionPataHogar.Estilos;

namespace SistemaAdministracionPataHogar.Vistas
{
    // VISTA de la ventana principal: el menu lateral y el area donde se
    // muestra cada seccion. No decide que hacer cuando se hace clic en algo:
    // avisa con un evento y ControladorPrincipal resuelve.
    public partial class VistaPrincipal : Form, IVistaPrincipal
    {
        // Avisos hacia el controlador (ver IVistaPrincipal).
        public event EventHandler VistaMostrada;
        public event EventHandler AbrirSolicitudesSolicitado;
        public event EventHandler CerrarSesionSolicitado;

        public VistaPrincipal()
        {
            InitializeComponent();

            panelEncabezado.Paint += panelEncabezado_Paint;
            btnSolicitudes.Click += btnSolicitudes_Click;
            lnkCerrarSesion.LinkClicked += lnkCerrarSesion_LinkClicked;
            Shown += VistaPrincipal_Shown;
        }

        // ---- Eventos de los controles: solo reenvian el aviso ----

        private void VistaPrincipal_Shown(object sender, EventArgs e)
        {
            if (VistaMostrada != null)
            {
                VistaMostrada(this, EventArgs.Empty);
            }
        }

        private void btnSolicitudes_Click(object sender, EventArgs e)
        {
            if (AbrirSolicitudesSolicitado != null)
            {
                AbrirSolicitudesSolicitado(this, EventArgs.Empty);
            }
        }

        private void lnkCerrarSesion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (CerrarSesionSolicitado != null)
            {
                CerrarSesionSolicitado(this, EventArgs.Empty);
            }
        }

        // Dibuja una linea gris fina debajo del encabezado, para separarlo
        // visualmente del contenido. Es puramente visual.
        private void panelEncabezado_Paint(object sender, PaintEventArgs e)
        {
            Pen lapiz = new Pen(TemaApp.Borde);
            e.Graphics.DrawLine(lapiz, 0, panelEncabezado.Height - 1, panelEncabezado.Width, panelEncabezado.Height - 1);
        }

        // ---- Ordenes que recibe del controlador ----

        public void MostrarBienvenida()
        {
            lblTituloSeccion.Text = "Bienvenido, admin";
            lblSubtituloSeccion.Text = "Seleccioná una opción del menú para comenzar.";

            panelContenido.Controls.Clear();

            Label mensaje = new Label();
            mensaje.Text = "👈  Elegí una sección en el menú de la izquierda.";
            mensaje.Font = TemaApp.FuenteSubtitulo;
            mensaje.ForeColor = TemaApp.TextoSecundario;
            mensaje.Dock = DockStyle.Fill;
            mensaje.TextAlign = ContentAlignment.MiddleCenter;

            panelContenido.Controls.Add(mensaje);
        }

        // Reemplaza lo que haya en el area de contenido por otra pantalla
        // (un UserControl) y actualiza el titulo del encabezado.
        public void MostrarContenido(Control contenido, string titulo, string subtitulo)
        {
            lblTituloSeccion.Text = titulo;
            lblSubtituloSeccion.Text = subtitulo;

            panelContenido.Controls.Clear();

            contenido.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(contenido);
        }

        public void ResaltarSeccionSolicitudes()
        {
            btnSolicitudes.Seleccionado = true;
        }

        public void MostrarEstadoBaseDeDatos(bool conectado, string mensaje)
        {
            if (conectado)
            {
                lblEstadoBaseDeDatos.Text = "🟢  " + mensaje;
            }
            else
            {
                lblEstadoBaseDeDatos.Text = "🔴  Sin conexión a la base de datos.";
            }
        }

        public bool ConfirmarCerrarSesion()
        {
            DialogResult resultado = MessageBox.Show(this, "¿Querés cerrar la sesión actual?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return resultado == DialogResult.Yes;
        }

        public void CerrarVentana()
        {
            Close();
        }
    }
}
