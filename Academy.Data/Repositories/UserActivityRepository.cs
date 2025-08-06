using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.UserActivities;

namespace Academy.Data.Repositories;

public class UserActivityRepository(AcademyMongoDbContext context) : MongoDbRepository<UserActivity>(context),  IUserActivityRepository;