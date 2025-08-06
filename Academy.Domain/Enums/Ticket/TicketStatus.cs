using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.Ticket;

public enum TicketStatus : byte
{
    [Display(Name = "پاسخ داده شده")]
    SupporterAnswered,
    
    [Display(Name = "در حال بررسی")]
    InProgress,
    
    [Display(Name = "در انتظار پاسخ کاربر")]
    PendingForUserAnswer,
    
    [Display(Name = "بسته شده")]
    Closed,
}

public enum FilterByTicketStatus : byte     
{
    [Display(Name = "همه")]
    All,
    
    [Display(Name = "پاسخ داده شده")]
    SupporterAnswered,
    
    [Display(Name = "در حال بررسی")]
    InProgress,
    
    [Display(Name = "در انتظار پاسخ کاربر")]
    PendingForUserAnswer,
    
    [Display(Name = "بسته شده")]
    Closed,
}