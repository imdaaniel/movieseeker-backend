namespace MovieSeeker.Application.Settings
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public Jwt Jwt { get; set; }
        public MailSettings Mail { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class Jwt
    {
        public string Secret { get; set; }
    }
}