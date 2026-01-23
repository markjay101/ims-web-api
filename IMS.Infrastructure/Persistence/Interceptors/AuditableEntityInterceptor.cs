using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IMS.Infrastructure.Persistence.Interceptors
{
    internal class AuditableEntityInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
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
        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<IBaseAuditableEntity>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified)
                {
                    var utcNow = DateTime.UtcNow;
                    var userId = currentUserService.UserId;

                    if (entry.State == EntityState.Added)
                        entry.Entity.CreatedBy = userId;

                    entry.Entity.UpdatedBy = userId;
                    entry.Entity.UpdatedAt = utcNow;
                }
            }
        }
    }
}
