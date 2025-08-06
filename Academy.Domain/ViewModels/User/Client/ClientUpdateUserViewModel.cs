using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.User;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.User.Client;

public class ClientUpdateUserViewModel
{
    public int Id { get; set; }
    
    
    [Display(Name = "نام")]
    [MaxLength(150, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? FirstName { get; set; }
    
    
    [Display(Name = "نام خانوادگی")]
    [MaxLength(150, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? LastName { get; set; }     
    
    
    [Display(Name = "تلفن همراه")]
    [RegularExpression(SiteRegex.MobileRegex, ErrorMessage = ErrorMessages.NotValid)]
    [MaxLength(11, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? Mobile { get; set; }    
    
    
    [Display(Name = "ایمیل")]
    [RegularExpression(SiteRegex.EmailRegex, ErrorMessage = ErrorMessages.NotValid)]
    [MaxLength(150, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? Email { get; set; }
    
    
    [Display(Name = "تاریخ تولد")]
    public string? BirthDate { get; set; }
    
    
    [Display(Name = "کد پستی")]
    [MaxLength(20, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? PostalCode { get; set; }
    
    
    [Display(Name = "نام پدر")]
    [MaxLength(100, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? FatherName { get; set; }
    
    
    [Display(Name = "نحوه آشنایی با ما")]
    public UserReferralSource ReferralSource { get; set; }
    
    
    [Display(Name = "جنسیت")]
    public UserGender Gender { get; set; }
    
    
    [Display(Name = "آدرس")]
    [MaxLength(500, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? Address { get; set; }
    
    
    [Display(Name = "کد ملی")]
    [RegularExpression(SiteRegex.NationalCodeRegex, ErrorMessage = ErrorMessages.NotValid)]
    [MaxLength(10, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? NationalCode { get; set; }
    
    
    [Display(Name = "شماره شناسنامه")]
    [MaxLength(20, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string? BirthCertificateNumber { get; set; }
    
    
    public string? ActiveCode { get; set; }

    public DateTime? ActiveCodeExpireTime { get; set; }

    public string? EmailActiveCode { get; set; }

    public DateTime? EmailActiveCodeExpireTime { get; set; }

    public string? MobileActiveCode { get; set; }

    public DateTime? MobileActiveCodeExpireTime { get; set; }

    [Display(Name = "رمز عبور")]
    public bool HasPassword { get; set; }
}