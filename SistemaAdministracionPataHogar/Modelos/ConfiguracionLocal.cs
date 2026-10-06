using System.Text.Json;

namespace SistemaAdministracionPataHogar.Modelos
{
    // Lee los datos sensibles (contraseña de la base de datos y credenciales del
    // administrador) desde un archivo LOCAL llamado "configuracion.local.json".
    //
    // Ese archivo NO esta en el codigo ni se sube al repositorio (esta en el
    // .gitignore). En el repositorio solo existe la plantilla
    // "configuracion.ejemplo.json", con valores de ejemplo. Cada persona que instala el
    // sistema copia la plantilla y completa sus propios datos.
    //
    // El archivo se busca al lado del ejecutable. Al compilar, el proyecto lo copia
    // automaticamente desde la carpeta del proyecto (ver el .csproj).
    //
    // Si el archivo no existe, no es un JSON valido o le falta algun dato, Cargar()
    // lanza una InvalidOperationException con un mensaje que explica que falta. Programa.cs
    // lo atrapa al iniciar y muestra ese mensaje.
    public static class ConfiguracionLocal
    {
        private const string NombreArchivo = "configuracion.local.json";

        // Indica si ya se leyo el archivo (para leerlo una sola vez).
        private static bool _cargada = false;

        private static string _servidor;
        private static string _puerto;
        private static string _nombreBaseDeDatos;
        private static string _usuarioBaseDeDatos;
        private static string _contrasenaBaseDeDatos;
        private static string _usuarioAdministrador;
        private static string _contrasenaAdministrador;

        // ---- Datos de conexion a MySQL ----
        public static string Servidor { get { AsegurarCarga(); return _servidor; } }
        public static string Puerto { get { AsegurarCarga(); return _puerto; } }
        public static string NombreBaseDeDatos { get { AsegurarCarga(); return _nombreBaseDeDatos; } }
        public static string UsuarioBaseDeDatos { get { AsegurarCarga(); return _usuarioBaseDeDatos; } }
        public static string ContrasenaBaseDeDatos { get { AsegurarCarga(); return _contrasenaBaseDeDatos; } }

        // ---- Credenciales para ingresar a esta aplicacion ----
        public static string UsuarioAdministrador { get { AsegurarCarga(); return _usuarioAdministrador; } }
        public static string ContrasenaAdministrador { get { AsegurarCarga(); return _contrasenaAdministrador; } }

        // Si todavia no se leyo el archivo, lo lee ahora.
        private static void AsegurarCarga()
        {
            if (!_cargada)
            {
                Cargar();
            }
        }

        // Lee el archivo, verifica que esten todos los datos y los guarda.
        public static void Cargar()
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, NombreArchivo);

            if (!File.Exists(ruta))
            {
                throw new InvalidOperationException(
                    "No se encontró el archivo de configuración '" + NombreArchivo + "' en:\n" +
                    AppContext.BaseDirectory + "\n\n" +
                    "Copie 'configuracion.ejemplo.json' como '" + NombreArchivo + "' y complételo " +
                    "con sus datos (ver README).");
            }

            string texto;
            try
            {
                texto = File.ReadAllText(ruta);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "No se pudo leer el archivo '" + NombreArchivo + "': " + ex.Message);
            }

            JsonDocument documento;
            try
            {
                documento = JsonDocument.Parse(texto);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    "El archivo '" + NombreArchivo + "' no tiene un formato JSON válido: " + ex.Message);
            }

            using (documento)
            {
                JsonElement raiz = documento.RootElement;

                _servidor = LeerTexto(raiz, "baseDeDatos", "servidor");
                _puerto = LeerTexto(raiz, "baseDeDatos", "puerto");
                _nombreBaseDeDatos = LeerTexto(raiz, "baseDeDatos", "nombre");
                _usuarioBaseDeDatos = LeerTexto(raiz, "baseDeDatos", "usuario");
                _contrasenaBaseDeDatos = LeerTexto(raiz, "baseDeDatos", "contrasena");
                _usuarioAdministrador = LeerTexto(raiz, "administrador", "usuario");
                _contrasenaAdministrador = LeerTexto(raiz, "administrador", "contrasena");
            }

            _cargada = true;
        }

        // Busca "seccion.clave" dentro del JSON y devuelve su valor como texto.
        // Si falta la seccion, falta la clave o esta vacia, lanza un error claro.
        private static string LeerTexto(JsonElement raiz, string seccion, string clave)
        {
            JsonElement elementoSeccion;
            if (raiz.ValueKind != JsonValueKind.Object ||
                !raiz.TryGetProperty(seccion, out elementoSeccion) ||
                elementoSeccion.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException(
                    "En '" + NombreArchivo + "' falta la sección '" + seccion + "'.");
            }

            JsonElement elementoClave;
            if (!elementoSeccion.TryGetProperty(clave, out elementoClave))
            {
                throw new InvalidOperationException(
                    "En '" + NombreArchivo + "' falta el dato '" + seccion + "." + clave + "'.");
            }

            string valor;
            if (elementoClave.ValueKind == JsonValueKind.String)
            {
                valor = elementoClave.GetString();
            }
            else if (elementoClave.ValueKind == JsonValueKind.Number)
            {
                valor = elementoClave.ToString(); // por ejemplo, un puerto escrito sin comillas
            }
            else
            {
                throw new InvalidOperationException(
                    "En '" + NombreArchivo + "' el dato '" + seccion + "." + clave + "' debe ser un texto.");
            }

            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new InvalidOperationException(
                    "En '" + NombreArchivo + "' el dato '" + seccion + "." + clave + "' está vacío.");
            }

            return valor;
        }
    }
}
