using System.Security.Claims;

using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        ClaimsPrincipal GetClaimsPrincipalFromToken(string token);
        Task<Guid> GetUserIdFromToken(string token);
        // bool ValidateToken(string token, out ClaimsPrincipal principal);
    }
}