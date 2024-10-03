using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Framework.Entities;

public static class RepositoryExtensions
{
    public static async Task<TEntity> GetByIdAsync<TEntity, TEntityId>(
        this IRepository<TEntity, TEntityId> repository,
        TEntityId id,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : class, IIdentifier
    {
        var entity = await repository.GetByIdOrDefaultAsync(id, cancellationToken).ConfigureAwait(false);

        return entity ?? throw new InvalidOperationException($"Entity with id {id} not found");
        // TODO
        //return EntityNotFoundException.ThrowIfNull(entity, $"Entity with id {id} not found");
    }

    public static Task<TEntity?> GetByIdOrDefaultAsync<TEntity, TEntityId>(
        this IRepository<TEntity, TEntityId> repository,
        TEntityId entityId,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : class, IIdentifier
    {
        return repository.GetFirstOrDefaultAsync(entity => entity.Id == entityId, cancellationToken);
    }

    public static Task<IReadOnlyList<TEntity>> GetByIdsAsync<TEntity, TEntityId>(
        this IRepository<TEntity, TEntityId> repository,
        IReadOnlyList<TEntityId> entityIds,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : class, IIdentifier
    {
        return repository.GetAllAsync(entity => entityIds.Contains(entity.Id), cancellationToken);
    }

    public static Task<IReadOnlyList<TEntity>> GetAllAsync<TEntity, TEntityId>(
        this IRepository<TEntity, TEntityId> repository,
        CancellationToken cancellationToken)
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : class, IIdentifier
    {
        return repository.GetAllAsync(entity => true, cancellationToken);
    }
}
