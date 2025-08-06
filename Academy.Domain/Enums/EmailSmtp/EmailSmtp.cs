using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.Enums.EmailSmtp;

public enum EmailSmtpType
{
    [Display(Name = "ساده")]
    Simple,
    [Display(Name = "میل سرور")]
    MailServer
}