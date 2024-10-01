using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Framework.EntityFramework;

public static class QueryableExtensions
{
    public static async Task<IReadOnlyList<TSource>> ToReadOnlyListAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken)
    {
        return await source.ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}
