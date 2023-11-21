using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> EditUserNameAsync(Guid userId, UserEditNameRequestDto userEditNameRequestDto)
        {
            User? user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) {
                throw new InvalidOperationException("Usuario não encontrado");
            }

            user.FirstName = userEditNameRequestDto.FirstName;
            user.LastName = userEditNameRequestDto.LastName;

            int affectedRows = await _userRepository.UpdateUserAsync(user);

            if (affectedRows < 1) {
                throw new DbUpdateException("Não foi possível editar o nome do usuário");
            }

            return true;
        }
    }
}