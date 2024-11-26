using Application.Common;
using Domain.Saas;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class SaasContext : DbContext, ISaasContext
{
    public SaasContext(DbContextOptions<SaasContext> options) : base(options)
    {
    }

    public DbSet<TenantEntity> Tenants { get; set; }
    
    public async Task<bool> SaveChangesAsync()
    {
        var changes = await base.SaveChangesAsync();
        return changes > 0;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(SaasContext).Assembly);
    }
}