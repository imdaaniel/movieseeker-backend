using MovieSeeker.Application.Dtos.User;

namespace MovieSeeker.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<UserSignInRequestDto> AuthenticateUserAsync(UserSignInRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<UserSignUpRequestDto> RegisterUserAsync(UserSignUpRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}