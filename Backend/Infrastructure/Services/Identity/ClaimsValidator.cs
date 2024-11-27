using System.Security.Claims;

namespace Infrastructure.Services.Identity;

public interface IClaimsValidator
{
    Task<bool> ValidateScope(string scope, ClaimsPrincipal principal);
}


public class ClaimsValidator : IClaimsValidator
{
    public async Task<bool> ValidateScope(string scope, ClaimsPrincipal principal)
    {
        if (principal.Identity?.AuthenticationType == "ApiKey")
        {
            return principal.HasClaim("scope", scope);
        }
        
        // For JWT auth - check JWT roles/claims
        // if (principal.Identity?.AuthenticationType == "Jwt")
        // {
        //     return principal.IsInRole("Admin") || principal.HasClaim("permissions", scope);
        // }

        return false;
    }
}