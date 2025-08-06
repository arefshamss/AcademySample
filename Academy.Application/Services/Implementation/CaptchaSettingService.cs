using Academy.Application.Cache;
using Academy.Application.Services.Interfaces;
using Academy.Application.Mapper.Captcha;
using Academy.Domain.Contracts;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.CaptchaSetting;

namespace Academy.Application.Services.Implementation;

public class CaptchaSettingService(ICaptchaSettingRepository captchaSettingRepository , IMemoryCacheService memoryCacheService) : ICaptchaSettingService
{

    public async Task<AdminSideUpdateCaptchaSettingViewModel> FillModelForEditAsync()
    {
        var captchaSettings = await captchaSettingRepository.GetAllAsync();
        return new AdminSideUpdateCaptchaSettingViewModel
        {
            CaptchaSettings = captchaSettings.Select(s => s.ToAdminSideCaptchaSettingViewModel()).ToList()
        };
    }

    public async Task<Result> UpdateCaptchaSettingsAsync(AdminSideUpdateCaptchaSettingViewModel model)
    {
        foreach (var captchaSection in model.CaptchaSettings)
        {
            var captchaSetting =
                await captchaSettingRepository.FirstOrDefaultAsync(s =>
                    s.CaptchaSection == captchaSection.CaptchaSection);
            if (captchaSetting is null)
            {
                captchaSetting = captchaSection.ToCaptchaSetting();
                await captchaSettingRepository.InsertAsync(captchaSetting);
            }
            else
            {
                captchaSetting.CaptchaType = captchaSection.CaptchaType;
                captchaSettingRepository.Update(captchaSetting);
            }
        }

        await captchaSettingRepository.SaveChangesAsync();
        await memoryCacheService.RemoveByPrefixAsync(CacheKeys.CaptchaPrefix);
        return Result.Success(SuccessMessages.SavedChangesSuccessfully);
    }
}