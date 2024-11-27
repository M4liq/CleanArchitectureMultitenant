using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authentication;

public class ScopeRequirement : IAuthorizationRequirement
{
    public string Scope { get; }

    public ScopeRequirement(string scope)
    {
        Scope = scope;
    }
}

public class ScopeAuthorizationHandler : AuthorizationHandler<ScopeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ScopeRequirement requirement)
    {
        if (context.User.HasClaim(c =>
                c.Type == "scope" &&
                c.Value == requirement.Scope))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequireScopeAttribute : Attribute
{
    public string ScopeValue { get; }

    public RequireScopeAttribute(string scopeValue)
    {
        ScopeValue = scopeValue;
    }
}