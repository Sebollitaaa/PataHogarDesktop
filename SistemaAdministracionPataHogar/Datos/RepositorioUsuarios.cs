using MongoDB.Driver;
using SistemaAdministracionPataHogar.Modelos;

namespace SistemaAdministracionPataHogar.Datos
{
    // Unica clase que consulta la coleccion "usuarios" de MongoDB. Igual que
    // RepositorioSolicitudes con MySQL, esconde COMO se lee la base: el resto del programa solo
    // le pide un usuario por nombre y recibe un UsuarioAdministrador.
    public static class RepositorioUsuarios
    {
        // Busca un usuario por su nombre de usuario.
        //  - Si existe, devuelve sus datos.
        //  - Si NO existe, devuelve null.
        //  - Si no se puede conectar con MongoDB, lanza una excepcion (la atrapa el controlador
        //    para mostrar un mensaje claro en pantalla).
        public static UsuarioAdministrador BuscarPorNombreUsuario(string nombreUsuario)
        {
            IMongoCollection<UsuarioMongo> coleccion = ConexionMongo.ObtenerColeccionUsuarios();

            // Equivalente en MQL: db.usuarios.find({ nombreUsuario: "..." })
            UsuarioMongo documento = coleccion.Find(u => u.NombreUsuario == nombreUsuario).FirstOrDefault();

            if (documento == null)
            {
                return null;
            }

            UsuarioAdministrador usuario = new UsuarioAdministrador();
            usuario.NombreUsuario = documento.NombreUsuario;
            usuario.ContrasenaHash = documento.ContrasenaHash;
            usuario.Activo = documento.Activo;
            return usuario;
        }
    }
}
