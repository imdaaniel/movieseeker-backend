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
            // Fazer duas validações:
            // 1 - se a activation atual é válida ou não
            // INVÁLIDA NOS SEGUINTES CASOS:
            // 1.1 - a activation não existe
            // 1.2 - a activation está expired
            // 1.3 - a activation foi gerada há mais de 3 horas
            // 1.3.1 - marcar esta activation como expired

            // 2 - se devo gerar e enviar uma nova activation
            // NÃO GERAR NOS SEGUINTES CASOS:
            // 2.1 - o usuário já está ativo
            // 2.2 - o usuário possui alguma outra activation válida
            // 2.2.1 - Não expired
            // 2.2.2 - SendingMoment <= 3 horas atrás

            // 3 - ao gerar uma nova UserActivation, invalidar todas as anteriores daquele usuario

            var response = new ResponseService<object>();
            bool expiredActivation = false;
            List<string> errorMessages = new List<string>();

            // Localizar id no banco
            UserActivation? userActivation = await _userActivationRepository.FindById(activationId);

            if (userActivation == null)
            {
                response.AddError("Link de ativação inválido");
                return response;
            }

            if (userActivation.User.Active)
            {
                response.AddError("O usuário já está ativo");
                return response;
            }

            if (userActivation.Expired || userActivation.Activated)
            {
                expiredActivation = true;
            }
            else
            {
                // Verificar se está válido (SendingMoment). Validade 3h
                TimeSpan differenceBetweenNowAndSendingMoment = DateTime.UtcNow - userActivation.SendingMoment;

                if (differenceBetweenNowAndSendingMoment.TotalHours > 3)
                {
                    userActivation.Expired = true;
                    _userActivationRepository.UpdateAsync(userActivation);

                    expiredActivation = true;
                }
            }

            if (expiredActivation)
            {
                errorMessages.Add("Link de ativação expirado");
                
                UserActivation? validUserActivation = await _userActivationRepository.FindValidUserActivation(userActivation.UserId);

                if (validUserActivation == null)
                {
                    await _userActivationRepository.InvalidateAllUserActivations(userActivation.UserId);
                    validUserActivation = await CreateAsync(userActivation.User);
                }

                await SendActivationEmailAsync(validUserActivation);
                errorMessages.Add("Foi enviado um novo e-mail para o usuário contendo um link de ativação");
            }

            if (errorMessages.Count > 0)
            {
                response.AddError(errorMessages);
                return response;
            }

            // Marcar activation como Activated, e marcar usuario como ativo
            userActivation.Activated = true;
            userActivation.ActivationMoment = DateTime.UtcNow;
            await _userActivationRepository.UpdateAsync(userActivation);

            userActivation.User.Active = true;
            await _userRepository.UpdateUserAsync(userActivation.User);

            response.Messages.Add("Usuário ativado com sucesso");

            return response;
        }

        public async Task<UserActivation> CreateAsync(User user)
        {
            UserActivation userActivation = GenerateNewUserActivation(user);

            return await _userActivationRepository.CreateAsync(userActivation);
        }

        private UserActivation GenerateNewUserActivation(User user)
        {
            return new()
            {
                UserId = user.Id,
                SendingMoment = DateTime.UtcNow,
                Expired = false,
                User = user
            };
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
                <br>
                <p>Clique no link abaixo para confirmar seu cadastro.</p>
                <p>{activationLink}</p>
            </body>
            </html>";

            return await _mailService.SendMail(userActivation.User.Email, "Confirme seu cadastro no MovieSeeker", content);
        }

        private string GetActivationUrl(UserActivation userActivation)
        {
            return $"{_appSettings.BaseUrl}/Authentication/Activate/{userActivation.Id}";
        }
    }
}