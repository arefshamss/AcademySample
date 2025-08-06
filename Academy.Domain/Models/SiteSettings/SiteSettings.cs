using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.SiteSettings;

public class SiteSettings : BaseEntity<short>
{
    public short? DefaultSimpleSmtpId { get; set; }

    public short? DefaultMailServerSmtpId { get; set; }

    public string SiteName { get; set; }
    
    public string Copyright { get; set; }

    public string Logo { get; set; }

    public string Favicon { get; set; }
    
    #region Relations

    public EmailSmtp.EmailSmtp? DefaultSimpleSmtp { get; set; }

    public EmailSmtp.EmailSmtp? DefaultMailServerSmtp { get; set; }

    #endregion

}