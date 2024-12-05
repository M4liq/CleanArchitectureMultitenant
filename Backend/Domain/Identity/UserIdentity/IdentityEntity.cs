using Domain.Common.Base;

namespace Domain.Identity.UserIdentity;

public class IdentityEntity : BaseEntity
{
    public string Email { get; private set; }
}