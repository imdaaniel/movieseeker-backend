using System.Net;

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

        // public async Task<ResponseService<User>> EditUserNameAsync(Guid userId, UserEditNameRequestDto userEditNameRequestDto)
        // {
        //     var response = new ResponseService<User>();

        //     User? user = await _userRepository.GetUserByIdAsync(userId);

        //     if (user == null)
        //     {
        //         response.AddError("Usuário não encontrado");
        //         return default;
        //     }

        //     user.FirstName = userEditNameRequestDto.FirstName;
        //     user.LastName = userEditNameRequestDto.LastName;

        //     int affectedRows = await _userRepository.UpdateUserAsync(user);

        //     if (affectedRows < 1)
        //     {
        //         response.AddError("Ocorreu um erro ao editar o nome do usuário", HttpStatusCode.InternalServerError);
        //         return default;
        //     }

        //     response.Data = user;
        //     return response;
        // }

        public async Task<ResponseService<User>> GetUserByIdAsync(Guid userId)
        {
            var response = new ResponseService<User>();

            User? user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) {
                response.AddError("Usuário não encontrado");
                return response;
            }

            response.Data = user;
            return response;
        }
    }
}