using System.ComponentModel.DataAnnotations;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.User.Client;

public class ClientAddPasswordViewModel
{
    [Display(Name = "کد تایید")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(6, ErrorMessage = ErrorMessages.MaxLengthError)]
    [RegularExpression(SiteRegex.ConfirmationCodeRegex, ErrorMessage = ErrorMessages.RegexIncorrectFormat)]
    public string OtpCode { get; set; }
    
    
    [Display(Name = "رمز عبور")]
    [DataType(DataType.Password)]
    [MaxLength(500, ErrorMessage = ErrorMessages.MaxLengthError)]
    [MinLength(6, ErrorMessage = ErrorMessages.MinLengthError)]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public string Password { get; set; }

    
    [Display(Name = "تکرار رمز عبور")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [Compare("Password", ErrorMessage = ErrorMessages.PasswordCompareError)]
    public string ConfirmPassword { get; set; }
}