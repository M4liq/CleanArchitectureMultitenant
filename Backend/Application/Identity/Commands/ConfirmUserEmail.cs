using System.Web;
using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Services;
using Domain.Common;
using Domain.Identity;
using MediatR;

namespace Application.Identity.Commands;

public static class ConfirmUserEmail
{
    public record ConfirmUserEmailCommand(Guid id, string token) : IRequest<ConfirmUserEmailResponse>;
        
    public class Validator : IDomainValidationHandler<ConfirmUserEmailCommand>
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IErrorManager _errorManager;

        public Validator(IApplicationUserManager userManager, IErrorManager errorManager)
        {
            _userManager = userManager;
            _errorManager = errorManager;
        }

        public async Task<ValidationResult> Validate(ConfirmUserEmailCommand request)
        {
            var existingUser = await _userManager.FindByIdAsync(request.id);

            if (existingUser is null)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationUserDomainErrors.IncorrectUserIdError>();
            }

            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<ConfirmUserEmailCommand, ConfirmUserEmailResponse>
    {
        private readonly IApplicationUserManager _userManager;

        public Handler(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<ConfirmUserEmailResponse> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
        {
            var token = HttpUtility.UrlDecode(request.token);
            var user = await _userManager.FindByIdAsync(request.id);
            
            var userCreationResult = await _userManager.ConfirmEmailAsync(user,token);
            
            if (!userCreationResult.Succeeded)
            {
                var response = new ConfirmUserEmailResponse();
                response.Fail(userCreationResult.Errors.Select(e => e.Description).ToList());
                return response;
            }

            return new ConfirmUserEmailResponse();
        }
    }

    public record ConfirmUserEmailResponse : BaseResponse;
}