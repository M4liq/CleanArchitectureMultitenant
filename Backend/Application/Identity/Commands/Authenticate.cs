using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common;
using Application.Common.Core;
using Application.Common.Settings;
using Domain.Common.Base;
using Domain.Identity.RefreshToken;
using Domain.Identity.User;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Application.Identity.Commands;

public class Authenticate
{
    public record AuthenticateCommand(UserEntity User) : IRequest<AuthenticateResponse>;

    public class Handler : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
    {
        private readonly IDataContext _context;
        private readonly IJwtSettings _jwtSettings;
        private readonly ILogger<AuthenticateCommand> _logger;

        public Handler(IDataContext context, IJwtSettings jwtSettings, ILogger<AuthenticateCommand> logger)
        {
            _context = context;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        public class AuthenticationValidator : IDomainValidationHandler<AuthenticateCommand>
        {
            public async Task<ValidationResult> Validate(AuthenticateCommand request)
            {
                return new ValidationResult();
            }
        }
        
        public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, request.User.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, request.User.Email),
                    new Claim("userId", request.User.Id.ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.JwtTokenLifeTime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new ApplicationRefreshTokensEntity
            {
                Token = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = request.User.Id,
                ExpiryDate = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenLifeTime)
            };

            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Authorization event. UserId: {0}, JwtTokenExpiration: {1}, RefreshTokenExpiration: {2}", 
                request.User.Id, 
                token.ValidTo,
                refreshToken.ExpiryDate);

            return new AuthenticateResponse
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token.ToString()
            };
        }
    }

    public record AuthenticateResponse : BaseResponse
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
    }
}