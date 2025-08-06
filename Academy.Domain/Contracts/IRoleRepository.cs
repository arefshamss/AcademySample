using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.Permissions;
using Academy.Domain.Models.Roles;

namespace Academy.Domain.Contracts;

public interface IRoleRepository : IEfRepository<Role, short>
{
    Task InsertPermissionsToRoleAsync(List<RolePermissionMapping> permissions);

    Task DeleteRolePermissionsByIdAsync(short roleId);

    Task<List<short>> GetSelectedPermission(short roleId);
}