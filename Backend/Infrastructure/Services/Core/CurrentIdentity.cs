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

    public Guid? IdentityId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(CustomClaimTypes.IdentityId)?.Value;
            return userId != null ? Guid.Parse(userId) : null;
        }
    }

    public IdentityType IdentityType
    {
        get
        {
            var type = _httpContextAccessor.HttpContext?.User.FindFirst(CustomClaimTypes.IdentityType)?.Value;
            return Enum.Parse<IdentityType>(type ?? throw new InvalidOperationException());
        }
    }
    
    public string IdentityName => 
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}