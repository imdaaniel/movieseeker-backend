using Microsoft.EntityFrameworkCore;

using MovieSeeker.Application.Services;
using MovieSeeker.Domain.Entities;
using MovieSeeker.Infra.Data.Context;

namespace MovieSeeker.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordHashService _passwordHashService;

        public UserRepository(ApplicationDbContext context, IPasswordHashService passwordHashService)
        {
            _dbContext = context;
            _passwordHashService = passwordHashService;
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || _passwordHashService.VerifyPassword(password, user.Password) == false) {
                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}