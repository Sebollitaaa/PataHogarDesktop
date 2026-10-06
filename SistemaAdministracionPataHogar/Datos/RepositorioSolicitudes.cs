using System.Collections.Generic;
using System.IO;
using MySqlConnector;
using SistemaAdministracionPataHogar.Modelos;

namespace SistemaAdministracionPataHogar.Datos
{
    // Esta clase es la UNICA que sabe de donde salen (y a donde van) las
    // solicitudes de verificacion. Ahora lee y escribe en la base de datos
    // MySQL real (tablas "verification_requests" y "verification_documents",
    // la misma base que usa el sitio web).
    //
    // Si en algun momento la base no responde (por ejemplo, si no se
    // arranco el servidor MySQL), esta clase no rompe el programa: muestra
    // 1 o 2 solicitudes de EJEMPLO como respaldo, para que la pantalla nunca
    // quede vacia y se pueda seguir mostrando/probando la aplicacion.
    public static class RepositorioSolicitudes
    {
        // Carpeta donde se generan los archivos de ejemplo que simulan los
        // documentos adjuntos de las solicitudes DE RESPALDO (no de las reales).
        private static readonly string CarpetaDocumentosDeEjemplo =
            Path.Combine(Path.GetTempPath(), "PataHogar", "DocumentosEjemplo");

        // -----------------------------------------------------------------
        // Lectura
        // -----------------------------------------------------------------

        // Devuelve la lista completa de solicitudes. Primero intenta leerlas
        // de la base de datos real; si no puede (por ejemplo, MySQL apagado),
        // devuelve las solicitudes de ejemplo de respaldo.
        public static List<SolicitudVerificacion> ObtenerTodas()
        {
            try
            {
                return ObtenerTodasDesdeLaBaseDeDatos();
            }
            catch (Exception)
            {
                return CrearSolicitudesDeRespaldo();
            }
        }

        private static List<SolicitudVerificacion> ObtenerTodasDesdeLaBaseDeDatos()
        {
            List<SolicitudVerificacion> lista = new List<SolicitudVerificacion>();

            using (MySqlConnection conexion = ConexionBaseDatos.CrearConexion())
            {
                conexion.Open();

                string consulta =
                    "SELECT id, organization_name, organization_type, responsible_name, email, phone, " +
                    "address, city, province, years_in_operation, animals_housed, website, description, " +
                    "status, rejection_reason, resolved_at, created_at " +
                    "FROM verification_requests " +
                    "ORDER BY created_at DESC";

                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                using (MySqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        SolicitudVerificacion solicitud = new SolicitudVerificacion();

                        solicitud.Id = Convert.ToInt32(lector["id"]);
                        solicitud.NombreOrganizacion = Convert.ToString(lector["organization_name"]);
                        solicitud.Tipo = ConvertirTipoOrganizacion(Convert.ToString(lector["organization_type"]));
                        solicitud.NombreResponsable = Convert.ToString(lector["responsible_name"]);
                        solicitud.Correo = Convert.ToString(lector["email"]);
                        solicitud.Telefono = Convert.ToString(lector["phone"]);
                        solicitud.Direccion = Convert.ToString(lector["address"]);
                        solicitud.Ciudad = Convert.ToString(lector["city"]);
                        solicitud.Provincia = Convert.ToString(lector["province"]);
                        solicitud.AniosFuncionamiento = Convert.ToInt32(lector["years_in_operation"]);
                        solicitud.AnimalesAlbergados = Convert.ToInt32(lector["animals_housed"]);
                        solicitud.Descripcion = Convert.ToString(lector["description"]);
                        solicitud.Estado = ConvertirEstadoSolicitud(Convert.ToString(lector["status"]));
                        solicitud.FechaSolicitud = Convert.ToDateTime(lector["created_at"]);

                        if (lector["website"] == DBNull.Value)
                        {
                            solicitud.SitioWebORedes = null;
                        }
                        else
                        {
                            solicitud.SitioWebORedes = Convert.ToString(lector["website"]);
                        }

                        if (lector["rejection_reason"] == DBNull.Value)
                        {
                            solicitud.MotivoRechazo = null;
                        }
                        else
                        {
                            solicitud.MotivoRechazo = Convert.ToString(lector["rejection_reason"]);
                        }

                        if (lector["resolved_at"] == DBNull.Value)
                        {
                            solicitud.FechaResolucion = null;
                        }
                        else
                        {
                            solicitud.FechaResolucion = Convert.ToDateTime(lector["resolved_at"]);
                        }

                        solicitud.Documentos = new List<DocumentoAdjunto>();
                        lista.Add(solicitud);
                    }
                }

                // Ahora que ya cerramos el primer lector, buscamos los
                // documentos de cada solicitud (una consulta por solicitud).
                foreach (SolicitudVerificacion solicitud in lista)
                {
                    solicitud.Documentos = ObtenerDocumentosDeLaSolicitud(conexion, solicitud.Id);
                }
            }

            return lista;
        }

        private static List<DocumentoAdjunto> ObtenerDocumentosDeLaSolicitud(MySqlConnection conexion, int idSolicitud)
        {
            List<DocumentoAdjunto> documentos = new List<DocumentoAdjunto>();

            string consulta =
                "SELECT original_filename, file_path FROM verification_documents " +
                "WHERE request_id = @idSolicitud ORDER BY id";

            using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
            {
                comando.Parameters.AddWithValue("@idSolicitud", idSolicitud);

                using (MySqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        DocumentoAdjunto documento = new DocumentoAdjunto();
                        documento.Nombre = Convert.ToString(lector["original_filename"]);
                        documento.RutaArchivo = Convert.ToString(lector["file_path"]);
                        documentos.Add(documento);
                    }
                }
            }

            return documentos;
        }

        // Traduce el texto que viene de la columna "organization_type" al
        // enum que usa el resto del programa.
        private static TipoOrganizacion ConvertirTipoOrganizacion(string valorDeLaBase)
        {
            if (valorDeLaBase == "refugio") return TipoOrganizacion.Refugio;
            if (valorDeLaBase == "veterinaria") return TipoOrganizacion.Veterinaria;
            return TipoOrganizacion.Asociacion;
        }

        // Traduce el texto que viene de la columna "status" al enum que usa
        // el resto del programa.
        private static EstadoSolicitud ConvertirEstadoSolicitud(string valorDeLaBase)
        {
            if (valorDeLaBase == "aprobada") return EstadoSolicitud.Aprobada;
            if (valorDeLaBase == "rechazada") return EstadoSolicitud.Rechazada;
            return EstadoSolicitud.Pendiente;
        }

        // -----------------------------------------------------------------
        // Escritura (aprobar / rechazar)
        // -----------------------------------------------------------------

        // Guarda en la base de datos que la solicitud fue aprobada.
        public static void AprobarSolicitud(int idSolicitud)
        {
            using (MySqlConnection conexion = ConexionBaseDatos.CrearConexion())
            {
                conexion.Open();

                string consulta =
                    "UPDATE verification_requests " +
                    "SET status = 'aprobada', rejection_reason = NULL, resolved_at = NOW() " +
                    "WHERE id = @idSolicitud";

                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                    comando.ExecuteNonQuery();
                }
            }
        }

        // Guarda en la base de datos que la solicitud fue rechazada, junto
        // con el motivo escrito por el administrador.
        public static void RechazarSolicitud(int idSolicitud, string motivo)
        {
            using (MySqlConnection conexion = ConexionBaseDatos.CrearConexion())
            {
                conexion.Open();

                string consulta =
                    "UPDATE verification_requests " +
                    "SET status = 'rechazada', rejection_reason = @motivo, resolved_at = NOW() " +
                    "WHERE id = @idSolicitud";

                using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@idSolicitud", idSolicitud);
                    comando.Parameters.AddWithValue("@motivo", motivo);
                    comando.ExecuteNonQuery();
                }
            }
        }

        // -----------------------------------------------------------------
        // Datos de respaldo (solo se usan si la base de datos no responde)
        // -----------------------------------------------------------------

        private static List<SolicitudVerificacion> CrearSolicitudesDeRespaldo()
        {
            List<SolicitudVerificacion> lista = new List<SolicitudVerificacion>();

            SolicitudVerificacion solicitud1 = new SolicitudVerificacion
            {
                Id = -1,
                NombreOrganizacion = "Refugio Huellas de Esperanza (ejemplo sin conexión)",
                Tipo = TipoOrganizacion.Refugio,
                NombreResponsable = "María Fernández",
                Correo = "contacto@huellasdeesperanza.org",
                Telefono = "351 455-2310",
                Direccion = "Av. Colón 3542",
                Ciudad = "Córdoba",
                Provincia = "Córdoba",
                FechaSolicitud = DateTime.Now.AddDays(-2),
                AniosFuncionamiento = 6,
                AnimalesAlbergados = 48,
                Descripcion = "Este es un dato de EJEMPLO: se muestra porque no se pudo conectar a la base de datos real. Refugio sin fines de lucro dedicado al rescate de perros y gatos en situación de calle.",
                SitioWebORedes = "instagram.com/huellasdeesperanza.cba",
                Documentos = CrearDocumentosDeEjemplo(-1, "Refugio Huellas de Esperanza")
            };
            lista.Add(solicitud1);

            SolicitudVerificacion solicitud2 = new SolicitudVerificacion
            {
                Id = -2,
                NombreOrganizacion = "Refugio Patitas Felices (ejemplo sin conexión)",
                Tipo = TipoOrganizacion.Refugio,
                NombreResponsable = "Carlos Medina",
                Correo = "patitasfelices.mza@hotmail.com",
                Telefono = "261 498-2201",
                Direccion = "Ruta 40 Km 12",
                Ciudad = "Mendoza",
                Provincia = "Mendoza",
                FechaSolicitud = DateTime.Now.AddDays(-14),
                AniosFuncionamiento = 9,
                AnimalesAlbergados = 63,
                Descripcion = "Este es un dato de EJEMPLO: se muestra porque no se pudo conectar a la base de datos real. Refugio con predio propio, foco en perros de gran tamaño y adultos mayores de difícil adopción.",
                SitioWebORedes = "facebook.com/patitasfelicesmza",
                Documentos = CrearDocumentosDeEjemplo(-2, "Refugio Patitas Felices"),
                Estado = EstadoSolicitud.Aprobada,
                FechaResolucion = DateTime.Now.AddDays(-10)
            };
            lista.Add(solicitud2);

            return lista;
        }

        private static List<DocumentoAdjunto> CrearDocumentosDeEjemplo(int idSolicitud, string nombreOrganizacion)
        {
            List<DocumentoAdjunto> documentos = new List<DocumentoAdjunto>();

            documentos.Add(CrearDocumentoDeEjemplo(idSolicitud, 1, "Estatuto o constancia de la organización.txt",
                "Constancia de la organización\n\n" + nombreOrganizacion + " declara operar como organización sin fines de lucro " +
                "dedicada al rescate y adopción responsable de animales.\n\n(Documento de ejemplo, sin conexión a la base de datos real.)"));

            documentos.Add(CrearDocumentoDeEjemplo(idSolicitud, 2, "DNI del responsable.txt",
                "Documento Nacional de Identidad\n\nFrente y dorso presentados por el responsable de la organización.\n\n" +
                "(Documento de ejemplo, sin conexión a la base de datos real.)"));

            return documentos;
        }

        private static DocumentoAdjunto CrearDocumentoDeEjemplo(int idSolicitud, int numeroDeDocumento, string nombreArchivo, string contenido)
        {
            Directory.CreateDirectory(CarpetaDocumentosDeEjemplo);

            string nombreUnico = idSolicitud + "_" + numeroDeDocumento + "_" + nombreArchivo;
            string ruta = Path.Combine(CarpetaDocumentosDeEjemplo, nombreUnico);
            File.WriteAllText(ruta, contenido);

            DocumentoAdjunto documento = new DocumentoAdjunto();
            documento.Nombre = nombreArchivo;
            documento.RutaArchivo = ruta;
            return documento;
        }
    }
}
