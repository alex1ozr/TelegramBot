using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TelegramBot.Framework.Entities;

namespace TelegramBot.Framework.EntityFramework.Interceptors;

/// <summary>
/// Interceptor for saving changes to implement soft deletion
/// </summary>
public sealed class DeletableInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _timeProvider;

    public DeletableInterceptor(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        var deletedEntities = eventData
            .Context?
            .ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(entry => entry is { State: EntityState.Deleted, Entity.DeletedAt: not null })
            .ToList();

        if (deletedEntities is null || !deletedEntities.Any())
        {
            return ValueTask.FromResult(result);
        }

        var deletedAt = _timeProvider.GetUtcNow().UtcDateTime;

        foreach (var deletableEntity in deletedEntities)
        {
            deletableEntity.State = EntityState.Modified;
            deletableEntity.Property(entity => entity.DeletedAt).CurrentValue = deletedAt;
        }

        return ValueTask.FromResult(result);
    }
}
