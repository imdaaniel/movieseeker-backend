using Microsoft.AspNetCore.Mvc;

using MovieSeeker.API.Models.User;
using MovieSeeker.Application.Dtos.User;
using MovieSeeker.Application.Services.User;

namespace MovieSeeker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpRequestModel model)
        {
            UserSignUpRequestDto userSignUpRequestDto = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password
            };

            var result = await _userService.RegisterUserAsync(userSignUpRequestDto);

            if (result == null)
            {
                return BadRequest("Ocorreu um erro ao cadastrar o usuário");
            }

            return CreatedAtAction(nameof(SignUp), result);
        }
    }
}