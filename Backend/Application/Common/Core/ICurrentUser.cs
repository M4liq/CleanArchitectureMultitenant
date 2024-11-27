namespace Application.Common.Core;

public interface ICurrentUser
{
    Guid? UserId { get; }
    string UserName { get; }
    bool IsAuthenticated { get; }
}