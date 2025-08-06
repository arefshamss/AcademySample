using Academy.Domain.Enums.Captcha;

namespace Academy.Domain.ViewModels.Captcha;

public class ClientCaptchaViewModel
{
    public CaptchaType CaptchaType { get; set; }

    public string? SiteKey { get; set; }

    public string? SecretKey { get; set; }

    public string? CaptchaToken { get; set; }
}