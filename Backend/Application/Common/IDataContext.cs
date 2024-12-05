using Domain.Common.Base;
using Domain.Identity.Tenant;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

public interface IDataContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

    DbSet<TenantEntity> Tenants { get; set; }
    public IDisposable UseSystemContext();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}