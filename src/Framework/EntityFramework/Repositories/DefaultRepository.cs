using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Framework.Entities;
using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Framework.EntityFramework.Repositories;

public class DefaultRepository<TContext, TEntity, TEntityId> :
    IRepository<TEntity, TEntityId>
    where TContext : DbContext
    where TEntity : class, IEntity<TEntityId>
    where TEntityId : class, IIdentifier
{
    protected readonly TContext Context;

    protected readonly DbSet<TEntity> DbSet;

    private readonly Func<IQueryable<TEntity>, IQueryable<TEntity>>? _sort;
    private readonly Func<IQueryable<TEntity>, IQueryable<TEntity>>? _filter;

    public DefaultRepository(
        TContext context,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? sort = default,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = default)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
        _sort = sort;
        _filter = filter;
    }

    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Read()
            .FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Read()
            .Where(predicate)
            .ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        return Context.SaveChangesAsync(cancellationToken);
    }

    protected IQueryable<TEntity> Read()
    {
        var query = DbSet.AsQueryable();

        query = _filter is null ? query : _filter(query);
        query = _sort is null ? query : _sort(query);

        return query;
    }
}
