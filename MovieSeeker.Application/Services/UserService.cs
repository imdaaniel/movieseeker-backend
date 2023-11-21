using MovieSeeker.Application.Dtos;
using MovieSeeker.Application.Repositories;
using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHashService _passwordHashService;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHashService = passwordHashService;
        }

        public async Task<User> CreateUserAsync(UserSignUpRequestDto signUpRequestDto)
        {
            if (await _userRepository.EmailExistsAsync(signUpRequestDto.Email)) {
                throw new InvalidOperationException("Email já cadastrado");
            }

            string hashedPassword = _passwordHashService.HashPassword(signUpRequestDto.Password);

            User user = new()
            {
                FirstName = signUpRequestDto.FirstName,
                LastName = signUpRequestDto.LastName,
                Email = signUpRequestDto.Email,
                Password = hashedPassword,
            };

            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<string> AuthenticateUserAsync(UserSignInRequestDto signInRequestDto)
        {
            var user = await _userRepository.AuthenticateUserAsync(signInRequestDto.Email, signInRequestDto.Password);

            return _tokenService.GenerateToken(user);
        }
    }
}