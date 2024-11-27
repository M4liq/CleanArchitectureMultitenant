using System.Security.Claims;
using System.Text.Encodings.Web;
using Application.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private const string TenantIdHeaderName = "X-TenantId";
    private readonly IDataContext _context;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IDataContext context)
        : base(options, logger, encoder)
    {
        _context = context;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey) ||
            !Request.Headers.TryGetValue(TenantIdHeaderName, out var tenantId))
        {
            return AuthenticateResult.Fail("Required headers missing");
        }

        var apiClient = await _context.ApiClients.FirstOrDefaultAsync(x => x.ApiKey == apiKey);

        if (apiClient == null)
        {
            return AuthenticateResult.Fail("Invalid API key");
        }

        if (!Guid.TryParse(tenantId, out var parsedTenantId))
        {
            return AuthenticateResult.Fail("Invalid tenant ID");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, apiClient.Id.ToString()),
            new(ClaimTypes.Name, apiClient.Name),
            new("TenantId", parsedTenantId.ToString())
        };

        claims.AddRange(apiClient.AllowedScopes.Select(scope => 
            new Claim("scope", scope)));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
