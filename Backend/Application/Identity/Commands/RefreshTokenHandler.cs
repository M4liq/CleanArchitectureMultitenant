using System.IdentityModel.Tokens.Jwt;
using Application.Common;
using Application.Common.Core;
using Application.Identity.Constants;
using Domain.Common.Base;
using Domain.Identity.ApiClient;
using Domain.Identity.RefreshToken;
using Domain.Identity.UserIdentity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Commands;

public static class RefreshTokenHandler
{
    public record RefreshTokenCommand(string RequestToken, string RequestRefreshToken) : IRequest<RefreshTokenResponse>;

    public class LoginValidator : IValidationHandler<RefreshTokenCommand>
    {
        private readonly IDataContext _context;
        private readonly IDomainMessageManager _domainMessageManager;
        private readonly ICalendar _calendar;

        public LoginValidator(IDomainMessageManager domainMessageManager, IDataContext context, ICalendar calendar)
        {
            _domainMessageManager = domainMessageManager;
            _context = context;
            _calendar = calendar;
        }

        public async Task<ValidationResult> Validate(RefreshTokenCommand request, CancellationToken ct)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenJson = handler.ReadJwtToken(request.RequestToken);

            var jwtId = tokenJson.Payload.Jti;

            if (jwtId == null)
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<RefreshTokenDomainErrors.JwtTokenIsNoValid>(ct);
            }

            var storedRefreshToken =
                await _context.Set<RefreshTokenEntity>().SingleOrDefaultAsync(refreshToken => refreshToken.Token.ToString() == request.RequestRefreshToken, cancellationToken: ct);

            if (storedRefreshToken == null)
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<RefreshTokenDomainErrors.RefreshTokenDoesNotExits>(ct);
            }

            if (_calendar.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<RefreshTokenDomainErrors.RefreshTokenHasExpired>(ct);
            }

            if (storedRefreshToken.Invalidated)
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<
                        RefreshTokenDomainErrors.RefreshTokenHasBeenInvalidated>(ct);
            }

            if (storedRefreshToken.Used)
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<RefreshTokenDomainErrors.RefreshTokenHasBeenUsed>(ct);
            }

            if (storedRefreshToken.JwtId != jwtId)
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<
                        RefreshTokenDomainErrors.RefreshTokenDoesNotMatchJwt>(ct);
            }

            if (!Guid.TryParse(tokenJson.Claims.First(x => x.Type == CustomClaimTypes.IdentityId).Value,
                    out var identityId))
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<
                        RefreshTokenDomainErrors.CouldNotGetIdentityId>(ct);
            }
            
            if (!Enum.TryParse<IdentityType>(tokenJson.Claims.First(x => x.Type == CustomClaimTypes.IdentityId).Value,
                    out var identityType))
            {
                return await _domainMessageManager
                    .GetValidationResultAsync<
                        RefreshTokenDomainErrors.CouldNotGetIdentityType>(ct);
            }
            
            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IMediator _mediator;
        private readonly IDataContext _dataContext;

        public Handler(IMediator mediator, IDataContext dataContext)
        {
            _mediator = mediator;
            _dataContext = dataContext;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var storedRefreshToken =
                await _dataContext.Set<RefreshTokenEntity>().SingleAsync(
                    refreshToken => refreshToken.Token.ToString() == request.RequestRefreshToken, 
                    cancellationToken: ct);

            storedRefreshToken.Used = true;
            _dataContext.Set<RefreshTokenEntity>().Update(storedRefreshToken);
            await _dataContext.SaveChangesAsync(ct);

            var handler = new JwtSecurityTokenHandler();
            var tokenJson = handler.ReadJwtToken(request.RequestToken);
            
            var authType = tokenJson.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.IdentityType)?.Value;
            
            if (authType == IdentityType.ApiClientIdentity.ToString())
            {
                var apiClientResult = await HandleApiClientRefresh(tokenJson, ct);
                
                return new RefreshTokenResponse
                {
                    Token = apiClientResult.Token,
                    RefreshToken = apiClientResult.RefreshToken
                };
            }
            
            var userRefreshResult = await HandleUserRefresh(tokenJson, ct);

            return new RefreshTokenResponse
            {
                Token = userRefreshResult.Token,
                RefreshToken = userRefreshResult.RefreshToken
            };
        }

        private async Task<AuthenticateApiClient.AuthenticateApiClientResponse> HandleApiClientRefresh(JwtSecurityToken token, CancellationToken ct)
        {
            var clientId = Guid.Parse(token.Claims.First(x => x.Type == CustomClaimTypes.IdentityId).Value);
            
            var apiClient = await _dataContext.Set<ApiClientEntity>().FirstAsync(x => x.Id == clientId && !x.IsDeleted, ct);
            
            return await _mediator.Send(
                new AuthenticateApiClient.AuthenticateApiClientCommand(apiClient.ApiKey), 
                ct);
        }

        private async Task<AuthenticateUser.AuthenticateUserResponse> HandleUserRefresh(JwtSecurityToken token, CancellationToken ct)
        {
            var userId = Guid.Parse(token.Claims.First(x => x.Type == CustomClaimTypes.IdentityId).Value);
            var user = await _dataContext.Set<IdentityEntity>().FindAsync(userId);
    
            return await _mediator.Send(
                new AuthenticateUser.AuthenticateUserCommand(user.Email), 
                ct);
        }
    }

    public record RefreshTokenResponse : BaseResponse
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
    }
}