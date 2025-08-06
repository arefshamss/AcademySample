using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.Roles;

namespace Academy.Domain.Contracts;

public interface IUserRoleMappingRepository : IEfRepository<UserRoleMapping, short>;