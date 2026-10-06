using System.Collections.Generic;

namespace SistemaAdministracionPataHogar.Modelos
{
    // Esta clase es una "ficha" con todos los datos de una solicitud de
    // verificacion. No tiene logica, solo guarda informacion.
    public class SolicitudVerificacion
    {
        public int Id { get; set; }
        public string NombreOrganizacion { get; set; }
        public TipoOrganizacion Tipo { get; set; }
        public string NombreResponsable { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int AniosFuncionamiento { get; set; }
        public int AnimalesAlbergados { get; set; }
        public string Descripcion { get; set; }
        public string SitioWebORedes { get; set; }
        public List<DocumentoAdjunto> Documentos { get; set; }

        // Estos tres campos empiezan con un valor por defecto porque cuando
        // se crea una solicitud todavia no fue revisada por el administrador.
        public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
        public string MotivoRechazo { get; set; }
        public DateTime? FechaResolucion { get; set; }
    }
}
