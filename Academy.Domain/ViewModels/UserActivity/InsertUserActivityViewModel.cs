namespace Academy.Domain.ViewModels.UserActivity;

public class InsertUserActivityViewModel
{
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
    
    public string Area { get; set; }

    public string? Url { get; set; }
    
    public string? FormData { get; set; }
    
    public int StatusCode { get; set; }
    

}