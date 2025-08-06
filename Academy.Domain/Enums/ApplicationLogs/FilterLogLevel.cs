using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.ApplicationLogs;

public enum FilterLogLevel
{
    [Display(Name = "همه")]
    All , 
    Verbose , 
    Debug , 
    Information , 
    Warning , 
    Error , 
    Fatal 
}