using System.Net;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using MovieSeeker.Application.Settings;

namespace MovieSeeker.Application.Services
{
    public class JwtService : IJwtService
    {
        // private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            // _appSettings = configuration.GetSection("MySettings").Get<AppSettings>();
        }

        public void AddJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                ConfigureJwtBearerOptions(options);
            });
        }

        private void ConfigureJwtBearerOptions(JwtBearerOptions options)
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("MySettings:Jwt:Secret").Value))
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = OnTokenChallenge,
                OnTokenValidated = OnTokenValidated
            };
        }

        private Task OnTokenChallenge(JwtBearerChallengeContext context)
        {
            context.HandleResponse();

            var response = new ResponseService<Object>();
            response.AddError("Token de autenticação inválido", HttpStatusCode.Unauthorized);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401;
            return context.Response.WriteAsJsonAsync(response);
        }

        private Task OnTokenValidated(TokenValidatedContext context)
        {
            if (context.Principal.Identity is ClaimsIdentity claimsIdentity)
            {
                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    context.HttpContext.Items["UserId"] = userIdClaim.Value;
                }
            }

            return Task.CompletedTask;
        }
    }

}