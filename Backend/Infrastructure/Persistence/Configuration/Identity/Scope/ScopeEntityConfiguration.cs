using Domain.Identity.Scope;
using Infrastructure.Persistence.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity.Scope;

public class ApiClientScopeConfiguration : BaseEntityConfiguration<ScopeEntity>
{
    public override void Configure(EntityTypeBuilder<ScopeEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("Scopes");

        builder.Property(x => x.ScopeValue)
            .IsRequired()
            .HasMaxLength(100);
    }
}
