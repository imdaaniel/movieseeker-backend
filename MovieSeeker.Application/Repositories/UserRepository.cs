using Microsoft.EntityFrameworkCore;

using MovieSeeker.Domain.Entities;
using MovieSeeker.Infra.Data.Context;

namespace MovieSeeker.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}