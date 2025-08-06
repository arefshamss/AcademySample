using System.ComponentModel.DataAnnotations;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.User.Account;

// public class LoginOrRegisterViewModel : GoogleReCaptchaViewModel
public class LoginOrRegisterViewModel
{
    
    [Display(Name = "ایمیل یا شماره همراه")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public string MobileOrEmail { get; set; }
    
    [Display(Name = "رمز عبور")]
    [DataType(DataType.Password)]
    [MaxLength(500, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? Password { get; set; }   
    
    public string? ReturnUrl { get; set; }
    
    public bool IsLoginByPassword { get; set; } 
}