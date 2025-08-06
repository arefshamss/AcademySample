using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Academy.Domain.Models.Logs;

public class ApplicationLog 
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement("Timestamp")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime TimeStamp { get; set; }

    public string MessageTemplate { get; set; }
    public string RenderedMessage { get; set; }

    public string? Exception { get; set; }
    public string Level { get; set; }   
    
}