using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}