using System.Linq.Expressions;
using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Framework.Entities;

public interface IRepository<TEntity, TEntityId>
    where TEntity : class, IEntity<TEntityId>
    where TEntityId : IIdentifier
{
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
}
