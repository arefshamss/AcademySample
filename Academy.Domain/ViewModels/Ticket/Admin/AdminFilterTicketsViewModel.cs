using System.ComponentModel.DataAnnotations;
using Academy.Domain.Attributes;
using Academy.Domain.Enums.Common;
using Academy.Domain.Enums.Ticket;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Common;

namespace Academy.Domain.ViewModels.Ticket.Admin;

public class AdminFilterTicketsViewModel : BasePaging<AdminTicketViewModel>
{
    
    [Display(Name = "موضوع"),FilterInput]
    public string? Title { get; set; }   
    
    
    [Display(Name = "کاربر"),FilterInput]
    public int? UserId { get; set; }      

    
    [Display(Name = "وضعیت تیکت"),FilterInput]
    public FilterByTicketStatus TicketStatus { get; set; }    
    
    
    [Display(Name = "واحد پشتیبانی"),FilterInput]
    public FilterByTicketSection TicketSection { get; set; }    
    
    
    [Display(Name = "اولویت تیکت"),FilterInput]
    public FilterByTicketPriority TicketPriority { get; set; }    
    
    
    [Display(Name = "مرتب سازی بر اساس"),FilterInput]   
    public FilterOrderBy FilterOrderBy { get; set; }
    
    
    [Display(Name = "وضعیت حذف"),FilterInput]   
    public DeleteStatus DeleteStatus { get; set; }
    
    
    [Display(Name = "از تاریخ"), FilterInput]
    public string? FromDate { get; set; }

    
    [Display(Name = "تا تاریخ"), FilterInput]
    public string? ToDate { get; set; }
}