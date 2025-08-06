using Academy.Domain.Enums.Role;
using Academy.Domain.Models.Common;
using Academy.Domain.Models.Permissions;

namespace Academy.Domain.Models.Roles;

public class Role : BaseEntity<short>
{
    #region Properties 

    public string RoleName { get; set; }

    public RoleSection RoleSection { get; set; }

    #endregion

    #region Relations

    public ICollection<UserRoleMapping> UserRoleMappings { get; set; }

    public ICollection<RolePermissionMapping> RolePermissionMappings { get; set; }

    #endregion

}