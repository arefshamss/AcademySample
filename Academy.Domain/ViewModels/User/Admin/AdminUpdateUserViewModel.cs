using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.User;
using Academy.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace Academy.Domain.ViewModels.User.Admin;

public class AdminUpdateUserViewModel
{
    public int Id { get; set; }
    
    
    public string? AvatarImageName {get;set;}
    
    
     [Display(Name = "تصویر پروفایل")]
    public IFormFile? AvatarImageFile { get; set; }
    
    
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
    
    
    [Display(Name = "رمز عبور")]
    [DataType(DataType.Password)]
    [MaxLength(500, ErrorMessage = ErrorMessages.MaxLengthError)]
    [MinLength(6, ErrorMessage = ErrorMessages.MinLengthError)]
    public string? Password { get; set; }   
    
    
    [Display(Name = "تکرار رمز عبور")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = ErrorMessages.PasswordCompareError)]
    public string? ConfirmPassword { get; set; }
    
    
    [Display(Name = "شماره تلفن فعال است؟")]
    public bool IsMobileActive { get; set; }
    
    
    [Display(Name = "ایمیل فعال است؟")]
    public bool IsEmailActive { get; set; }
    
    
    [Display(Name = "   مسدود کردن از ارسال تیکت")]
    public bool IsBannedFromTicket { get; set; }
    
    
    [Display(Name = "   مسدود کردن کاربر")]
    public bool IsBanned { get; set; }
    
    
    [Display(Name = "   مسدود کردن از ارسال کامنت")]
    public bool IsBannedFromComment { get; set; }
    
    
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
    
}