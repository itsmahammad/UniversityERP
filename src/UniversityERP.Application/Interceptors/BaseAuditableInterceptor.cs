using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UniversityERP.Application.Contexts;
using UniversityERP.Domain.Entities.Common;

namespace UniversityERP.Application.Interceptors;

internal class BaseAuditableInterceptor(IHttpContextAccessor _contextAccessor) : SaveChangesInterceptor
{

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpsertAuditableColumns(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpsertAuditableColumns(eventData);

        return base.SavingChanges(eventData, result);
    }


    private void UpsertAuditableColumns(DbContextEventData eventData)
    {
        if (eventData.Context is AppDbContext context)
        {

            var entries = context.ChangeTracker.Entries<BaseAuditableEntity>();
            var loggedUser = _contextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value ?? "System";

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = loggedUser;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = loggedUser; // You can replace this with the actual user

                        break;

                    case EntityState.Deleted:
                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = loggedUser;
                        entry.State = EntityState.Modified; // Mark as modified to update the IsDeleted flag
                        break;
                }
            }
        }
    }
}
