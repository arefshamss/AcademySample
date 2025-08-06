namespace Academy.Domain.ViewModels.User.Account;

public class ConfirmLoginViewModel
{
    public int UserId { get; set; }
    
    public string MobileOrEmail { get; set; }
    
    public string FullName { get; set; }
    
    public string? ActiveCode { get; set; }     
    
    public bool IsLoginByPassword { get; set; } 
}