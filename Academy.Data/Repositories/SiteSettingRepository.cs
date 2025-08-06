using Academy.Data.Context;
using Academy.Data.Repositories.Generics;
using Academy.Domain.Contracts;
using Academy.Domain.Models.SiteSettings;

namespace Academy.Data.Repositories;

public class SiteSettingRepository(AcademyContext context)
    : EfRepository<SiteSettings, short>(context), ISiteSettingRepository;
