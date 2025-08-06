using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.User;

namespace Academy.Domain.Contracts;

public interface IUserRepository:IEfRepository<User>;