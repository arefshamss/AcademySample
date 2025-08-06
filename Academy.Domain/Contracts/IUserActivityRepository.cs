using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.UserActivities;

namespace Academy.Domain.Contracts;

public interface IUserActivityRepository : IMongoDbRepository<UserActivity>;