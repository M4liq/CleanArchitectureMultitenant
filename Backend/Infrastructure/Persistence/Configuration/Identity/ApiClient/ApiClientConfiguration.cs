using Domain.Identity.ApiClient;
using Infrastructure.Persistence.Configuration.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.Identity.ApiClient;

public class ApiClientConfiguration : BaseEntityConfiguration<ApiClientEntity>
{
    public override void Configure(EntityTypeBuilder<ApiClientEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ApiKey)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property<List<string>>("_allowedScopes")
            .HasColumnName("AllowedScopes")
            .HasConversion(
                v => string.Join(';', v),
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList())
            .IsRequired();
    }
}