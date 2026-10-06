using SistemaAdministracionPataHogar.Controladores;
using SistemaAdministracionPataHogar.Modelos;
using SistemaAdministracionPataHogar.Vistas;

namespace SistemaAdministracionPataHogar
{
    // Punto de entrada del programa. Es la "raiz de composicion": el unico lugar
    // donde se crean las vistas y se les conecta su controlador.
    internal static class Programa
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Se lee la configuracion local (contraseñas) antes de mostrar nada. Si falta el
            // archivo o algun dato, se avisa con un mensaje claro y el programa termina.
            try
            {
                ConfiguracionLocal.Cargar();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Configuración inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool volverAMostrarInicioSesion = true;

            while (volverAMostrarInicioSesion)
            {
                volverAMostrarInicioSesion = false;

                // ---- Inicio de sesion: vista + controlador ----
                VistaInicioSesion vistaInicioSesion = new VistaInicioSesion();
                ControladorInicioSesion controladorInicioSesion = new ControladorInicioSesion(vistaInicioSesion);

                DialogResult resultadoDelInicioSesion = vistaInicioSesion.ShowDialog();
                vistaInicioSesion.Dispose();

                if (resultadoDelInicioSesion != DialogResult.OK)
                {
                    return; // el usuario cerro la ventana de inicio de sesion sin ingresar
                }

                // ---- Ventana principal: vista + controlador ----
                VistaPrincipal vistaPrincipal = new VistaPrincipal();
                ControladorPrincipal controladorPrincipal = new ControladorPrincipal(vistaPrincipal);

                Application.Run(vistaPrincipal);

                if (controladorPrincipal.CerrarSesionSolicitada)
                {
                    volverAMostrarInicioSesion = true;
                }
            }
        }
    }
}
