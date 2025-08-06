using Academy.Domain.Enums.User;
using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.User;

public class UserInformation : BaseEntity
{
    #region Properties

    public DateTime? BirthDate { get; set; }

    public string? PostalCode  { get; set; }
    
    public string? FatherName  { get; set; }
    
    public UserReferralSource ReferralSource { get; set; }
    
    public UserGender Gender { get; set; }
    
    public string? Address { get; set; }
    
    public string? NationalCode  { get; set; }
    
    public string? BirthCertificateNumber  { get; set; }
    
    public int UserId { get; set; }

    #endregion
    
    #region Relations

    public User User { get; set; }
    
    #endregion
    
}