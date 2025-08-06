using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.SmsProvider;

namespace Academy.Data.Repositories;

public class SmsProviderRepository(AcademyContext context) : EfRepository<SmsProvider, short>(context), ISmsProviderRepository;