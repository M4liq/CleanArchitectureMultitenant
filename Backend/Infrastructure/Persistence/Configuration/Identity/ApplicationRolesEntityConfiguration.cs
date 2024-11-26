using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity;

public class ApplicationRolesEntityConfiguration : IEntityTypeConfiguration<ApplicationRoleEntity>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleEntity> builder)
    {
        builder.ToTable("IdentityRoles");
    }
}