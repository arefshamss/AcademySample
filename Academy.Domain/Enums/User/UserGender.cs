using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.User;

public enum UserGender
{
    [Display(Name = "...")]
    None,
    [Display(Name = "مرد")]
    Male,
    [Display(Name = "زن")]
    Female
}