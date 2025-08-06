using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.Ticket;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.TicketMessage.Client;

namespace Academy.Domain.ViewModels.Ticket.Client;

public class ClientCreateTicketViewModel
{
    [Display(Name = "موضوع")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(60, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string Title { get; set; }
    
    
    [Display(Name = "اولویت تیکت")]
    public TicketPriority TicketPriority { get; set; }
    
    
    [Display(Name = "واحد پشتیبانی")]
    public TicketSection TicketSection { get; set; }
    
    
    public int UserId { get; set; } 
    
    
    public ClientCreateTicketMessageViewModel Message { get; set; } 
}