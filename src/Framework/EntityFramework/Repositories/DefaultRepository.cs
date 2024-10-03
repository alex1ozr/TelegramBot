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
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Read()
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateRangeAndDetachAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken)
    {
        if (!entities.Any())
        {
            return;
        }

        DbSet.UpdateRange(entities);
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        foreach (var entity in entities)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AddRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken)
    {
        if (!entities.Any())
        {
            return;
        }

        await DbSet.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task AddRangeAndDetachAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken)
    {
        if (!entities.Any())
        {
            return;
        }

        DbSet.AddRange(entities);
        await Context.SaveChangesAsync(cancellationToken);

        foreach (var entity in entities)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }
    }

    public Task RemoveRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken)
    {
        if (!entities.Any())
        {
            return Task.CompletedTask;
        }

        DbSet.RemoveRange(entities);
        return Context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        return Context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken)
    {
        if (!entities.Any())
        {
            return Task.CompletedTask;
        }

        DbSet.UpdateRange(entities);
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
