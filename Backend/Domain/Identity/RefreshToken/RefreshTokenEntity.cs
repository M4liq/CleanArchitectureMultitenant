using Domain.Common.Base;

namespace Domain.Identity.RefreshToken;

public class RefreshTokenEntity : BaseEntity
{
    public DateTime ExpiryDate { get; set; }
    public bool Invalidated { get; set; }
    public string JwtId { get; set; }
    public Guid Token { get; set; }
    public bool Used { get; set; }
}