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

        public async Task<User> AddAsync(User user)
        {
            
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}