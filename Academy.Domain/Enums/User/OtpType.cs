using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.User;

public enum OtpType
{
    [Display(Name = "پیش‌فرض")]
    Default,
    [Display(Name = "ایمیل")]
    Email,
    [Display(Name = "موبایل")]
    Mobile,
}