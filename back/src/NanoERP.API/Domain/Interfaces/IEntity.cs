using MongoDB.Bson;

namespace NanoERP.API.Domain.interfaces;

public interface IEntity
{
    public ObjectId Id { get; set; }
}