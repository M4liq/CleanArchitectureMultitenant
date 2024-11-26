using System.Web;
using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Services;
using Domain.Common;
using Domain.Identity;
using MediatR;

namespace Application.Identity.Commands;

public static class ChangePassword
{
    public record ChangePasswordCommand(Guid id, string newPassword, string token) : IRequest<ChangePasswordResponse>;

    public class Validator : IDomainValidationHandler<ChangePasswordCommand>
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IErrorManager _errorManager;

        public Validator(IApplicationUserManager userManager, IErrorManager errorManager)
        {
            _userManager = userManager;
            _errorManager = errorManager;
        }

        public async Task<ValidationResult> Validate(ChangePasswordCommand request)
        {
            var user = await _userManager.FindByIdAsync(request.id);

            if (user is null)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationUserDomainErrors.IncorrectUserIdError>();
            }

            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponse>
    {
        private readonly IMediator _mediator;
        private readonly IApplicationUserManager _userManager;

        public Handler(IMediator mediator, IApplicationUserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<ChangePasswordResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.id);

            var token = HttpUtility.UrlDecode(request.token);
            var identityResult = await _userManager.ResetPasswordAsync(user, token, request.newPassword);

            if (!identityResult.Success)
            {
                var response = new ChangePasswordResponse();
                response.Fail(identityResult.Errors);
                return response;
            }
            
            var result = await _mediator.Send(new Authenticate.AuthenticateCommand(user), cancellationToken);
                
            return new ChangePasswordResponse
            {
               Token = result.Token,
               RefreshToken = result.RefreshToken
            };
        }
    }

    public record ChangePasswordResponse : BaseResponse
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
    }
}