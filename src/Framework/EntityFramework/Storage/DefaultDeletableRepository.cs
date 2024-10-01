using System.Linq.Expressions;
using AuroraScienceHub.Framework.Entities;
using AuroraScienceHub.Framework.Entities.Identifiers;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Framework.Entities;

namespace TelegramBot.Framework.EntityFramework.Storage;

public class DefaultDeletableRepository<TContext, TEntity, TEntityId> :
    DefaultRepository<TContext, TEntity, TEntityId>,
    IDeletableRepository<TEntity, TEntityId>
    where TContext : DbContext
    where TEntity : class, IEntity<TEntityId>, ISoftDeletable
    where TEntityId : class, IIdentifier
{
    protected DefaultDeletableRepository(TContext context, Func<IQueryable<TEntity>, IQueryable<TEntity>>? sort)
        : base(context, sort, DefaultFilter)
    { }

    private static IQueryable<TEntity> DefaultFilter(IQueryable<TEntity> queryable)
    {
        return queryable.Where(repository => repository.DeletedAt == null);
    }

    public Task<IReadOnlyList<TEntity>> GetAllWithDeletedAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return DbSet
            .Where(predicate)
            .ToReadOnlyListAsync(cancellationToken);
    }
}
