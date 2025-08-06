namespace Academy.Domain.ViewModels.User.Account;

public class ConfirmLoginForGoogleViewModel
{
    public required string  GoogleAuthenticationId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string? Mobile { get; set; }
}