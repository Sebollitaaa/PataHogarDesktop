namespace SistemaAdministracionPataHogar.Modelos
{
    // Los tipos de organizacion que pueden pedir verificacion.
    public enum TipoOrganizacion
    {
        Refugio,
        Veterinaria,
        Asociacion
    }

    // Funciones sueltas (no son metodos de la clase) para transformar el enum
    // de arriba en el texto y el icono que se muestran en pantalla.
    // Se llaman asi: TipoOrganizacionAyudante.ObtenerTexto(unTipo)
    public static class TipoOrganizacionAyudante
    {
        public static string ObtenerTexto(TipoOrganizacion tipo)
        {
            if (tipo == TipoOrganizacion.Refugio) return "Refugio";
            if (tipo == TipoOrganizacion.Veterinaria) return "Veterinaria";
            if (tipo == TipoOrganizacion.Asociacion) return "Asociación protectora";
            return tipo.ToString();
        }

        public static string ObtenerIcono(TipoOrganizacion tipo)
        {
            if (tipo == TipoOrganizacion.Refugio) return "🏠";
            if (tipo == TipoOrganizacion.Veterinaria) return "🩺";
            if (tipo == TipoOrganizacion.Asociacion) return "🤝";
            return "📋";
        }
    }
}
