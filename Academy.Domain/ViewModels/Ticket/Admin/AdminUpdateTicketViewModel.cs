using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.Ticket;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.Ticket.Admin;

public class AdminUpdateTicketViewModel 
{
    public int Id { get; set; } 
    
    
    [Display(Name = "موضوع")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(60, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string Title { get; set; }
    
    
    [Display(Name = "اولویت تیکت")]
    public TicketPriority TicketPriority { get; set; }

    
    [Display(Name = "واحد پشتیبانی")]
    public TicketSection TicketSection { get; set; }
}