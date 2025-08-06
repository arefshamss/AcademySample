using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.Permissions;
using Academy.Domain.Models.Roles;
using Microsoft.EntityFrameworkCore;

namespace Academy.Data.Repositories;

public class RoleRepository(AcademyContext context) : EfRepository<Role, short>(context), IRoleRepository
{
    public async Task InsertPermissionsToRoleAsync(List<RolePermissionMapping> permissions) =>
        await context.RolePermissionMappings.AddRangeAsync(permissions);

    public async Task DeleteRolePermissionsByIdAsync(short roleId) =>
        await context.RolePermissionMappings.Where(s => s.RoleId == roleId).ExecuteDeleteAsync();

    public async Task<List<short>> GetSelectedPermission(short roleId) =>
         await context.RolePermissionMappings.Where(s => s.RoleId == roleId)
            .Select(s => s.PermissionId).ToListAsync();
}