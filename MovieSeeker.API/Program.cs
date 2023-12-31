using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using MovieSeeker.API.Filters;
using MovieSeeker.API.Middleware;
using MovieSeeker.Application.Repositories;
using MovieSeeker.Application.Services;
using MovieSeeker.Application.Services.Authentication;
using MovieSeeker.Application.Services.Mail;
using MovieSeeker.Application.Settings;
using MovieSeeker.Infra.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(options => {
    builder.Configuration.GetSection("MySettings").Bind(options);
});

// Autenticação
var jwtService = new JwtService(builder.Configuration);
jwtService.AddJwtAuthentication(builder.Services);

// Conexão com banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetSection("MySettings:ConnectionStrings:DefaultConnection").Value)
);

// Repositórios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserActivationRepository, UserActivationRepository>();

// Serviços
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<IResponseService, ResponseService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserActivationService, UserActivationService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers(options => {
    options.Filters.Add<CustomResponseFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "MovieSeeker API",
        Version = "v1"
    });

    // Configuração do esquema de segurança JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticação JWT usando o schema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Autenticação (permitir/negar acesso à rotas)
app.UseAuthentication();

// Autorização (permitir/negar ações específicas do usuário)
app.UseAuthorization();

// Middlewares
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
