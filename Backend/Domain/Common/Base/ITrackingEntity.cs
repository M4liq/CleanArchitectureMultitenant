namespace Domain.Common.Base;

public interface ITrackingEntity
{
    DateTimeOffset CreatedDate { get; }
    Guid CreatedByUserId { get; }
    DateTimeOffset LastModifiedDate { get; }
    Guid LastModifiedByUserId { get; }
    Guid TenantId { get; }
    bool IsDeleted { get; }
    Guid? DeletedByUserId { get; }
    DateTimeOffset? DeletedDate { get; }

    void SetModificationTracking(Guid userId, DateTimeOffset modifiedDate);

    void SetCreationTracking(Guid userId, DateTimeOffset createdDate, Guid tenantId);

    void SetDeletionTracking(Guid userId, DateTimeOffset deletedDate);

    void Restore(Guid userId, DateTimeOffset restoredDate);
}