using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.SmsSetting;

namespace Academy.Data.Repositories;

public class SmsSettingRepository(AcademyContext context):EfRepository<SmsSetting,short>(context), ISmsSettingRepository;