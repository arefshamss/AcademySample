using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.SiteSettings;

namespace Academy.Domain.Contracts;

public interface ISiteSettingRepository : IEfRepository<SiteSettings , short>;