namespace SistemaAdministracionPataHogar.Modelos
{
    // Los tres estados posibles de una solicitud de verificacion.
    public enum EstadoSolicitud
    {
        Pendiente,
        Aprobada,
        Rechazada
    }

    // Funcion suelta para transformar el enum de arriba en el texto que se
    // muestra en pantalla. Se llama asi: EstadoSolicitudAyudante.ObtenerTexto(unEstado)
    public static class EstadoSolicitudAyudante
    {
        public static string ObtenerTexto(EstadoSolicitud estado)
        {
            if (estado == EstadoSolicitud.Aprobada) return "Aprobada";
            if (estado == EstadoSolicitud.Rechazada) return "Rechazada";
            return "Pendiente";
        }
    }
}
