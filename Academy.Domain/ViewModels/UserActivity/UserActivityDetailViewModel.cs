using MongoDB.Bson;

namespace Academy.Domain.ViewModels.UserActivity;

public class UserActivityDetailViewModel
{
    public ObjectId Id { get; set; }

    public string? Description { get; set; }

    public string? Device { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }
    
    public string? Os { get; set; }

    public string? OsVersion { get; set; }

    public string? OsPlatform { get; set; }
    
    public string? BrowserName { get; set; }

    public string? BrowserVersion { get; set; }
    
    public string? IpAddress { get; set; }

    public bool IsBot { get; set; }

    public string? BotName  { get; set; }

    public string? BotCategory { get; set; }

    public string? BotProducer { get; set; }

    public string? BotUrl { get; set; }

    public int? UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserNationalCode { get; set; }

    public long? CustomerId { get; set; }
    
    public string Area { get; set; }

    public string? Url { get; set; }
    
    public string? FormData { get; set; }
    
    public int StatusCode { get; set; }

    public string  CreatedDate { get; set; }
}