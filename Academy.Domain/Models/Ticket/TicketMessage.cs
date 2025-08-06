using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.Ticket;

public sealed class TicketMessage : BaseEntity
{
    #region Properties

    public int TicketId { get; set; }   
    
    public int SenderId { get; set; }
    
    public required string Message { get; set; }
    
    public string? Attachment { get; set; }   
    
    public bool ReadBySupporter { get; set; }
    
    public bool ReadByUser { get; set; }

    #endregion

    #region Relations

    public Ticket Ticket { get; set; }
    
    public User.User Sender { get; set; }   

    #endregion
}