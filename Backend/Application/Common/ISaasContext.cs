using Domain.Saas;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Common;

public interface ISaasContext
{
    DbSet<TenantEntity> Tenants { get; set; }
    
    Task<bool> SaveChangesAsync();
}