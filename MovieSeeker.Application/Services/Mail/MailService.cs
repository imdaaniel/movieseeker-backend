using System.Net;
using System.Net.Mail;

using Microsoft.Extensions.Options;

using MovieSeeker.Application.Settings;

namespace MovieSeeker.Application.Services.Mail
{
    public class MailService : IMailService 
    {
        private readonly AppSettings _appSettings;

        public MailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<bool> SendMail(string receiver, string subject, string body)
        {
            if (!ValidateParameters(receiver, subject, body))
            {
                return false;
            }

            try
            {
                var client = new SmtpClient(_appSettings.Mail.Smtp.Server, _appSettings.Mail.Smtp.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_appSettings.Mail.Smtp.Username, _appSettings.Mail.Smtp.Password),
                    EnableSsl = true
                };

                var message = new MailMessage(_appSettings.Mail.Sender.Email, receiver, subject, body);
                await client.SendMailAsync(message);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool ValidateParameters(string receiver, string subject, string body)
        {
            try
            {
                var mailAddress = new MailAddress(receiver);
            }
            catch (Exception)
            {
                return false;
            }

            if (subject == null || subject.Trim().Length < 1 || body == null || body.Trim().Length < 1)
            {
                return false;
            }

            return true;
        }
    }
}