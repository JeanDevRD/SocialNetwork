using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SocialNetwork.Core.Application.DTOs.Email;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Domain.Settings;

namespace SocialNetwork.Infrastructure.Shared
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> options, ILogger<EmailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendAsync(EmailRequestDto emailRequest)
        {
            try
            {

                emailRequest.ToRange?.Add(emailRequest.To ?? "");

                MimeMessage email = new MimeMessage()
                {
                    Sender = MailboxAddress.Parse(_settings.EmailFrom),
                    Subject = emailRequest.Subject,
                };
                foreach (var to in emailRequest.ToRange ?? [])
                {
                    email.To.Add(MailboxAddress.Parse(to));
                }

                BodyBuilder builder = new()
                {
                    HtmlBody = emailRequest.BodyHtml
                };

                email.Body = builder.ToMessageBody();
                using MailKit.Net.Smtp.SmtpClient smtp = new();

                await smtp.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }


            catch (Exception ex)
            {

                _logger.LogError(ex, "Error sending email");
            }
        }
    }

    
}
