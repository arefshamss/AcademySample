using Academy.Domain.Enums.Captcha;

namespace Academy.Domain.ViewModels.Captcha;

public class AdminSideCaptchaViewModel
{
    public short Id { get; set; }

    public string Title { get; set; }

    public CaptchaType CaptchaType { get; set; }

    public bool IsActive { get; set; }
}