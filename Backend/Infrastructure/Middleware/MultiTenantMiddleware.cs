using System.Net;
using Application.Common.Core;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middleware;

public class MultiTenantMiddleware : IMiddleware
{
    private readonly ITenantProvider _tenantProvider;

    public MultiTenantMiddleware(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    private const string TenantIdHeaderName = "X-TenantId";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Headers.TryGetValue(TenantIdHeaderName, out var headerTenantId) &&
            Guid.TryParse(headerTenantId, out var parsedHeaderTenantId))
        {
            _tenantProvider.SetTenant(parsedHeaderTenantId);
            await next(context);
            return;
        }
        
        var tenantClaim = context.User.Claims.FirstOrDefault(x => x.Type == "tenantId");
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