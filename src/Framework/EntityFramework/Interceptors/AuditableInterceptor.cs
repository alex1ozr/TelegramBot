using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TelegramBot.Framework.Entities;

namespace TelegramBot.Framework.EntityFramework.Interceptors;

/// <summary>
/// Interceptor for saving changes to fill in the fields of auditable entities
/// </summary>
public sealed class AuditableInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;

    public AuditableInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    /// <inheritdoc />
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        var modifiedEntities = eventData
            .Context?
            .ChangeTracker
            .Entries<IAuditable>()
            .Where(entry => entry.State == EntityState.Modified)
            .ToList();

        if (modifiedEntities is null || !modifiedEntities.Any())
        {
            return ValueTask.FromResult(result);
        }

        var updatedAt = _timeProvider.GetUtcNow();

        foreach (var auditableEntity in modifiedEntities)
        {
            auditableEntity.Property(entity => entity.UpdatedAt).CurrentValue = updatedAt.UtcDateTime;
        }

        return ValueTask.FromResult(result);
    }
}
