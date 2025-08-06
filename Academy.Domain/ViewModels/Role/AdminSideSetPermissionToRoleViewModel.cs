using Academy.Domain.ViewModels.Permission;

namespace Academy.Domain.ViewModels.Role;

public class AdminSideSetPermissionToRoleViewModel
{
    public short RoleId { get; set; }

    public List<short>? PermissionIds { get; set; }

    public List<short>? SelectedPermissionIds { get; set; }

    public List<AdminSidePermissionViewModel>? Permissions { get; set; }
}