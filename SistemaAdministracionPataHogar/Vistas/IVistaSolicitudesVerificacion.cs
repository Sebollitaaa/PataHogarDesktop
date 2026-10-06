using SistemaAdministracionPataHogar.Modelos;

namespace SistemaAdministracionPataHogar.Vistas
{
    // El "contrato" de la pantalla de Solicitudes de Verificacion.
    //
    // Forma de comunicarse vista -> controlador: la vista guarda el dato en una
    // propiedad (por ejemplo SolicitudElegida) y despues dispara un evento
    // (SolicitudSeleccionada). El evento avisa QUE paso; la propiedad dice
    // SOBRE QUE dato paso.
    public interface IVistaSolicitudesVerificacion
    {
        // ---- Datos que el controlador puede leer ----
        string FiltroElegido { get; }                 // "Todas", "Pendiente", "Aprobada" o "Rechazada"
        SolicitudVerificacion SolicitudElegida { get; } // la tarjeta en la que se hizo clic
        string RutaDocumentoElegido { get; }          // el documento cuyo boton "Abrir" se apreto
        string MotivoRechazoEscrito { get; }          // lo que escribio el admin en la caja de motivo

        // ---- Avisos de la vista al controlador ----
        event EventHandler FiltroCambiado;
        event EventHandler SolicitudSeleccionada;
        event EventHandler AbrirDocumentoSolicitado;
        event EventHandler AprobarSolicitado;
        event EventHandler RechazarSolicitado;

        // ---- Ordenes del controlador a la vista ----
        // idSeleccionado = 0 significa "ninguna solicitud seleccionada".
        void MostrarListado(List<SolicitudVerificacion> solicitudes, string filtroActivo, int idSeleccionado);
        void MarcarSolicitudSeleccionada(int idSolicitud);
        void MostrarDetalle(SolicitudVerificacion solicitud);
        void MostrarDetalleVacio();

        void MostrarInformacion(string titulo, string mensaje);
        void MostrarAdvertencia(string titulo, string mensaje);
        void MostrarError(string titulo, string mensaje);
        bool Confirmar(string titulo, string mensaje);
    }
}
