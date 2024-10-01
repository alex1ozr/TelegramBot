using TelegramBot.Domain.Accounting.Users;

namespace TelegramBot.Domain.Analytics;

/// <summary>
/// User analytics repository
/// </summary>
public interface IUserAnalyticsRepository
{
    /// <summary>
    /// Save information about command executed by user
    /// </summary>
    Task SaveUserCommandAsync(
        UserId userId,
        string command,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get top 10 commands executed by users for the specified period
    /// </summary>
    Task<IReadOnlyList<(string Command, int Count)>> GetTopCommandAsync(TimeSpan period, CancellationToken cancellationToken);

    /// <summary>
    /// Get top 10 users by number of commands executed for the specified period
    /// </summary>
    Task<IReadOnlyList<(string UserId, int CommandsExecuted)>> GetTopUsersAsync(TimeSpan period, CancellationToken cancellationToken);
}
