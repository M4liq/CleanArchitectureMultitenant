using Domain.Common.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Base;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.HasIndex(_ => _.Id);

        builder
            .Property(_ => _.CreatedDate)
            .IsRequired();

        builder
            .Property(_ => _.CreatedById)
            .IsRequired();

        builder
            .Property(_ => _.LastModifiedDate)
            .IsRequired();

        builder
            .Property(_ => _.LastModifiedById)
            .IsRequired();

        builder.Property(_ => _.TenantId)
            .IsRequired();
        
        builder.HasIndex(_ => _.TenantId);
    }
}