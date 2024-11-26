using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Services;
using Domain.Common;
using Domain.Identity;
using MediatR;

namespace Application.Identity.Commands;

public static class RegisterAnyUser
{
    public record RegisterAnyUserCommand(string email, string password) : IRequest<RegisterAnyUserResponse>;
        
    public class Validator : IDomainValidationHandler<RegisterAnyUserCommand>
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IErrorManager _errorManager;

        public Validator(IApplicationUserManager userManager, IErrorManager errorManager)
        {
            _userManager = userManager;
            _errorManager = errorManager;
        }

        public async Task<ValidationResult> Validate(RegisterAnyUserCommand request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.email);

            if (existingUser != null)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationUserDomainErrors.UserWithGivenEmailCurrentlyExists>();
            }

            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<RegisterAnyUserCommand, RegisterAnyUserResponse>
    {
        private readonly IApplicationUserManager _userManager;

        public Handler(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterAnyUserResponse> Handle(RegisterAnyUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUserEntity
            {
                Id = Guid.NewGuid(),
                Email = request.email,
                UserName = request.email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var userCreationResult = await _userManager.CreateAsync(user, request.password);
            
            if (!userCreationResult.Success)
            {
                var response = new RegisterAnyUserResponse();
                response.Fail(userCreationResult.Errors);
                return response;
            }

            return new RegisterAnyUserResponse();
        }
    }

    public record RegisterAnyUserResponse : BaseResponse;
}