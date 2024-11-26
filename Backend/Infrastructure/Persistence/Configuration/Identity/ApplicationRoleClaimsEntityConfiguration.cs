using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity;

public class ApplicationRoleClaimsEntityConfiguration : IEntityTypeConfiguration<ApplicationRoleClaimsEntity>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaimsEntity> builder)
    {
        builder.ToTable("IdentityRoleClaims");
        builder.HasKey(c => c.Id);
    }
}