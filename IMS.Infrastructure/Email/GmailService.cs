using IMS.Application.Common.Interfaces;
using IMS.Infrastructure.Common.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace IMS.Infrastructure.Email
{
    internal class GmailService(IOptions<EmailOptions> emailOptions) : IEmailService
    {
        private readonly GmailOptions _gmailSettings = emailOptions.Value.Gmail;

        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("IMS System", _gmailSettings.Email));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(_gmailSettings.Email, _gmailSettings.AppPassword);

                await smtp.SendAsync(email, cancellationToken);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
