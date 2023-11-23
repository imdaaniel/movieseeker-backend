using MovieSeeker.Application.Dtos.Authentication;
using MovieSeeker.Application.Repositories;
using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHashService _passwordHashService;

        public AuthenticationService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHashService = passwordHashService;
        }

        public async Task<ResponseService<User>> CreateUserAsync(SignUpRequestDto signUpRequestDto)
        {
            var response = new ResponseService<User>();

            if (await _userRepository.EmailExistsAsync(signUpRequestDto.Email))
            {
                response.AddError("Email já cadastrado");
                return response;
            }

            string hashedPassword = _passwordHashService.HashPassword(signUpRequestDto.Password);

            User user = new()
            {
                FirstName = signUpRequestDto.FirstName,
                LastName = signUpRequestDto.LastName,
                Email = signUpRequestDto.Email,
                Password = hashedPassword,
            };

            response.Data = await _userRepository.CreateUserAsync(user);

            return response;
        }

        public async Task<ResponseService<Object>> AuthenticateUserAsync(SignInRequestDto signInRequestDto)
        {
            var response = new ResponseService<Object>();

            User? user = await _userRepository.GetUserByEmailAsync(signInRequestDto.Email);

            if (user == null || _passwordHashService.VerifyPassword(signInRequestDto.Password, user.Password) == false)
            {
                response.AddError("Usuário não encontrado");
                return response;
            }
            else if (!user.Active)
            {
                response.AddError("Usuário não verificou o e-mail");
                return response;
            }

            response.Data = _tokenService.GenerateToken(user);
            return response;
        }
    }
}