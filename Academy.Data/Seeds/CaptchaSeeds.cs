using Academy.Data.Statics;
using Academy.Domain.Enums.Captcha;
using Academy.Domain.Models.Captcha;

namespace Academy.Data.Seeds;

public class CaptchaSeeds
{
    public static List<Captcha> CaptchaList { get; } =
    [
        new()
        {
            Id = 1,
            CaptchaType = CaptchaType.ARCaptcha,
            CreatedDate = SeedStaticDateTime.Date,
            IsActive = true,
            Title = "ar captcha",
            SiteKey = "hfz7j1jzqn",
            SecretKey = "o4berc6rqidu8zbd5uqs"
        },
        new()
        {
            Id = 2,
            CaptchaType = CaptchaType.GoogleRecaptchaV2,
            CreatedDate = SeedStaticDateTime.Date,
            IsActive = true,
            Title = "google recaptcha 2",
            SiteKey = "6LfhYIMqAAAAANG-u6hSXn78NNaHkh9YC0Dl8A9k",
            SecretKey = "6LfhYIMqAAAAAMnSYhdAQQIQ0xfoT4jB0M3n4hjt"
        },
        new()
        {
            Id = 3,
            CaptchaType = CaptchaType.GoogleRecaptchaV3,
            CreatedDate = SeedStaticDateTime.Date,
            IsActive = true,
            Title = "google recaptcha 3",
            SiteKey = "6LdlYYMqAAAAAMZPw2mzADp3pSNynHA2UQ5svTWA",
            SecretKey = "6LdlYYMqAAAAAOBvpRDN6dYVJyF91VT91LZNi0Rk"
        },
    ];
}