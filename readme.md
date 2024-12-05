# CleanArchitectureWithIdentityFramework 

### Internationalization
Backend application supports different languages. Pass the X-Requested-Language header containing two letter iso country code to use it. 
Currently it fully supports en and pl country codes. The default application language is en. We ignore the Application-Language header,
as we want to be explicit about language and this header is often set automatically.

### Swagger
Application uses swagger for API documentation. Go to /swagger for reaching it.

### Api clients and scopes 
System supports api client authentication. System client with all scopes assigned is created automatically in development environment.
Scopes are defined as value objects deriving from `ScopeValueObject`. They should be defined in `Domain` project and be alligned with commands/queries defined in `Application` project.

### Multitenancy
Application relies on discriminator based multitenancy. Every entity based on `BaseEntity` is automatically filtered by tenant context, unless the system context is used.
The system context accessible through `IDataContext` interface using `IDisposible` pattern. Inside scope application can access all resources so use it carefully.
The tenant context is set during authentication process and stored in `tenantId` claim.

### CQRS and request validation
We utilize cqrs in our application using `MediatR`. We use pipeline behaviours for performing validation,
any class implementing `IValidationHandler` will be automatically registered and the implementation of `Validate` method will be called before passing the request to the request handler.
All validation messages must implement `IDomainMessage` interface and be returned through `IDomainMessageManager`.

### Soft delete
System uses soft delete and all resources marked as deleted are automatically filtered out from queries unless the system context is used.

### Migrations
To crate migration for data context run:

    dotnet ef migrations add <MigrationName> -c DataContext -o Migrations --project Backend/Infrastructure/Infrastructure.csproj -s Backend/WebApi/WebApi.csproj

To update database:

    dotnet ef database update --project Backend/Infrastructure/Infrastructure.csproj -s Backend/WebApi/WebApi.csproj

To revert/remove migration:

    dotnet ef database update <previous-migration-name> --project Backend/Infrastructure/Infrastructure.csproj -s Backend/WebApi/WebApi.csproj
    dotnet ef migrations remove --project Backend/Infrastructure/Infrastructure.csproj -s Backend/WebApi/WebApi.csproj

Migrations are automatically performed on application startup in development environment.

### Authorization
Backend application uses JWT tokens for authorization. To authorize request pass Authorization header with a value in format `bearer <JWTtoken>`.

JWT tokens default lifetime is 5 minutes (24h for development environmeny). It can be changed in appsettings.json. 
The reason of JWT token lifetime limitation is to be able to limit time of user session in case of unauthorize account interception. 
It also cretes possiblilty of adding feature of automatic user logging out when there is no activity. 
To exend time of user session use refresh token provided together with JWT token after authentication. 
The endpoint designed for this purpose is /identity/refresh-token. 
It creates new refresh token together with new JWT token, enabling user session lengthening.
Mind refreshing requires a valid JWT token issued by the backend application. It can be expired. 
If the refresh token is invalidated user session can not be extended. The default lifetime of refresh token is 6 months.
It can be changed in appsettings.json.

**Important:** refresh token lifetime can not be longer than access token lifetime in order to ensure access to the application and the resource server at the same time.