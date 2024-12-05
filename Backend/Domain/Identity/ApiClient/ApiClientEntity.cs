using Domain.Common.Base;
using Domain.Identity.Scope;

namespace Domain.Identity.ApiClient;

public class ApiClientEntity : BaseEntity
{
    public string Name { get; private set; }
    public string ApiKey { get; private set; }
    public bool IsSystem { get; private set; }
    private readonly List<ScopeEntity> _scopes = new();
    public virtual IReadOnlyCollection<ScopeEntity> Scopes => _scopes.AsReadOnly();
    
    public static ApiClientEntity Create(string name, string apiKey, IEnumerable<ScopeValueObject> scopes,
        bool isSystem)
    {
        var entity = new ApiClientEntity
        {
            Name = name,
            ApiKey = apiKey,
            IsSystem = isSystem
        };

        foreach (var scope in scopes)
        {
            entity._scopes.Add(ScopeEntity.Create(entity.Id, scope));
        }

        return entity;
    }

    public bool HasScope(Scope.ScopeValueObject scopeValueObject) =>
        _scopes.Any(s => s.ScopeValue == scopeValueObject.Value);
}