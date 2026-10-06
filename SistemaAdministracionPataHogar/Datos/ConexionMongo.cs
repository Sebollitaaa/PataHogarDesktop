using MongoDB.Driver;
using SistemaAdministracionPataHogar.Modelos;

namespace SistemaAdministracionPataHogar.Datos
{
    // Conexion a MongoDB Atlas. Se usa SOLAMENTE para las credenciales de los usuarios que
    // pueden ingresar a esta aplicacion (inicio de sesion). Todo lo demas (las solicitudes de
    // verificacion) sigue en MySQL, a traves de ConexionBaseDatos.
    //
    // Es el equivalente a la clase "Conexion" del trabajo practico de MongoDB. Los datos de
    // conexion NO estan escritos aca: salen de configuracion.local.json (seccion "mongoDb").
    public static class ConexionMongo
    {
        private const string NombreColeccionUsuarios = "usuarios";

        // El cliente de MongoDB se crea una sola vez y se reutiliza (asi lo recomienda el driver).
        private static MongoClient _cliente = null;

        // Devuelve la coleccion "usuarios" de la base elegida en la configuracion.
        public static IMongoCollection<UsuarioMongo> ObtenerColeccionUsuarios()
        {
            if (_cliente == null)
            {
                MongoClientSettings configuracion =
                    MongoClientSettings.FromConnectionString(ConfiguracionLocal.MongoCadenaDeConexion);

                // Por defecto el driver espera hasta 30 segundos si no encuentra el servidor,
                // y la pantalla de inicio de sesion quedaria "colgada". Con 5 segundos, si
                // no hay internet o la IP no esta permitida en Atlas, avisa rapido.
                configuracion.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
                configuracion.ConnectTimeout = TimeSpan.FromSeconds(5);

                _cliente = new MongoClient(configuracion);
            }

            IMongoDatabase baseDeDatos = _cliente.GetDatabase(ConfiguracionLocal.MongoNombreBaseDeDatos);
            return baseDeDatos.GetCollection<UsuarioMongo>(NombreColeccionUsuarios);
        }
    }
}
