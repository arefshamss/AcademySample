using Academy.Domain.Models.Common;
using Academy.Domain.Models.Roles;
using Academy.Domain.Models.Ticket;
using Academy.Domain.Enums.User;

namespace Academy.Domain.Models.User;

public class User : BaseEntity
{
    #region Properties

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string AvatarImageName { get; set; }

    public string? Password { get; set; }
    
    public string? ActiveCode { get; set; }
    
    public string? MobileActiveCode { get; set; }
    
    public string? EmailActiveCode { get; set; }
    
    public DateTime? ActiveCodeExpireTime { get; set; }
    
    public DateTime? MobileActiveCodeExpireTime { get; set; }
    
    public DateTime? EmailActiveCodeExpireTime { get; set; }

    public bool IsMobileActive { get; set; }

    public bool IsEmailActive { get; set; }
    
    public bool IsBanned { get; set; }
    
    public bool IsBannedFromTicket { get; set; }
    
    public bool IsBannedFromComment { get; set; }

    public string? GoogleAuthenticationId { get; set; }

    #endregion

    #region Relations

    public ICollection<UserRoleMapping> UserRoleMappings { get; set; }
    
    public UserInformation? UserInformation { get; set; }
    
    public ICollection<Ticket.Ticket> Tickets { get; set; }
    
    public ICollection<TicketMessage> TicketMessages { get; set; }
    
    #endregion
}