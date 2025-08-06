
using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.Roles;

namespace Academy.Data.Repositories;

public class UserRoleMappingRepository(AcademyContext context) : EfRepository<UserRoleMapping, short>(context), IUserRoleMappingRepository;