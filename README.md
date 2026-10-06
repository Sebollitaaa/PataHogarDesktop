# Pata Hogar - Panel de administración (escritorio)

Aplicación de escritorio para Windows que usa el administrador de **Pata Hogar**, un sitio de
adopción de mascotas. Desde acá se revisan las **solicitudes de verificación** que envían
refugios, veterinarias y asociaciones protectoras desde la web, y se decide si se les otorga la
cuenta verificada (aprobar) o no (rechazar, con un motivo).

Está desarrollada en **C# con Windows Forms**, usa el patrón **MVC** y trabaja sobre la misma
base de datos **MySQL** que el sitio web.

> Proyecto académico. La web y la app móvil de Pata Hogar son proyectos aparte; este
> repositorio contiene solamente la aplicación de escritorio.

---

## Tabla de contenidos

1. [Funcionalidades](#funcionalidades)
2. [Tecnologías](#tecnologías)
3. [Arquitectura del proyecto](#arquitectura-del-proyecto)
4. [Requisitos previos](#requisitos-previos)
5. [Instalación](#instalación)
   - [Parte A: sistema web y base de datos](#parte-a-sistema-web-y-base-de-datos)
   - [Parte B: aplicación de escritorio](#parte-b-aplicación-de-escritorio)
6. [Configuración](#configuración)
7. [Base de datos](#base-de-datos)
8. [Ejecución](#ejecución)
9. [Cómo funciona (flujo de uso)](#cómo-funciona-flujo-de-uso)
10. [Flujo de trabajo para desarrollar](#flujo-de-trabajo-para-desarrollar)
11. [Seguridad y datos confidenciales](#seguridad-y-datos-confidenciales)
12. [Solución de problemas](#solución-de-problemas)
13. [Limitaciones conocidas y próximos pasos](#limitaciones-conocidas-y-próximos-pasos)
14. [Documentación](#documentación)
15. [Licencia](#licencia)
16. [Autores](#autores)

---

## Funcionalidades

- **Inicio de sesión** de administrador.
- **Menú lateral** con indicador del estado de la conexión a la base de datos.
- **Solicitudes de verificación**:
  - Listado de solicitudes leídas de la base de datos real.
  - Filtros por estado: Todas, Pendientes, Aprobadas y Rechazadas.
  - Detalle completo de cada solicitud (datos de la organización, descripción y documentos).
  - Apertura de cada **documento adjunto** con el programa que Windows tenga asociado.
  - **Aprobar** o **rechazar** (el rechazo exige escribir un motivo). Los cambios se guardan
    en la base de datos.
- **Integración con el sistema web**: al aprobar o rechazar una solicitud, el sistema web
  detecta el cambio y le asigna al usuario la insignia de organización verificada y le envía una
  notificación (ver [Cómo funciona](#cómo-funciona-flujo-de-uso)).
- **Cerrar sesión** y volver a la pantalla de inicio de sesión.
- Si la base de datos no responde, la aplicación **no se cierra**: muestra datos de ejemplo de
  respaldo y avisa con un indicador rojo.

## Tecnologías

| Tecnología | Uso |
|---|---|
| C# / .NET 10 (`net10.0-windows`) | Lenguaje y plataforma |
| Windows Forms | Interfaz gráfica |
| MySQL 8.4 | Base de datos (compartida con el sitio web) |
| MySqlConnector 2.6.2 | Conexión de C# con MySQL (paquete NuGet) |
| GDI+ (`System.Drawing`) | Dibujo de los botones y paneles con esquinas redondeadas |

## Arquitectura del proyecto

El sistema sigue el patrón **MVC (Modelo - Vista - Controlador)** con vistas pasivas: las
vistas solo dibujan y avisan con eventos, y los controladores contienen toda la lógica.

```
SistemaAdministracionPataHogar/
|-- SistemaAdministracionPataHogar.slnx        solución de Visual Studio
|-- README.md
|-- DOCUMENTACION_TECNICA.md / .pdf            documentación técnica detallada
|-- base_de_datos/
|   `-- crear_base_y_usuario.sql               script SQL: crea la base y el usuario
`-- SistemaAdministracionPataHogar/            proyecto
    |-- configuracion.ejemplo.json             plantilla de configuración (sin datos reales)
    |-- Programa.cs                            punto de entrada
    |-- Modelos/          entidades y reglas (SolicitudVerificacion, ServicioAutenticacion...)
    |-- Datos/            acceso a la base (ConexionBaseDatos, RepositorioSolicitudes)
    |-- Vistas/           pantallas e interfaces (VistaInicioSesion, VistaPrincipal...)
    |-- Controladores/    lógica de cada pantalla
    `-- Estilos/          colores y controles visuales reutilizables
```

| Capa | Carpetas | Responsabilidad |
|---|---|---|
| Modelo | `Modelos/`, `Datos/` | Qué datos hay, qué reglas se cumplen y cómo se leen o guardan |
| Vista | `Vistas/`, `Estilos/` | Cómo se ve cada pantalla y qué avisa el usuario |
| Controlador | `Controladores/` | Qué hacer ante cada acción del usuario |

Reglas de dependencia: las vistas nunca llaman a `Datos/` ni a `Controladores/`, y los
controladores no usan nada de Windows Forms (solo conocen las interfaces de las vistas).

## Requisitos previos

- **Windows 10 u 11**.
- **.NET 10 SDK** (<https://dotnet.microsoft.com/download>).
- **Visual Studio 2026** (o una versión que soporte .NET 10) con la carga de trabajo
  *Desarrollo de escritorio con .NET*. Es opcional: también se puede compilar y ejecutar desde
  la terminal con el comando `dotnet`.
- **MySQL 8.x** (servidor), con permiso para crear bases de datos y usuarios.
- **Node.js (versión LTS)**, necesario para instalar y ejecutar el **sistema web de Pata Hogar**,
  que es quien genera la estructura de la base de datos y guarda los archivos adjuntos.
- **El sistema web de Pata Hogar** (repositorio aparte), instalado en la misma computadora.
  Esta aplicación de escritorio **no funciona de forma independiente**: depende de la base de
  datos y de los archivos del sistema web.
- **Git**, para clonar los repositorios.

## Instalación

El sistema completo tiene dos partes que se instalan **en este orden**: primero el sistema web
con su base de datos (Parte A) y después esta aplicación de escritorio (Parte B).

### Parte A: sistema web y base de datos

1. **Instalar MySQL 8.x** y dejar el servidor encendido. Anotar el puerto en el que escucha
   (3306 por defecto).

2. **Ejecutar el script SQL que crea la base de datos y el usuario.** El script está en
   [`base_de_datos/crear_base_y_usuario.sql`](base_de_datos/crear_base_y_usuario.sql). Antes de
   ejecutarlo, abrirlo y reemplazar `CAMBIAR_CONTRASENA` por una contraseña propia:

   ```bash
   mysql -u root -p < base_de_datos/crear_base_y_usuario.sql
   ```

   Si el servidor no usa el puerto 3306, agregar `-P <puerto>` al comando.

3. **Clonar e instalar el sistema web** (repositorio aparte):

   ```bash
   git clone <URL-DEL-REPOSITORIO-DEL-SISTEMA-WEB>
   cd <carpeta-del-sistema-web>/backend
   npm install
   cd ../frontend
   npm install
   ```

4. **Configurar el sistema web**: completar `backend/.env` con los datos de conexión a la base
   (servidor, puerto, nombre de la base, usuario y la contraseña elegida en el paso 2), tal como
   indica la documentación del sistema web.

5. **Generar la estructura de la base de datos** (las tablas). Desde la carpeta `backend`:

   ```bash
   npm run migrate
   npm run seed
   ```

   `migrate` crea todas las tablas, incluidas `verification_requests` y
   `verification_documents`, que son las que usa esta aplicación. `seed` carga los datos base
   (especies y ciudades).

   > **Alternativa con un script SQL exportado:** si se parte de un backup del sistema web,
   > restaurarlo con `mysql -u root -p < archivo.sql` y ejecutar después `npm run migrate` para
   > aplicar las migraciones pendientes. Un backup anterior a las tablas de verificación no las
   > incluye.

6. **Iniciar el sistema web** (backend y frontend) siguiendo su documentación y comprobar que
   abre en el navegador. **El backend debe quedar en ejecución**: es el que aplica la insignia
   de verificado y envía la notificación cuando se resuelve una solicitud.

7. **Cargar una solicitud de prueba**: con un usuario registrado, completar el formulario
   `/solicitar-verificacion` del sitio web. Esa solicitud aparecerá en la aplicación de
   escritorio.

### Parte B: aplicación de escritorio

1. **Clonar este repositorio**

   ```bash
   git clone <URL-DE-ESTE-REPOSITORIO>
   cd <carpeta-del-repositorio>
   ```

2. **Crear el archivo de configuración local** a partir de la plantilla (ver
   [Configuración](#configuración)):

   ```bash
   copy SistemaAdministracionPataHogar\configuracion.ejemplo.json SistemaAdministracionPataHogar\configuracion.local.json
   ```

   Después abrir `configuracion.local.json` y completar los datos reales (los mismos de la base
   creada en la Parte A).

3. **Restaurar paquetes y compilar**

   ```bash
   dotnet build SistemaAdministracionPataHogar.slnx
   ```

   (Visual Studio restaura los paquetes NuGet automáticamente al abrir la solución.)

## Configuración

Los datos sensibles **no están escritos en el código**: se leen de un archivo local llamado
`configuracion.local.json`, que está en el `.gitignore` y **nunca se sube al repositorio**.
En el repositorio solo se incluye la plantilla `configuracion.ejemplo.json`, con valores de
ejemplo.

```json
{
  "baseDeDatos": {
    "servidor": "127.0.0.1",
    "puerto": "3306",
    "nombre": "nombre_de_la_base",
    "usuario": "usuario_de_la_base",
    "contrasena": "contrasena_de_la_base"
  },
  "administrador": {
    "usuario": "usuario_administrador",
    "contrasena": "contrasena_administrador"
  }
}
```

| Clave | Qué es |
|---|---|
| `baseDeDatos.servidor` / `puerto` | Dónde escucha el servidor MySQL |
| `baseDeDatos.nombre` | Nombre de la base de datos del sitio web |
| `baseDeDatos.usuario` / `contrasena` | Credenciales de MySQL con permiso de lectura y escritura sobre las tablas de verificación |
| `administrador.usuario` / `contrasena` | Credenciales para ingresar a esta aplicación |

> Si el archivo falta, no tiene formato JSON válido o le falta algún dato, la aplicación muestra un
> mensaje que indica qué hay que corregir y no inicia. Conviene elegir contraseñas propias y no
> reutilizar las de otros servicios.

## Base de datos

La estructura de la base de datos la crea el **sistema web** (script SQL de creación de la base y
migraciones, ver la [Parte A](#parte-a-sistema-web-y-base-de-datos) de la instalación). Esta aplicación
**no crea ni modifica tablas**: solo lee y actualiza estas dos:

**`verification_requests`** (una fila por solicitud)

| Columna | Descripción |
|---|---|
| `id` | Identificador de la solicitud |
| `organization_name`, `organization_type` | Nombre y tipo (`refugio`, `veterinaria`, `asociacion`) |
| `responsible_name`, `email`, `phone` | Datos de contacto |
| `address`, `city`, `province` | Ubicación |
| `years_in_operation`, `animals_housed` | Antigüedad y cantidad de animales |
| `website`, `description` | Sitio web (opcional) y descripción |
| `status` | `pendiente`, `aprobada` o `rechazada` |
| `rejection_reason`, `resolved_at` | Motivo y fecha de resolución (se completan al resolver) |
| `created_at` | Fecha de la solicitud |

**`verification_documents`** (una fila por archivo adjunto)

| Columna | Descripción |
|---|---|
| `request_id` | Solicitud a la que pertenece |
| `original_filename` | Nombre original del archivo |
| `file_path` | Ruta absoluta del archivo en el disco |

Los archivos adjuntos (fotos, PDF) **no están en la base**: la base guarda solo la ruta. Por
eso la aplicación debe correr en la misma computadora donde el sitio web guarda los archivos.

## Ejecución

Con Visual Studio: abrir `SistemaAdministracionPataHogar.slnx` y presionar **F5**.

Desde la terminal:

```bash
dotnet run --project SistemaAdministracionPataHogar
```

Antes de abrir la aplicación hay que tener **el servidor MySQL encendido**. Si no lo está, la
aplicación igual abre, pero muestra el indicador rojo y datos de ejemplo.

## Cómo funciona (flujo de uso)

```
Inicio de sesión  ->  Ventana principal  ->  Solicitudes de verificación
                                                    |
                          +-------------------------+-------------------------+
                          v                         v                         v
                    Filtrar por estado     Elegir una solicitud       Abrir documentos
                                                    |
                                      +-------------+-------------+
                                      v                           v
                                   Aprobar                 Rechazar (con motivo)
                                      |                           |
                                      +------------+--------------+
                                                   v
                              Se guarda en la base y se recarga la lista
```

1. **Iniciar sesión** con las credenciales de administrador definidas en la configuración.
2. En el menú lateral, abrir **Solicitudes de verificación**. Se cargan las solicitudes desde la
   base de datos.
3. **Filtrar** con los botones Todas / Pendientes / Aprobadas / Rechazadas.
4. **Elegir una solicitud** de la lista para ver su detalle.
5. **Revisar la documentación**: el botón *Abrir* de cada documento lo abre con el programa
   asociado de Windows.
6. Si la solicitud está pendiente, **Aprobar verificación** o **Rechazar solicitud** (escribiendo
   el motivo). Se pide confirmación y el cambio se guarda en la base de datos.
7. **El sistema web completa el proceso**: su backend revisa la base cada 15 segundos, detecta
   las solicitudes resueltas, le asigna al usuario la insignia de organización verificada (si fue
   aprobada) y le envía una notificación con el resultado. Esta aplicación solo escribe el estado,
   el motivo y la fecha de resolución.
8. **Cerrar sesión** desde el menú lateral para volver al inicio de sesión.

## Flujo de trabajo para desarrollar

### Ramas y commits

- `main`: versión estable. No se trabaja directamente sobre esta rama.
- Cada cambio nuevo se hace en una rama propia, por ejemplo `funcionalidad/reportes` o
  `arreglo/filtro-rechazadas`.
- Mensajes de commit cortos y en español, que expliquen **qué** se hizo y **por qué**.

```bash
git checkout -b funcionalidad/nombre-del-cambio
# ... hacer los cambios ...
git add <archivos>
git commit -m "Descripción corta del cambio"
git push -u origin funcionalidad/nombre-del-cambio
```

### Cómo agregar una sección nueva al menú (siguiendo MVC)

1. **Modelo:** la entidad en `Modelos/` y el acceso a la base en `Datos/`.
2. **Contrato:** una interfaz `IVistaXxx` en `Vistas/` con los datos, avisos y órdenes.
3. **Vista:** un `UserControl` en `Vistas/` que implemente la interfaz (sin lógica).
4. **Controlador:** una clase en `Controladores/` que reciba la interfaz y use el repositorio.
5. **Menú:** un botón en `VistaPrincipal`, su evento en `IVistaPrincipal` y su manejador en
   `ControladorPrincipal`.

El detalle paso a paso está en la sección 11 de la documentación técnica.

### Convenciones

- Todos los nombres propios (clases, métodos, variables, archivos) van **en español**; solo
  quedan en inglés los nombres de .NET y Windows Forms.
- Las vistas no llaman a la base de datos ni a los controladores; los controladores no usan
  controles de Windows Forms.

## Seguridad y datos confidenciales

**Nunca se deben subir al repositorio:**

- `configuracion.local.json` (contraseñas de la base y del administrador).
- Archivos de documentos de usuarios (DNI, fotos, PDF), ni capturas que muestren datos
  personales reales.
- Las carpetas generadas `bin/`, `obj/` y `.vs/`.

El archivo `.gitignore` del proyecto ya excluye estos elementos. Antes de cada `git push`,
conviene revisar con `git status` qué archivos se van a subir.

Si alguna contraseña se subiera por error, **no alcanza con borrar el archivo**: hay que
cambiar esa contraseña en MySQL, porque el historial de Git conserva la versión anterior.

## Solución de problemas

| Síntoma | Causa probable | Solución |
|---|---|---|
| El menú muestra un círculo rojo y "Sin conexión a la base de datos" | El servidor MySQL está apagado o los datos de `configuracion.local.json` son incorrectos | Encender MySQL y revisar servidor, puerto, usuario y contraseña |
| Aparecen solicitudes marcadas "(ejemplo sin conexión)" | No se pudo leer la base | Es el modo de respaldo; resolver la conexión y volver a abrir la sección |
| Falla al guardar al aprobar o rechazar | La base se apagó o el usuario de MySQL no tiene permiso de escritura | Revisar la conexión y los permisos del usuario |
| El botón *Abrir* da error | El archivo no existe en esa ruta (la app corre en otra PC) | Ejecutar la aplicación en la computadora donde están los archivos |
| `dotnet build` falla por la versión del SDK | No está instalado .NET 10 | Instalar el SDK de .NET 10 |
| Faltan tablas (por ejemplo `verification_requests`) | No se ejecutaron las migraciones del sistema web | En `backend` del sistema web, ejecutar `npm run migrate` |
| Se aprobó una solicitud pero el usuario no recibe la insignia | El backend del sistema web está apagado | Iniciar el backend; procesa las solicitudes pendientes al arrancar |
| La aplicación avisa que falta la configuración | No existe `configuracion.local.json` o le falta un dato | Copiar `configuracion.ejemplo.json` y completarlo |

## Limitaciones conocidas y próximos pasos

- Las consultas a la base son **síncronas**: si el servidor no responde, la ventana espera hasta
  el tiempo de espera (5 segundos).
- Al aprobar o rechazar no se registra **qué administrador** lo hizo (la columna `resolved_by`
  queda vacía) ni se controla que la solicitud siga pendiente.
- La insignia y la notificación al usuario las aplica el **backend del sistema web** (revisa la base cada
  15 segundos): si está apagado, se procesan cuando vuelve a encenderse.
- Los documentos solo se abren si la aplicación corre en la misma PC que guarda los archivos.
- No hay pruebas automáticas en el repositorio.
- Próxima funcionalidad prevista: **moderación de reportes de publicaciones**.

La lista completa está en la sección 12 de la documentación técnica.

## Documentación

- [`DOCUMENTACION_TECNICA.pdf`](DOCUMENTACION_TECNICA.pdf) / [`.md`](DOCUMENTACION_TECNICA.md):
  explicación detallada de la arquitectura, cada archivo, los flujos completos, recetas para
  hacer cambios y preguntas frecuentes.

## Licencia

Este proyecto se distribuye bajo la licencia **MIT**. El texto completo está en el archivo
[`LICENSE`](LICENSE).

## Autores

- Sebastián Carbonetti
- Santiago Monterubianessi

Proyecto Pata Hogar.
