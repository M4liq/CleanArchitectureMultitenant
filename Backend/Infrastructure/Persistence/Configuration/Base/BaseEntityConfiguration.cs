using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Base;

public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.HasIndex(_ => _.Id);

        builder.HasOne(_ => _.CreatedBy)
            .WithMany()
            .HasForeignKey(_ => _.CreatedUserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(_ => _.LastModifiedBy)
            .WithMany()
            .HasForeignKey(_ => _.LastModifiedUserId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}