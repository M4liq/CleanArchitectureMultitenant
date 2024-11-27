using Domain.Identity.RefreshToken;
using Infrastructure.Persistence.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity;

public class ApplicationRefreshTokenEntityConfiguration : BaseEntityConfiguration<ApplicationRefreshTokensEntity>
{
    public override void Configure(EntityTypeBuilder<ApplicationRefreshTokensEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("IdentityRefreshTokens");
        
        builder.HasKey(_ => _.Id);
        builder.HasIndex(_ => _.Id);

        builder.HasOne(_ => _.User)
            .WithMany()
            .HasForeignKey(_ => _.UserId);
        
        builder
            .Property(_ => _.CreatedDate)
            .IsRequired();

        builder
            .Property(_ => _.CreatedByUserId)
            .IsRequired();

        builder
            .Property(_ => _.LastModifiedDate)
            .IsRequired();

        builder
            .Property(_ => _.LastModifiedByUserId)
            .IsRequired();

        builder.Property(_ => _.TenantId)
            .IsRequired();
        
        builder.HasIndex(_ => _.TenantId);
    }
}