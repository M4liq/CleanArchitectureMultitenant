using Domain.Identity.Scope;

namespace Domain.Identity.Tenant;

public class TenantScopeValueObject : ScopeValueObject
{
    private TenantScopeValueObject(string value) : base($"tenant:{value}") {}

    public static readonly TenantScopeValueObject Create = new("create");
    public static readonly TenantScopeValueObject Switch = new("switch");
}