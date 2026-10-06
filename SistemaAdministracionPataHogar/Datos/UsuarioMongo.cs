using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SistemaAdministracionPataHogar.Datos
{
    // Describe COMO se ve un documento de la coleccion "usuarios" en MongoDB, por ejemplo:
    //
    //   {
    //     "_id": ObjectId("..."),
    //     "nombreUsuario": "admin",
    //     "contrasenaHash": "$2b$10$....",
    //     "activo": true
    //   }
    //
    // Cada propiedad indica con [BsonElement] el nombre EXACTO del campo en MongoDB (los
    // campos de la base van en minuscula y las propiedades de C# en mayuscula). Es la misma
    // idea que las clases Paciente y Contacto del trabajo practico de MongoDB.
    //
    // Esta clase es solo para leer de la base; el resto del programa trabaja con
    // UsuarioAdministrador (en Modelos), que no sabe nada de MongoDB.
    [BsonIgnoreExtraElements] // si el documento tiene campos de mas, se ignoran
    public class UsuarioMongo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("nombreUsuario")]
        public string NombreUsuario { get; set; }

        [BsonElement("contrasenaHash")]
        public string ContrasenaHash { get; set; }

        // Si el documento no tiene el campo "activo", queda en false (por seguridad: ante la
        // duda, no se deja ingresar).
        [BsonElement("activo")]
        public bool Activo { get; set; }
    }
}
