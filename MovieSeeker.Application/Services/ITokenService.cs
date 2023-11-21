using Microsoft.Extensions.Configuration;

using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}