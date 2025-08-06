using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.Role;

public enum RoleSection
{
    [Display(Name = "ادمین")]
    Admin,
    [Display(Name = "مدرس")]
    Master
}
public enum FilterRoleSection
{
    [Display(Name = "همه")]
    All,
    [Display(Name = "ادمین")]
    Admin,
    [Display(Name = "مدرس")]
    Master
}