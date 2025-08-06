using System.ComponentModel.DataAnnotations;
using Academy.Domain.Attributes;
using Academy.Domain.Enums.ApplicationLogs;
using Academy.Domain.ViewModels.Common;

namespace Academy.Domain.ViewModels.ApplicationLogs;

public class FilterApplicationLogsViewModel : BasePaging<ApplicationLogsViewModel>
{
    [FilterInput]
    public FilterLogLevel Level { get; set; } 
    
    [Display(Name= "از تاریخ") , FilterInput]
    public string? FromDate { get; set; }
    
    [Display(Name= "تا تاریخ") , FilterInput]
    public string? ToDate { get; set; }
}