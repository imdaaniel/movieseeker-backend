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
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        private readonly IMailService _mailService;

        public UserActivationService(
            IUserActivationRepository userActivationRepository,
            IUserRepository userRepository,
            IUserService userService,
            IOptions<AppSettings> appSettings,
            IMailService mailService)
        {
            _userActivationRepository = userActivationRepository;
            _userRepository = userRepository;
            _userService = userService;
            _appSettings = appSettings.Value;
            _mailService = mailService;
        }

        public async Task<ResponseService<object>> ActivateUser(Guid activationId)
        {
            var response = new ResponseService<object>();

            // Localizar id no banco
            UserActivation? userActivation = await _userActivationRepository.FindById(activationId);
            
            if (userActivation == null)
            {
                response.AddError("Link de ativação inválido");
                return response;
            }

            // Verificar se está expired
            if (userActivation.Expired) {
                response.AddError("Link de ativação expirado");
                return response;
            }

            userActivation.Expired = true;
            _userActivationRepository.UpdateAsync(userActivation);

            // Verificar se está válido (SendingMoment) validade 3h
            TimeSpan differenceBetweenNowAndSendingMoment = DateTime.UtcNow - userActivation.SendingMoment;

            if (differenceBetweenNowAndSendingMoment.TotalHours > 3)
            {
                response.AddError("Link de ativação expirado");
                return response;
            }
            
            // Marcar como expired e marcar usuario como ativo
            userActivation.User.Active = true;
            await _userRepository.UpdateUserAsync(userActivation.User);

            response.Messages.Add("Usuário ativado com sucesso");
            
            return response;
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