using Academy.Domain.ViewModels.Captcha;

namespace Academy.Application.Mapper.Captcha;

public static class CaptchaMapper
{
    public static AdminSideCaptchaViewModel ToAdminSideCaptchaViewModel(this Domain.Models.Captcha.Captcha model) =>
        new()
        {
            Id = model.Id,
            CaptchaType = model.CaptchaType,
            IsActive = model.IsActive,
            Title = model.Title
        };

    public static Domain.Models.Captcha.Captcha ToCaptcha(this AdminSideCreateCaptchaViewModel model) =>
        new()
        {
            IsActive = model.IsActive,
            CaptchaType = model.CaptchaType,
            SiteKey = model.SiteKey,
            SecretKey = model.SecretKey,
            Title = model.Title
        };

    public static AdminSideUpdateCaptchaViewModel ToAdminSideUpdateCaptchaViewModel(this Domain.Models.Captcha.Captcha model) =>
        new()
        {
            Id = model.Id,
            CaptchaType = model.CaptchaType,
            IsActive = model.IsActive,
            SecretKey = model.SecretKey,
            Title = model.Title,
            SiteKey = model.SiteKey
        };

    public static void ToCaptcha(this AdminSideUpdateCaptchaViewModel model, Domain.Models.Captcha.Captcha captcha)
    {
        captcha.IsActive = model.IsActive;
        captcha.Title = model.Title;
        captcha.SiteKey = model.SiteKey;
        captcha.SecretKey = model.SecretKey;
    }

    public static ClientCaptchaViewModel ToClientCaptchaViewModel(this Domain.Models.Captcha.Captcha model) =>
        new()
        {
            CaptchaType = model.CaptchaType,
            SecretKey = model.SecretKey,
            SiteKey = model.SiteKey
        };
}