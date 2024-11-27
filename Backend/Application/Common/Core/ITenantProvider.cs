namespace Application.Common.Core;

public interface ITenantProvider
{
    Guid? TenantId { get; }
    void SetTenant(Guid tenantId);
}