using System.ComponentModel.DataAnnotations;
using Academy.Domain.Attributes;
using Academy.Domain.Enums.Common;
using Academy.Domain.ViewModels.Common;

namespace Academy.Domain.ViewModels.EmailSmtp;

public class FilterAdminSideEmailSmtpViewModel : BasePaging<AdminSideEmailSmtpViewModel>
{
    [FilterInput,Display(Name = "ایمیل")]
    public string? EmailAddress { get; set; }

    [FilterInput,Display(Name = "وضعیت حذف")]
    public DeleteStatus DeleteStatus { get; set; }
}