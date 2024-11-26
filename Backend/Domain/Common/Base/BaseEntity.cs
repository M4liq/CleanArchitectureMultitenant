using Domain.Identity;

namespace Domain.Common;

public abstract class BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public virtual ApplicationUserEntity CreatedBy { get; set; }
    public Guid CreatedUserId { get; set; }
    public Guid Id { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public virtual ApplicationUserEntity LastModifiedBy { get; set; }
    public Guid LastModifiedUserId { get; set; }
}