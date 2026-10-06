namespace SistemaAdministracionPataHogar.Modelos
{
    // Representa un documento que un refugio/veterinaria/asociacion adjunto
    // a su solicitud (por ejemplo: "DNI del responsable.txt").
    public class DocumentoAdjunto
    {
        // El nombre que se muestra en pantalla.
        public string Nombre { get; set; }

        // La ruta real del archivo en la computadora, para poder abrirlo.
        public string RutaArchivo { get; set; }
    }
}
