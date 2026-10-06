using SistemaAdministracionPataHogar.Estilos;

namespace SistemaAdministracionPataHogar.Vistas
{
    partial class VistaTarjetaSolicitud
    {
        private System.ComponentModel.IContainer components = null;

        private PanelRedondeado pnlTarjeta;
        private Label lblNombre;
        private Label lblTipoCiudad;
        private Label lblFecha;
        private PanelRedondeado pnlEstado;
        private Label lblEstado;

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

            pnlTarjeta = new PanelRedondeado();
            lblNombre = new Label();
            lblTipoCiudad = new Label();
            lblFecha = new Label();
            pnlEstado = new PanelRedondeado();
            lblEstado = new Label();

            SuspendLayout();

            pnlTarjeta.Dock = DockStyle.Fill;
            pnlTarjeta.RadioEsquinas = 10;
            pnlTarjeta.ColorBorde = TemaApp.Borde;
            pnlTarjeta.BackColor = TemaApp.FondoTarjeta;
            pnlTarjeta.Padding = new Padding(14, 12, 14, 12);
            pnlTarjeta.Cursor = Cursors.Hand;

            lblNombre.Font = TemaApp.FuenteCuerpoNegrita;
            lblNombre.ForeColor = TemaApp.TextoPrincipal;
            lblNombre.AutoSize = false;
            lblNombre.AutoEllipsis = true;
            lblNombre.Location = new Point(14, 12);
            lblNombre.Size = new Size(220, 20);
            lblNombre.Cursor = Cursors.Hand;

            lblTipoCiudad.Font = TemaApp.FuenteChica;
            lblTipoCiudad.ForeColor = TemaApp.TextoSecundario;
            lblTipoCiudad.AutoSize = false;
            lblTipoCiudad.AutoEllipsis = true;
            lblTipoCiudad.Location = new Point(14, 36);
            lblTipoCiudad.Size = new Size(280, 18);
            lblTipoCiudad.Cursor = Cursors.Hand;

            lblFecha.Font = TemaApp.FuenteChica;
            lblFecha.ForeColor = TemaApp.TextoSecundario;
            lblFecha.AutoSize = false;
            lblFecha.TextAlign = ContentAlignment.MiddleLeft;
            lblFecha.Location = new Point(14, 62);
            lblFecha.Size = new Size(200, 16);
            lblFecha.Cursor = Cursors.Hand;

            pnlEstado.RadioEsquinas = 10;
            pnlEstado.GrosorBorde = 0;
            pnlEstado.Location = new Point(240, 12);
            pnlEstado.Size = new Size(82, 22);
            pnlEstado.Cursor = Cursors.Hand;

            lblEstado.Font = TemaApp.FuenteChica;
            lblEstado.Dock = DockStyle.Fill;
            lblEstado.TextAlign = ContentAlignment.MiddleCenter;
            lblEstado.Cursor = Cursors.Hand;

            pnlEstado.Controls.Add(lblEstado);

            pnlTarjeta.Controls.Add(lblNombre);
            pnlTarjeta.Controls.Add(lblTipoCiudad);
            pnlTarjeta.Controls.Add(lblFecha);
            pnlTarjeta.Controls.Add(pnlEstado);

            Controls.Add(pnlTarjeta);

            Size = new Size(336, 96);
            Margin = new Padding(0, 0, 0, 10);

            // true = aplicar ahora el diseño pendiente (el Dock = Fill del panel
            // interno). Con false, el panel quedaba con su tamaño por defecto.
            ResumeLayout(true);
        }
    }
}
