using SistemaAdministracionPataHogar.Modelos;
using SistemaAdministracionPataHogar.Estilos;

namespace SistemaAdministracionPataHogar.Vistas
{
    // VISTA de la pantalla de Solicitudes de Verificacion.
    //
    // Que hace: dibuja la lista de tarjetas, los botones de filtro y el panel
    // de detalle, y avisa con eventos cuando el usuario hace algo.
    // Que NO hace: no lee la base de datos, no filtra, no aprueba ni rechaza,
    // no abre archivos. Todo eso lo hace ControladorSolicitudesVerificacion.
    //
    // Se organiza en tres bloques:
    //   1) Propiedades y eventos del contrato (IVistaSolicitudesVerificacion)
    //   2) Lista de tarjetas y filtros (columna izquierda)
    //   3) Panel de detalle (columna derecha)
    public partial class VistaSolicitudesVerificacion : UserControl, IVistaSolicitudesVerificacion
    {
        // Ancho fijo que usamos para todo lo que dibujamos en el panel de detalle.
        private const int AnchoContenido = 660;

        // ---- Datos que se guardan para que el controlador los pueda leer ----
        private string _filtroElegido = "Todas";
        private SolicitudVerificacion _solicitudElegida = null;
        private string _rutaDocumentoElegido = null;
        private TextBox _txtMotivoRechazo = null;

        // Todas las tarjetas dibujadas actualmente en la lista.
        private List<VistaTarjetaSolicitud> _tarjetas = new List<VistaTarjetaSolicitud>();

        // Posicion vertical (en pixeles) donde va el PROXIMO control que
        // agreguemos al panel de detalle. Es como un cursor que va bajando
        // a medida que vamos agregando cosas.
        private int _proximoY = 0;

        // ---------------------------------------------------------------
        // 1) Contrato con el controlador
        // ---------------------------------------------------------------

        public event EventHandler FiltroCambiado;
        public event EventHandler SolicitudSeleccionada;
        public event EventHandler AbrirDocumentoSolicitado;
        public event EventHandler AprobarSolicitado;
        public event EventHandler RechazarSolicitado;

        public string FiltroElegido
        {
            get { return _filtroElegido; }
        }

        public SolicitudVerificacion SolicitudElegida
        {
            get { return _solicitudElegida; }
        }

        public string RutaDocumentoElegido
        {
            get { return _rutaDocumentoElegido; }
        }

        public string MotivoRechazoEscrito
        {
            get
            {
                if (_txtMotivoRechazo == null) return string.Empty;
                return _txtMotivoRechazo.Text;
            }
        }

        public VistaSolicitudesVerificacion()
        {
            InitializeComponent();

            btnFiltroTodas.Click += BotonDeFiltro_Click;
            btnFiltroPendientes.Click += BotonDeFiltro_Click;
            btnFiltroAprobadas.Click += BotonDeFiltro_Click;
            btnFiltroRechazadas.Click += BotonDeFiltro_Click;
        }

        // ---------------------------------------------------------------
        // 2) Filtros y lista de tarjetas (columna izquierda)
        // ---------------------------------------------------------------

        // Los 4 botones de filtro llaman a este mismo metodo. Sabemos cual
        // se apreto mirando su propiedad Tag (la configuramos en el Designer).
        private void BotonDeFiltro_Click(object sender, EventArgs e)
        {
            BotonRedondeado boton = (BotonRedondeado)sender;
            _filtroElegido = (string)boton.Tag;

            if (FiltroCambiado != null)
            {
                FiltroCambiado(this, EventArgs.Empty);
            }
        }

        // Dibuja una tarjeta por cada solicitud que le pasa el controlador.
        // La vista no decide cuales se muestran: solo dibuja lo que le dan.
        public void MostrarListado(List<SolicitudVerificacion> solicitudes, string filtroActivo, int idSeleccionado)
        {
            _filtroElegido = filtroActivo;

            flujoLista.Controls.Clear();
            _tarjetas.Clear();

            foreach (SolicitudVerificacion solicitud in solicitudes)
            {
                VistaTarjetaSolicitud tarjeta = new VistaTarjetaSolicitud();
                tarjeta.CargarDatos(solicitud);
                tarjeta.Seleccionada += Tarjeta_Seleccionada;

                flujoLista.Controls.Add(tarjeta);
                _tarjetas.Add(tarjeta);
            }

            if (_tarjetas.Count == 0)
            {
                Label lblVacio = new Label();
                lblVacio.Text = "No hay solicitudes en esta categoría.";
                lblVacio.Font = TemaApp.FuenteEtiqueta;
                lblVacio.ForeColor = TemaApp.TextoSecundario;
                lblVacio.AutoSize = false;
                lblVacio.TextAlign = ContentAlignment.MiddleCenter;
                lblVacio.Size = new Size(320, 60);
                flujoLista.Controls.Add(lblVacio);
            }

            ActualizarBotonesDeFiltro();
            MarcarSolicitudSeleccionada(idSeleccionado);
        }

        private void ActualizarBotonesDeFiltro()
        {
            ActualizarUnBotonDeFiltro(btnFiltroTodas);
            ActualizarUnBotonDeFiltro(btnFiltroPendientes);
            ActualizarUnBotonDeFiltro(btnFiltroAprobadas);
            ActualizarUnBotonDeFiltro(btnFiltroRechazadas);
        }

        private void ActualizarUnBotonDeFiltro(BotonRedondeado boton)
        {
            bool estaActivo = ((string)boton.Tag) == _filtroElegido;

            if (estaActivo)
            {
                boton.ColorNormal = TemaApp.Principal;
                boton.ForeColor = Color.White;
                boton.ColorResaltado = TemaApp.PrincipalOscuro;
            }
            else
            {
                boton.ColorNormal = TemaApp.FondoTarjeta;
                boton.ForeColor = TemaApp.TextoSecundario;
                boton.ColorResaltado = TemaApp.PrincipalClaro;
            }

            boton.Invalidate();
        }

        // Pinta con borde verde la tarjeta de la solicitud seleccionada (y
        // deja normales a las demas). Con idSolicitud = 0 no queda ninguna marcada.
        public void MarcarSolicitudSeleccionada(int idSolicitud)
        {
            foreach (VistaTarjetaSolicitud tarjeta in _tarjetas)
            {
                tarjeta.MarcarSeleccion(tarjeta.Solicitud.Id == idSolicitud);
            }
        }

        // Una tarjeta aviso que le hicieron clic: guardamos cual fue y avisamos al controlador.
        private void Tarjeta_Seleccionada(object sender, EventArgs e)
        {
            VistaTarjetaSolicitud tarjetaClickeada = (VistaTarjetaSolicitud)sender;
            _solicitudElegida = tarjetaClickeada.Solicitud;

            if (SolicitudSeleccionada != null)
            {
                SolicitudSeleccionada(this, EventArgs.Empty);
            }
        }

        // ---------------------------------------------------------------
        // 3) Panel de detalle (columna derecha)
        // ---------------------------------------------------------------

        public void MostrarDetalleVacio()
        {
            pnlDetalleContenido.Controls.Clear();
            pnlDetalleContenido.AutoScrollMinSize = Size.Empty;
            _txtMotivoRechazo = null;

            Label lbl = new Label();
            lbl.Text = "Seleccioná una solicitud de la lista para ver el detalle.";
            lbl.Font = TemaApp.FuenteSubtitulo;
            lbl.ForeColor = TemaApp.TextoSecundario;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            pnlDetalleContenido.Controls.Add(lbl);
        }

        // Arma, control por control, todo el panel derecho. Cada metodo
        // "Agregar..." coloca algo en la posicion del cursor (_proximoY) y
        // despues baja el cursor.
        public void MostrarDetalle(SolicitudVerificacion solicitud)
        {
            pnlDetalleContenido.Controls.Clear();
            pnlDetalleContenido.AutoScrollPosition = new Point(0, 0);

            _txtMotivoRechazo = null;
            _proximoY = 0;

            AgregarNombreOrganizacion(solicitud);
            AgregarFilaTipoYEstado(solicitud);

            AgregarCampo("Responsable", solicitud.NombreResponsable);
            AgregarCampo("Email", solicitud.Correo);
            AgregarCampo("Teléfono", solicitud.Telefono);
            AgregarCampo("Dirección", solicitud.Direccion + ", " + solicitud.Ciudad + ", " + solicitud.Provincia);
            AgregarCampo("Antigüedad", solicitud.AniosFuncionamiento + " años en funcionamiento");
            AgregarCampo("Animales albergados", ObtenerTextoAnimalesAlbergados(solicitud));
            AgregarCampo("Sitio web / Redes", ObtenerTextoSitioWeb(solicitud));
            AgregarCampo("Fecha de solicitud", solicitud.FechaSolicitud.ToString("dd/MM/yyyy HH:mm"));

            _proximoY += 10;
            AgregarTituloDeSeccion("Descripción");
            AgregarTextoLargo(solicitud.Descripcion);

            AgregarTituloDeSeccion("Documentación adjunta");
            foreach (DocumentoAdjunto documento in solicitud.Documentos)
            {
                AgregarDocumento(documento);
            }

            _proximoY += 10;
            AgregarLineaSeparadora();

            if (solicitud.Estado == EstadoSolicitud.Pendiente)
            {
                AgregarSeccionAprobarRechazar();
            }
            else
            {
                AgregarSeccionYaResuelta(solicitud);
            }

            pnlDetalleContenido.AutoScrollMinSize = new Size(0, _proximoY + 24);
        }

        private string ObtenerTextoAnimalesAlbergados(SolicitudVerificacion solicitud)
        {
            if (solicitud.AnimalesAlbergados > 0) return solicitud.AnimalesAlbergados.ToString();
            return "No aplica";
        }

        private string ObtenerTextoSitioWeb(SolicitudVerificacion solicitud)
        {
            if (string.IsNullOrEmpty(solicitud.SitioWebORedes)) return "No informado";
            return solicitud.SitioWebORedes;
        }

        private void AgregarNombreOrganizacion(SolicitudVerificacion solicitud)
        {
            Label lbl = new Label();
            lbl.Text = solicitud.NombreOrganizacion;
            lbl.Font = TemaApp.FuenteEncabezado;
            lbl.ForeColor = TemaApp.TextoPrincipal;
            lbl.AutoSize = false;
            lbl.Location = new Point(0, _proximoY);
            lbl.Size = new Size(AnchoContenido, 30);

            pnlDetalleContenido.Controls.Add(lbl);
            _proximoY += 34;
        }

        private void AgregarFilaTipoYEstado(SolicitudVerificacion solicitud)
        {
            Label lblTipo = new Label();
            lblTipo.Text = TipoOrganizacionAyudante.ObtenerIcono(solicitud.Tipo) + " " + TipoOrganizacionAyudante.ObtenerTexto(solicitud.Tipo);
            lblTipo.Font = TemaApp.FuenteEtiqueta;
            lblTipo.ForeColor = TemaApp.TextoSecundario;
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(0, _proximoY + 4);

            PanelRedondeado etiquetaEstado = CrearEtiquetaDeEstado(solicitud.Estado);
            etiquetaEstado.Location = new Point(160, _proximoY);

            pnlDetalleContenido.Controls.Add(lblTipo);
            pnlDetalleContenido.Controls.Add(etiquetaEstado);
            _proximoY += 40;
        }

        private static PanelRedondeado CrearEtiquetaDeEstado(EstadoSolicitud estado)
        {
            PanelRedondeado etiqueta = new PanelRedondeado();
            etiqueta.RadioEsquinas = 10;
            etiqueta.GrosorBorde = 0;
            etiqueta.BackColor = TemaApp.ObtenerColorDeFondoDeEstado(estado);
            etiqueta.Size = new Size(90, 22);

            Label lblTexto = new Label();
            lblTexto.Text = EstadoSolicitudAyudante.ObtenerTexto(estado);
            lblTexto.Font = TemaApp.FuenteChica;
            lblTexto.ForeColor = TemaApp.ObtenerColorDeEstado(estado);
            lblTexto.Dock = DockStyle.Fill;
            lblTexto.TextAlign = ContentAlignment.MiddleCenter;

            etiqueta.Controls.Add(lblTexto);
            return etiqueta;
        }

        // Agrega un campo del estilo "Titulo chico arriba / valor abajo",
        // uno debajo del otro. Cada campo ocupa 44 pixeles de alto.
        private void AgregarCampo(string titulo, string valor)
        {
            Label lblTitulo = new Label();
            lblTitulo.Text = titulo;
            lblTitulo.Font = TemaApp.FuenteChica;
            lblTitulo.ForeColor = TemaApp.TextoSecundario;
            lblTitulo.AutoSize = false;
            lblTitulo.Location = new Point(0, _proximoY);
            lblTitulo.Size = new Size(AnchoContenido, 16);

            Label lblValor = new Label();
            lblValor.Text = valor;
            lblValor.Font = TemaApp.FuenteCuerpo;
            lblValor.ForeColor = TemaApp.TextoPrincipal;
            lblValor.AutoSize = false;
            lblValor.AutoEllipsis = true;
            lblValor.Location = new Point(0, _proximoY + 18);
            lblValor.Size = new Size(AnchoContenido, 20);

            pnlDetalleContenido.Controls.Add(lblTitulo);
            pnlDetalleContenido.Controls.Add(lblValor);
            _proximoY += 44;
        }

        private void AgregarTituloDeSeccion(string texto)
        {
            Label lbl = new Label();
            lbl.Text = texto;
            lbl.Font = TemaApp.FuenteCuerpoNegrita;
            lbl.ForeColor = TemaApp.TextoPrincipal;
            lbl.AutoSize = false;
            lbl.Location = new Point(0, _proximoY);
            lbl.Size = new Size(AnchoContenido, 20);

            pnlDetalleContenido.Controls.Add(lbl);
            _proximoY += 28;
        }

        private void AgregarTextoLargo(string texto)
        {
            Label lbl = new Label();
            lbl.Text = texto;
            lbl.Font = TemaApp.FuenteCuerpo;
            lbl.ForeColor = TemaApp.TextoPrincipal;
            lbl.AutoSize = false;
            lbl.Location = new Point(0, _proximoY);
            lbl.Size = new Size(AnchoContenido, 60);

            pnlDetalleContenido.Controls.Add(lbl);
            _proximoY += 74;
        }

        private void AgregarDocumento(DocumentoAdjunto documento)
        {
            Label lblIcono = new Label();
            lblIcono.Text = "📄";
            lblIcono.Font = TemaApp.FuenteCuerpo;
            lblIcono.Location = new Point(0, _proximoY);
            lblIcono.Size = new Size(26, 24);

            Label lblNombre = new Label();
            lblNombre.Text = documento.Nombre;
            lblNombre.Font = TemaApp.FuenteCuerpo;
            lblNombre.ForeColor = TemaApp.TextoPrincipal;
            lblNombre.AutoSize = false;
            lblNombre.AutoEllipsis = true;
            lblNombre.TextAlign = ContentAlignment.MiddleLeft;
            lblNombre.Location = new Point(28, _proximoY);
            lblNombre.Size = new Size(AnchoContenido - 140, 24);

            // Guardamos la ruta del archivo en el Tag del boton, para poder
            // recuperarla despues en BotonAbrirDocumento_Click.
            BotonRedondeado btnAbrir = new BotonRedondeado();
            btnAbrir.Text = "Abrir";
            btnAbrir.Tag = documento.RutaArchivo;
            btnAbrir.Font = TemaApp.FuenteChica;
            btnAbrir.RadioEsquinas = 6;
            btnAbrir.Location = new Point(AnchoContenido - 90, _proximoY - 3);
            btnAbrir.Size = new Size(90, 30);
            btnAbrir.Click += BotonAbrirDocumento_Click;

            pnlDetalleContenido.Controls.Add(lblIcono);
            pnlDetalleContenido.Controls.Add(lblNombre);
            pnlDetalleContenido.Controls.Add(btnAbrir);
            _proximoY += 38;
        }

        // La vista NO abre el archivo: guarda la ruta y avisa al controlador.
        private void BotonAbrirDocumento_Click(object sender, EventArgs e)
        {
            BotonRedondeado boton = (BotonRedondeado)sender;
            _rutaDocumentoElegido = (string)boton.Tag;

            if (AbrirDocumentoSolicitado != null)
            {
                AbrirDocumentoSolicitado(this, EventArgs.Empty);
            }
        }

        private void AgregarLineaSeparadora()
        {
            Panel linea = new Panel();
            linea.Location = new Point(0, _proximoY);
            linea.Size = new Size(AnchoContenido, 1);
            linea.BackColor = TemaApp.Borde;

            pnlDetalleContenido.Controls.Add(linea);
            _proximoY += 20;
        }

        // Caja de motivo + botones Aprobar/Rechazar (solo para solicitudes Pendientes).
        private void AgregarSeccionAprobarRechazar()
        {
            Label lblMotivoTitulo = new Label();
            lblMotivoTitulo.Text = "Motivo de rechazo (obligatorio si vas a rechazar la solicitud)";
            lblMotivoTitulo.Font = TemaApp.FuenteEtiquetaNegrita;
            lblMotivoTitulo.ForeColor = TemaApp.TextoPrincipal;
            lblMotivoTitulo.AutoSize = false;
            lblMotivoTitulo.Location = new Point(0, _proximoY);
            lblMotivoTitulo.Size = new Size(AnchoContenido, 18);
            pnlDetalleContenido.Controls.Add(lblMotivoTitulo);
            _proximoY += 22;

            PanelRedondeado pnlMotivo = new PanelRedondeado();
            pnlMotivo.Location = new Point(0, _proximoY);
            pnlMotivo.Size = new Size(AnchoContenido, 70);
            pnlMotivo.RadioEsquinas = 8;
            pnlMotivo.Padding = new Padding(12, 8, 12, 8);

            TextBox txtMotivo = new TextBox();
            txtMotivo.Multiline = true;
            txtMotivo.BorderStyle = BorderStyle.None;
            txtMotivo.Dock = DockStyle.Fill;
            txtMotivo.Font = TemaApp.FuenteCuerpo;
            txtMotivo.ForeColor = TemaApp.TextoPrincipal;
            txtMotivo.PlaceholderText = "Ej: la documentación no corresponde al domicilio declarado…";

            pnlMotivo.Controls.Add(txtMotivo);
            pnlDetalleContenido.Controls.Add(pnlMotivo);
            _txtMotivoRechazo = txtMotivo;
            _proximoY += 82;

            FlowLayoutPanel filaBotones = new FlowLayoutPanel();
            filaBotones.Location = new Point(0, _proximoY);
            filaBotones.Size = new Size(AnchoContenido, 44);
            filaBotones.FlowDirection = FlowDirection.LeftToRight;
            filaBotones.WrapContents = false;

            BotonRedondeado btnAprobar = new BotonRedondeado();
            btnAprobar.Text = "✔  Aprobar verificación";
            btnAprobar.Size = new Size(220, 42);
            btnAprobar.ColorNormal = TemaApp.Exito;
            btnAprobar.ColorResaltado = Color.FromArgb(34, 140, 90);
            btnAprobar.Margin = new Padding(0, 0, 12, 0);
            btnAprobar.Click += BotonAprobar_Click;

            BotonRedondeado btnRechazar = new BotonRedondeado();
            btnRechazar.Text = "✖  Rechazar solicitud";
            btnRechazar.Size = new Size(200, 42);
            btnRechazar.ColorNormal = TemaApp.Peligro;
            btnRechazar.ColorResaltado = Color.FromArgb(185, 50, 56);
            btnRechazar.Click += BotonRechazar_Click;

            filaBotones.Controls.Add(btnAprobar);
            filaBotones.Controls.Add(btnRechazar);
            pnlDetalleContenido.Controls.Add(filaBotones);
            _proximoY += 44;
        }

        private void BotonAprobar_Click(object sender, EventArgs e)
        {
            if (AprobarSolicitado != null)
            {
                AprobarSolicitado(this, EventArgs.Empty);
            }
        }

        private void BotonRechazar_Click(object sender, EventArgs e)
        {
            if (RechazarSolicitado != null)
            {
                RechazarSolicitado(this, EventArgs.Empty);
            }
        }

        // Cartel de resultado (aprobada/rechazada) para solicitudes ya resueltas.
        private void AgregarSeccionYaResuelta(SolicitudVerificacion solicitud)
        {
            bool aprobada = solicitud.Estado == EstadoSolicitud.Aprobada;

            int altoDelPanel = 56;
            if (!aprobada) altoDelPanel = 82;

            PanelRedondeado pnlResolucion = new PanelRedondeado();
            pnlResolucion.Location = new Point(0, _proximoY);
            pnlResolucion.Size = new Size(AnchoContenido, altoDelPanel);
            pnlResolucion.RadioEsquinas = 8;
            pnlResolucion.GrosorBorde = 0;
            pnlResolucion.BackColor = TemaApp.ObtenerColorDeFondoDeEstado(solicitud.Estado);
            pnlResolucion.Padding = new Padding(16, 12, 16, 12);

            string textoResumen;
            if (aprobada)
            {
                textoResumen = "✔  Organización verificada el " + solicitud.FechaResolucion.Value.ToString("dd/MM/yyyy") + ".";
            }
            else
            {
                textoResumen = "✖  Solicitud rechazada el " + solicitud.FechaResolucion.Value.ToString("dd/MM/yyyy") + ".";
            }

            Label lblResumen = new Label();
            lblResumen.Text = textoResumen;
            lblResumen.Font = TemaApp.FuenteCuerpoNegrita;
            lblResumen.ForeColor = TemaApp.ObtenerColorDeEstado(solicitud.Estado);
            lblResumen.AutoSize = false;
            lblResumen.Location = new Point(16, 10);
            lblResumen.Size = new Size(AnchoContenido - 32, 20);
            pnlResolucion.Controls.Add(lblResumen);

            if (!aprobada && !string.IsNullOrEmpty(solicitud.MotivoRechazo))
            {
                Label lblMotivo = new Label();
                lblMotivo.Text = "Motivo: " + solicitud.MotivoRechazo;
                lblMotivo.Font = TemaApp.FuenteChica;
                lblMotivo.ForeColor = TemaApp.TextoPrincipal;
                lblMotivo.AutoSize = false;
                lblMotivo.Location = new Point(16, 34);
                lblMotivo.Size = new Size(AnchoContenido - 32, 40);
                pnlResolucion.Controls.Add(lblMotivo);
            }

            pnlDetalleContenido.Controls.Add(pnlResolucion);
            _proximoY += altoDelPanel;
        }

        // ---------------------------------------------------------------
        // Mensajes al usuario (cuadros de dialogo)
        // ---------------------------------------------------------------

        public void MostrarInformacion(string titulo, string mensaje)
        {
            MessageBox.Show(this, mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MostrarAdvertencia(string titulo, string mensaje)
        {
            MessageBox.Show(this, mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void MostrarError(string titulo, string mensaje)
        {
            MessageBox.Show(this, mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool Confirmar(string titulo, string mensaje)
        {
            DialogResult resultado = MessageBox.Show(this, mensaje, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return resultado == DialogResult.Yes;
        }
    }
}
