using System.Reflection;
using Domain.Common.Base;
using Domain.Identity.Tenant;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

//DataContext.Multitenancy
public partial class DataContext
{
   public DbSet<TenantEntity> Tenants { get; set; }
   
   protected override void OnModelCreating(ModelBuilder builder)
   {
       builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
       ConfigureTenantFilters(builder);
   }
   
   private void ConfigureTenantFilters(ModelBuilder builder)
   {
       foreach (var entityType in builder.Model.GetEntityTypes())
       {
           var type = entityType.ClrType;
           ValidateEntityType(type);
           
           if (typeof(BaseEntity).IsAssignableFrom(type) && type != typeof(TenantEntity))
           {
               var method = typeof(DataContext)
                   .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)
                   ?.MakeGenericMethod(type);
               method?.Invoke(this, new object[] { builder });
           }
       }
   }

   private void ValidateEntities()
   {
       foreach (var entry in ChangeTracker.Entries())
       {
           var type = entry.Entity.GetType();
           ValidateEntityType(type);
           
           if (entry.Entity is BaseEntity entity)
           {
               ValidateEntityTenant(entity, entry.State, type);
           }
       }
   }

   private void ValidateEntityType(Type type)
   {
       var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
       var isBaseEntity = typeof(BaseEntity).IsAssignableFrom(type);
       var isTenantEntity = type == typeof(TenantEntity);

       if (!isValueObject && !isBaseEntity && !isTenantEntity)
       {
           throw new InvalidOperationException(
               $"Entity {type.Name} must inherit from BaseEntity, be TenantEntity, or be a ValueObject");
       }
   }

   private void ValidateEntityTenant(BaseEntity entity, EntityState state, Type type)
   {
       var currentTenantId = GetCurrentTenantId();
       if (state == EntityState.Added && entity.TenantId != currentTenantId)
       {
           throw new InvalidOperationException($"Cannot add {type.Name} for different tenant");
       }
   }

   private void SetTenantFilter<T>(ModelBuilder builder) where T : BaseEntity
   {
       builder.Entity<T>().HasQueryFilter(e =>
           (_bypassTenantFilter || e.TenantId == _tenantProvider.TenantId) &&
           !e.IsDeleted);
   }

   private Guid GetCurrentTenantId() => 
       _systemContext?.TenantId ?? _tenantProvider.TenantId ?? 
       throw new InvalidOperationException("Tenant must be specified");

   private Guid GetCurrentUserId() =>
       _systemContext?.UserId ?? _currentIdentityService.IdentityId;

   private record SystemContext(Guid TenantId, Guid UserId);
}