namespace Domain.Identity.ApiClient;

public sealed record Scope
{
    private Scope(string Value)
    {
        this.Value = Value;
    }

    public string Value { get; }

    public static implicit operator string(Scope scope) => scope.Value;

    public static class Tenants
    {
        public static readonly Scope Create = new("tenant:create");
    }
    
    public static IEnumerable<Scope> GetAllScopes()
    {
        return typeof(Scope)
            .GetNestedTypes()
            .SelectMany(t => t.GetFields())
            .Where(f => f.FieldType == typeof(Scope))
            .Select(f => (Scope)f.GetValue(null))
            .ToList();
    }
}