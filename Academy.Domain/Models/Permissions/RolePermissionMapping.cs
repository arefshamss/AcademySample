using Academy.Domain.Models.Roles;

namespace Academy.Domain.Models.Permissions;

public class RolePermissionMapping
{

    #region Ctor

    public RolePermissionMapping(short roleId, short permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    #endregion

    #region Properties

    public short RoleId { get; set; }

    public short PermissionId { get; set; }

    #endregion

    #region Relations

    public Role Role { get; set; }

    public Permission Permission { get; set; }

    #endregion

}