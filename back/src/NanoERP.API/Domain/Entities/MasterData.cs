using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NanoERP.API.Domain.Entities
{
    public abstract class MasterData
    {
        [Key]
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [JsonPropertyOrder(-2)]
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StringId => Id.ToString();
        [JsonPropertyOrder(-2)]
        public virtual string Name { get; set; } = string.Empty;
    }
}