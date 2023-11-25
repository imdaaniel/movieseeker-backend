using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MovieSeeker.API.Models.Authentication;
using MovieSeeker.Application.Dtos.Authentication;
using MovieSeeker.Application.Services;
using MovieSeeker.Application.Services.Authentication;

namespace MovieSeeker.API.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class AuthenticationController : MovieSeekerController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserActivationService _userActivationService;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            IUserActivationService userActivationService)
        {
            _authenticationService = authenticationService;
            _userActivationService = userActivationService;
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

        [HttpPost("Activation/{activationId}")]
        public async Task<IActionResult> ActivateUser(Guid activationId)
        {
            var response = await _userActivationService.ActivateUser(activationId);

            return Response(response);
        }
    }
}