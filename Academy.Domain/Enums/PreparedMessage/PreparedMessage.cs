using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.PreparedMessage;

public enum PreparedMessageSection
{
    [Display(Name = "نظرات دوره")]
    CourseComment,

    [Display(Name = "نظرات مقاله")]
    BlogComment,

    [Display(Name = "رزومه مدرسین")]
    MasterResume,

    [Display(Name = "فرآیند احراز هویت کاربر")]
    UserAuthenticationProcess,

    [Display(Name = "گزارش نظرات مقاله")]
    BlogCommentReport,

    [Display(Name = "گزارش نظرات دوره")]
    CourseCommentReport
}

public enum FilterPreparedMessageSection
{
    [Display(Name = "همه")]
    All,

    [Display(Name = "نظرات دوره")]
    CourseComment,

    [Display(Name = "نظرات مقاله")]
    BlogComment,

    [Display(Name = "رزومه مدرسین")]
    MasterResume,

    [Display(Name = "فرآیند احراز هویت کاربر")]
    UserAuthenticationProcess,

    [Display(Name = "گزارش نظرات مقاله")]
    BlogCommentReport,

    [Display(Name = "گزارش نظرات دوره")]
    CourseCommentReport
}