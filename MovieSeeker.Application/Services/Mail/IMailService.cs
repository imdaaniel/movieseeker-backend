namespace MovieSeeker.Application.Services.Mail
{
    public interface IMailService
    {
        Task<bool> SendMail(string receiver, string subject, string body);
    }
}