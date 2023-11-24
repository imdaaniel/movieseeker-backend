using System.Net;
using System.Net.Mail;

using MovieSeeker.Application.Configuration;

namespace MovieSeeker.Application.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public async Task<bool> SendMail(string receiver, string subject, string body)
        {
            if (!ValidateParameters(receiver, subject, body))
            {
                return false;
            }

            try
            {
                var client = new SmtpClient(_mailSettings.Smtp.Server, _mailSettings.Smtp.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_mailSettings.Smtp.Username, _mailSettings.Smtp.Password),
                    EnableSsl = true
                };

                var message = new MailMessage(_mailSettings.From, receiver, subject, body);
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