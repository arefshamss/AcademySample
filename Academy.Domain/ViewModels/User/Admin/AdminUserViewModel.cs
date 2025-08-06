using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.User;

namespace Academy.Domain.ViewModels.User.Admin;

public class AdminUserViewModel
{
    
    public int Id { get; set; } 
    
    public string? AvatarImageName { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    
    
    public string? Mobile { get; set; }
    
    public DateTime CreatedDate { get; set; }  
    
    public bool IsDeleted { get; set; }
    
    public bool IsMobileActive { get; set; }

    public bool IsEmailActive { get; set; }
    
    public bool IsBanned { get; set; }
    
    public bool IsBannedFromTicket { get; set; }
    
    public bool IsBannedFromComment { get; set; }
    
    public UserType UserType { get; set; }

    public string Status{ get; set; }   

    #region Helpers

    public string FullName => $"{FirstName} {LastName}";

    #endregion
}