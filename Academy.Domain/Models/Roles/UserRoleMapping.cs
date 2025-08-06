using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.Roles;

public class UserRoleMapping : BaseEntity<short>
{
    #region Properties 

    public short RoleId { get; set; }

    public int UserId { get; set; }

    #endregion

    #region Relations

    public Role Role { get; set; }

    public User.User User { get; set; }

    #endregion
}