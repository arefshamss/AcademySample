using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.Ticket;

public enum TicketPriority : byte
{
    [Display(Name = "کم")]
    Low,
    
    [Display(Name = "متوسط")]
    Medium,
    
    [Display(Name = "زیاد")]
    High,
}

public enum FilterByTicketPriority : byte
{
    [Display(Name = "همه")]
    All,
    
    [Display(Name = "کم")]
    Low,
    
    [Display(Name = "متوسط")]
    Medium,
    
    [Display(Name = "زیاد")]
    High,
}
