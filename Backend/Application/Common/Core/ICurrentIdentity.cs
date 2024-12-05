using Application.Identity.Constants;

namespace Application.Common.Core;

public interface ICurrentIdentity
{
    Guid IdentityId { get; }
    IdentityType IdentityType { get; }
    string IdentityName { get; }
    bool IsAuthenticated { get; }
}