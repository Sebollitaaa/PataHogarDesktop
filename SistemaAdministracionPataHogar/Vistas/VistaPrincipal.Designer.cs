using SistemaAdministracionPataHogar.Estilos;

namespace SistemaAdministracionPataHogar.Vistas
{
    partial class VistaPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelBarraLateral;
        private Label lblMarca;
        private BotonNavegacion btnSolicitudes;
        private Panel panelPieBarraLateral;
        private Label lblUsuarioActivo;
        private Label lblEstadoBaseDeDatos;
        private LinkLabel lnkCerrarSesion;

        private Panel panelPrincipal;
        private Panel panelEncabezado;
        private Label lblTituloSeccion;
        private Label lblSubtituloSeccion;
        private Panel panelContenido;

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

            panelBarraLateral = new Panel();
            lblMarca = new Label();
            btnSolicitudes = new BotonNavegacion();
            panelPieBarraLateral = new Panel();
            lblUsuarioActivo = new Label();
            lblEstadoBaseDeDatos = new Label();
            lnkCerrarSesion = new LinkLabel();

            panelPrincipal = new Panel();
            panelEncabezado = new Panel();
            lblTituloSeccion = new Label();
            lblSubtituloSeccion = new Label();
            panelContenido = new Panel();

            // ---- Barra lateral (menu de la izquierda) ----

            panelBarraLateral.Dock = DockStyle.Left;
            panelBarraLateral.Width = 240;
            panelBarraLateral.BackColor = TemaApp.BarraLateral;

            lblMarca.Text = "🐾  Pata Hogar";
            lblMarca.Font = new Font(TemaApp.FamiliaFuente, 13F, FontStyle.Bold);
            lblMarca.ForeColor = Color.White;
            lblMarca.AutoSize = false;
            lblMarca.TextAlign = ContentAlignment.MiddleLeft;
            lblMarca.Padding = new Padding(24, 0, 0, 0);
            lblMarca.Location = new Point(0, 0);
            lblMarca.Size = new Size(240, 64);

            btnSolicitudes.Text = "Solicitudes de verificación";
            btnSolicitudes.Location = new Point(0, 84);
            btnSolicitudes.Size = new Size(240, 46);

            // ---- Pie de la barra lateral: usuario, estado de la base y cerrar sesion ----

            panelPieBarraLateral.Dock = DockStyle.Bottom;
            panelPieBarraLateral.Height = 104;
            panelPieBarraLateral.BackColor = TemaApp.BarraLateral;

            lblUsuarioActivo.Text = "👤  admin";
            lblUsuarioActivo.Font = TemaApp.FuenteEtiquetaNegrita;
            lblUsuarioActivo.ForeColor = Color.White;
            lblUsuarioActivo.AutoSize = false;
            lblUsuarioActivo.TextAlign = ContentAlignment.MiddleLeft;
            lblUsuarioActivo.Padding = new Padding(24, 0, 0, 0);
            lblUsuarioActivo.Location = new Point(0, 8);
            lblUsuarioActivo.Size = new Size(240, 22);

            lblEstadoBaseDeDatos.Text = "Verificando base de datos...";
            lblEstadoBaseDeDatos.Font = TemaApp.FuenteChica;
            lblEstadoBaseDeDatos.ForeColor = TemaApp.BarraLateralTexto;
            lblEstadoBaseDeDatos.AutoSize = false;
            lblEstadoBaseDeDatos.TextAlign = ContentAlignment.MiddleLeft;
            lblEstadoBaseDeDatos.Padding = new Padding(24, 0, 8, 0);
            lblEstadoBaseDeDatos.Location = new Point(0, 32);
            lblEstadoBaseDeDatos.Size = new Size(240, 32);

            lnkCerrarSesion.Text = "Cerrar sesión";
            lnkCerrarSesion.LinkColor = TemaApp.BarraLateralTexto;
            lnkCerrarSesion.ActiveLinkColor = Color.White;
            lnkCerrarSesion.VisitedLinkColor = TemaApp.BarraLateralTexto;
            lnkCerrarSesion.LinkBehavior = LinkBehavior.HoverUnderline;
            lnkCerrarSesion.Font = TemaApp.FuenteEtiqueta;
            lnkCerrarSesion.BackColor = Color.Transparent;
            lnkCerrarSesion.AutoSize = false;
            lnkCerrarSesion.TextAlign = ContentAlignment.MiddleLeft;
            lnkCerrarSesion.Padding = new Padding(24, 0, 0, 0);
            lnkCerrarSesion.Location = new Point(0, 68);
            lnkCerrarSesion.Size = new Size(240, 24);

            panelPieBarraLateral.Controls.Add(lblUsuarioActivo);
            panelPieBarraLateral.Controls.Add(lblEstadoBaseDeDatos);
            panelPieBarraLateral.Controls.Add(lnkCerrarSesion);

            panelBarraLateral.Controls.Add(lblMarca);
            panelBarraLateral.Controls.Add(btnSolicitudes);
            panelBarraLateral.Controls.Add(panelPieBarraLateral);

            // ---- Area principal (a la derecha de la barra lateral) ----

            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.BackColor = TemaApp.Fondo;

            panelEncabezado.Dock = DockStyle.Top;
            panelEncabezado.Height = 76;
            panelEncabezado.BackColor = TemaApp.FondoTarjeta;
            panelEncabezado.Padding = new Padding(32, 0, 32, 0);

            lblTituloSeccion.Text = "Bienvenido";
            lblTituloSeccion.Font = TemaApp.FuenteEncabezado;
            lblTituloSeccion.ForeColor = TemaApp.TextoPrincipal;
            lblTituloSeccion.AutoSize = false;
            lblTituloSeccion.Location = new Point(32, 12);
            lblTituloSeccion.Size = new Size(600, 26);

            lblSubtituloSeccion.Text = "Seleccioná una opción del menú para comenzar.";
            lblSubtituloSeccion.Font = TemaApp.FuenteEtiqueta;
            lblSubtituloSeccion.ForeColor = TemaApp.TextoSecundario;
            lblSubtituloSeccion.AutoSize = false;
            lblSubtituloSeccion.Location = new Point(32, 40);
            lblSubtituloSeccion.Size = new Size(700, 22);

            panelEncabezado.Controls.Add(lblTituloSeccion);
            panelEncabezado.Controls.Add(lblSubtituloSeccion);

            panelContenido.Dock = DockStyle.Fill;
            panelContenido.BackColor = TemaApp.Fondo;
            panelContenido.Padding = new Padding(32, 24, 32, 24);

            panelPrincipal.Controls.Add(panelContenido);
            panelPrincipal.Controls.Add(panelEncabezado);

            // ---- Ventana (VistaPrincipal) ----

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 720);
            MinimumSize = new Size(1000, 620);
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            Text = "Pata Hogar — Administración";
            BackColor = TemaApp.Fondo;

            Controls.Add(panelPrincipal);
            Controls.Add(panelBarraLateral);
        }
    }
}
