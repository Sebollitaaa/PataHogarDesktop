using System.Diagnostics;
using SistemaAdministracionPataHogar.Datos;
using SistemaAdministracionPataHogar.Modelos;
using SistemaAdministracionPataHogar.Vistas;

namespace SistemaAdministracionPataHogar.Controladores
{
    // CONTROLADOR de la pantalla de Solicitudes de Verificacion.
    //
    // Es el "cerebro" de la pantalla: guarda el estado (que filtro esta activo,
    // que solicitud esta seleccionada), le pide los datos al modelo
    // (RepositorioSolicitudes), decide que mostrar y se lo ordena a la vista.
    //
    // Como funciona, en pocas palabras:
    //   1) Iniciar() carga la lista y deja el detalle vacio.
    //   2) Cada accion del usuario (filtrar, elegir una tarjeta, abrir un
    //      documento, aprobar, rechazar) llega como un EVENTO de la vista.
    //   3) El metodo "On..." correspondiente hace el trabajo y le ordena a la
    //      vista que se actualice.
    public class ControladorSolicitudesVerificacion
    {
        private IVistaSolicitudesVerificacion _vista;

        // Cual de los 4 filtros esta activo: "Todas", "Pendiente", "Aprobada" o "Rechazada".
        private string _filtroActual = "Todas";

        // La solicitud que esta seleccionada (o null si no hay ninguna).
        private SolicitudVerificacion _solicitudSeleccionada = null;

        // Todas las solicitudes leidas la ultima vez de la base de datos.
        private List<SolicitudVerificacion> _todasLasSolicitudes = new List<SolicitudVerificacion>();

        public ControladorSolicitudesVerificacion(IVistaSolicitudesVerificacion vista)
        {
            _vista = vista;

            _vista.FiltroCambiado += AlCambiarFiltro;
            _vista.SolicitudSeleccionada += AlSeleccionarSolicitud;
            _vista.AbrirDocumentoSolicitado += AlSolicitarAbrirDocumento;
            _vista.AprobarSolicitado += AlSolicitarAprobar;
            _vista.RechazarSolicitado += AlSolicitarRechazar;
        }

        // Se llama una vez, cuando la pantalla se acaba de crear.
        public void Iniciar()
        {
            CargarListado();
            _vista.MostrarDetalleVacio();
        }

        // ---------------------------------------------------------------
        // Listado y filtros
        // ---------------------------------------------------------------

        // Lee todas las solicitudes del modelo, se queda solo con las que
        // cumplen el filtro actual y le pide a la vista que las dibuje.
        private void CargarListado()
        {
            _todasLasSolicitudes = RepositorioSolicitudes.ObtenerTodas();

            List<SolicitudVerificacion> filtradas = new List<SolicitudVerificacion>();
            foreach (SolicitudVerificacion solicitud in _todasLasSolicitudes)
            {
                if (CoincideConFiltro(solicitud))
                {
                    filtradas.Add(solicitud);
                }
            }

            int idSeleccionado = 0; // 0 = ninguna seleccionada
            if (_solicitudSeleccionada != null)
            {
                idSeleccionado = _solicitudSeleccionada.Id;
            }

            _vista.MostrarListado(filtradas, _filtroActual, idSeleccionado);
        }

        private bool CoincideConFiltro(SolicitudVerificacion solicitud)
        {
            if (_filtroActual == "Todas") return true;
            if (_filtroActual == "Pendiente") return solicitud.Estado == EstadoSolicitud.Pendiente;
            if (_filtroActual == "Aprobada") return solicitud.Estado == EstadoSolicitud.Aprobada;
            if (_filtroActual == "Rechazada") return solicitud.Estado == EstadoSolicitud.Rechazada;
            return true;
        }

        private void AlCambiarFiltro(object sender, EventArgs e)
        {
            _filtroActual = _vista.FiltroElegido;
            CargarListado();
        }

        // ---------------------------------------------------------------
        // Seleccion y detalle
        // ---------------------------------------------------------------

        private void AlSeleccionarSolicitud(object sender, EventArgs e)
        {
            SeleccionarSolicitud(_vista.SolicitudElegida);
        }

        private void SeleccionarSolicitud(SolicitudVerificacion solicitud)
        {
            _solicitudSeleccionada = solicitud;
            _vista.MarcarSolicitudSeleccionada(solicitud.Id);
            _vista.MostrarDetalle(solicitud);
        }

        // ---------------------------------------------------------------
        // Abrir documentos
        // ---------------------------------------------------------------

        // Pide a Windows que abra el archivo con el programa que tenga
        // asociado (igual que hacer doble clic en el Explorador de archivos).
        private void AlSolicitarAbrirDocumento(object sender, EventArgs e)
        {
            string ruta = _vista.RutaDocumentoElegido;

            try
            {
                ProcessStartInfo informacionDeInicio = new ProcessStartInfo(ruta);
                informacionDeInicio.UseShellExecute = true;
                Process.Start(informacionDeInicio);
            }
            catch (Exception ex)
            {
                _vista.MostrarError("Error al abrir archivo", "No se pudo abrir el archivo:\n" + ex.Message);
            }
        }

        // ---------------------------------------------------------------
        // Aprobar y rechazar
        // ---------------------------------------------------------------

        private void AlSolicitarAprobar(object sender, EventArgs e)
        {
            SolicitudVerificacion solicitud = _solicitudSeleccionada;
            if (solicitud == null) return;

            bool confirmado = _vista.Confirmar("Aprobar solicitud",
                "¿Confirmás la verificación de \"" + solicitud.NombreOrganizacion + "\"?\n\n" +
                "La organización pasará a tener la insignia de cuenta verificada.");

            if (!confirmado) return;

            try
            {
                RepositorioSolicitudes.AprobarSolicitud(solicitud.Id);
            }
            catch (Exception ex)
            {
                _vista.MostrarError("Error al guardar", "No se pudo guardar el cambio en la base de datos:\n" + ex.Message);
                return;
            }

            TrasResolucion(solicitud.Id);
            _vista.MostrarInformacion("Solicitud aprobada", "La organización fue verificada correctamente.");
        }

        private void AlSolicitarRechazar(object sender, EventArgs e)
        {
            SolicitudVerificacion solicitud = _solicitudSeleccionada;
            if (solicitud == null) return;

            string motivo = _vista.MotivoRechazoEscrito.Trim();
            if (motivo == string.Empty)
            {
                _vista.MostrarAdvertencia("Falta información", "Ingresá un motivo de rechazo antes de continuar.");
                return;
            }

            bool confirmado = _vista.Confirmar("Rechazar solicitud",
                "¿Confirmás el rechazo de \"" + solicitud.NombreOrganizacion + "\"?");

            if (!confirmado) return;

            try
            {
                RepositorioSolicitudes.RechazarSolicitud(solicitud.Id, motivo);
            }
            catch (Exception ex)
            {
                _vista.MostrarError("Error al guardar", "No se pudo guardar el cambio en la base de datos:\n" + ex.Message);
                return;
            }

            TrasResolucion(solicitud.Id);
            _vista.MostrarInformacion("Solicitud rechazada", "La solicitud fue rechazada.");
        }

        // Despues de aprobar/rechazar, volvemos a leer la lista completa desde
        // la base de datos (asi la pantalla siempre muestra lo que realmente
        // quedo guardado) y volvemos a seleccionar la misma solicitud, ahora
        // ya actualizada.
        private void TrasResolucion(int idSolicitud)
        {
            _filtroActual = "Todas";
            CargarListado();

            SolicitudVerificacion solicitudActualizada = BuscarPorId(idSolicitud);
            if (solicitudActualizada != null)
            {
                SeleccionarSolicitud(solicitudActualizada);
            }
        }

        private SolicitudVerificacion BuscarPorId(int idSolicitud)
        {
            foreach (SolicitudVerificacion solicitud in _todasLasSolicitudes)
            {
                if (solicitud.Id == idSolicitud)
                {
                    return solicitud;
                }
            }

            return null;
        }
    }
}
