using Domain.Common.Base;
using Domain.Identity.User;

namespace Domain.Identity.RefreshToken;

public class ApplicationRefreshTokensEntity : BaseEntity
{
    public DateTime ExpiryDate { get; set; }
    public bool Invalidated { get; set; }
    public string JwtId { get; set; }
    public Guid Token { get; set; }
    public bool Used { get; set; }
    public virtual UserEntity User { get; set; }
    public Guid UserId { get; set; }
}