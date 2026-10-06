namespace SistemaAdministracionPataHogar.Vistas
{
    // VISTA del inicio de sesión. Su unica responsabilidad es mostrar la pantalla y
    // avisar cuando el usuario quiere ingresar. NO decide si el usuario y la
    // contraseña son correctos: eso lo hace ControladorInicioSesion.
    public partial class VistaInicioSesion : Form, IVistaInicioSesion
    {
        // Aviso hacia el controlador (ver IVistaInicioSesion).
        public event EventHandler IngresarSolicitado;

        // Datos que el controlador puede leer.
        public string Usuario
        {
            get { return txtUsuario.Text; }
        }

        public string Contrasena
        {
            get { return txtContrasena.Text; }
        }

        public VistaInicioSesion()
        {
            InitializeComponent();

            // Con AcceptButton, apretar Enter en cualquier campo equivale a
            // hacer clic en "Ingresar".
            AcceptButton = btnIngresar;

            btnIngresar.Click += btnIngresar_Click;
        }

        // La vista no valida nada: solo avisa que el usuario quiere ingresar.
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (IngresarSolicitado != null)
            {
                IngresarSolicitado(this, EventArgs.Empty);
            }
        }

        // ---- Ordenes que recibe del controlador ----

        public void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            lblError.Visible = true;
        }

        public void LimpiarError()
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
        }

        // Cierra la ventana devolviendo DialogResult.OK, que es la señal que
        // Programa.cs espera para abrir la ventana principal.
        public void CerrarConExito()
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
