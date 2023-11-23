namespace MovieSeeker.Application.Configuration
{
    public class MailSettings
    {
        public SmtpSettings smtp { get; set; }
        public string From { get; set; }
    }
}