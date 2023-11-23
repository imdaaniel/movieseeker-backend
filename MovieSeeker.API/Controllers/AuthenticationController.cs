using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MovieSeeker.API.Models.Authentication;
using MovieSeeker.Application.Dtos.Authentication;
using MovieSeeker.Application.Services.Authentication;

namespace MovieSeeker.API.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthenticationController : MovieSeekerController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestModel model)
        {
            SignUpRequestDto userSignUpRequestDto = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password
            };

            var response = await _authenticationService.CreateUserAsync(userSignUpRequestDto);

            return Response(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestModel model)
        {
            SignInRequestDto userSignInRequestDto = new()
            {
                Email = model.Email,
                Password = model.Password
            };

            var response = await _authenticationService.AuthenticateUserAsync(userSignInRequestDto);

            return Response(response);
        }
    }
}