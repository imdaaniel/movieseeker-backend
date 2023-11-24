using MovieSeeker.Domain.Entities;
using MovieSeeker.Infra.Data.Context;

namespace MovieSeeker.Application.Repositories
{
    public class UserActivationRepository : IUserActivationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserActivationRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<UserActivation> CreateAsync(UserActivation userActivation)
        {
            _dbContext.UserActivations.Add(userActivation);
            await _dbContext.SaveChangesAsync();

            return userActivation;
        }
    }
}