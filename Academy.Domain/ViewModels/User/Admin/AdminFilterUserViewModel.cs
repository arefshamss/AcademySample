using System.ComponentModel.DataAnnotations;
using Academy.Domain.Attributes;
using Academy.Domain.Enums.Common;
using Academy.Domain.Enums.User;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Common;

namespace Academy.Domain.ViewModels.User.Admin;

public class AdminFilterUserViewModel : BasePaging<AdminUserViewModel>
{
    [Display(Name="نام و یا نام خانوادگی"),FilterInput]
    public string? FullName { get; set; }  
    
    
    [Display(Name="ایمیل"),FilterInput]
    public string? Email { get; set; } 
    
    
    [Display(Name="شماره تماس"),FilterInput]
    public string? Mobile { get; set; }

    
    [Display(Name = "مرتب سازی بر اساس"),FilterInput]
    public FilterOrderBy FilterOrderBy { get; set; }
    
    [Display(Name = "از تاریخ"), FilterInput]
    public string? FromDate { get; set; }

    [Display(Name = "تا تاریخ"), FilterInput]
    public string? ToDate { get; set; }
    
    [Display(Name = "وضعیت کاربر"),FilterInput]
    public FilterUserStatus UserStatus { get; set; }
    
    
    [Display(Name = "وضعیت شماره تلفن"),FilterInput]
    public FilterUserMobileStatus UserMobileStatus { get; set; }  
    
    
    [Display(Name = "وضعیت ایمیل"),FilterInput]
    public FilterUserEmailStatus UserEmailStatus { get; set; } 
    
    
    [Display(Name = "وضعیت ارسال تیکت"),FilterInput]
    public FilterUserTicketStatus UserTicketStatus { get; set; }  
    
    
    [Display(Name = "وضعیت ارسال کامنت"),FilterInput]
    public FilterUserCommentStatus UserCommentStatus { get; set; }    
    
    
    [Display(Name = "نوع کاربر"),FilterInput]
    public FilterUserType UserType { get; set; }
    
    
    [Display(Name = "وضعیت حذف"),FilterInput]
    public DeleteStatus DeleteStatus { get; set; } 

}