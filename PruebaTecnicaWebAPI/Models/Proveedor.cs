using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace PruebaTecnicaWebAPI.Models
{
    public class Proveedor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("NIT")]
        public string NIT { get; set; } = String.Empty;

        [BsonElement("razonSocial")]
        public string RazonSocial { get; set; } = String.Empty;

        [BsonElement("direccion")]
        public string Direccion { get; set; } = String.Empty;

        [BsonElement("ciudad")]
        public string Ciudad { get; set; } = String.Empty;

        [BsonElement("departamento")]
        public string Departamento { get; set; } = String.Empty;

        [BsonElement("correo")]
        public string Correo { get; set; } = String.Empty;

        [BsonElement("activo")]
        public bool Activo { get; set; }

        [BsonElement("fechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [BsonElement("nombreContacto")]
        public string NombreContacto { get; set; } = String.Empty;

        [BsonElement("correoContacto")]
        public string CorreoContacto { get; set; } = String.Empty;

    }
}
