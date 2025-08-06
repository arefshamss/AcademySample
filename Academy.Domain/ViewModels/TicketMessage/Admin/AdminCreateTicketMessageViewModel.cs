using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.Ticket;
using Academy.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace Academy.Domain.ViewModels.TicketMessage.Admin;

public class AdminCreateTicketMessageViewModel
{
    public TicketStatus TicketStatus { get; set; }
    
    public int TicketId { get; set; }   
    
    public int SenderId { get; set; }
    
    
    [Display(Name = "پیام")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(1000, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string Message { get; set; }
    
    
    [Display(Name = "ضمیمه")]
    public IFormFile? Attachment { get; set; }
    
}