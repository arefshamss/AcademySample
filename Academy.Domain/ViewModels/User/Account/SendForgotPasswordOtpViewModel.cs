namespace Academy.Domain.ViewModels.User.Account;

public class SendForgotPasswordOtpViewModel
{
    public int? UserId { get; set; }
    
    public string ActiveCodeExpireDateTime { get; set; }
}