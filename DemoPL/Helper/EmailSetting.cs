using Demo.DAL.Model;
using DemoPL.Settings;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;

namespace DemoPL.Helper
{
    public class EmailSetting : IEmailSettings
    {
        private readonly MailSettings options;

        public EmailSetting(IOptions<MailSettings> options)
        {
            this.options = options.Value;
        }
        public void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(options.Email),
                Subject = email.Subject,
            };

            mail.To.Add(MailboxAddress.Parse(email.To));
            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();
            mail.From.Add(new MailboxAddress(options.DisplayName, options.Email));
            using var smtp = new SmtpClient();
            smtp.Connect(options.Host, options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(options.Email, options.Password);
            smtp.Send(mail);
            smtp.Disconnect(true);
        }
    
    }
}
