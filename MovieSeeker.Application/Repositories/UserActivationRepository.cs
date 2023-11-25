using Microsoft.EntityFrameworkCore;

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

        public async Task<UserActivation?> FindById(Guid activationId)
        {
            return await _dbContext.UserActivations.FirstOrDefaultAsync(u => u.Id == activationId);
        }

        public async Task<UserActivation> UpdateAsync(UserActivation userActivation)
        {
            _dbContext.UserActivations.Update(userActivation);
            await _dbContext.SaveChangesAsync();

            return userActivation;
        }
    }
}