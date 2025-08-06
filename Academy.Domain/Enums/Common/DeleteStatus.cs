using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.Common;

public enum DeleteStatus
{
    [Display(Name = "حذف نشده")] NotDeleted,
    [Display(Name = "همه")] All,
    [Display(Name = "حذف شده")] Deleted
}