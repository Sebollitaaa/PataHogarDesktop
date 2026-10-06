using SistemaAdministracionPataHogar.Modelos;

namespace SistemaAdministracionPataHogar.Estilos
{
    // Todos los colores y fuentes de la aplicacion estan definidos ACA, en un
    // solo lugar. Si el dia de mañana queres cambiar el color verde principal
    // de toda la app, alcanza con cambiarlo en este archivo.
    public static class TemaApp
    {
        public static readonly Color Fondo = Color.FromArgb(247, 248, 250);
        public static readonly Color FondoTarjeta = Color.White;
        public static readonly Color Borde = Color.FromArgb(226, 229, 233);

        public static readonly Color BarraLateral = Color.FromArgb(31, 41, 55);
        public static readonly Color BarraLateralResaltada = Color.FromArgb(45, 55, 72);
        public static readonly Color BarraLateralSeleccionada = Color.FromArgb(58, 125, 68);
        public static readonly Color BarraLateralTexto = Color.FromArgb(214, 219, 226);

        public static readonly Color Principal = Color.FromArgb(58, 125, 68);
        public static readonly Color PrincipalOscuro = Color.FromArgb(43, 97, 51);
        public static readonly Color PrincipalClaro = Color.FromArgb(224, 238, 226);

        public static readonly Color TextoPrincipal = Color.FromArgb(43, 45, 66);
        public static readonly Color TextoSecundario = Color.FromArgb(108, 122, 137);

        public static readonly Color Exito = Color.FromArgb(46, 174, 113);
        public static readonly Color ExitoClaro = Color.FromArgb(224, 245, 233);
        public static readonly Color Peligro = Color.FromArgb(230, 73, 79);
        public static readonly Color PeligroClaro = Color.FromArgb(252, 228, 229);
        public static readonly Color Advertencia = Color.FromArgb(224, 160, 30);
        public static readonly Color AdvertenciaClaro = Color.FromArgb(252, 240, 214);

        public const string FamiliaFuente = "Segoe UI";

        public static Font FuenteTitulo => new Font(FamiliaFuente, 20F, FontStyle.Bold);
        public static Font FuenteSubtitulo => new Font(FamiliaFuente, 11F, FontStyle.Regular);
        public static Font FuenteEncabezado => new Font(FamiliaFuente, 14F, FontStyle.Bold);
        public static Font FuenteEtiqueta => new Font(FamiliaFuente, 9F, FontStyle.Regular);
        public static Font FuenteEtiquetaNegrita => new Font(FamiliaFuente, 9F, FontStyle.Bold);
        public static Font FuenteCuerpo => new Font(FamiliaFuente, 9.5F, FontStyle.Regular);
        public static Font FuenteCuerpoNegrita => new Font(FamiliaFuente, 9.5F, FontStyle.Bold);
        public static Font FuenteChica => new Font(FamiliaFuente, 8F, FontStyle.Regular);
        public static Font FuenteNavegacion => new Font(FamiliaFuente, 10F, FontStyle.Regular);

        // Devuelve el color "fuerte" segun el estado de una solicitud
        // (se usa para el texto de la etiqueta de estado).
        public static Color ObtenerColorDeEstado(EstadoSolicitud estado)
        {
            if (estado == EstadoSolicitud.Aprobada) return Exito;
            if (estado == EstadoSolicitud.Rechazada) return Peligro;
            return Advertencia;
        }

        // Devuelve el color "clarito" de fondo segun el estado de una solicitud
        // (se usa como fondo de la etiqueta de estado).
        public static Color ObtenerColorDeFondoDeEstado(EstadoSolicitud estado)
        {
            if (estado == EstadoSolicitud.Aprobada) return ExitoClaro;
            if (estado == EstadoSolicitud.Rechazada) return PeligroClaro;
            return AdvertenciaClaro;
        }
    }
}
