using SistemaAdministracionPataHogar.Estilos;

namespace SistemaAdministracionPataHogar.Vistas
{
    partial class VistaSolicitudesVerificacion
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelListado;
        private Panel panelFiltros;
        private FlowLayoutPanel flujoFiltros;
        private BotonRedondeado btnFiltroTodas;
        private BotonRedondeado btnFiltroPendientes;
        private BotonRedondeado btnFiltroAprobadas;
        private BotonRedondeado btnFiltroRechazadas;
        private FlowLayoutPanel flujoLista;

        private PanelRedondeado panelDetalle;
        private Panel pnlDetalleContenido;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            panelListado = new Panel();
            panelFiltros = new Panel();
            flujoFiltros = new FlowLayoutPanel();
            btnFiltroTodas = new BotonRedondeado();
            btnFiltroPendientes = new BotonRedondeado();
            btnFiltroAprobadas = new BotonRedondeado();
            btnFiltroRechazadas = new BotonRedondeado();
            flujoLista = new FlowLayoutPanel();

            panelDetalle = new PanelRedondeado();
            pnlDetalleContenido = new Panel();

            SuspendLayout();

            // ---- Columna izquierda: filtros + lista de tarjetas ----

            panelListado.Dock = DockStyle.Left;
            panelListado.Width = 380;
            panelListado.BackColor = TemaApp.Fondo;
            panelListado.Padding = new Padding(0, 0, 20, 0);

            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.Height = 40;
            panelFiltros.BackColor = TemaApp.Fondo;

            flujoFiltros.Dock = DockStyle.Fill;
            flujoFiltros.FlowDirection = FlowDirection.LeftToRight;
            flujoFiltros.WrapContents = false;
            flujoFiltros.BackColor = TemaApp.Fondo;

            // Cada boton de filtro guarda en su "Tag" el nombre del filtro
            // que representa. Asi, un solo metodo (BotonDeFiltro_Click) sirve para
            // los cuatro botones: solo hay que mirar el Tag para saber cual
            // se aprieto.
            // (Los anchos suman menos que los 360 px de la columna, contando el
            // margen de 8 px de cada boton, para que ninguno quede recortado.)
            ConfigurarBotonDeFiltro(btnFiltroTodas, "Todas", "Todas", 60);
            ConfigurarBotonDeFiltro(btnFiltroPendientes, "Pendientes", "Pendiente", 88);
            ConfigurarBotonDeFiltro(btnFiltroAprobadas, "Aprobadas", "Aprobada", 84);
            ConfigurarBotonDeFiltro(btnFiltroRechazadas, "Rechazadas", "Rechazada", 90);

            flujoFiltros.Controls.Add(btnFiltroTodas);
            flujoFiltros.Controls.Add(btnFiltroPendientes);
            flujoFiltros.Controls.Add(btnFiltroAprobadas);
            flujoFiltros.Controls.Add(btnFiltroRechazadas);

            panelFiltros.Controls.Add(flujoFiltros);

            flujoLista.Dock = DockStyle.Fill;
            flujoLista.FlowDirection = FlowDirection.TopDown;
            flujoLista.WrapContents = false;
            flujoLista.AutoScroll = true;
            flujoLista.BackColor = TemaApp.Fondo;
            flujoLista.Padding = new Padding(0, 12, 0, 0);

            panelListado.Controls.Add(flujoLista);
            panelListado.Controls.Add(panelFiltros);

            // ---- Columna derecha: detalle de la solicitud elegida ----

            panelDetalle.Dock = DockStyle.Fill;
            panelDetalle.RadioEsquinas = 12;
            panelDetalle.ColorBorde = TemaApp.Borde;
            panelDetalle.BackColor = TemaApp.FondoTarjeta;
            panelDetalle.Padding = new Padding(3);

            pnlDetalleContenido.Dock = DockStyle.Fill;
            pnlDetalleContenido.BackColor = TemaApp.FondoTarjeta;
            pnlDetalleContenido.AutoScroll = true;
            pnlDetalleContenido.Padding = new Padding(28, 24, 24, 24);

            panelDetalle.Controls.Add(pnlDetalleContenido);

            // ---- Control completo ----

            Controls.Add(panelDetalle);
            Controls.Add(panelListado);
            Size = new Size(1000, 700);

            ResumeLayout(false);
        }

        // Deja lista la apariencia de un boton-filtro (forma de pastilla) y
        // le guarda en el Tag el nombre del filtro que representa.
        private static void ConfigurarBotonDeFiltro(BotonRedondeado boton, string texto, string nombreDelFiltro, int ancho)
        {
            boton.Text = texto;
            boton.Tag = nombreDelFiltro;
            boton.Font = TemaApp.FuenteChica;
            boton.Size = new Size(ancho, 28);
            boton.RadioEsquinas = 14;
            boton.Margin = new Padding(0, 0, 8, 0);
            boton.ColorNormal = TemaApp.FondoTarjeta;
            boton.ForeColor = TemaApp.TextoSecundario;
            boton.ColorResaltado = TemaApp.PrincipalClaro;
        }
    }
}
