using Domain.Identity.ApiClient;
using Domain.Identity.RefreshToken;
using Domain.Identity.Tenant;
using Domain.Identity.User;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

public interface IDataContext
{
    DbSet<ApiClientEntity> ApiClients { get; set; }
    DbSet<ApplicationRefreshTokensEntity> RefreshTokens { get; set; }
    DbSet<TenantEntity> Tenants { get; set; }
    
    DbSet<UserEntity> Users { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}