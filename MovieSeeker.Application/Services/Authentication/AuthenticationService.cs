using System.Net;

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
        private readonly IUserActivationService _userActivationService;

        public AuthenticationService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IPasswordHashService passwordHashService,
            IUserActivationService userActivationService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHashService = passwordHashService;
            _userActivationService = userActivationService;
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

            User createdUser = await _userRepository.CreateUserAsync(user);

            UserActivation userActivation = await _userActivationService.CreateAsync(createdUser);

            bool activationEmailSent = await _userActivationService.SendActivationEmailAsync(userActivation);

            if (activationEmailSent == false)
            {
                response.AddError("O usuário foi cadastrado, mas ocorreu um erro ao enviar o email de ativação de conta");
                return response;
            }

            response.Data = createdUser;
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

            try
            {
                response.Data = new
                {
                    token = _tokenService.GenerateToken(user)
                };
            }
            catch (Exception)
            {
                response.AddError("Ocorreu um erro ao gerar o token", HttpStatusCode.InternalServerError);
            }

            return response;
        }
    }
}