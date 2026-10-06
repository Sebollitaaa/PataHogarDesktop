# Pata Hogar - Panel de administración (escritorio)

Aplicación de escritorio para Windows que usa el administrador de **Pata Hogar**, un sitio de
adopción de mascotas. Desde acá se revisan las **solicitudes de verificación** que envían
refugios, veterinarias y asociaciones protectoras desde la web, y se decide si se les otorga la
cuenta verificada (aprobar) o no (rechazar, con un motivo).

Está desarrollada en **C# con Windows Forms**, usa el patrón **MVC** y trabaja con dos bases de datos:

- **MySQL**, la misma base que usa el sitio web: de ahí se leen y actualizan las solicitudes de verificación.
- **MongoDB Atlas**, una base aparte que se usa **solamente para el inicio de sesión**: guarda los usuarios
  que pueden ingresar a la aplicación, con sus contraseñas protegidas (hash).

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
   - [Parte B: base de usuarios en MongoDB Atlas](#parte-b-base-de-usuarios-en-mongodb-atlas)
   - [Parte C: aplicación de escritorio](#parte-c-aplicación-de-escritorio)
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

- **Inicio de sesión** validado contra una base **MongoDB Atlas**: solo ingresan los usuarios registrados y
  activos, y las contraseñas se guardan como hash (nunca en texto plano).
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
| MongoDB Atlas | Base de datos en la nube, usada solo para los usuarios del inicio de sesión |
| MongoDB.Driver 3.12.0 | Conexión de C# con MongoDB (paquete NuGet) |
| BCrypt.Net-Next 4.2.0 | Verificación de las contraseñas contra su hash bcrypt (paquete NuGet) |
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
    |-- Datos/            acceso a las bases (MySQL: ConexionBaseDatos, RepositorioSolicitudes;
    |                     MongoDB: ConexionMongo, RepositorioUsuarios)
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
- **El sistema web de Pata Hogar** (repositorio aparte:
  <https://github.com/Sebollitaaa/PataHogarWeb>), instalado en la misma computadora.
  Esta aplicación de escritorio **no funciona de forma independiente**: depende de la base de
  datos y de los archivos del sistema web.
- Una cuenta gratuita en **MongoDB Atlas** y **conexión a internet**, necesarias para el inicio de sesión
  (ver [Parte B](#parte-b-base-de-usuarios-en-mongodb-atlas)).
- **Git**, para clonar los repositorios.

## Instalación

El sistema completo tiene tres partes que se instalan **en este orden**: primero el sistema web
con su base de datos (Parte A), después la base de usuarios en MongoDB Atlas (Parte B) y por último
esta aplicación de escritorio (Parte C).

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
   git clone https://github.com/Sebollitaaa/PataHogarWeb.git
   cd PataHogarWeb/backend
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

### Parte B: base de usuarios en MongoDB Atlas

La aplicación valida el inicio de sesión contra una base **MongoDB** en la nube (MongoDB Atlas). Es una
base **aparte** de MySQL y se usa **solamente** para las credenciales de las personas que pueden ingresar
a la aplicación.

1. **Crear una cuenta y un cluster gratuito** en [MongoDB Atlas](https://www.mongodb.com/atlas) (plan
   *Free*, M0). Si ya existe un cluster, se puede reutilizar.

2. **Permitir la IP de la computadora**: en *Network Access* → *Add IP Address*, agregar la dirección IP
   actual (*Add Current IP Address*). Si se cambia de red, hay que agregar la IP nueva.

3. **Crear un usuario de base de datos de solo lectura**: en *Database Access* → *Add New Database User*,
   método *Password*. En *Database User Privileges* → *Specific Privileges* agregar el rol `read` sobre la
   base `PataHogar` (como alternativa más simple, sirve el rol integrado *Only read any database*).
   Anotar el usuario y la contraseña: se usan en el paso 6. La aplicación solo **lee** esta base, por eso
   alcanza con permiso de lectura.

4. **Crear la base y la colección**: en *Browse Collections* → *Add My Own Data* (o desde MongoDB
   Compass), con base de datos `PataHogar` y colección `usuarios`.

5. **Cargar al menos un usuario** en la colección `usuarios` (en Compass: *Add Data* → *Insert Document*)
   con esta estructura:

   ```json
   {
     "nombreUsuario": "tu_usuario",
     "contrasenaHash": "<hash bcrypt de la contraseña>",
     "activo": true
   }
   ```

   La contraseña **nunca se guarda en texto plano**: se guarda su hash bcrypt. Para generarlo, ejecutar
   esto desde la carpeta `backend` del sistema web (usa su librería `bcrypt`; pide la contraseña y
   muestra el hash):

   ```bash
   node -e "const rl=require('readline').createInterface({input:process.stdin,output:process.stdout});rl.question('Contraseña: ',p=>{console.log(require('bcrypt').hashSync(p,10));rl.close()})"
   ```

   El resultado empieza con `$2b$10$`. Se copia completo en el campo `contrasenaHash`. El valor
   de `nombreUsuario` distingue mayúsculas y minúsculas.

6. **Obtener la cadena de conexión**: en *Database* → *Connect* → *Drivers* (C# / .NET), copiar la cadena
   `mongodb+srv://...` y reemplazar el marcador de contraseña por la contraseña del usuario del paso 3
   (si tiene caracteres especiales, deben escribirse codificados en formato URL). Esa cadena se pega en
   `configuracion.local.json` (Parte C).

### Parte C: aplicación de escritorio

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

   Después abrir `configuracion.local.json` y completar los datos reales: los de la base MySQL creada en
   la Parte A y la cadena de conexión de MongoDB obtenida en la Parte B.

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
  "mongoDb": {
    "cadenaConexion": "mongodb+srv://usuario:contrasena@tu-cluster.mongodb.net/?appName=PataHogarEscritorio",
    "nombreBaseDeDatos": "PataHogar"
  }
}
```

| Clave | Qué es |
|---|---|
| `baseDeDatos.servidor` / `puerto` | Dónde escucha el servidor MySQL |
| `baseDeDatos.nombre` | Nombre de la base de datos del sitio web |
| `baseDeDatos.usuario` / `contrasena` | Credenciales de MySQL con permiso de lectura y escritura sobre las tablas de verificación |
| `mongoDb.cadenaConexion` | Cadena de conexión de MongoDB Atlas, con el usuario y la contraseña de solo lectura (Parte B, paso 6) |
| `mongoDb.nombreBaseDeDatos` | Nombre de la base de MongoDB que contiene la colección `usuarios` (`PataHogar`) |

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

### Base de usuarios (MongoDB)

Además de MySQL, la aplicación usa una base **MongoDB** que contiene **únicamente** los usuarios que
pueden iniciar sesión. Estructura de la colección `usuarios` (base `PataHogar`):

| Campo | Tipo | Descripción |
|---|---|---|
| `nombreUsuario` | texto | Nombre con el que la persona inicia sesión (distingue mayúsculas y minúsculas) |
| `contrasenaHash` | texto | Hash **bcrypt** de la contraseña (empieza con `$2b$`). Nunca la contraseña real |
| `activo` | `true` / `false` | Si es `false`, o si el campo falta, el usuario **no** puede ingresar |

Para dar de baja a una persona alcanza con poner `activo` en `false`. Al iniciar sesión, la aplicación
busca el usuario por `nombreUsuario` y verifica la contraseña escrita contra su hash. Si el usuario no
existe, está inactivo o la contraseña no coincide, se muestra el mismo mensaje de error.

## Ejecución

Con Visual Studio: abrir `SistemaAdministracionPataHogar.slnx` y presionar **F5**.

Desde la terminal:

```bash
dotnet run --project SistemaAdministracionPataHogar
```

Antes de abrir la aplicación hay que tener **el servidor MySQL encendido**. Si no lo está, la
aplicación igual abre, pero muestra el indicador rojo y datos de ejemplo.

Para **iniciar sesión** hace falta además **conexión a internet** y que la IP de la computadora esté
permitida en MongoDB Atlas (Parte B, paso 2).

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

1. **Iniciar sesión** con un usuario registrado en la colección `usuarios` de MongoDB. La aplicación busca
   el usuario, comprueba que esté activo y verifica la contraseña contra su hash.
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

- `configuracion.local.json` (contraseña de MySQL y cadena de conexión de MongoDB Atlas, que incluye
  usuario y contraseña).
- Archivos de documentos de usuarios (DNI, fotos, PDF), ni capturas que muestren datos
  personales reales.
- Las carpetas generadas `bin/`, `obj/` y `.vs/`.

El archivo `.gitignore` del proyecto ya excluye estos elementos. Antes de cada `git push`,
conviene revisar con `git status` qué archivos se van a subir.

Medidas aplicadas en el inicio de sesión:

- Las contraseñas de los usuarios se guardan **solo como hash bcrypt** en MongoDB; no existen en el código ni
  en el repositorio.
- El usuario de Atlas que usa la aplicación es de **solo lectura** sobre la base `PataHogar`.
- Conviene **no** habilitar `0.0.0.0/0` en *Network Access* de Atlas (permite el acceso desde cualquier
  lugar); es preferible agregar únicamente las IP necesarias.
- Ante un usuario inexistente, inactivo o con contraseña incorrecta se muestra el mismo mensaje, para no
  revelar qué usuarios existen.

Si alguna contraseña se subiera por error, **no alcanza con borrar el archivo**: hay que
cambiar esa contraseña (en MySQL o, para la de MongoDB, en *Database Access* de Atlas), porque el historial
de Git conserva la versión anterior.

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
| Al iniciar sesión aparece "No se pudo conectar con la base de usuarios" | Sin internet, la IP no está permitida en Atlas o la cadena de conexión está incompleta (por ejemplo, sin reemplazar la contraseña) | Revisar internet, *Network Access* en Atlas y `mongoDb.cadenaConexion` en `configuracion.local.json` |
| "Usuario o contraseña incorrectos" aunque los datos parezcan correctos | El usuario no existe en `usuarios`, el campo `activo` es `false` o no está, el hash se copió incompleto, o el nombre difiere en mayúsculas | Revisar el documento del usuario en Compass (o en *Browse Collections*) |
| La aplicación avisa que falta la configuración | No existe `configuracion.local.json` o le falta un dato | Copiar `configuracion.ejemplo.json` y completarlo |

## Limitaciones conocidas y próximos pasos

- Las consultas a la base son **síncronas**: si el servidor no responde, la ventana espera hasta
  el tiempo de espera (5 segundos).
- Al aprobar o rechazar no se registra **qué administrador** lo hizo (la columna `resolved_by`
  queda vacía) ni se controla que la solicitud siga pendiente.
- La insignia y la notificación al usuario las aplica el **backend del sistema web** (revisa la base cada
  15 segundos): si está apagado, se procesan cuando vuelve a encenderse.
- El inicio de sesión **requiere internet** y que la IP esté permitida en Atlas: sin conexión nadie puede
  ingresar (no hay inicio de sesión sin conexión).
- Los usuarios se cargan y se dan de baja **a mano** en MongoDB; la aplicación no tiene una pantalla para
  administrarlos.
- La ventana principal muestra el nombre "admin" de forma fija, sin importar con qué usuario se ingresó.
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
