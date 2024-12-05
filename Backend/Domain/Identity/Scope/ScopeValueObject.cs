using System.Reflection;
using Domain.Common.Base;

namespace Domain.Identity.Scope;

public abstract class ScopeValueObject : ValueObject
{
    protected ScopeValueObject(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(ScopeValueObject scopeValueObject) => scopeValueObject.Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static IEnumerable<ScopeValueObject> GetAllScopes()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && 
                        a.FullName?.StartsWith("Domain") == true);

        return assemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(t => !t.IsAbstract && typeof(ScopeValueObject).IsAssignableFrom(t))
            .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Where(f => typeof(ScopeValueObject).IsAssignableFrom(f.FieldType))
            .Select(f => (ScopeValueObject)f.GetValue(null))
            .ToList();
    }
}