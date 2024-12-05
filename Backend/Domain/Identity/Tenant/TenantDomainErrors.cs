using Domain.Common.Interfaces;

namespace Domain.Identity.Tenant;

public static class TenantDomainErrors
{
    public class TenantWithSuchNameAlreadyExists : IDomainMessage
    {
        public string Code { get; init; } = nameof(TenantWithSuchNameAlreadyExists);
        public string DefaultMessagePl { get; init; } = "Tenant o podanym identyfikatorze już istnieje.";
        public string DefaultMessageEn { get; init; } = "A tenant with this identifier already exists.";
    }
    
    public class UnauthorizedTenantSwitch : IDomainMessage
    {
        public string Code { get; init; } = nameof(UnauthorizedTenantSwitch);
        public string DefaultMessagePl { get; init; } = "Nieautoryzowana zmiana kontekstu.";
        public string DefaultMessageEn { get; init; } = "Unauthorized tenant context switch.";
    }
}