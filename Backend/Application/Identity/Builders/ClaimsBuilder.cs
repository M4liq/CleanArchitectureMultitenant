using System.Security.Claims;
using Application.Identity.Constants;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Application.Identity.Builders;

public class ClaimsBuilder
{
    public static List<Claim> CreateClaims(string name, Guid id, Guid tenantId, IdentityType identityType)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(CustomClaimTypes.IdentityId, id.ToString()),
            new(CustomClaimTypes.TenantId, tenantId.ToString()),
            new(CustomClaimTypes.IdentityType, identityType.ToString())
        };

        return claims;
    }
}