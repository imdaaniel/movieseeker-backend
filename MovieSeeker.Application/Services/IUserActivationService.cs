using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public interface IUserActivationService
    {
        Task<UserActivation> CreateAsync(User user);
        Task<bool> SendActivationEmailAsync(UserActivation userActivation);
    }
}