using System.Web;
using Application.Common.Dtos;
using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Services;
using Domain.Common;
using Domain.Identity;
using MediatR;

namespace Application.Identity.Commands;

public static class MailEmailConfirmation
{
    public record MailEmailConfirmationCommand(string email) : IRequest<MailEmailConfirmationResponse>;

    public class Validator : IDomainValidationHandler<MailEmailConfirmationCommand>
    {
        public async Task<ValidationResult> Validate(MailEmailConfirmationCommand request)
        {
            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<MailEmailConfirmationCommand, MailEmailConfirmationResponse>
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IMessageManager _messageManager;
        private readonly IEmailClient _emailClient;

        public Handler(IApplicationUserManager userManager, IMessageManager messageManager, IEmailClient emailClient)
        {
            _userManager = userManager;
            _messageManager = messageManager;
            _emailClient = emailClient;
        }

        public async Task<MailEmailConfirmationResponse> Handle(MailEmailConfirmationCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.email);

            if (user is null)
            {
                return await _messageManager
                    .GetMessagesForAsync<ApplicationUserDomainMessages.MailEmailConfirmationDomainMessage,
                        MailEmailConfirmationResponse>();
            }

            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));

            var emailDto = new EmailMessageDto
            {
                From = "no-reply@papahora.com",
                ToEmail = request.email,
                Subject = "Email confirmation.",
                Content = "Token: " + token + "\n" + "UserId: " + user.Id
            };
            
            await _emailClient.SendEmailAsync(emailDto);
            
            return await _messageManager
                .GetMessagesForAsync<ApplicationUserDomainMessages.MailEmailConfirmationDomainMessage,
                    MailEmailConfirmationResponse>();
        }
    }

    public record MailEmailConfirmationResponse : BaseResponse;
}