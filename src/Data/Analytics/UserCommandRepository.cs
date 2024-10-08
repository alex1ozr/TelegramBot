using Microsoft.EntityFrameworkCore;
using TelegramBot.Data.Engine;
using TelegramBot.Domain.Analytics;
using TelegramBot.Framework.EntityFramework.Repositories;

namespace TelegramBot.Data.Analytics;

internal sealed class UserCommandRepository :
    DefaultRepository<DataContext, UserCommand, UserCommandId>,
    IUserCommandRepository
{
    private readonly TimeProvider _timeProvider;

    public UserCommandRepository(DataContext context, TimeProvider timeProvider)
        : base(context)
    {
        _timeProvider = timeProvider;
    }

    public async Task<IReadOnlyList<(string Command, int Count)>> GetTopCommandAsync(
        TimeSpan period,
        CancellationToken cancellationToken)
    {
        var fromMoment = _timeProvider.GetUtcNow().UtcDateTime - period;

        var topCommands = await Context.UserCommands
            .Where(x => x.CreatedAt >= fromMoment)
            .GroupBy(x => x.CommandName)
            .Select(x => new { Command = x.Key, Count = x.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return topCommands.Select(x => (x.Command, x.Count)).ToList();
    }

    public async Task<IReadOnlyList<(string UserId, int CommandsExecuted)>> GetTopUsersAsync(TimeSpan period,
        CancellationToken cancellationToken)
    {
        var fromMoment = _timeProvider.GetUtcNow().UtcDateTime - period;

        var topUsers = await Context.UserCommands
            .Where(x => x.CreatedAt >= fromMoment)
            .GroupBy(x => x.UserId)
            .Select(x => new { UserId = x.Key, Count = x.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return topUsers.Select(x => (x.UserId.ToString(), x.Count)).ToList();
    }
}
