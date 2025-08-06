using Academy.Domain.Models.SiteSettings;
using Academy.Domain.ViewModels.SiteSetting;

namespace Academy.Application.Mapper.SiteSetting;

public static class SiteSettingMapper
{

    public static SiteSettingViewModel ToSiteSettingViewModel(this SiteSettings siteSetting)
        => new()
        {
            SiteName = siteSetting.SiteName,
            Copyright = siteSetting.Copyright,
            Logo = siteSetting.Logo,
            Favicon = siteSetting.Favicon,
            DefaultMailServerSmtpId = siteSetting.DefaultMailServerSmtpId,
            DefaultSimpleSmtpId = siteSetting.DefaultSimpleSmtpId,
        };
    public static UpdateSiteSettingViewModel ToUpdateSiteSettingViewModel(this SiteSettings siteSetting)
        => new()
        {
            SiteName = siteSetting.SiteName,
            Copyright = siteSetting.Copyright,
            Logo = siteSetting.Logo,
            Favicon = siteSetting.Favicon,
        };

    public static void MapToSiteSetting(this UpdateSiteSettingViewModel model, SiteSettings siteSetting)
    {
        siteSetting.Copyright = model.Copyright;
        siteSetting.Favicon = model.Favicon!;
        siteSetting.SiteName = model.SiteName;
        siteSetting.Logo = model.Logo!;
        siteSetting.DefaultSimpleSmtpId = model.DefaultSimpleSmtpId;
        siteSetting.DefaultMailServerSmtpId = model.DefaultMailServerSmtpId;
    
    }
}