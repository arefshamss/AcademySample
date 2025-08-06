namespace Academy.Domain.ViewModels.User.Account;

public class AuthenticateUserViewModel
{
    public int UserId { get; set; }

    public string MobileOrEmail { get; set; }
    
    
    public string FullName;
}