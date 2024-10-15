using TelegramBot.Framework.Entities.Identifiers;
using TelegramBot.Framework.Exceptions;

namespace TelegramBot.Framework.Entities;

/// <summary>
/// Extensions for <see cref="IRepository{TEntity,TEntityId}"/>
/// </summary>
public static class RepositoryExtensions
{
    /// <summary>
    /// Get entity by id or throw <see cref="EntityNotFoundException"/>
    /// </summary>
    public static async Task<TEntity> GetByIdAsync<TEntity, TEntityId>(
        this IRepository<TEntity, TEntityId> repository,
        TEntityId id,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : class, IIdentifier
    {
        var entity = await repository.GetByIdOrDefaultAsync(id, cancellationToken).ConfigureAwait(false);

        return EntityNotFoundException.ThrowIfNull(entity, $"Entity with id {id} wasn't found");
    }

    /// <summary>
    /// Get entity by id or default value
    /// </summary>
    public static Task<TEntity?> GetByIdOrDefaultAsync<TEntity, TEntityId>(
        this IRepository<TEntity, TEntityId> repository,
        TEntityId entityId,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : class, IIdentifier
    {
        return repository.GetFirstOrDefaultAsync(entity => entity.Id == entityId, cancellationToken);
    }
}
