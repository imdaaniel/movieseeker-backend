using MovieSeeker.Application.Dtos.Authentication;
using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<ResponseService<User>> CreateUserAsync(SignUpRequestDto request);
        Task<ResponseService<Object>> AuthenticateUserAsync(SignInRequestDto request);
    }    
}