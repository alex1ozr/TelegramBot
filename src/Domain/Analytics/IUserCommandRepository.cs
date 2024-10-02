using TelegramBot.Framework.Entities;

namespace TelegramBot.Domain.Analytics;

/// <summary>
/// User commands repository
/// </summary>
public interface IUserCommandRepository: IRepository<UserCommand, UserCommandId>
{
    /// <summary>
    /// Get top 10 commands executed by users for the specified period
    /// </summary>
    Task<IReadOnlyList<(string Command, int Count)>> GetTopCommandAsync(TimeSpan period, CancellationToken cancellationToken);

    /// <summary>
    /// Get top 10 users by number of commands executed for the specified period
    /// </summary>
    Task<IReadOnlyList<(string UserId, int CommandsExecuted)>> GetTopUsersAsync(TimeSpan period, CancellationToken cancellationToken);
}
