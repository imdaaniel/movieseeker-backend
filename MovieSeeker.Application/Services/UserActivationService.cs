using Microsoft.Extensions.Options;

using MovieSeeker.Application.Repositories;
using MovieSeeker.Application.Services.Mail;
using MovieSeeker.Application.Settings;
using MovieSeeker.Domain.Entities;

namespace MovieSeeker.Application.Services
{
    public class UserActivationService : IUserActivationService
    {
        private readonly IUserActivationRepository _userActivationRepository;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        private readonly IMailService _mailService;

        public UserActivationService(
            IUserActivationRepository userActivationRepository,
            IUserService userService,
            IOptions<AppSettings> appSettings,
            IMailService mailService)
        {
            _userActivationRepository = userActivationRepository;
            _userService = userService;
            _appSettings = appSettings.Value;
            _mailService = mailService;
        }

        public async Task<UserActivation> CreateAsync(User user)
        {
            UserActivation userActivation = new()
            {
                UserId = user.Id,
                SendingMoment = DateTime.UtcNow,
                Expired = false,
                User = user
            };

            return await _userActivationRepository.CreateAsync(userActivation);
        }

        public async Task<bool> SendActivationEmailAsync(UserActivation userActivation)
        {
            string userFullname = _userService.GetUserFullName(userActivation.User);
            string activationLink = GetActivationUrl(userActivation);

            var content = 
            @$"<html>
            <head></head>
            <body>
                <p>Olá, {userFullname}.</p>
                <br>Clique no link abaixo para ativar sua conta.</p>
                <p>{activationLink}</p>
            </body>
            </html>";

            return await _mailService.SendMail(userActivation.User.Email, "Confirme seu cadastro", content);
        }

        private string GetActivationUrl(UserActivation userActivation)
        {
            return $"{_appSettings.BaseUrl}/Authentication/Activate/{userActivation.Id}";
        }
    }
}