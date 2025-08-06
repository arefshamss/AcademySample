using Academy.Data.Statics;
using Academy.Domain.Enums.SmsProvider;
using Academy.Domain.Enums.SmsSetting;
using Academy.Domain.Models.SmsSetting;

namespace Academy.Data.Seeds;

public class SmsSettingSeeds
{
    public static List<SmsSetting> SmsSettingList { get; } =
    [
        new()
        {
            Id = 1,
            SmsProviderType = SmsProviderType.Kavenegar,
            SmsSettingSection = SmsSettingSection.Account,
            CreatedDate = SeedStaticDateTime.Date
        },
        new()
        {
            Id = 2,
            SmsProviderType = SmsProviderType.Kavenegar,
            SmsSettingSection = SmsSettingSection.Payment,
            CreatedDate = SeedStaticDateTime.Date
        },
        new()
        {
            Id = 3,
            SmsProviderType = SmsProviderType.Kavenegar,
            SmsSettingSection = SmsSettingSection.Certificate,
            CreatedDate = SeedStaticDateTime.Date
        },
        new()
        {
            Id = 4,
            SmsProviderType = SmsProviderType.Kavenegar,
            SmsSettingSection = SmsSettingSection.Ticket,
            CreatedDate = SeedStaticDateTime.Date
        },
        new()
        {
            Id = 5,
            SmsProviderType = SmsProviderType.Kavenegar,
            SmsSettingSection = SmsSettingSection.Course,
            CreatedDate = SeedStaticDateTime.Date
        },
        new()
        {
            Id = 6,
            SmsProviderType = SmsProviderType.Kavenegar,
            SmsSettingSection = SmsSettingSection.OffSmsNews,
            CreatedDate = SeedStaticDateTime.Date
        },
    ];
}