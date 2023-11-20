using BCryptNet = BCrypt.Net.BCrypt;

using MovieSeeker.Application.Dtos;
using MovieSeeker.Application.Repositories;
using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(UserSignUpRequestDto signUpRequestDto)
        {
            string hashedPasword = BCryptNet.HashPassword(signUpRequestDto.Password);

            User user = new()
            {
                FirstName = signUpRequestDto.FirstName,
                LastName = signUpRequestDto.LastName,
                Email = signUpRequestDto.Email,
                Password = hashedPasword,
            };

            return await _userRepository.CreateUserAsync(user);
        }

        public Task<UserSignInRequestDto> AuthenticateUserAsync(UserSignInRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}