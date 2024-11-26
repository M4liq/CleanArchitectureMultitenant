using System.Text;
using Application.Common;
using Application.Common.Interfaces.Core;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Settings;
using Domain.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services.Core;
using Infrastructure.Services.Identity;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.InitializeConfiguration(configuration);
        services.InitializeAuthorization(configuration);

        services.AddDbContext<SaasContext>((sp, options) =>
        {
            var saasSettings = sp.GetRequiredService<ISaasSettings>();
            options.UseSqlServer(
                saasSettings.ConnectionString,
                b => b.MigrationsAssembly("Infrastructure"));
        });
        
        // Design-time factory for DataContext migrations
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(
                "Name=DefaultConnection", // Placeholder for migration generation
                b => b.MigrationsAssembly("Infrastructure")));

        // Runtime context factory
        services.AddScoped<DataContext>((sp) => {
            var tenantProvider = sp.GetRequiredService<ITenantProvider>();
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(tenantProvider.ConnectionString);
            return new DataContext(optionsBuilder.Options);
        });
        
        services.AddIdentity<ApplicationUserEntity, ApplicationRoleEntity>(_ => _.SignIn.RequireConfirmedAccount = true)
            .AddUserManager<ApplicationUserManager>()
            .AddUserStore<ApplicationUserStore>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IDataContext, DataContext>();
        services.InitializeServices();
    }

    private static void InitializeConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection("SaasSettings").Get<SaasSettings>();
        services.AddSingleton<ISaasSettings>(databaseSettings);
        
        var smtpClientSettings = configuration.GetSection("SmtpClientSettings").Get<SmtpClientSettings>();
        services.AddSingleton<ISmtpClientSettings>(smtpClientSettings);
        
        var errorsAndMessagesSettings = configuration.GetSection("ErrorsAndMessagesSettings").Get<ErrorsAndMessagesSettings>();
        services.AddSingleton<IErrorsAndMessagesSettings>(errorsAndMessagesSettings);
    }

    private static void InitializeServices(this IServiceCollection services)
    {
        services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
        services.AddScoped<IErrorManager, ErrorManager>();
        services.AddScoped<IMessageManager, MessageManager>();
        services.AddScoped<IEmailClient, EmailClient>();
        services.AddScoped<ITenantProvider, TenantProvider>();
    }

    private static void InitializeAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.AddSingleton<IJwtSettings>(jwtSettings);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(_ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(_ =>
            {
                _.SaveToken = true;
                _.TokenValidationParameters = tokenValidationParameters;
            });
    }
}