using Academy.Domain.Enums.EmailSmtp;
using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.EmailSmtp;

public class EmailSmtp:BaseEntity<short>
{
    #region Properties

    public string DisplayName { get; set; }
    
    public string EamilAddress { get; set; }

    public string SmtpAddress { get; set; }

    public string Password { get; set; }

    public int? Port { get; set; }

    public bool EnableSSL { get; set; }

    public EmailSmtpType EmailSmtpType { get; set; }

    #endregion

    #region Relations

    public ICollection<SiteSettings.SiteSettings> SimpleSmtpSiteSettings { get; set; }    
    
    public ICollection<SiteSettings.SiteSettings> MailServerSmtpSiteSettings { get; set; }    

    #endregion
}