namespace MovieSeeker.Application.Configuration
{
    public class MailSettings
    {
        public SmtpSettings Smtp { get; set; }
        public string From { get; set; }
    }
}