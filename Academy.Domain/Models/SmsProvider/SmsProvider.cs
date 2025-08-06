using Academy.Domain.Enums.SmsProvider;
using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.SmsProvider;

public class SmsProvider:BaseEntity<short>
{
    #region Properties

    public string Title { get; set; }

    public SmsProviderType SmsProviderType { get; set; }

    public string ApiKey { get; set; }

    #endregion
}