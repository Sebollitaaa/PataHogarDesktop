using MySqlConnector;
using SistemaAdministracionPataHogar.Modelos;

namespace SistemaAdministracionPataHogar.Datos
{
    // Esta clase junta en un solo lugar todo lo necesario para conectarse a
    // la base de datos MySQL del sistema web (la misma base que usa la
    // pagina/app de adopciones).
    //
    // Los datos de conexion (servidor, puerto, base, usuario y contraseña) NO estan
    // escritos aca: se leen del archivo local "configuracion.local.json" a traves de
    // ConfiguracionLocal. Asi la contraseña nunca queda en el codigo ni en el
    // repositorio.
    public static class ConexionBaseDatos
    {
        // Arma el "texto de conexion" que usa MySqlConnector para saber a
        // donde conectarse y con que usuario. "Connection Timeout=5" hace
        // que, si el servidor no responde, el programa no se quede colgado
        // esperando: a los 5 segundos avisa que no pudo conectar.
        public static string ObtenerCadenaDeConexion()
        {
            return "Server=" + ConfiguracionLocal.Servidor + ";" +
                   "Port=" + ConfiguracionLocal.Puerto + ";" +
                   "Database=" + ConfiguracionLocal.NombreBaseDeDatos + ";" +
                   "Uid=" + ConfiguracionLocal.UsuarioBaseDeDatos + ";" +
                   "Pwd=" + ConfiguracionLocal.ContrasenaBaseDeDatos + ";" +
                   "Connection Timeout=5;";
        }

        // Crea una conexion nueva lista para usar. Quien la use es
        // responsable de abrirla (Open) y cerrarla (Close/Dispose) cuando
        // termine, tal como se hace con cualquier conexion ADO.NET
        // (la misma idea que SqlConnection para SQL Server, pero para MySQL).
        public static MySqlConnection CrearConexion()
        {
            return new MySqlConnection(ObtenerCadenaDeConexion());
        }

        // Intenta abrir la conexion una sola vez, para verificar que los
        // datos (servidor, usuario, contraseña) son correctos. Devuelve
        // true/false y, en "mensaje", una explicacion para mostrar en
        // pantalla. No lanza ninguna excepcion hacia afuera: cualquier
        // error de conexion queda atrapado aca adentro.
        public static bool ProbarConexion(out string mensaje)
        {
            try
            {
                using (MySqlConnection conexion = CrearConexion())
                {
                    conexion.Open();
                }

                mensaje = "Conectado a la base de datos '" + ConfiguracionLocal.NombreBaseDeDatos + "'.";
                return true;
            }
            catch (Exception ex)
            {
                mensaje = "Sin conexión a la base de datos: " + ex.Message;
                return false;
            }
        }
    }
}
