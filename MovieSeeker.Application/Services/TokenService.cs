using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using MovieSeeker.Application.Repositories;
using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public TokenService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            
            try
            {
                return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            }
            catch (SecurityTokenException ex)
            {
                // Trate a exceção, se necessário.
                throw new SecurityTokenException("Token inválido.");
            }

        }

        public async Task<Guid> GetUserIdFromToken(string token)
        {
            var principal = GetClaimsPrincipalFromToken(token);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                var user = await _userRepository.GetUserByIdAsync(userId);

                if (user != null) {
                    return userId;
                }
            }

            throw new Exception("ID do usuário inválido");
        }

        // public bool ValidateToken(string token, out ClaimsPrincipal principal)
        // {
        //     principal = null;

        //     try
        //     {
        //         if (!token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        //         {
        //             return false;
        //         }
        //         else
        //         {
        //             token = token.Substring("Bearer ".Length).Trim();
        //         }

        //         var tokenHandler = new JwtSecurityTokenHandler();
        //         var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

        //         TokenValidationParameters tokenValidationParameters = new()
        //         {
        //             ValidateIssuer = false,
        //             ValidateAudience = false,
        //             ValidateIssuerSigningKey = true,
        //             IssuerSigningKey = new SymmetricSecurityKey(key),
        //             ClockSkew = TimeSpan.Zero,
        //         };

        //         principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

        //         return true;
        //     }
        //     catch (SecurityTokenException ex)
        //     {
        //         return false;
        //     }
        // }
    }
}