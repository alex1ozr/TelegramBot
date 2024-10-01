using System.Linq.Expressions;
using AuroraScienceHub.Framework.Entities;
using AuroraScienceHub.Framework.Entities.Identifiers;
using AuroraScienceHub.Framework.Entities.Storage;

namespace TelegramBot.Framework.Entities;

public interface IDeletableRepository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
    where TEntity : class, IEntity<TEntityId>, ISoftDeletable
    where TEntityId : IIdentifier
{
    Task<IReadOnlyList<TEntity>> GetAllWithDeletedAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
}
