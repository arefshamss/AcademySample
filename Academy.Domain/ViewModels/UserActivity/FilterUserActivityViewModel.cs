using System.ComponentModel.DataAnnotations;
using Academy.Domain.Attributes;
using Academy.Domain.ViewModels.Common;

namespace Academy.Domain.ViewModels.UserActivity;

public class FilterUserActivityViewModel : BasePaging<UserActivityViewModel>
{
    [Display(Name = "توضیحات"), FilterInput]
    public string? Description { get; set; }

    [Display(Name = "Url"), FilterInput] public string? Url { get; set; }

    [Display(Name = "آی پی کاربر"), FilterInput]
    public string? IpAddress { get; set; }

    [Display(Name = "نام مرورگر"), FilterInput]
    public string? BrowserName { get; set; }

    [Display(Name = "از تاریخ"), FilterInput]
    public string? FromDate { get; set; }

    [Display(Name = "تا تاریخ"), FilterInput]
    public string? ToDate { get; set; }
}