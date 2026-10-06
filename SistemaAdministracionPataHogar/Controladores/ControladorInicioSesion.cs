using SistemaAdministracionPataHogar.Datos;
using SistemaAdministracionPataHogar.Modelos;
using SistemaAdministracionPataHogar.Vistas;

namespace SistemaAdministracionPataHogar.Controladores
{
    // CONTROLADOR del inicio de sesión. Escucha lo que pasa en la vista, consulta al
    // modelo y le ordena a la vista que hacer.
    //
    // Flujo: la vista avisa "IngresarSolicitado" -> el controlador lee usuario y
    // contraseña de la vista -> busca el usuario en MongoDB (RepositorioUsuarios) ->
    // le pregunta al modelo (ServicioAutenticacion) si la contraseña es valida -> le ordena
    // a la vista cerrarse con exito o mostrar el error.
    //
    // La busqueda en MongoDB la hace el CONTROLADOR (y no ServicioAutenticacion) porque las
    // carpetas Modelos no pueden depender de Datos.
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

            // Si falta algo no tiene sentido consultar la base (evita una espera innecesaria).
            if (usuario == string.Empty || contrasena == string.Empty)
            {
                _vista.MostrarError("Ingrese su usuario y su contraseña.");
                return;
            }

            UsuarioAdministrador usuarioEncontrado;
            try
            {
                usuarioEncontrado = RepositorioUsuarios.BuscarPorNombreUsuario(usuario);
            }
            catch (Exception)
            {
                _vista.MostrarError("No se pudo conectar con la base de usuarios. Revise internet y la IP permitida en Atlas.");
                return;
            }

            if (ServicioAutenticacion.CredencialesValidas(usuarioEncontrado, contrasena))
            {
                _vista.LimpiarError();
                _vista.CerrarConExito();
            }
            else
            {
                // El mismo mensaje para "usuario inexistente", "usuario inactivo" y "contraseña
                // incorrecta": asi no se le revela a un extraño que usuarios existen.
                _vista.MostrarError("Usuario o contraseña incorrectos.");
            }
        }
    }
}
