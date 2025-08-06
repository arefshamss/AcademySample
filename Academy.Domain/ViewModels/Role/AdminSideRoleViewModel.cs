using Academy.Domain.Enums.Role;

namespace Academy.Domain.ViewModels.Role;

public class AdminSideRoleViewModel
{
    public short Id { get; set; }

    public string RoleName { get; set; }

    public RoleSection RoleSection { get; set; }

    public bool IsDeleted { get; set; }
}