using Domain.Identity.ApiClient;
using Domain.Identity.RefreshToken;
using Domain.Identity;
using Domain.Identity.UserIdentity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

//DataContext.Identity
public partial class DataContext 
{
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<ApiClientEntity> ApiClients { get; set; }
    public DbSet<IdentityEntity> Users { get; set; }
}