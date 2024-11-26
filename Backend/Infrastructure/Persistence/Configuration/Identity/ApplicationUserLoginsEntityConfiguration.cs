using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity;

public class ApplicationUserLoginsEntityConfiguration : IEntityTypeConfiguration<ApplicationUserLoginsEntity>
{
    public void Configure(EntityTypeBuilder<ApplicationUserLoginsEntity> builder)
    {
        builder.ToTable("IdentityUserLogins");
        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });
    }
}