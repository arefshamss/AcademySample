using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Academy.Data.Repositories;

public class PermissionRepository(AcademyContext context)
    : EfRepository<Permission, short>(context), IPermissionRepository
{
    public async Task<List<RolePermissionMapping>> GetAllRolePermissions()
        => await context.RolePermissionMappings
            .Include(s => s.Permission)
            .ToListAsync();
}