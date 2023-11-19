using MovieSeeker.Application.Dtos.User;

namespace MovieSeeker.Application.Services.User
{
    public interface IUserService
    {
        Task<UserSignUpRequestDto> RegisterUserAsync(UserSignUpRequestDto request);
        Task<UserSignInRequestDto> AuthenticateUserAsync(UserSignInRequestDto request);
    }
}