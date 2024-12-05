namespace Domain.Common.Base;

public interface ITrackingEntity
{
    DateTimeOffset CreatedDate { get; }
    Guid CreatedById { get; }
    DateTimeOffset LastModifiedDate { get; }
    Guid LastModifiedById { get; }
    Guid TenantId { get; }
    bool IsDeleted { get; }
    Guid? DeletedById { get; }
    DateTimeOffset? DeletedDate { get; }

    void SetModificationTracking(Guid userId, DateTimeOffset modifiedDate);

    void SetCreationTracking(Guid userId, DateTimeOffset createdDate, Guid tenantId);

    void SetDeletionTracking(Guid userId, DateTimeOffset deletedDate);

    void Restore(Guid userId, DateTimeOffset restoredDate);
}