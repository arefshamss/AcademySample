using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.AdminSearch;

namespace Academy.Domain.Contracts;

public interface IAdminSearchEntryRepository : IMongoDbRepository<AdminSearchEntry>;