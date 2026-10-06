namespace SistemaAdministracionPataHogar.Modelos
{
    // REGLA DE NEGOCIO del inicio de sesión: decide si un usuario y una contraseña son
    // validos. Antes esta regla estaba escrita adentro del formulario de inicio de sesión;
    // en MVC las reglas viven en el Modelo, no en la Vista.
    //
    // El usuario y la contraseña validos NO estan escritos en el codigo: se leen del
    // archivo local "configuracion.local.json" (seccion "administrador") a traves de
    // ConfiguracionLocal.
    //
    // El dia de mañana, si se valida contra la tabla "users" de la base de
    // datos, SOLO hay que cambiar este archivo: ni la vista ni el controlador
    // se enteran.
    public static class ServicioAutenticacion
    {
        public static bool CredencialesValidas(string usuario, string contrasena)
        {
            return usuario == ConfiguracionLocal.UsuarioAdministrador &&
                   contrasena == ConfiguracionLocal.ContrasenaAdministrador;
        }
    }
}
