using Application.Common.Interfaces.Core;

namespace Infrastructure.Services.Core;

public class TenantProvider : ITenantProvider
{
    private string? _tenant;
    private string? _connection;
    
    public string TenantName
    {
        get => _tenant ?? throw new InvalidOperationException("Tenant has not been set.");
        private set => _tenant = value;
    }

    public string ConnectionString
    {
        get => _connection ?? throw new InvalidOperationException("Tenant's connection string has not been set.");
        private set => _connection = value;
    }
    
    public async Task SetTenantAsync(string tenantName)
    {
        _connection = "test123"; //get connection service here
        _tenant = tenantName;
    }
}