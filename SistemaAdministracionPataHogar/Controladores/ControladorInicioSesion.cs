using SistemaAdministracionPataHogar.Modelos;
using SistemaAdministracionPataHogar.Vistas;

namespace SistemaAdministracionPataHogar.Controladores
{
    // CONTROLADOR del inicio de sesión. Escucha lo que pasa en la vista, consulta al
    // modelo y le ordena a la vista que hacer.
    //
    // Flujo: la vista avisa "IngresarSolicitado" -> el controlador lee usuario y
    // contraseña de la vista -> le pregunta al modelo (ServicioAutenticacion)
    // si son validos -> le ordena a la vista cerrarse con exito o mostrar el error.
    public class ControladorInicioSesion
    {
        private IVistaInicioSesion _vista;

        // Recibe la vista por su INTERFAZ: no sabe (ni le importa) que por
        // detras es un Form de Windows Forms.
        public ControladorInicioSesion(IVistaInicioSesion vista)
        {
            _vista = vista;

            // Nos "suscribimos" al aviso de la vista: cuando la vista dispare
            // IngresarSolicitado, se ejecuta nuestro metodo AlSolicitarIngreso.
            _vista.IngresarSolicitado += AlSolicitarIngreso;
        }

        private void AlSolicitarIngreso(object sender, EventArgs e)
        {
            string usuario = _vista.Usuario.Trim();
            string contrasena = _vista.Contrasena;

            if (ServicioAutenticacion.CredencialesValidas(usuario, contrasena))
            {
                _vista.LimpiarError();
                _vista.CerrarConExito();
            }
            else
            {
                _vista.MostrarError("Usuario o contraseña incorrectos.");
            }
        }
    }
}
