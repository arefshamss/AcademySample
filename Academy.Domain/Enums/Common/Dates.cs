using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.Common;

public enum DayOfWeek
{
    [Display(Name = "همه روز ها")]
    All,

    [Display(Name = "شنبه")]
    Saturday,

    [Display(Name = "یکشنبه")]
    Sunday,

    [Display(Name = "دوشنبه")]
    Monday,

    [Display(Name = "سه شنبه")]
    Tuesday,

    [Display(Name = "چهار شنبه")]
    Wednesday,

    [Display(Name = "پنج شنبه")]
    Thursday,

    [Display(Name = "جمعه")]
    Friday
}