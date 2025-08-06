using Academy.Domain.Enums.Captcha;

namespace Academy.Domain.ViewModels.CaptchaSetting;

public class AdminSideCaptchaSettingViewModel
{
    public CaptchaType CaptchaType { get; set; }

    public CaptchaSection CaptchaSection { get; set; }
}