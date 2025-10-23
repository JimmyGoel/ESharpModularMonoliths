using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.Interceptor
{
    public static class Extention
    {
        public static bool HasChangedOwnEntities(this EntityEntry entry) =>
         entry.References.Any(r =>
             r.TargetEntry != null &&
             r.TargetEntry.Metadata.IsOwned() &&
             (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified)
         );
    }
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null) return;
            foreach (var entry in context.ChangeTracker.Entries<IEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "Jimmy Goel";
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnEntities())
                {
                    entry.Entity.LastModifiedBy = "Jimmy Goel";
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }

        }

    }

}
