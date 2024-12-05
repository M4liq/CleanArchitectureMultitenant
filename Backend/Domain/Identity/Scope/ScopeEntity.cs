using Domain.Common.Base;
using Domain.Identity.ApiClient;

namespace Domain.Identity.Scope;

public class ScopeEntity : BaseEntity
{
    public Guid ApiClientId { get; private set; }
    public virtual ApiClientEntity ApiClient { get; private set; }
    public string ScopeValue { get; private set; }

    public static ScopeEntity Create(Guid apiClientId, ScopeValueObject scopeValueObject)
    {
        return new ScopeEntity
        {
            ApiClientId = apiClientId,
            ScopeValue = scopeValueObject.Value
        };
    }
}