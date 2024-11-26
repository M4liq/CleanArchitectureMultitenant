using Domain.Identity;
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

        builder.HasOne(_ => _.User)
            .WithMany()
            .HasForeignKey(_ => _.UserId);
    }
}