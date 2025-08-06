using Academy.Domain.ViewModels.SmsSetting;

namespace Academy.Application.Mapper.SmsSetting;

public static class SmsSettingMapper
{
    public static AdminSideSmsSettingViewModel ToAdminSideSmsSettingViewModel(
        this  Domain.Models.SmsSetting.SmsSetting  model) =>
        new()
        {
            SmsProviderType = model.SmsProviderType,
            SmsSettingSection = model.SmsSettingSection
        };

    public static Domain.Models.SmsSetting.SmsSetting ToSmsSetting(this AdminSideSmsSettingViewModel model) =>
        new()
        {
            SmsProviderType = model.SmsProviderType,
            SmsSettingSection = model.SmsSettingSection
        };

}