namespace SistemaAdministracionPataHogar.Modelos
{
    // Representa a una persona que puede ingresar a la aplicacion de escritorio.
    // Es una "ficha" sin logica: los datos vienen de la coleccion "usuarios" de MongoDB
    // (ver RepositorioUsuarios) y la regla que decide si puede ingresar esta en
    // ServicioAutenticacion.
    public class UsuarioAdministrador
    {
        // Nombre con el que la persona inicia sesion.
        public string NombreUsuario { get; set; }

        // La contraseña NUNCA se guarda en texto plano: se guarda su "hash" (bcrypt),
        // que no se puede convertir de nuevo en la contraseña original.
        public string ContrasenaHash { get; set; }

        // Si es false, el usuario existe pero no puede ingresar (por ejemplo, dado de baja).
        public bool Activo { get; set; }
    }
}
