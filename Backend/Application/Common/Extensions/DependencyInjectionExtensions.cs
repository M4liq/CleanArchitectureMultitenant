using Application.Common.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<IValidationHandler>()
            .AddClasses(classes => classes.AssignableTo<IValidationHandler>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
    
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var applicationAssembly = typeof(IBoundedService).Assembly;

        var serviceTypes = applicationAssembly
            .GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                !t.IsInterface &&
                t.GetInterfaces()
                    .Any(i => i.IsAssignableTo(typeof(IBoundedService))));

        foreach (var serviceType in serviceTypes)
        {
            var serviceInterface = serviceType.GetInterfaces()
                .First(i => i != typeof(IBoundedService) &&
                            i.IsAssignableTo(typeof(IBoundedService)));

            services.AddScoped(serviceInterface, serviceType);
        }
    }
}