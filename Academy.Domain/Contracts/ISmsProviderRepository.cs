using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.SmsProvider;

namespace Academy.Domain.Contracts;

public interface ISmsProviderRepository:IEfRepository<SmsProvider,short>;