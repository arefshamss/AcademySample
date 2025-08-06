using Academy.Application.Cache;
using Academy.Application.Services.Interfaces;
using Academy.Application.Mapper.Permission;
using Academy.Domain.Contracts;
using Academy.Domain.Enums.Role;
using Academy.Domain.ViewModels.Permission;

namespace Academy.Application.Services.Implementation;

public class PermissionService(
    IPermissionRepository permissionRepository,
    IUserRoleMappingRepository userRoleMappingRepository , 
    IMemoryCacheService memoryCacheService 
) : IPermissionService
{
    public async Task<bool> CheckUserPermissionAsync(int userId, string permissionName)
    {
        var userRoleMappings = await memoryCacheService.GetOrCreateAsync(
                CacheKey.Format(CacheKeys.UserRoleMappings, userId),
            async () => await userRoleMappingRepository.GetAllAsync(s => s.UserId == userId));

        if (userRoleMappings is null || userRoleMappings.Count == 0) return false;

        var rolePermissionMappings = await memoryCacheService.GetOrCreateAsync(CacheKeys.RolePermissionMappings,
            async () => await permissionRepository.GetAllRolePermissions()); 

        return userRoleMappings.Any(s => rolePermissionMappings.Any(p => p.RoleId == s.RoleId && p.Permission.UniqueName == permissionName));
    }

    public async Task<bool> CheckUserPermissionsAsync(int userId, IEnumerable<string> permissionNames)
    {
        List<bool> permissions = [];
        foreach (var permissionName in permissionNames ?? []) permissions.Add(await CheckUserPermissionAsync(userId , permissionName));
        return permissions.Any(hasAccess => hasAccess);
    }

    public async Task<List<AdminSidePermissionViewModel>> GetPermissionsForRoleAsync(RoleSection roleSection)
    {
        var permissions = await permissionRepository.GetAllAsync(s => s.RoleSection == roleSection);
        return permissions.Select(s => s.ToAdminSidePermissionViewModel()).ToList();
    }
}