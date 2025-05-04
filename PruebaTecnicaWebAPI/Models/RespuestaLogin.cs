using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace PruebaTecnicaWebAPI.Models
{
    public class RespuestaLogin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        [BsonElement("usuario")]
        public string Usuario { get; set; } = String.Empty;

        [BsonElement("token")]
        public string Token { get; set; } = String.Empty;

        [BsonElement("expira")]
        public int Expira { get; set; } 
    }
}
