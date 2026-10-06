-- ============================================================================
-- Pata Hogar - Creacion de la base de datos y del usuario de la aplicacion
-- ============================================================================
-- Este script PREPARA el servidor MySQL: crea la base de datos y el usuario con
-- el que se conectan el sistema web y la aplicacion de escritorio.
--
-- Importante:
--   * NO crea las tablas. La estructura (users, pets, verification_requests,
--     verification_documents, etc.) la genera el sistema web de Pata Hogar con
--     sus migraciones:   cd backend   y luego   npm run migrate   (ver README).
--   * Antes de ejecutarlo, reemplazar CAMBIAR_CONTRASENA por una contrasena
--     propia. Esa misma contrasena va despues en la configuracion local de la
--     aplicacion (configuracion.local.json) y en el archivo .env del backend web.
--   * No subir al repositorio una version de este archivo con una contrasena real.
--
-- Ejecucion (con el servidor MySQL encendido, indicando el puerto que uses):
--   mysql -u root -p -P 3307 < base_de_datos/crear_base_y_usuario.sql
-- ============================================================================

CREATE DATABASE IF NOT EXISTS patahogar
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

CREATE USER IF NOT EXISTS 'patahogar_app'@'localhost' IDENTIFIED BY 'CAMBIAR_CONTRASENA';

GRANT ALL PRIVILEGES ON patahogar.* TO 'patahogar_app'@'localhost';

FLUSH PRIVILEGES;
