using Application.Common.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<IDomainValidationHandler>()
            .AddClasses(classes => classes.AssignableTo<IDomainValidationHandler>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}