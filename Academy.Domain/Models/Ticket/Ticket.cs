using Academy.Domain.Enums.Ticket;
using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.Ticket;

public sealed class Ticket : BaseEntity
{
    #region Properties

    public required string Title { get; set; }

    public TicketStatus TicketStatus { get; set; }

    public TicketPriority TicketPriority { get; set; }

    public TicketSection TicketSection { get; set; }

    public int UserId { get; set; }

    public bool ReadBySupporter { get; set; }

    public bool ReadByUser { get; set; }

    #endregion

    #region Relations

    public User.User User { get; set; }

    public ICollection<TicketMessage> TicketMessages { get; set; }

    #endregion
}