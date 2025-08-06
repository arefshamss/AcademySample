using System.ComponentModel.DataAnnotations;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.User.Client;

public class ClientUpdatePasswordViewModel
{
    [Display(Name = "رمز عبور فعلی")]
    [DataType(DataType.Password)]
    [MaxLength(500, ErrorMessage = ErrorMessages.MaxLengthError)]
    [MinLength(6, ErrorMessage = ErrorMessages.MinLengthError)]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public string CurrentPassword { get; set; }

    [Display(Name = "رمز عبور جدید")]
    [DataType(DataType.Password)]
    [MaxLength(500, ErrorMessage = ErrorMessages.MaxLengthError)]
    [MinLength(6, ErrorMessage = ErrorMessages.MinLengthError)]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public string NewPassword { get; set; }

    [Display(Name = "تکرار رمز عبور جدید")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [Compare("NewPassword", ErrorMessage = ErrorMessages.PasswordCompareError)]
    public string ConfirmNewPassword { get; set; }
}