using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity;

public class ApplicationUserRolesEntityConfiguration : IEntityTypeConfiguration<ApplicationUserRolesEntity>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRolesEntity> builder)
    {
        builder.ToTable("IdentityUserRoles");
        builder.HasKey(r => new { r.UserId, r.RoleId });
    }
}