using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.Ticket;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.TicketMessage.Admin;

namespace Academy.Domain.ViewModels.Ticket.Admin;

public class AdminCreateTicketViewModel
{
    [Display(Name = "موضوع")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(60, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string Title { get; set; }


    [Display(Name = "کاربر")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public int UserId { get; set; }

    public string UserDisplay { get; set; }


    [Display(Name = "اولویت تیکت")]
    public TicketPriority TicketPriority { get; set; }
    
    
    [Display(Name = "واحد پشتیبانی")]
    public TicketSection TicketSection { get; set; }


    public AdminCreateTicketMessageViewModel TicketMessage { get; set; }
}