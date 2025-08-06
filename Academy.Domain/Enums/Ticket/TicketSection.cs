using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.Ticket;

public enum TicketSection : byte
{
    [Display(Name = "واحد پشتیبانی")]
    Supporter,
}


public enum FilterByTicketSection : byte
{
    [Display(Name = "همه")]
    All,
    
    [Display(Name = "واحد پشتیبانی")]
    Supporter,
}
