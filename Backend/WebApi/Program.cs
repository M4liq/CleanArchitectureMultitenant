using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }).Build();

        if (IsDevelopmentEnvironment())
        {
            RunDbMigration(host);
        }

        host.Run();
    }

    private static bool IsDevelopmentEnvironment() =>
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    private static void RunDbMigration(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            var context = services.GetService<DataContext>();

            if (((RelationalDatabaseCreator)context.GetService<IDatabaseCreator>()).Exists())
            {
                logger.LogInformation("Database already exists. Running program with no migration.");
                return;
            }

            logger.LogInformation("Database instance does not exist. Creating new one ...");

            context.Database.Migrate();

            logger.LogInformation("Database created successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the DB.");
        }
    }        
}