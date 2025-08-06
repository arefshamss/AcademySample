using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.AdminSearch;

namespace Academy.Data.Repositories;

public class AdminSearchEntryRepository(AcademyMongoDbContext context) 
    : MongoDbRepository<AdminSearchEntry>(context), IAdminSearchEntryRepository;