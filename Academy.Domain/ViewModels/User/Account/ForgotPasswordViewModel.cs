using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.ViewModels.User.Account;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "لطفاً ایمیل یا شماره موبایل را وارد کنید.")]
    public string MobileOrEmail { get; set; }
}