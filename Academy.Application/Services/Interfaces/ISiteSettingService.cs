using Academy.Domain.Shared;
using Academy.Domain.ViewModels.SiteSetting;

namespace Academy.Application.Services.Interfaces;

public interface ISiteSettingService
{
    Task<SiteSettingViewModel> GetSiteSettingAsync();

    Task<UpdateSiteSettingViewModel> GetSiteSettingForUpdateAsync();
    
    Task<Result> UpdateSiteSettingAsync(UpdateSiteSettingViewModel model);
}