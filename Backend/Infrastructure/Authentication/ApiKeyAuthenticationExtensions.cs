using System.Text;
using Application.Common.Settings;
using Domain.Identity.ApiClient;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public static class ApiKeyAuthenticationExtensions
{
    public static IServiceCollection AddApiKeyAndJwtAuthentication(
        this IServiceCollection services, 
        IConfiguration configuration)
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

        services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = "Mixed";
                options.DefaultChallengeScheme = "Mixed";
                options.DefaultScheme = "Mixed";
            })
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null)
            .AddJwtBearer("Jwt", options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            })
            .AddPolicyScheme("Mixed", "ApiKey or JWT", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    if (context.Request.Headers.ContainsKey("X-Api-Key"))
                        return "ApiKey";
                
                    return "Jwt";
                };
            });

        services.AddAuthorization(options =>
        {
            foreach (var scope in Scope.GetAllScopes())
            {
                options.AddPolicy(scope.Value, policy =>
                    policy.AddRequirements(new ScopeRequirement(scope.Value)));
            }
        });

        services.AddScoped<IAuthorizationHandler, ScopeAuthorizationHandler>();
        
        return services;
    }
}