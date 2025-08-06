using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Academy.Domain.Models.Common;

public abstract class MongoDBBaseEntity
{
    protected MongoDBBaseEntity()
    {
        CreatedDate = DateTime.Now;
    }

    [BsonId]
    public ObjectId Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public bool IsDeleted { get; set; }
}