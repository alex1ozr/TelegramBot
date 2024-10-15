using System.Linq.Expressions;
using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Framework.Entities;

/// <summary>
/// Repository for domain entities
/// </summary>
public interface IRepository<TEntity, TEntityId>
    where TEntity : class, IEntity<TEntityId>
    where TEntityId : IIdentifier
{
    /// <summary>
    /// Get first or default entity by predicate
    /// </summary>
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Get all entities by predicate
    /// </summary>
    Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Update entity
    /// </summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Add entity to repository
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
}
