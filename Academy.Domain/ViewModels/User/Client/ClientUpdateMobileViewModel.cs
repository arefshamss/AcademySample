using System.ComponentModel.DataAnnotations;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.User.Client;

public class ClientUpdateMobileViewModel
{
    [Display(Name = "شماره موبایل جدید")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(11, ErrorMessage = ErrorMessages.MaxLengthError)]
    [RegularExpression(SiteRegex.MobileRegex, ErrorMessage = ErrorMessages.RegexIncorrectFormat)]
    public string Mobile { get; set; }

    [Display(Name = "کد تایید")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(6, ErrorMessage = ErrorMessages.MaxLengthError)]
    [RegularExpression(SiteRegex.ConfirmationCodeRegex, ErrorMessage = ErrorMessages.RegexIncorrectFormat)]
    public string ConfirmationCode { get; set; }
}