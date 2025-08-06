using Academy.Domain.ViewModels.Common;

namespace Academy.Domain.ViewModels.Ticket.Client;

public class ClientFilterTicketsViewModel : BasePaging<ClientTicketViewModel>
{
    public int UserId { get; set; } 
}