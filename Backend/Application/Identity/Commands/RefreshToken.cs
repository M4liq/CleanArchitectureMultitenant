using System.IdentityModel.Tokens.Jwt;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Services;
using Domain.Common;
using Domain.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Commands;

public static class RefreshToken
{
    public record RefreshTokenCommand(string requestToken, string requestRefreshToken) : IRequest<RefreshTokenResponse>;

    public class LoginValidator : IDomainValidationHandler<RefreshTokenCommand>
    {
        private readonly IDataContext _context;
        private readonly IErrorManager _errorManager;

        public LoginValidator(IErrorManager errorManager, IDataContext context)
        {
            _errorManager = errorManager;
            _context = context;
        }

        public async Task<ValidationResult> Validate(RefreshTokenCommand request)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenJson = handler.ReadJwtToken(request.requestToken);

            var jwtId = tokenJson.Payload.Jti;

            if (jwtId == null)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationRefreshTokenDomainErrors.JwtTokenIsNoValid>();
            }

            var storedRefreshToken =
                await _context.RefreshTokens.SingleOrDefaultAsync(
                    _ => _.Token.ToString() == request.requestRefreshToken);

            if (storedRefreshToken == null)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationRefreshTokenDomainErrors.RefreshTokenDoesNotExits>();
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationRefreshTokenDomainErrors.RefreshTokenHasExpired>();
            }

            if (storedRefreshToken.Invalidated)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<
                        ApplicationRefreshTokenDomainErrors.RefreshTokenHasBeenInvalidated>();
            }

            if (storedRefreshToken.Used)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<ApplicationRefreshTokenDomainErrors.RefreshTokenHasBeenUsed>();
            }

            if (storedRefreshToken.JwtId != jwtId)
            {
                return await _errorManager
                    .GetValidationResultForErrorAsync<
                        ApplicationRefreshTokenDomainErrors.RefreshTokenDoesNotMatchJwt>();
            }

            return new ValidationResult();
        }
    }

    public class Handler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IMediator _mediator;
        private readonly IApplicationUserManager _userManager;
        private readonly IDataContext _context;

        public Handler(IMediator mediator, IApplicationUserManager userManager, IDataContext context)
        {
            _mediator = mediator;
            _userManager = userManager;
            _context = context;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var storedRefreshToken =
                await _context.RefreshTokens.SingleOrDefaultAsync(
                    _ => _.Token.ToString() == request.requestRefreshToken, cancellationToken: cancellationToken);

            storedRefreshToken.Used = true;

            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId);

            var result = await _mediator.Send(new Authenticate.AuthenticateCommand(user), cancellationToken);

            return new RefreshTokenResponse
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken
            };
        }
    }

    public record RefreshTokenResponse : BaseResponse
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
    }
}