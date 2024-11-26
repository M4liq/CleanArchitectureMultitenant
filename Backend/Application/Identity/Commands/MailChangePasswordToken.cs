using System.Web;
using Application.Common.Dtos;
using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Services;
using Domain.Common;
using Domain.Identity;
using MediatR;

namespace Application.Identity.Commands;

public static class MailChangePasswordToken
{
    public record MailChangePasswordTokenCommand(string email) : IRequest<MailChangePasswordTokenResponse>;

    public class Validator : IDomainValidationHandler<MailChangePasswordTokenCommand>
    {
        public async Task<ValidationResult> Validate(MailChangePasswordTokenCommand request)
        {
            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<MailChangePasswordTokenCommand, MailChangePasswordTokenResponse>
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

        public async Task<MailChangePasswordTokenResponse> Handle(MailChangePasswordTokenCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.email);

            if (user is null)
            {
                return await _messageManager
                    .GetMessagesForAsync<ApplicationUserDomainMessages.MailChangePasswordTokenDomainMessage,
                        MailChangePasswordTokenResponse>();
            }

            var token = HttpUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));

            var emailDto = new EmailMessageDto
            {
                From = "no-reply@papahora.com",
                ToEmail = request.email,
                Subject = "Password reset",
                Content = "Token: " + token + "\n" + "UserId: " + user.Id
            };
            
            await _emailClient.SendEmailAsync(emailDto);
            
            return await _messageManager
                .GetMessagesForAsync<ApplicationUserDomainMessages.MailChangePasswordTokenDomainMessage,
                    MailChangePasswordTokenResponse>();
        }
    }

    public record MailChangePasswordTokenResponse : BaseResponse;
}