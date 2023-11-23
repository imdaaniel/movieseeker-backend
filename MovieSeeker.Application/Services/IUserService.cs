using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public interface IUserService
    {
        // Task<ResponseService<User>> EditUserNameAsync(UserEditNameRequestDto userEditNameRequestDto);
        Task<ResponseService<User>> GetUserByIdAsync(Guid userId);
    }    
}