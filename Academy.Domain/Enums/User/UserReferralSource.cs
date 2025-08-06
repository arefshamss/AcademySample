using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.User;

public enum UserReferralSource
{
    [Display(Name = "...")]
    None,
    [Display(Name = "تلگرام")]
    Telegram,
    [Display(Name = "اینستاگرام")]
    Instagram,
    [Display(Name = "سایت تاپلرن")]
    TopLearn,
    [Display(Name = "معرفی دوستان و آشنایان")]
    Friends
}