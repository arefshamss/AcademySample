using Academy.Domain.Enums.Captcha;
using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.Captcha;

public class Captcha : BaseEntity<short>
{
    #region Properties

    public string Title { get; set; }

    public string SiteKey { get; set; }

    public string SecretKey { get; set; }

    public CaptchaType CaptchaType { get; set; }

    public bool IsActive { get; set; }

    #endregion
}