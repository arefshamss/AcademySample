using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.SmsProvider;

public enum SmsProviderType
{
    [Display(Name = "کاوه نگار")]
    Kavenegar,

    [Display(Name = "پارس گرین")]
    ParsGreen
}