using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.User;

namespace Academy.Data.Repositories;

public class UserInformationRepository(AcademyContext context) : EfRepository<UserInformation>(context), IUserInformationRepository
{
    
}