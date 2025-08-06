using Academy.Application.Cache;
using Academy.Application.Services.Interfaces;
using Academy.Application.Statics;
using Academy.Application.Extensions;
using Academy.Application.Mapper.SiteSetting;
using Academy.Domain.Contracts;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.SiteSetting;

namespace Academy.Application.Services.Implementation;

public class SiteSettingService(ISiteSettingRepository siteSettingRepository , IMemoryCacheService memoryCacheService) : ISiteSettingService
{
    public async Task<SiteSettingViewModel> GetSiteSettingAsync()
        => await memoryCacheService.GetOrCreateAsync(CacheKeys.SiteSetting, async() =>
        {
            var siteSetting = await siteSettingRepository.FirstOrDefaultAsync();
            return siteSetting!.ToSiteSettingViewModel();
        });
    public async Task<UpdateSiteSettingViewModel> GetSiteSettingForUpdateAsync()
        => await memoryCacheService.GetOrCreateAsync(CacheKeys.UpdateSiteSettingCache, async() =>
        {
            var siteSetting = await siteSettingRepository.FirstOrDefaultAsync();
            return siteSetting!.ToUpdateSiteSettingViewModel();
        });

    public async Task<Result> UpdateSiteSettingAsync(UpdateSiteSettingViewModel model)
    {
        var siteSetting  = await siteSettingRepository.FirstOrDefaultAsync();
        
        if (model.LogoFile != null)
        {
            var fileNameResult = await model.LogoFile.AddImageToServer( FilePaths.SiteLogoPath , suggestedFileName: siteSetting!.SiteName ,   deleteFileName:siteSetting.Logo);
            model.Logo = fileNameResult.Value;
        }
        if (model.FavIconFile != null)
        {
            string fileName = "favicon" + Path.GetExtension(model.FavIconFile.FileName);
            await model.FavIconFile.AddFilesToServer(fileName, FilePaths.FaviconPath, siteSetting!.Favicon, false);
            model.Favicon = fileName;
        }
        
        model.MapToSiteSetting(siteSetting!);
        siteSettingRepository.Update(siteSetting!);
        
        await siteSettingRepository.SaveChangesAsync();
        
        await memoryCacheService.RemoveByPrefixAsync(CacheKeys.SiteSettingPrefix);
        
        if(siteSetting!.DefaultMailServerSmtpId!=model.DefaultMailServerSmtpId)
            await memoryCacheService.RemoveAsync(CacheKeys.DefaultMailServerSmtpId);
        
        if(siteSetting!.DefaultSimpleSmtpId!=model.DefaultSimpleSmtpId)
            await memoryCacheService.RemoveAsync(CacheKeys.DefaultSimpleSmtpId);
        
        return Result.Success();
    }
}