using Academy.Domain.Enums.EmailSmtp;

namespace Academy.Domain.ViewModels.EmailSmtp;

public class AdminSideEmailSmtpViewModel
{
    public short Id { get; set; }

    public string DisplayName { get; set; }
    
    public string EmailAddress { get; set; }

    public string SmtpAddress { get; set; }

    public int? Port { get; set; }

    public bool EnableSSL { get; set; }

    public EmailSmtpType EmailSmtpType { get; set; }

    public bool IsDeleted { get; set; }
}