using System.Net;
using Application.Common.Interfaces.Core;
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
        var tenant = "testTenant";

        if (string.IsNullOrEmpty(tenant))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Invalid request: Unable to identify the tenant subdomain.");
            return;
        }

        try
        {
            await _tenantProvider.SetTenantAsync(tenant);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(e.Message);
            return;
        }
        
        await next(context);
    }
}