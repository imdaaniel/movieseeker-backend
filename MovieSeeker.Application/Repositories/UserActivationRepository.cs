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
            UserActivation? userActivation = await _dbContext.UserActivations.FirstOrDefaultAsync(u => u.Id == activationId);

            if (userActivation != null) {
                userActivation.User = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userActivation.UserId);
                return userActivation;
            }

            return null;
        }

        public async Task<UserActivation?> FindValidUserActivation(Guid userId)
        {
            return await _dbContext.UserActivations.FirstOrDefaultAsync(u => 
                !u.Expired
                && !u.Activated
                && (DateTime.UtcNow - u.SendingMoment).TotalHours <= 3
            );
        }

        public async Task<bool> InvalidateAllUserActivations(Guid userId)
        {
            try
            {
                List<UserActivation> previousUserActivations = await _dbContext.UserActivations.Where(u => u.UserId == userId).ToListAsync();

                previousUserActivations.ForEach(u => u.Expired = true);

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UserActivation> UpdateAsync(UserActivation userActivation)
        {
            _dbContext.UserActivations.Update(userActivation);
            await _dbContext.SaveChangesAsync();

            return userActivation;
        }
    }
}