using Academy.Domain.ViewModels.SmsProvider;

namespace Academy.Application.Mapper.SmsProvider;

public static class SmsProviderMapper
{
    public static AdminSideSmsProviderViewModel ToAdminSideSmsProviderViewModel(
        this Domain.Models.SmsProvider.SmsProvider model) =>
        new()
        {
            Id = model.Id,
            ApiKey = model.ApiKey,
            SmsProviderType = model.SmsProviderType,
            Title = model.Title
        };

    public static AdminSideUpdateSmsProviderViewModel ToAdminSideUpdateSmsProviderViewModel(
        this Domain.Models.SmsProvider.SmsProvider model) =>
        new()
        {
            Id = model.Id,
            ApiKey = model.ApiKey,
            SmsProviderType = model.SmsProviderType,
            Title = model.Title
        };

    public static void ToSmsProvider(this AdminSideUpdateSmsProviderViewModel model,
        Domain.Models.SmsProvider.SmsProvider smsProvider)
    {
        smsProvider.ApiKey=model.ApiKey;
        smsProvider.Title=model.Title;
    }

}