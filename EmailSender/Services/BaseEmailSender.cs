using EmailSender.Abstractions;
using EmailSender.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace EmailSender.Services
{
    public abstract class BaseEmailSender<TSmtpOptions> : IBaseEmailSender where TSmtpOptions : BaseEmailOptions
    {
        private readonly TSmtpOptions emailOptions;

        private readonly ILogger logger;

        public BaseEmailSender(TSmtpOptions emailOptions, ILogger logger/*ILogger<BaseEmailSender<TSmtpOptions>> logger*/)
        {
            this.emailOptions = emailOptions;
            this.logger = logger;
        }

        public async Task<bool> Send(string to, string subject, string body)
        {
            if (emailOptions.EmailSendingIsDisabled)
            {
                logger.LogInformation("Letter {Email} with Subject {Subject} was not sent because email sending option was dsiabled", to, subject);
            }

            var message = new MimeMessage()
            {
                Subject = subject,
                Body = new TextPart(TextFormat.Html)
                {
                    Text = body,
                    ContentTransferEncoding = ContentEncoding.QuotedPrintable
                }
            };

            message.From.Add(MailboxAddress.Parse(emailOptions.From));
            message.To.Add(MailboxAddress.Parse(emailOptions.From));

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(emailOptions.Host, emailOptions.Port, emailOptions.IsSSL);

                if (emailOptions.UseNtlmAuth)
                    await client.AuthenticateAsync(new SaslMechanismNtlm());
                else if (!string.IsNullOrEmpty(emailOptions.Login) && !string.IsNullOrEmpty(emailOptions.Login))
                    await client.AuthenticateAsync(emailOptions.Login, emailOptions.Password);
                else
                {
                    logger.LogError("Email service is not configured!");
                    return false;
                }

                await client.SendAsync(message);
                logger.LogInformation("Letter was sent to email {Email} with subject {Subject}", message.To.First().Name, message.Subject);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Letter was not sent to email {Email} with subject {Subject}!", to, subject);
                return false;
            }
        }
    }
}
