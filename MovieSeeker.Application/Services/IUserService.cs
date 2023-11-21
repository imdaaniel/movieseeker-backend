using MovieSeeker.Application.Dtos;
using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserSignUpRequestDto request);
        Task<string> AuthenticateUserAsync(UserSignInRequestDto request);
    }
}