using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
    }
}