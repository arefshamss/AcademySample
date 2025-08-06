using System.ComponentModel.DataAnnotations;
using Academy.Domain.Attributes;
using Academy.Domain.Enums.Common;
using Academy.Domain.Enums.Role;
using Academy.Domain.ViewModels.Common;

namespace Academy.Domain.ViewModels.Role;

public class AdminSideFilterRoleViewModel:BasePaging<AdminSideRoleViewModel> 
{
    [Display(Name = "نام نقش"),FilterInput]
    public string? RoleName { get; set; }

    [Display(Name = "نوع نقش"), FilterInput]
    public FilterRoleSection RoleSection { get; set; }

    [Display(Name = "وضعیت حذف"), FilterInput]
    public DeleteStatus DeleteStatus { get; set; }
}