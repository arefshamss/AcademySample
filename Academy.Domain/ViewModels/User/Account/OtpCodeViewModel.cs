using System.ComponentModel.DataAnnotations;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.User.Account;

public class OtpCodeViewModel
{
    public int UserId { get; set; }
    
    
    public string MobileOrEmail { get; set; }  
    
    
    public string? ReturnUrl { get; set; }
    

    [Display(Name = "کد تایید")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(6, ErrorMessage = ErrorMessages.MaxLengthError)]
    [RegularExpression(SiteRegex.ConfirmationCodeRegex, ErrorMessage = ErrorMessages.RegexIncorrectFormat)]
    public string Code { get; set; }
}