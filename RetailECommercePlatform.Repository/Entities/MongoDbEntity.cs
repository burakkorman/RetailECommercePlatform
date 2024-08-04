using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RetailECommercePlatform.Repository.Entities;

public abstract class MongoDbEntity : IEntity<string>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement(Order = 101)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    
    public bool IsActive { get; set; }
    
    public MongoDbEntity()
    {
        CreatedAt = DateTime.Now;
        IsActive = true;
    }
}