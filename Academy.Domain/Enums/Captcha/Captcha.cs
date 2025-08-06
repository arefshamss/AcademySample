using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.Captcha;

public enum CaptchaType
{
    [Display(Name = "گوگل کپتچا ورژن دو")]
    GoogleRecaptchaV2,

    [Display(Name = "گوگل کپتچا ورژن سه")]
    GoogleRecaptchaV3, 

    [Display(Name = "آر کپتچا")]
    ARCaptcha
}

public enum CaptchaSection
{
    [Display(Name = "بخش اکانت کاربری(ورود ، فراموشی کلمه عبور ، ...)")]
    Account,

    [Display(Name = "پرداخت")]
    Payment,

    [Display(Name = "تیکت")]
    Ticket,

    [Display(Name = "تماس با ما")]
    ContactUs
}