using Application.Common;
using Application.Common.Core;
using Application.Common.Settings;
using Infrastructure.Authentication;
using Infrastructure.Middleware;
using Infrastructure.Persistence;
using Infrastructure.Services.Core;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.InitializeConfiguration(configuration);
        services.AddJwtAuthentication(configuration);

        services.AddDbContext<DataContext>((sp, options) =>
        {
            var dataContextSettings = sp.GetRequiredService<IDataContextSettings>();
            options.UseSqlServer(
                    dataContextSettings.ConnectionString,
                    b => b.MigrationsAssembly("Infrastructure"))
                .UseLazyLoadingProxies();
        });

        services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());

        services.InitializeServices();
    }

    private static void InitializeConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection("DataContextSettings").Get<DataContextSettings>();
        services.AddSingleton<IDataContextSettings>(databaseSettings);

        var smtpClientSettings = configuration.GetSection("SmtpClientSettings").Get<SmtpClientSettings>();
        services.AddSingleton<ISmtpClientSettings>(smtpClientSettings);

        var errorsAndMessagesSettings =
            configuration.GetSection("ErrorsAndMessagesSettings").Get<ErrorsAndMessagesSettings>();
        services.AddSingleton<IErrorsAndMessagesSettings>(errorsAndMessagesSettings);
    }

    private static void InitializeServices(this IServiceCollection services)
    {
        services.AddSingleton<IDomainMessageManager, DomainMessageManager>();

        services.AddScoped<IEmailClient, EmailClient>();
        services.AddScoped<ITenantProvider, TenantProvider>();

        services.AddScoped<ICalendar, Calendar>();
        services.AddScoped<ICurrentIdentity, CurrentIdentity>();
        services.AddSingleton<ICurrentLanguage, CurrentLanguage>();

        services.AddHttpContextAccessor();

        services.AddScoped<MultiTenantMiddleware>();
        services.AddScoped<LanguageMiddleware>();
    }
}