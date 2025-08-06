using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.SmsSetting;

public enum SmsSettingSection
{
    [Display(Name = "بخش اکانت کاربری(ورود ، فراموشی کلمه عبور ، ...)")]
    Account,

    [Display(Name = "پرداخت")]
    Payment,

    [Display(Name = "مدرک")]
    Certificate,
    
    [Display(Name = "تیکت")]
    Ticket,

    [Display(Name = "دوره")]
    Course,

    [Display(Name = "اس ام اس تخفیفات")]
    OffSmsNews
}