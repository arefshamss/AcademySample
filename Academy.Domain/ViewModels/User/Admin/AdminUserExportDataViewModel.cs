using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.User;

namespace Academy.Domain.ViewModels.User.Admin;

public class AdminUserExportDataViewModel
{
    [Display(Name = "شناسه")]
    public int Id { get; set; } 
    
    
    [Display(Name = "نام")]
    public string? FirstName { get; set; }
    
    
    [Display(Name = "نام خانوادگی")]
    public string? LastName { get; set; }
    
    
    [Display(Name = "شماره تلفن")]
    public string? Mobile { get; set; }
    
    
    [Display(Name = "وضعیت شماره تلفن")]
    public string IsMobileActive { get; set; }
    
    
    [Display(Name = "ایمیل")]
    public string? Email { get; set; }
    
    
    [Display(Name = "وضعیت ایمیل")]
    public string IsEmailActive { get; set; }
    
    
    [Display(Name = "تاریخ عضویت")]
    public string CreatedDate { get; set; }  
    
    
    [Display(Name = "وضعیت کاربر")]
    public string IsBanned { get; set; }
    
    
    [Display(Name = "وضعیت ارسال تیکت")]
    public string IsBannedFromTicket { get; set; }
    
    
    [Display(Name = "وضعیت ثبت کامنت")]
    public string IsBannedFromComment { get; set; }
    
        
    [Display(Name = "وضعیت حذف")]
    public string IsDeleted { get; set; }
}

public class AdminUserExportMobileDataViewModel
{
    [Display(Name = "شماره تلفن")]
    public string? Mobile { get; set; }
}