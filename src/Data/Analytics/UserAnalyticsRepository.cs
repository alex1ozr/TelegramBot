using ClickHouse.Client;
using Dapper;
using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Domain.Analytics;

namespace TelegramBot.Data.Analytics;

internal sealed class UserAnalyticsRepository : IUserAnalyticsRepository
{
    private const string TableName = "SpaceWeather.tbot_user_commands";

    private readonly IClickHouseConnection _clickHouseConnection;
    private readonly TimeProvider _timeProvider;

    public UserAnalyticsRepository(
        IClickHouseConnection clickHouseConnection,
        TimeProvider timeProvider)
    {
        _clickHouseConnection = clickHouseConnection;
        _timeProvider = timeProvider;
    }

    public async Task SaveUserCommandAsync(
        UserId userId,
        string command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        var userIdString = userId.ToString();

        await _clickHouseConnection.ExecuteAsync(
                $"INSERT INTO {TableName} (user_id, command, timestamp) VALUES (@userIdString, @command, @utcNow)",
                new { userIdString, command, utcNow })
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<(string Command, int Count)>> GetTopCommandAsync(
        TimeSpan period,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        var from = utcNow - period;

        var result = await _clickHouseConnection.QueryAsync<(string Command, int Count)>(
                $"SELECT command, count() as count FROM {TableName} WHERE timestamp >= @from GROUP BY command ORDER BY count DESC LIMIT 10",
                new { from })
            .ConfigureAwait(false);

        return result.ToList();
    }

    public async Task<IReadOnlyList<(string UserId, int CommandsExecuted)>> GetTopUsersAsync(
        TimeSpan period,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        var from = utcNow - period;

        var result = await _clickHouseConnection.QueryAsync<(string UserId, int CommandsExecuted)>(
                $"SELECT user_id, count() as count FROM {TableName} WHERE timestamp >= @from GROUP BY user_id ORDER BY count DESC LIMIT 10",
                new { from })
            .ConfigureAwait(false);

        return result.ToList();
    }
}
