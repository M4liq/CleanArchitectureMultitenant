using System.Net;
using Application.Common.Core;
using Application.Identity.Constants;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middleware;

public class MultiTenantMiddleware : IMiddleware
{
    private readonly ITenantProvider _tenantProvider;

    public MultiTenantMiddleware(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var tenantClaim = context.User.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.TenantId);
        if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var parsedTokenTenantId))
        {
            _tenantProvider.SetTenant(parsedTokenTenantId);
            await next(context);
            return;
        }
        
        if (context.User.Identity?.IsAuthenticated == true)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Tenant ID is required for authenticated requests.");
            return;
        }
        
        await next(context);
    }
}