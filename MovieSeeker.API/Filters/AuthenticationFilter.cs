using System.Security.Claims;

using Microsoft.AspNetCore.Mvc.Filters;

using MovieSeeker.Application.Services;

namespace MovieSeeker.API.Filters
{
    public class AuthenticationFilter : IAsyncActionFilter
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthenticationFilter(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Verifica se o token JWT foi enviado no header da requisição
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                throw new UnauthorizedAccessException("Token de autenticação não enviado");
            }

            // Valida o token e extrai disponibiliza as informações do usuário contidas
            var validationSuccessful = _tokenService.ValidateToken(token, out var principal);

            if (!validationSuccessful || principal == null)
            {
                throw new UnauthorizedAccessException("Token de autenticação inválido");
            }

            // Extrai o ID do usuário do token
            var userIdClaim = principal.FindFirst(ClaimTypes.Name);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Token de autenticação inválido");
            }

            // Valida o usuário existe e se está ativo (ativou a conta)
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Usuário não encontrado");
            }
            else if (!user.Active)
            {
                throw new UnauthorizedAccessException("Usuário não ativou a conta");
            }

            // Adiciona o ID do usuário ao contexto para que fique acessível em outros locais
            context.HttpContext.Items["UserId"] = userId;

            await next(); // Continua a requisição
        }
    }
}