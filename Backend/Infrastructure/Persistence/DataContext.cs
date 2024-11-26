using Application.Common;
using Application.Common.Interfaces;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

 public class DataContext : 
     IdentityDbContext<ApplicationUserEntity, ApplicationRoleEntity, Guid, ApplicationUserClaimsEntity,
     ApplicationUserRolesEntity, ApplicationUserLoginsEntity, ApplicationRoleClaimsEntity, ApplicationUserTokensEntity>, IDataContext
 {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DataContext()
        {
        }

        public DbSet<ApplicationRefreshTokensEntity> RefreshTokens { get; set; }
        
        public async Task<bool> SaveChangesAsync()
        {
             var changes = await base.SaveChangesAsync();
             return changes > 0;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }