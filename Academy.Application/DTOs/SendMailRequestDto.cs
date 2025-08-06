namespace Academy.Application.DTOs;

public class SendMailRequestDto
{
    public string From { get; set; }
    
    public string To { get; set; }
    public string Subject { get; set; }
    
    public string Title { get; set; }

    public string Body { get; set; }
}