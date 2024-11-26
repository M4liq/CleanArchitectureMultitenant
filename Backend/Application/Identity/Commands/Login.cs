using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Services;
using Domain.Common;
using Domain.Identity;
using MediatR;

namespace Application.Identity.Commands;

public static class Login
{
    public record ApplicationLoginCommand(string email, string password) : IRequest<ApplicationLoginResponse>;

    public class LoginValidator : IDomainValidationHandler<ApplicationLoginCommand>
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IErrorManager _errorManager;

        public LoginValidator(IApplicationUserManager userManager, IErrorManager errorManager)
        {
            _userManager = userManager;
            _errorManager = errorManager;
        }

        public async Task<ValidationResult> Validate(ApplicationLoginCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.email);

            if (user is null)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationUserDomainErrors.IncorrectEmailLoginError>();
            }

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!isEmailConfirmed)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationUserDomainErrors.UnconfirmedEmailError>();
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.password);

            if (!isPasswordValid)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationUserDomainErrors.IncorrectUserPasswordError>();
            }

            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<ApplicationLoginCommand, ApplicationLoginResponse>
    {
        private readonly IMediator _mediator;
        private readonly IApplicationUserManager _userManager;

        public Handler(IMediator mediator, IApplicationUserManager userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<ApplicationLoginResponse> Handle(ApplicationLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.email);
            var result = await _mediator.Send(new Authenticate.AuthenticateCommand(user), cancellationToken);
                
            return new ApplicationLoginResponse
            {
               Token = result.Token,
               RefreshToken = result.RefreshToken
            };
        }
    }

    public record ApplicationLoginResponse : BaseResponse
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
    }
}