using Domain.Identity.ApiClient;
using Domain.Identity.RefreshToken;
using Domain.Identity.Tenant;
using Domain.Identity.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

//DataContext.Identity
public partial class DataContext 
{
    public DbSet<ApplicationRefreshTokensEntity> RefreshTokens { get; set; }
    public DbSet<ApiClientEntity> ApiClients { get; set; }
    public DbSet<UserEntity> Users { get; set; }
}