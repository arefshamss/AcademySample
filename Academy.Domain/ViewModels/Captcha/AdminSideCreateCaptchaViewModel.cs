using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.Captcha;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.Captcha;

public class AdminSideCreateCaptchaViewModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(250,ErrorMessage = ErrorMessages.MaxLengthError)]
    public string Title { get; set; }

    [Display(Name = "SiteKey")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(450, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string SiteKey { get; set; }

    [Display(Name = "SecretKey")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(450, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string SecretKey { get; set; }

    [Display(Name = "نوع کپتچا")]
    public CaptchaType CaptchaType { get; set; }

    [Display(Name = "فعال ؟")]
    public bool IsActive { get; set; }
}