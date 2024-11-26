using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity;

public class ApplicationUserTokensEntityConfiguration : IEntityTypeConfiguration<ApplicationUserTokensEntity>
{
    public void Configure(EntityTypeBuilder<ApplicationUserTokensEntity> builder)
    {
        builder.ToTable("IdentityUserTokens");
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
    }
}