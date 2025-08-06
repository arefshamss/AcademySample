using Academy.Domain.ViewModels.User.Admin;

namespace Academy.Domain.ViewModels.TicketMessage.Admin;

public class AdminTicketMessageViewModel
{
    public int Id { get; set; } 
    
    public int TicketId { get; set; }   
    
    public int SenderId { get; set; }
    
    public string Message { get; set; }
    
    public string? AttachmentUrl { get; set; }     
    
    public bool ReadBySupporter { get; set; }
    
    public bool ReadByUser { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public AdminUserViewModel Sender { get; set; }  
}