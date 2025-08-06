using System.ComponentModel.DataAnnotations;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.User.Client;

public class ClientUpdateEmailViewModel
{
    [Display(Name = "ایمیل جدید")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(150, ErrorMessage = ErrorMessages.MaxLengthError)]
    [RegularExpression(SiteRegex.EmailRegex, ErrorMessage = ErrorMessages.RegexIncorrectFormat)]
    public string Email { get; set; }

    [Display(Name = "کد تایید")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(6, ErrorMessage = ErrorMessages.MaxLengthError)]
    [RegularExpression(SiteRegex.ConfirmationCodeRegex, ErrorMessage = ErrorMessages.RegexIncorrectFormat)]
    public string ConfirmationCode { get; set; }
}