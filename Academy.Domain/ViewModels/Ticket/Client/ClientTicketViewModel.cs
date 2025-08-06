using Academy.Domain.Enums.Ticket;
using Academy.Domain.ViewModels.TicketMessage.Client;
using Academy.Domain.ViewModels.User.Client;

namespace Academy.Domain.ViewModels.Ticket.Client;

public class ClientTicketViewModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public TicketStatus TicketStatus { get; set; }
    
    public TicketPriority TicketPriority { get; set; }

    public TicketSection TicketSection { get; set; }
    
    public bool ReadBySupporter { get; set; }

    public bool ReadByUser { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public ClientUserViewModel User { get; set; }
    
    public List<ClientTicketMessageViewModel> Messages { get; set; }    
}