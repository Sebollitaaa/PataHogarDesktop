using SistemaAdministracionPataHogar.Estilos;

namespace SistemaAdministracionPataHogar.Vistas
{
    partial class VistaInicioSesion
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelIzquierdo;
        private Label lblLogoIcono;
        private Label lblTitulo;
        private Label lblSubtituloIzquierda;

        private Panel panelDerecho;
        private Panel panelFormulario;
        private Label lblIniciarSesion;
        private Label lblSubtituloDerecha;
        private Label lblUsuario;
        private PanelRedondeado pnlUsuario;
        private TextBox txtUsuario;
        private Label lblContrasena;
        private PanelRedondeado pnlContrasena;
        private TextBox txtContrasena;
        private Label lblError;
        private BotonRedondeado btnIngresar;
        private Label lblAyuda;

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
            panelIzquierdo = new Panel();
            lblLogoIcono = new Label();
            lblTitulo = new Label();
            lblSubtituloIzquierda = new Label();
            panelDerecho = new Panel();
            panelFormulario = new Panel();
            lblIniciarSesion = new Label();
            lblSubtituloDerecha = new Label();
            lblUsuario = new Label();
            pnlUsuario = new PanelRedondeado();
            txtUsuario = new TextBox();
            lblContrasena = new Label();
            pnlContrasena = new PanelRedondeado();
            txtContrasena = new TextBox();
            lblError = new Label();
            btnIngresar = new BotonRedondeado();
            lblAyuda = new Label();
            panelIzquierdo.SuspendLayout();
            panelDerecho.SuspendLayout();
            panelFormulario.SuspendLayout();
            pnlUsuario.SuspendLayout();
            pnlContrasena.SuspendLayout();
            SuspendLayout();
            // 
            // panelIzquierdo
            // 
            panelIzquierdo.BackColor = Color.FromArgb(58, 125, 68);
            panelIzquierdo.Controls.Add(lblLogoIcono);
            panelIzquierdo.Controls.Add(lblTitulo);
            panelIzquierdo.Controls.Add(lblSubtituloIzquierda);
            panelIzquierdo.Dock = DockStyle.Left;
            panelIzquierdo.Location = new Point(0, 0);
            panelIzquierdo.Name = "panelIzquierdo";
            panelIzquierdo.Size = new Size(380, 560);
            panelIzquierdo.TabIndex = 1;
            // 
            // lblLogoIcono
            // 
            lblLogoIcono.AutoSize = true;
            lblLogoIcono.BackColor = Color.Transparent;
            lblLogoIcono.Font = new Font("Segoe UI", 42F);
            lblLogoIcono.ForeColor = Color.White;
            lblLogoIcono.Location = new Point(148, 210);
            lblLogoIcono.Name = "lblLogoIcono";
            lblLogoIcono.Size = new Size(109, 74);
            lblLogoIcono.TabIndex = 0;
            lblLogoIcono.Text = "🐾";
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(0, 290);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(380, 42);
            lblTitulo.TabIndex = 1;
            lblTitulo.Text = "Pata Hogar";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubtituloIzquierda
            // 
            lblSubtituloIzquierda.BackColor = Color.Transparent;
            lblSubtituloIzquierda.Font = new Font("Segoe UI", 11F);
            lblSubtituloIzquierda.ForeColor = Color.FromArgb(224, 238, 226);
            lblSubtituloIzquierda.Location = new Point(0, 334);
            lblSubtituloIzquierda.Name = "lblSubtituloIzquierda";
            lblSubtituloIzquierda.Size = new Size(380, 24);
            lblSubtituloIzquierda.TabIndex = 2;
            lblSubtituloIzquierda.Text = "Panel de administración";
            lblSubtituloIzquierda.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelDerecho
            // 
            panelDerecho.BackColor = Color.White;
            panelDerecho.Controls.Add(panelFormulario);
            panelDerecho.Dock = DockStyle.Fill;
            panelDerecho.Location = new Point(380, 0);
            panelDerecho.Name = "panelDerecho";
            panelDerecho.Size = new Size(520, 560);
            panelDerecho.TabIndex = 0;
            // 
            // panelFormulario
            // 
            panelFormulario.Anchor = AnchorStyles.None;
            panelFormulario.BackColor = Color.Transparent;
            panelFormulario.Controls.Add(lblIniciarSesion);
            panelFormulario.Controls.Add(lblSubtituloDerecha);
            panelFormulario.Controls.Add(lblUsuario);
            panelFormulario.Controls.Add(pnlUsuario);
            panelFormulario.Controls.Add(lblContrasena);
            panelFormulario.Controls.Add(pnlContrasena);
            panelFormulario.Controls.Add(lblError);
            panelFormulario.Controls.Add(btnIngresar);
            panelFormulario.Controls.Add(lblAyuda);
            panelFormulario.Location = new Point(100, 100);
            panelFormulario.Name = "panelFormulario";
            panelFormulario.Size = new Size(320, 372);
            panelFormulario.TabIndex = 0;
            // 
            // lblIniciarSesion
            // 
            lblIniciarSesion.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblIniciarSesion.ForeColor = Color.FromArgb(43, 45, 66);
            lblIniciarSesion.Location = new Point(0, 0);
            lblIniciarSesion.Name = "lblIniciarSesion";
            lblIniciarSesion.Size = new Size(320, 30);
            lblIniciarSesion.TabIndex = 0;
            lblIniciarSesion.Text = "Iniciar sesión";
            // 
            // lblSubtituloDerecha
            // 
            lblSubtituloDerecha.Font = new Font("Segoe UI", 9F);
            lblSubtituloDerecha.ForeColor = Color.FromArgb(108, 122, 137);
            lblSubtituloDerecha.Location = new Point(0, 34);
            lblSubtituloDerecha.Name = "lblSubtituloDerecha";
            lblSubtituloDerecha.Size = new Size(320, 36);
            lblSubtituloDerecha.TabIndex = 1;
            lblSubtituloDerecha.Text = "Ingresá tus credenciales de administrador para continuar.";
            // 
            // lblUsuario
            // 
            lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUsuario.ForeColor = Color.FromArgb(43, 45, 66);
            lblUsuario.Location = new Point(0, 84);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(320, 18);
            lblUsuario.TabIndex = 2;
            lblUsuario.Text = "Usuario";
            // 
            // pnlUsuario
            // 
            pnlUsuario.BackColor = Color.White;
            pnlUsuario.ColorBorde = Color.FromArgb(226, 229, 233);
            pnlUsuario.GrosorBorde = 1;
            pnlUsuario.Controls.Add(txtUsuario);
            pnlUsuario.RadioEsquinas = 8;
            pnlUsuario.Location = new Point(0, 104);
            pnlUsuario.Name = "pnlUsuario";
            pnlUsuario.Padding = new Padding(12, 0, 12, 0);
            pnlUsuario.Size = new Size(320, 42);
            pnlUsuario.TabIndex = 3;
            // 
            // txtUsuario
            // 
            txtUsuario.BorderStyle = BorderStyle.None;
            txtUsuario.Dock = DockStyle.Fill;
            txtUsuario.Font = new Font("Segoe UI", 9.5F);
            txtUsuario.ForeColor = Color.FromArgb(43, 45, 66);
            txtUsuario.Location = new Point(12, 0);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.PlaceholderText = "admin";
            txtUsuario.Size = new Size(296, 17);
            txtUsuario.TabIndex = 0;
            // 
            // lblContrasena
            // 
            lblContrasena.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblContrasena.ForeColor = Color.FromArgb(43, 45, 66);
            lblContrasena.Location = new Point(0, 158);
            lblContrasena.Name = "lblContrasena";
            lblContrasena.Size = new Size(320, 18);
            lblContrasena.TabIndex = 4;
            lblContrasena.Text = "Contraseña";
            // 
            // pnlContrasena
            // 
            pnlContrasena.BackColor = Color.White;
            pnlContrasena.ColorBorde = Color.FromArgb(226, 229, 233);
            pnlContrasena.GrosorBorde = 1;
            pnlContrasena.Controls.Add(txtContrasena);
            pnlContrasena.RadioEsquinas = 8;
            pnlContrasena.Location = new Point(0, 178);
            pnlContrasena.Name = "pnlContrasena";
            pnlContrasena.Padding = new Padding(12, 0, 12, 0);
            pnlContrasena.Size = new Size(320, 42);
            pnlContrasena.TabIndex = 5;
            // 
            // txtContrasena
            // 
            txtContrasena.BorderStyle = BorderStyle.None;
            txtContrasena.Dock = DockStyle.Fill;
            txtContrasena.Font = new Font("Segoe UI", 9.5F);
            txtContrasena.ForeColor = Color.FromArgb(43, 45, 66);
            txtContrasena.Location = new Point(12, 0);
            txtContrasena.Name = "txtContrasena";
            txtContrasena.PlaceholderText = "Contraseña";
            txtContrasena.Size = new Size(296, 17);
            txtContrasena.TabIndex = 0;
            txtContrasena.UseSystemPasswordChar = true;
            // 
            // lblError
            // 
            lblError.Font = new Font("Segoe UI", 8F);
            lblError.ForeColor = Color.FromArgb(230, 73, 79);
            lblError.Location = new Point(0, 226);
            lblError.Name = "lblError";
            lblError.Size = new Size(320, 34);
            lblError.TabIndex = 6;
            lblError.Visible = false;
            // 
            // btnIngresar
            // 
            btnIngresar.BackColor = Color.FromArgb(58, 125, 68);
            btnIngresar.ColorResaltado = Color.FromArgb(43, 97, 51);
            btnIngresar.ColorNormal = Color.FromArgb(58, 125, 68);
            btnIngresar.RadioEsquinas = 8;
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Location = new Point(0, 266);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(320, 42);
            btnIngresar.TabIndex = 7;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = false;
            // 
            // lblAyuda
            // 
            lblAyuda.Font = new Font("Segoe UI", 8F);
            lblAyuda.ForeColor = Color.FromArgb(108, 122, 137);
            lblAyuda.Location = new Point(0, 322);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(320, 32);
            lblAyuda.TabIndex = 8;
            lblAyuda.Text = "Usuario de administrador precargado para esta versión de prueba.";
            lblAyuda.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // VistaInicioSesion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(900, 560);
            Controls.Add(panelDerecho);
            Controls.Add(panelIzquierdo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "VistaInicioSesion";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pata Hogar — Acceso de administrador";
            panelIzquierdo.ResumeLayout(false);
            panelIzquierdo.PerformLayout();
            panelDerecho.ResumeLayout(false);
            panelFormulario.ResumeLayout(false);
            pnlUsuario.ResumeLayout(false);
            pnlUsuario.PerformLayout();
            pnlContrasena.ResumeLayout(false);
            pnlContrasena.PerformLayout();
            ResumeLayout(false);
        }
    }
}
