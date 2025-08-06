namespace Academy.Domain.ViewModels.ApplicationLogs;

public class ApplicationLogDetailViewModel
{
    public DateTime TimeStamp { get; set; }
    
    public string MessageTemplate { get; set; }
    
    public string RenderedMessage { get; set; }
    
    public string? Exception { get; set; }
    
    public string Level { get; set; }
}