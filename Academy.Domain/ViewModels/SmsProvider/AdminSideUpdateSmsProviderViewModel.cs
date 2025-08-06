using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.SmsProvider;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.SmsProvider;

public class AdminSideUpdateSmsProviderViewModel
{
    public short Id { get; set; }

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(150, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string Title { get; set; }

    [Display(Name = "نوع سرویس")]
    public SmsProviderType SmsProviderType { get; set; }

    [Display(Name = "کلید API")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(450, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string ApiKey { get; set; }
}