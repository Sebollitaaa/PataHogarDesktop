namespace SistemaAdministracionPataHogar.Modelos
{
    // REGLA DE NEGOCIO del inicio de sesión: decide si una persona puede ingresar.
    //
    // Ya no hay usuario ni contraseña escritos en el codigo. Los usuarios estan en la
    // coleccion "usuarios" de MongoDB Atlas (los busca el controlador con
    // RepositorioUsuarios) y esta clase solo aplica la regla sobre el usuario encontrado:
    //   1) el usuario tiene que existir,
    //   2) tiene que estar activo,
    //   3) la contraseña escrita tiene que coincidir con el hash (bcrypt) guardado.
    //
    // La contraseña real nunca se guarda: solo su hash. BCrypt.Verify calcula el hash de lo
    // que escribio la persona y lo compara con el guardado.
    public static class ServicioAutenticacion
    {
        // "usuario" es lo que encontro RepositorioUsuarios (null si no existe).
        public static bool CredencialesValidas(UsuarioAdministrador usuario, string contrasena)
        {
            if (usuario == null || !usuario.Activo)
            {
                return false;
            }

            if (string.IsNullOrEmpty(usuario.ContrasenaHash))
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(contrasena, usuario.ContrasenaHash);
            }
            catch (Exception)
            {
                // El hash guardado en la base no tiene un formato valido: no se deja ingresar.
                return false;
            }
        }
    }
}
