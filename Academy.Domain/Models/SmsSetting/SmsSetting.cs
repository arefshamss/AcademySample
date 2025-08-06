using Academy.Domain.Enums.SmsProvider;
using Academy.Domain.Enums.SmsSetting;
using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.SmsSetting;

public class SmsSetting:BaseEntity<short>
{
    #region Properties

    public SmsProviderType SmsProviderType { get; set; }

    public SmsSettingSection  SmsSettingSection { get; set; }

    #endregion
}