using Academy.Domain.Enums.Captcha;
using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.Captcha;

public class CaptchaSetting:BaseEntity<short>
{
    #region Properties

    public CaptchaType CaptchaType { get; set; }

    public CaptchaSection CaptchaSection { get; set; }

    #endregion
}