using MongoDB.Bson;

namespace Academy.Domain.ViewModels.UserActivity;

public class UserActivityViewModel
{
    public ObjectId Id { get; set; }
    
    public string? Description { get; set; }
    
    public string? BrowserName { get; set; }

    public string? BrowserVersion { get; set; }
    
    public string? IpAddress { get; set; }
    
    public int StatusCode { get; set; }
    
    public DateTime CreatedDate { get; set; }
}