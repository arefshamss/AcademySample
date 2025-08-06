using Academy.Domain.Models.Captcha;
using Academy.Domain.ViewModels.CaptchaSetting;

namespace Academy.Application.Mapper.Captcha;

public static class CaptchaSettingMapper
{
    public static AdminSideCaptchaSettingViewModel ToAdminSideCaptchaSettingViewModel(this CaptchaSetting model) =>
        new()
        {
            CaptchaSection = model.CaptchaSection,
            CaptchaType = model.CaptchaType
        };

    public static CaptchaSetting ToCaptchaSetting(this AdminSideCaptchaSettingViewModel model) =>
        new()
        {
            CaptchaType = model.CaptchaType,
            CaptchaSection = model.CaptchaSection
        };

}