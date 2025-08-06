using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.Common;

public enum FilterCommentReportRead
{
    [Display(Name = "همه")]
    All ,
    [Display(Name = "خوانده شده")]
    Read  ,
    [Display(Name= "خوانده نشده")]
    Unread 
}