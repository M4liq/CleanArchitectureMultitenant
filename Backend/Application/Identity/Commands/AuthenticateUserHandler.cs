using System.Security.Claims;
using Application.Common;
using Application.Common.Core;
using Application.Identity.BoundedServices;
using Application.Identity.Builders;
using Application.Identity.Constants;
using Domain.Common.Base;
using Domain.Identity.Authentication;
using Domain.Identity.UserIdentity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Commands;

public static class AuthenticateUser
{
    public record AuthenticateUserCommand(string Email) : IRequest<AuthenticateUserResponse>;

    public class Validator : IValidationHandler<AuthenticateUserCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IDomainMessageManager _domainMessageManager;

        public Validator(IDataContext dataContext, IDomainMessageManager domainMessageManager)
        {
            _dataContext = dataContext;
            _domainMessageManager = domainMessageManager;
        }

        public async Task<ValidationResult> Validate(AuthenticateUserCommand request, CancellationToken ct)
        {
            var userExists = await _dataContext.Set<IdentityEntity>()
                .AnyAsync(user => user.Email == request.Email, cancellationToken: ct);

            if (userExists == false)
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<AuthenticationDomainErrors.UserDoesNotExistValidationMessage>(ct);
            }

            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResponse>
    {
        private readonly IAuthenticationTokenService _authenticationTokenService;
        private readonly IDataContext _dataContext;


        public Handler(IAuthenticationTokenService authenticationTokenService, IDataContext dataContext)
        {
            _authenticationTokenService = authenticationTokenService;
            _dataContext = dataContext;
        }

        public async Task<AuthenticateUserResponse> Handle(AuthenticateUserCommand request, CancellationToken ct)
        {
            var user = await _dataContext.Set<IdentityEntity>()
                .FirstAsync(user => user.Email == request.Email, cancellationToken: ct);
            var claims = ClaimsBuilder.CreateClaims(request.Email, user.Id, user.TenantId, IdentityType.UserIdentity);

            var authenticationToken =
                await _authenticationTokenService.GenerateTokenAsync(new ClaimsIdentity(claims), ct: ct);

            return new AuthenticateUserResponse
            {
                Token = authenticationToken.Token,
                RefreshToken = authenticationToken.Token
            };
        }
    }

    public record AuthenticateUserResponse : BaseResponse
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
    }
}