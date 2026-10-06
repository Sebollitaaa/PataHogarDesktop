namespace SistemaAdministracionPataHogar.Vistas
{
    // El "contrato" de la ventana principal (menu + area de contenido).
    public interface IVistaPrincipal
    {
        // ---- Avisos de la vista al controlador ----
        event EventHandler VistaMostrada;             // la ventana termino de aparecer
        event EventHandler AbrirSolicitudesSolicitado; // se hizo clic en "Solicitudes de verificacion"
        event EventHandler CerrarSesionSolicitado;     // se hizo clic en "Cerrar sesion"

        // ---- Ordenes del controlador a la vista ----
        void MostrarBienvenida();
        void MostrarContenido(Control contenido, string titulo, string subtitulo);
        void ResaltarSeccionSolicitudes();
        void MostrarEstadoBaseDeDatos(bool conectado, string mensaje);
        bool ConfirmarCerrarSesion();
        void CerrarVentana();
    }
}
