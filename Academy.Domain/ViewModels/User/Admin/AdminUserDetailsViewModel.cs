using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.User;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.User.Admin;

public class AdminUserDetailsViewModel
{
    public int Id { get; set; }
    
     public string? AvatarImageName {get;set;}
    
    
    [Display(Name = "نام:")]
    public string? FirstName { get; set; }
    
    
    [Display(Name = "نام خانوادگی:")]
    public string? LastName { get; set; }
    
    
    [Display(Name = "تلفن همراه:")]
    public string? Mobile { get; set; }    
    
    
    [Display(Name = "ایمیل:")]
    public string? Email { get; set; }
    
    public bool IsMobileActive { get; set; }
    
    public bool IsEmailActive { get; set; }
    
    
    [Display(Name = "وضعیت ارسال تیکت:")]
    public bool IsBannedFromTicket { get; set; }
    
    
    [Display(Name = "وضعیت کاربر")]
    public bool IsBanned { get; set; }
    
    
    [Display(Name = "وضعیت ثبت کامنت:")]
    public bool IsBannedFromComment { get; set; }
    
    
    [Display(Name = "تاریخ تولد:")]
    public DateTime? BirthDate { get; set; }
    
    
    [Display(Name = "کد پستی:")]
    public string? PostalCode { get; set; }
    
    
    [Display(Name = "نام پدر:")]
    public string? FatherName { get; set; }
    
    
    [Display(Name = "نحوه آشنایی با ما:")]
    public UserReferralSource ReferralSource { get; set; }
    
    
    [Display(Name = "جنسیت:")]
    public UserGender Gender { get; set; }
    
    
    [Display(Name = "آدرس:")]
    public string? Address { get; set; }
    
    
    [Display(Name = "کد ملی:")]
    public string? NationalCode { get; set; }
    
    
    [Display(Name = "شماره شناسنامه:")]
    public string? BirthCertificateNumber { get; set; }
    
    [Display(Name = "تاریخ عضویت:")]
    public DateTime CreatedDate { get; set; }
    
    public bool IsDeleted { get; set; }
     
    #region Helpers

    public string FullName => $"{FirstName} {LastName}";

    #endregion
}