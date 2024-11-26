using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity;

public class ApplicationUserClaimsEntityConfiguration : IEntityTypeConfiguration<ApplicationUserClaimsEntity>
{
    public void Configure(EntityTypeBuilder<ApplicationUserClaimsEntity> builder)
    {
        builder.ToTable("IdentityUserClaims");
        builder.HasKey(c => c.Id);
    }
}