using Academy.Domain.Enums.Role;
using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.Permissions;

public class Permission : BaseEntity<short>
{
    #region Properties

    public short? ParentId { get; set; }

    public string UniqueName { get; set; }

    public string DisplayName { get; set; }

    public RoleSection RoleSection { get; set; }

    #endregion

    #region Relations 

    public Permission? Parent { get; set; }

    public ICollection<RolePermissionMapping> RolePermissionMappings { get; set; }

    #endregion
}