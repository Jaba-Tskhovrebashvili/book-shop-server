using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;


namespace FirstProject.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration) {
            this._configuration= configuration;
        }

        public async Task SendEmailOtp(string recipientEmail, string link, string receiverName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["EmailSettings:SMTP_USER"], _configuration["EmailSettings:SMTP_USER"]));
            message.To.Add(new MailboxAddress(receiverName, recipientEmail));
            message.Subject = $"Hello {receiverName}";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"Your code is <a href=\"{_configuration["Client:Address"]}/sign-up?token={link}\">click here</a>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["EmailSettings:SMTP_HOST"], int.Parse(_configuration["EmailSettings:PORT"]), false);
                client.Authenticate(_configuration["EmailSettings:SMTP_USER"], _configuration["EmailSettings:SMTP_PASSWORD"]);

                await client.SendAsync(message);
                client.Disconnect(true);
            }

        }
        }
}
