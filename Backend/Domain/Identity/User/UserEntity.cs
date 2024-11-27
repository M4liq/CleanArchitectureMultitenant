using Domain.Common.Base;

namespace Domain.Identity.User;

public class UserEntity : BaseEntity
{
    public string Email { get; private set; }
}