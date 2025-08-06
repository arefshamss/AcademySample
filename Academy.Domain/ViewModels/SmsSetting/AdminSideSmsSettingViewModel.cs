using System.ComponentModel.DataAnnotations;
using Academy.Domain.Enums.SmsProvider;
using Academy.Domain.Enums.SmsSetting;

namespace Academy.Domain.ViewModels.SmsSetting;

public class AdminSideSmsSettingViewModel
{
    [Display(Name = "بخش")]
    public SmsProviderType SmsProviderType { get; set; }

    [Display(Name = "پروایدر ارسال اس ام اس")]
    public SmsSettingSection SmsSettingSection { get; set; }
}