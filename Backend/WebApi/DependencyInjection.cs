using Microsoft.OpenApi.Models;
using WebApi.Filters;

namespace WebApi;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        AddMvc(services);
        AddRateLimiting(services, configuration);
        AddSecurity(services);
    }

    private static void AddRateLimiting(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.AddMemoryCache();
    }

    private static void AddMvc(IServiceCollection services)
    {
        services.AddMvc();

        services.AddControllers();

        services.AddControllers(o => { o.Filters.Add(typeof(ResponseMappingFilter)); });
    }

    private static void AddSecurity(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Api", Version = "v1"
            });
        });
    }
}