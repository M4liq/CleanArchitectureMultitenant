namespace Application.Identity.Constants;

public static class CustomClaimTypes
{
    public const string IdentityId = "identityId";
    public const string IdentityType = "identityType";
    public const string TenantId = "tenantId";
    public const string Scope = "scope";
}

public enum IdentityType
{
    ApiClientIdentity,
    UserIdentity
}