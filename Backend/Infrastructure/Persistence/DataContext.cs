using Application.Common;
using Application.Common.Core;
using Domain.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public partial class DataContext : DbContext, IDataContext
{
   private readonly ICalendar _calendarService;
   private readonly ICurrentUser _currentUserService;

   public DataContext(
       DbContextOptions<DataContext> options,
       ITenantProvider tenantProvider,
       ICalendar calendarService,
       ICurrentUser currentUserService) : base(options)
   {
       _tenantProvider = tenantProvider;
       _calendarService = calendarService;
       _currentUserService = currentUserService;
   }

   public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
   {
       var currentUserId = GetCurrentUserId();
       var currentTenantId = GetCurrentTenantId();
       
       HandleEntityTracking(currentUserId, currentTenantId);
       ValidateEntities();

       return await base.SaveChangesAsync(cancellationToken);
   }
   
   public override int SaveChanges()
   {
       var currentUserId = GetCurrentUserId();
       var currentTenantId = GetCurrentTenantId();
       
       HandleEntityTracking(currentUserId, currentTenantId);
       ValidateEntities();

       return base.SaveChanges();
   }

   private void HandleEntityTracking(Guid currentUserId, Guid currentTenantId)
   {
       foreach (var entry in ChangeTracker.Entries<ITrackingEntity>().ToList())
       {
           switch (entry.State)
           {
               case EntityState.Added:
                   entry.Entity.SetCreationTracking(
                       currentUserId,
                       _calendarService.UtcNowOffset,
                       currentTenantId);
                   break;

               case EntityState.Modified:
                   if (entry.Property(nameof(ITrackingEntity.IsDeleted)).IsModified
                       && (entry.Entity).IsDeleted)
                   {
                       (entry.Entity).SetDeletionTracking(
                           currentUserId,
                           _calendarService.UtcNowOffset);
                   }
                   else
                   {
                       entry.Entity.SetModificationTracking(
                           currentUserId,
                           _calendarService.UtcNowOffset);
                   }
                   break;

               case EntityState.Deleted:
                   entry.State = EntityState.Modified;
                   (entry.Entity).SetDeletionTracking(
                       currentUserId,
                       _calendarService.UtcNowOffset);
                   break;
           }
       }
   }
}