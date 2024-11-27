namespace Domain.Common.Base;

public abstract class BaseEntity : ITrackingEntity
{
    public DateTimeOffset CreatedDate { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public Guid Id { get; private set; }
    public DateTimeOffset LastModifiedDate { get; private set; }
    public Guid LastModifiedByUserId { get; private set; }
    public Guid TenantId { get; set; }
    public bool IsDeleted { get; private set; }
    public Guid? DeletedByUserId { get; private set; }
    public DateTimeOffset? DeletedDate { get; private set; }
    
    public void SetCreationTracking(Guid userId, DateTimeOffset createdDate, Guid tenantId)
    {
        CreatedByUserId = userId;
        LastModifiedByUserId = userId;
        CreatedDate = createdDate;
        LastModifiedDate = createdDate;
        TenantId = tenantId;
    }

    public void SetModificationTracking(Guid userId, DateTimeOffset modifiedDate)
    {
        LastModifiedByUserId = userId;
        LastModifiedDate = modifiedDate;
    }
    
    public void SetDeletionTracking(Guid userId, DateTimeOffset deletedDate)
    {
        DeletedByUserId = userId;
        DeletedDate = deletedDate;
        IsDeleted = true;
    }

    public void Restore(Guid userId, DateTimeOffset restoredDate)
    {
        IsDeleted = false;
        DeletedByUserId = null;
        DeletedDate = null;
        SetModificationTracking(userId, restoredDate);
    }
}
