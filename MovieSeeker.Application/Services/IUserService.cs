using MovieSeeker.Application.Dtos;
using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserSignUpRequestDto request);
        Task<string> AuthenticateUserAsync(UserSignInRequestDto request);
        Task<bool> EditUserNameAsync(Guid userId, UserEditNameRequestDto userEditNameRequestDto);
        Task<User> GetUserByIdAsync(Guid userId);
    }
}