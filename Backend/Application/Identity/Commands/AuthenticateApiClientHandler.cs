using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common;
using Application.Common.Core;
using Application.Identity.BoundedServices;
using Application.Identity.Constants;
using Domain.Common.Base;
using Domain.Identity;
using Domain.Identity.ApiClient;
using Domain.Identity.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AuthenticateApiClient
{
    public record AuthenticateApiClientCommand(string ApiKey) : IRequest<AuthenticateApiClientResponse>;

    public class Validator : IDomainValidationHandler<AuthenticateApiClientCommand>
    {
        private readonly IDataContext _dataContext;
        private readonly IDomainMessageManager _domainMessageManager;

        public Validator(IDataContext dataContext, IDomainMessageManager domainMessageManager)
        {
            _dataContext = dataContext;
            _domainMessageManager = domainMessageManager;
        }

        public async Task<ValidationResult> Validate(AuthenticateApiClientCommand request, CancellationToken ct)
        {
            using (_dataContext.UseSystemContext())
            {
                var apiClientExists = await _dataContext.Set<ApiClientEntity>()
                    .AnyAsync(x => x.ApiKey == request.ApiKey && !x.IsDeleted, ct);

                if (apiClientExists == false)
                {
                    return await _domainMessageManager
                        .GetValidationResultAsync<
                            AuthenticationDomainErrors.ApiClientDoesNotExistValidationMessage>(ct);
                }

                return new ValidationResult();
            }
        }
    }

    public class Handler : IRequestHandler<AuthenticateApiClientCommand, AuthenticateApiClientResponse>
    {
        private readonly IDataContext _dataContext;
        private readonly IAuthenticationTokenService _authenticationTokenService;

        public Handler(IDataContext dataContext, IAuthenticationTokenService authenticationTokenService)
        {
            _dataContext = dataContext;
            _authenticationTokenService = authenticationTokenService;
        }

        public async Task<AuthenticateApiClientResponse> Handle(AuthenticateApiClientCommand request,
            CancellationToken ct)
        {
            using (_dataContext.UseSystemContext())
            {
                var apiClient = await _dataContext.Set<ApiClientEntity>()
                    .FirstAsync(x => x.ApiKey == request.ApiKey && !x.IsDeleted, ct);

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, $"api-client-{apiClient.Id}"),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(CustomClaimTypes.IdentityId, apiClient.Id.ToString()),
                    new(CustomClaimTypes.TenantId, apiClient.TenantId.ToString()),
                    new(CustomClaimTypes.IdentityType, IdentityType.ApiClientIdentity.ToString())
                };

                claims.AddRange(apiClient.AllowedScopes.Select(s => new Claim(CustomClaimTypes.Scope, s)));

                var authenticationTokenResult =
                    await _authenticationTokenService.GenerateTokenAsync(new ClaimsIdentity(claims), ct);

                return new AuthenticateApiClientResponse
                {
                    Token = authenticationTokenResult.Token,
                    RefreshToken = authenticationTokenResult.RefreshToken
                };
            }
        }
    }

    public record AuthenticateApiClientResponse : BaseResponse
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
    }
}