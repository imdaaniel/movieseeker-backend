namespace MovieSeeker.Application.Settings
{
    public class Smtp
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
    }

    public class Sender
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class Api
    {
        public string Url { get; set; }
        public string Key { get; set; }
    }

    public class MailSettings
    {
        public Smtp Smtp { get; set; }
        public Sender Sender { get; set; }
        public Api Api { get; set; }
    }
}