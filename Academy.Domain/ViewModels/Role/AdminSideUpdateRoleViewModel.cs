using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.Role;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.Role;

public class AdminSideUpdateRoleViewModel
{
    public short Id { get; set; }

    [Display(Name = "نام نقش")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(250, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string RoleName { get; set; }

    [Display(Name = "نوع نقش")]
    public RoleSection RoleSection { get; set; }
}