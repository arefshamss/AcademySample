using Academy.Domain.Enums.Ticket;
using Academy.Domain.ViewModels.TicketMessage.Admin;
using Academy.Domain.ViewModels.User.Admin;

namespace Academy.Domain.ViewModels.Ticket.Admin;

public class AdminTicketViewModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public TicketStatus TicketStatus { get; set; }
    
    public TicketPriority TicketPriority { get; set; }

    public TicketSection TicketSection { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public bool ReadBySupporter { get; set; }

    public bool ReadByUser { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public AdminUserViewModel User { get; set; }
    
    public List<AdminTicketMessageViewModel> Messages { get; set; } 
}