using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity;

public class ApplicationUsersEntityConfiguration : IEntityTypeConfiguration<ApplicationUserEntity>

{
    public void Configure(EntityTypeBuilder<ApplicationUserEntity> builder)
    {
        builder.ToTable("IdentityUsers");
    }
}