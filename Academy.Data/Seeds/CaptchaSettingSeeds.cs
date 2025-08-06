using Academy.Data.Statics;
using Academy.Domain.Enums.Captcha;
using Academy.Domain.Models.Captcha;

namespace Academy.Data.Seeds;

public class CaptchaSettingSeeds
{
    public static List<CaptchaSetting> CaptchaSettingList { get; } =
    [
        new()
        {
            CaptchaSection = CaptchaSection.Account,
            CaptchaType = CaptchaType.GoogleRecaptchaV3,
            Id = 1,
            CreatedDate = SeedStaticDateTime.Date,
        },
        new()
        {
            CaptchaSection = CaptchaSection.ContactUs,
            CaptchaType = CaptchaType.GoogleRecaptchaV3,
            Id = 2,
            CreatedDate = SeedStaticDateTime.Date,
        },
        new()
        {
            CaptchaSection = CaptchaSection.Payment,
            CaptchaType = CaptchaType.GoogleRecaptchaV3,
            Id = 3,
            CreatedDate = SeedStaticDateTime.Date,
        },
        new()
        {
            CaptchaSection = CaptchaSection.Ticket,
            CaptchaType = CaptchaType.GoogleRecaptchaV3,
            Id = 4,
            CreatedDate = SeedStaticDateTime.Date,
        }
    ];
}