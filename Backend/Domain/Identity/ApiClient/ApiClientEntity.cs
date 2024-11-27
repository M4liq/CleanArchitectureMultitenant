using Domain.Common.Base;

namespace Domain.Identity.ApiClient;

public class ApiClientEntity : BaseEntity
{
    public string Name { get; private set; }
    public string ApiKey { get; private set; }
    private List<string> _allowedScopes = new();
    
    public IReadOnlyCollection<string> AllowedScopes => _allowedScopes.AsReadOnly();

    private ApiClientEntity() { } // For EF

    public static ApiClientEntity Create(string name, string apiKey, IEnumerable<Scope> allowedScopes)
    {
        return new ApiClientEntity
        {
            Name = name,
            ApiKey = apiKey,
            _allowedScopes = allowedScopes.Select(s => s.Value).ToList()
        };
    }

    public bool HasScope(Scope scope) => _allowedScopes.Contains(scope.Value);
    
    public void AddScope(Scope scope)
    {
        if (!_allowedScopes.Contains(scope.Value))
        {
            _allowedScopes.Add(scope.Value);
        }
    }
    
    public void RemoveScope(Scope scope)
    {
        _allowedScopes.Remove(scope.Value);
    }
}