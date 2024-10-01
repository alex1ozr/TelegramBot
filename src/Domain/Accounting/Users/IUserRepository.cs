using AuroraScienceHub.Framework.Entities.Storage;

namespace TelegramBot.Domain.Accounting.Users;

public interface IUserRepository : IRepository<User, UserId>
{
    /// <summary>
    /// Get user by telegram user id
    /// </summary>
    public Task<User?> GetByTelegramUserIdAsync(
        string telegramUserId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get last registered users since the specified date and time (UTC)
    /// </summary>
    public Task<IReadOnlyList<User>> GetLastRegisteredUsersAsync(
        DateTime from,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get total number of users
    /// </summary>
    public Task<int> GetTotalUsersAsync(CancellationToken cancellationToken);
}
