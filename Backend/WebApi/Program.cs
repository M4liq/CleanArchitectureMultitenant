using System.Text.Json;
using Application;
using Domain.Common;
using Domain.Identity;
using Domain.Identity.ApiClient;
using Domain.Identity.Scope;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure;
using Infrastructure.Middleware;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NSwag;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddFastEndpoints();

        builder.Services.SwaggerDocument(o =>
        {
            o.EnableJWTBearerAuth = true;
        });
        
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<MultiTenantMiddleware>();
        app.UseMiddleware<LanguageMiddleware>();

        app.UseFastEndpoints(c =>
        {
            c.Endpoints.RoutePrefix = "api";
            c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        if (app.Environment.IsDevelopment())
        {
            RunDbMigration(app);
        }

        app.Run();
    }

    private static void RunDbMigration(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            var context = services.GetService<DataContext>();

            bool isNewDatabase = !((RelationalDatabaseCreator)context.GetService<IDatabaseCreator>()).Exists();

            if (isNewDatabase)
            {
                logger.LogInformation("Database instance does not exist. Creating new one ...");
                context.Database.Migrate();
                logger.LogInformation("Database created successfully.");
                
                CreateDefaultApiClient(context, logger);
            }
            else
            {
                logger.LogInformation("Database already exists. Running program with no migration.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the DB.");
        }
    }

    private static void CreateDefaultApiClient(DataContext context, ILogger logger)
    {
        try
        {
            using (context.UseSystemContext())
            {
                var allScopes = ScopeValueObject.GetAllScopes().ToList();
                var apiKey = Guid.NewGuid().ToString("N");

                var defaultClient = ApiClientEntity.Create(
                    "Default System Client",
                    apiKey,
                    allScopes,
                    true
                );

                context.Set<ApiClientEntity>().Add(defaultClient);
                context.SaveChanges();

                logger.LogInformation("Default API client created successfully.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create default API client.");
        }
    }
}