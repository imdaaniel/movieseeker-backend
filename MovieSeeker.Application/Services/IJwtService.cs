using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace MovieSeeker.Application.Services
{
    public interface IJwtService
    {
        void AddJwtAuthentication(IServiceCollection services);
    }
}