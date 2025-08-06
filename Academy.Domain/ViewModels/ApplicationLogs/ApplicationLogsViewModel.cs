using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Academy.Domain.ViewModels.ApplicationLogs;

public class ApplicationLogsViewModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    public DateTime TimeStamp { get; set; }
    
    public string MessageTemplate { get; set; }
    
    public string RenderedMessage { get; set; }
    
    public string Level { get; set; }
    
}