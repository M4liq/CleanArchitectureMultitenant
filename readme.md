# CleanArchitectureWithIdentityFramework 

## Setup 

### Database
To crate migration for data context run:

    dotnet ef migrations add <MigrationName> -c DataContext -o Migrations --project Backend/Infrastructure/Infrastructure.csproj -s Backend/WebApi/WebApi.csproj

To update database:

    dotnet ef database update --project Backend/Infrastructure/Infrastructure.csproj -s Backend/WebApi/WebApi.csproj

To revert/remove migration:

    dotnet ef database update <previous-migration-name> --project Backend/Infrastructure/Infrastructure.csproj -s Backend/WebApi/WebApi.csproj
    dotnet ef migrations remove --project Backend/Infrastructure/Infrastructure.csproj -s Backend/WebApi/WebApi.csproj