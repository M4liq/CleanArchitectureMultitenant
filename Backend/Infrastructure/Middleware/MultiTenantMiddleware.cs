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

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var success = context.Request.Headers.TryGetValue("X-TenantId", out var tenantId);
        
        if (!success)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Could not get tenant id.");
            return;
        }

        try
        {
            _tenantProvider.SetTenant(Guid.Parse(tenantId));
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Invalid tenant id.");
            return;
        }
        
        await next(context);
    }
}