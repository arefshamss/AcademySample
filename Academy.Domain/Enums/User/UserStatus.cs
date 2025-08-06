using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.User;

public enum UserStatus : byte
{
    [Display(Name = "فعال")]
    Active = 1,
    [Display(Name = "مسدود شده")]
    Banned = 2
}

public enum FilterUserStatus : byte
{
    [Display(Name = "همه")]
    All = 0,
    [Display(Name = "فعال")]
    Active = 1,
    [Display(Name = "مسدود شده")]
    Banned = 2
}
public enum FilterUserMobileStatus : byte
{
    [Display(Name = "همه")]
    All = 0,
    [Display(Name = "فعال")]
    Active = 1,
    [Display(Name = "غیرفعال")]
    NotActive = 2
}
public enum FilterUserEmailStatus : byte
{
    [Display(Name = "همه")]
    All = 0,
    [Display(Name = "فعال")]
    Active = 1,
    [Display(Name = "غیرفعال")]
    NotActive = 2
}

public enum FilterUserTicketStatus : byte
{
    [Display(Name = "همه")]
    All = 0,
    [Display(Name = "فعال")]
    Active = 1,
    [Display(Name = "مسدود شده")]
    Banned = 2
}

public enum FilterUserCommentStatus : byte
{
    [Display(Name = "همه")]
    All = 0,
    [Display(Name = "فعال")]
    Active = 1,
    [Display(Name = "مسدود شده")]
    Banned = 2
}

public enum UserType : byte
{
    [Display(Name = "کاربر عادی")]
    Normal,
    [Display(Name = "مدرس")]
    Master
}

public enum FilterUserType : byte
{
    [Display(Name = "همه")]
    All,
    [Display(Name = "عادی")]
    Normal,
    [Display(Name = "مدرس")]
    Master
}