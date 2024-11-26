namespace Application.Common.Interfaces.Core;

public interface ITenantProvider
{
    public string TenantName { get; }

    public string ConnectionString { get; }

    public Task SetTenantAsync(string tenantName);
}