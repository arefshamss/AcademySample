using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.EmailSmtp;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.EmailSmtp;

public class AdminSideUpdateEmailSmtpViewModel
{
    public short Id { get; set; }
    
    [Display(Name = "آدرس ایمیل")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(350, ErrorMessage = ErrorMessages.MaxLengthError)]
    [EmailAddress(ErrorMessage = ErrorMessages.EmailNotValidError)]
    public string EmailAddress { get; set; }
    
    [Display(Name = "نام نمایشی")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(100, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string DisplayName { get; set; }

    [Display(Name = "آدرس SMTP")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(350, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string SmtpAddress { get; set; }

    [Display(Name = "رمز")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(350, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string Password { get; set; }

    [Display(Name = "پورت")]
    public int? Port { get; set; }

    [Display(Name = "وضعیت SSL")]
    public bool EnableSSL { get; set; }

    [Display(Name = "نوع")]
    public EmailSmtpType EmailSmtpType { get; set; }
}