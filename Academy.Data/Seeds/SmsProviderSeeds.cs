using Academy.Domain.Enums.SmsProvider;
using Academy.Domain.Models.SmsProvider;

namespace Academy.Data.Seeds;

public class SmsProviderSeeds
{
    public static List<SmsProvider> SmsProviders { get; } =
    [
        new()
        {
            Id = 1,
            Title = "کاوه نگار",
            ApiKey = "16be6c43-2b7d-462f-8032-96b2472112c3",
            SmsProviderType = SmsProviderType.Kavenegar
        },
        new()
        {
            Id = 2,
            Title = "پارس  گرین",
            ApiKey = "36216de3-e60e-4fe6-9932-87cd79a12d0c",
            SmsProviderType = SmsProviderType.ParsGreen
        }
    ];
}