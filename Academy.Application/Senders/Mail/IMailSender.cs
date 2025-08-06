using Academy.Domain.Enums.EmailSmtp;

namespace Academy.Application.Senders.Mail
{
    public interface IMailSender
    {
        Task<bool> SendAsync(string to, string subject, string body,EmailSmtpType smtpType=EmailSmtpType.Simple);
        
        Task<bool> SendAsync(string[] to, string subject, string body,EmailSmtpType smtpType=EmailSmtpType.Simple);
    }
}