namespace SistemaAdministracionPataHogar.Vistas
{
    // El "contrato" de la pantalla de inicio de sesión. Lista TODO lo que el controlador
    // puede pedirle o escuchar de la vista, y nada mas. El controlador solo
    // conoce esta interfaz, nunca el formulario real (VistaInicioSesion).
    public interface IVistaInicioSesion
    {
        // ---- Datos que el controlador puede leer ----
        string Usuario { get; }
        string Contrasena { get; }

        // ---- Aviso de la vista al controlador: "el usuario quiere ingresar" ----
        event EventHandler IngresarSolicitado;

        // ---- Ordenes del controlador a la vista ----
        void MostrarError(string mensaje);
        void LimpiarError();
        void CerrarConExito();
    }
}
