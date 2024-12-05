using System.Security.Claims;
using Application.Common.Core;
using Application.Identity.Constants;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Core;

public class CurrentIdentity : ICurrentIdentity
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentIdentity(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid IdentityId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(CustomClaimTypes.IdentityId)?.Value;
            var parsed = userId != null
                ? Guid.TryParse(userId, out var identityId)
                : throw new InvalidOperationException("Identity must be specified.");
            
            return identityId;
        }
    }

    public IdentityType IdentityType
    {
        get
        {
            var type = _httpContextAccessor.HttpContext?.User.FindFirst(CustomClaimTypes.IdentityType)?.Value;
            return Enum.Parse<IdentityType>(type ?? throw new InvalidOperationException("Identity type must be specified."));
        }
    }

    public string IdentityName =>
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}