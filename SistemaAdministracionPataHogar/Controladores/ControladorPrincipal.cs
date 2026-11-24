using System.Diagnostics;
using SistemaAdministracionPataHogar.Datos;
using SistemaAdministracionPataHogar.Vistas;

namespace SistemaAdministracionPataHogar.Controladores
{
    // CONTROLADOR de la ventana principal. Decide que hacer cuando:
    //   - la ventana termina de mostrarse  -> revisar la conexion a la base
    //   - se hace clic en "Solicitudes"     -> armar y mostrar esa pantalla
    //   - se hace clic en "Cerrar sesion"   -> pedir confirmacion y cerrar
    public class ControladorPrincipal
    {
        private IVistaPrincipal _vista;

        // Nombre del PDF unico de documentacion (contiene el manual de usuario).
        private const string ArchivoManual = "Documentacion Proyecto Final - PataHogar Desktop.pdf";

        // Guardamos el controlador de la pantalla de solicitudes para que
        // siga "vivo" mientras la pantalla este abierta.
        private ControladorSolicitudesVerificacion _controladorSolicitudes;

        // Programa.cs lo consulta cuando se cierra la ventana principal, para
        // saber si hay que volver al inicio de sesión (true) o terminar el programa (false).
        public bool CerrarSesionSolicitada { get; private set; }

        public ControladorPrincipal(IVistaPrincipal vista)
        {
            _vista = vista;

            _vista.VistaMostrada += AlMostrarseVista;
            _vista.AbrirSolicitudesSolicitado += AlSolicitarAbrirSolicitudes;
            _vista.CerrarSesionSolicitado += AlSolicitarCerrarSesion;
            _vista.ManualSolicitado += AlSolicitarManual;

            _vista.MostrarBienvenida();
        }

        // Abre el PDF de la documentacion (que incluye el manual de usuario) con el
        // programa que Windows tenga para los PDF. El archivo se copia junto al
        // ejecutable al compilar (ver el .csproj).
        private void AlSolicitarManual(object sender, EventArgs e)
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, ArchivoManual);

            if (!File.Exists(ruta))
            {
                _vista.MostrarError("No se encontró el manual de usuario:\n" + ruta);
                return;
            }

            try
            {
                ProcessStartInfo informacionDeInicio = new ProcessStartInfo(ruta);
                informacionDeInicio.UseShellExecute = true;
                Process.Start(informacionDeInicio);
            }
            catch (Exception ex)
            {
                _vista.MostrarError("No se pudo abrir el manual de usuario:\n" + ex.Message);
            }
        }

        // Se ejecuta una sola vez, cuando la ventana ya se termino de mostrar.
        private void AlMostrarseVista(object sender, EventArgs e)
        {
            string mensaje;
            bool conectado = ConexionBaseDatos.ProbarConexion(out mensaje);

            _vista.MostrarEstadoBaseDeDatos(conectado, mensaje);
        }

        // Crea la pantalla de solicitudes (vista + controlador) y la muestra.
        private void AlSolicitarAbrirSolicitudes(object sender, EventArgs e)
        {
            VistaSolicitudesVerificacion vistaSolicitudes = new VistaSolicitudesVerificacion();
            _controladorSolicitudes = new ControladorSolicitudesVerificacion(vistaSolicitudes);
            _controladorSolicitudes.Iniciar();

            _vista.ResaltarSeccionSolicitudes();
            _vista.MostrarContenido(
                vistaSolicitudes,
                "Solicitudes de verificación",
                "Revisá la documentación de refugios, veterinarias y asociaciones para aprobar o rechazar su cuenta verificada.");
        }

        private void AlSolicitarCerrarSesion(object sender, EventArgs e)
        {
            if (_vista.ConfirmarCerrarSesion())
            {
                CerrarSesionSolicitada = true;
                _vista.CerrarVentana();
            }
        }
    }
}
