using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Repositories
{
    public interface IUserActivationRepository
    {
        Task<UserActivation> CreateAsync(UserActivation userActivation);
    }
}