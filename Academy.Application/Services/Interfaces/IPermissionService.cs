using Academy.Domain.Enums.Role;
using Academy.Domain.ViewModels.Permission;

namespace Academy.Application.Services.Interfaces;

public interface IPermissionService
{
    Task<bool> CheckUserPermissionAsync(int userId, string permissionName);
    Task<bool> CheckUserPermissionsAsync(int userId, IEnumerable<string> permissionNames);
    Task<List<AdminSidePermissionViewModel>> GetPermissionsForRoleAsync(RoleSection roleSection);
}