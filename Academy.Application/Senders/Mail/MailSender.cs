using System.Net;
using System.Net.Mail;
using Academy.Application.Services.Interfaces;
using Academy.Domain.Enums.EmailSmtp;
using Academy.Domain.ViewModels.EmailSmtp;
using Microsoft.Extensions.Logging;

namespace Academy.Application.Senders.Mail
{
    public class MailSender(
        IEmailSmtpService emailSmtpService,
        ILogger<MailSender> logger
    )
        : IMailSender
    {

        public async Task<bool> SendAsync(string to, string subject, string body, EmailSmtpType smtpType = EmailSmtpType.Simple)
        {
            try
            {
                EmailSmtpViewModel? defaultEmail = null;

                switch (smtpType)
                {
                    case EmailSmtpType.Simple:
                        defaultEmail =(await emailSmtpService.GetDefaultSimpleEmailSmtpAsync())?.Value;
                        break;

                    case EmailSmtpType.MailServer:
                        defaultEmail =(await emailSmtpService.GetDefaultMailServerEmailSmtpAsync())?.Value;
                        break;
                }

                if (defaultEmail == null)
                    throw new Exception("Not found smtp email");

                var mail = new MailMessage();

                var smtpServer = new SmtpClient(defaultEmail.SmtpAddress);

                mail.From = new MailAddress(defaultEmail.EmailAddress, defaultEmail.DisplayName);

                mail.To.Add(to);

                mail.Subject = subject;

                mail.Body = body;

                mail.IsBodyHtml = true;
                
                if (defaultEmail.Port.HasValue)
                    smtpServer.Port = defaultEmail.Port.Value;

                smtpServer.Credentials = new NetworkCredential(defaultEmail.EmailAddress, defaultEmail.Password);

                smtpServer.EnableSsl = defaultEmail.EnableSSL;

                smtpServer.Send(mail);

                return true;
            }
            catch (Exception e)
            {
                logger.LogError($"MailSender class: SendAsync Method: \n\tErrorMessage:: {e.Message}");
                return false;
            }
        }

        public async Task<bool> SendAsync(string[] to, string subject, string body, EmailSmtpType smtpType = EmailSmtpType.Simple)
        {
            try
            {
                EmailSmtpViewModel? defaultEmail = null;

                switch (smtpType)
                {
                    case EmailSmtpType.Simple:
                        defaultEmail =(await emailSmtpService.GetDefaultSimpleEmailSmtpAsync())?.Value;
                        break;

                    case EmailSmtpType.MailServer:
                        defaultEmail =(await emailSmtpService.GetDefaultMailServerEmailSmtpAsync())?.Value;
                        break;
                }


                if (defaultEmail == null)
                    throw new Exception("Not found smtp email");

                var mail = new MailMessage();

                var smtpServer = new SmtpClient(defaultEmail.SmtpAddress);

                mail.From = new MailAddress(defaultEmail.EmailAddress, defaultEmail.DisplayName);

                foreach (var m in to)
                {
                    mail.To.Add(m);
                }

                mail.Subject = subject;

                mail.Body = body;

                mail.IsBodyHtml = true;

                if (defaultEmail.Port.HasValue)
                    smtpServer.Port = defaultEmail.Port.Value;

                smtpServer.Credentials = new NetworkCredential(defaultEmail.EmailAddress, defaultEmail.Password);

                smtpServer.EnableSsl = defaultEmail.EnableSSL;

                smtpServer.Send(mail);

                return true;
            }
            catch (Exception e)
            {
                logger.LogError($"MailSender class: SendAsync Method: \n\tErrorMessage:: {e.Message}");
                return false;
            }
        }
    }
}