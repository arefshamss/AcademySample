namespace Academy.Domain.ViewModels.User.Client;

public class ClientUserViewModel
{
    public int Id { get; set; }

    public string? AvatarImageName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsMobileActive { get; set; }

    public bool IsEmailActive { get; set; }

    #region Helper

    public string FullName => $"{FirstName} {LastName}";

    #endregion
}