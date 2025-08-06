using Academy.Domain.Enums.SmsProvider;

namespace Academy.Domain.ViewModels.SmsProvider;

public class AdminSideSmsProviderViewModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public SmsProviderType SmsProviderType { get; set; }

    public string ApiKey { get; set; }
}