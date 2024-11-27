using Application.Common.Core;

namespace Infrastructure.Services.Core;

public class TenantProvider : ITenantProvider
{
    private Guid? _currentTenantId;

    public Guid? TenantId => _currentTenantId;

    public void SetTenant(Guid tenantId)
    {
        _currentTenantId = tenantId;
    }
}