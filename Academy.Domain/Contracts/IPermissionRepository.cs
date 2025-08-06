using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.Permissions;

namespace Academy.Domain.Contracts;

public interface IPermissionRepository : IEfRepository<Permission, short>
{
    Task<List<RolePermissionMapping>> GetAllRolePermissions();
}