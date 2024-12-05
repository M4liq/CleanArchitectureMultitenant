using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common;
using Application.Common.Settings;
using Domain.Identity.RefreshToken;
using Microsoft.IdentityModel.Tokens;

namespace Application.Identity.BoundedServices;

public interface IAuthenticationTokenService : IBoundedService
{
    Task<TokenResult> GenerateTokenAsync(ClaimsIdentity claims, CancellationToken ct);
}

public class AuthenticationTokenService : IAuthenticationTokenService
{
    private readonly IDataContext _context;
    private readonly IJwtSettings _jwtSettings;

    public AuthenticationTokenService(IDataContext context, IJwtSettings jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings;
    }

    public async Task<TokenResult> GenerateTokenAsync(ClaimsIdentity claims, CancellationToken ct)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        
        if (key.Length < 32)
        {
            throw new InvalidOperationException(
                "JWT secret must be at least 32 characters long for HMAC-SHA256");
        }
        
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.Add(_jwtSettings.JwtTokenLifeTime),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        });

        var refreshToken = new RefreshTokenEntity
        {
            Token = Guid.NewGuid(),
            JwtId = token.Id,
            ExpiryDate = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenLifeTime)
        };
        
        await _context.Set<RefreshTokenEntity>().AddAsync(refreshToken, ct);
        await _context.SaveChangesAsync(ct);

        return new TokenResult(
            tokenHandler.WriteToken(token),
            refreshToken.Token.ToString()
        );
    }
}

public record TokenResult(string Token, string RefreshToken);