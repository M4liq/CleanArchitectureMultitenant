using Domain.Common;

namespace Domain.Identity;

public class ApplicationRefreshTokensEntity : BaseEntity
{
    public DateTime ExpiryDate { get; set; }
    public bool Invalidated { get; set; }
    public string JwtId { get; set; }
    public Guid Token { get; set; }
    public bool Used { get; set; }
    public virtual ApplicationUserEntity User { get; set; }
    public Guid UserId { get; set; }
}