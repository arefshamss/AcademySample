using Academy.Domain.ViewModels.Role;

namespace Academy.Application.Mapper.Role;

public static class RoleMapper
{
    public static AdminSideRoleViewModel ToAdminSideRoleViewModel(this Domain.Models.Roles.Role model) =>
        new()
        {
            Id = model.Id,
            RoleSection = model.RoleSection,
            RoleName = model.RoleName,
            IsDeleted = model.IsDeleted,
        };

    public static Domain.Models.Roles.Role ToRole(this AdminSideCreateRoleViewModel model) =>
        new()
        {
            RoleSection = model.RoleSection,
            RoleName = model.RoleName,
        };

    public static AdminSideUpdateRoleViewModel ToAdminSideUpdateRoleViewModel(this Domain.Models.Roles.Role model) =>
        new()
        {
            Id = model.Id,
            RoleSection = model.RoleSection,
            RoleName = model.RoleName,
        };

    public static void UpdateRole(this AdminSideUpdateRoleViewModel model, Domain.Models.Roles.Role role)
    {
        role.RoleSection = model.RoleSection;
        role.RoleName = model.RoleName;
    }

}