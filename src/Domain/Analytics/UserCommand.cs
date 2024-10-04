using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Framework.Entities;

namespace TelegramBot.Domain.Analytics;

/// <summary>
/// Executed command by user.
/// </summary>
public sealed class UserCommand :
    IEntity<UserCommandId>,
    IAuditable
{
    private UserCommand(
        UserCommandId id,
        UserId userId,
        string commandName,
        DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        CommandName = commandName;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    /// <inheritdoc />
    public UserCommandId Id { get; private set; }

    /// <summary>
    /// User ID that executed the command.
    /// </summary>
    public UserId UserId { get; set; }

    /// <summary>
    /// Command name.
    /// </summary>
    public string CommandName { get; set; }

    /// <inheritdoc />
    public DateTime CreatedAt { get; private set; }

    /// <inheritdoc />
    public DateTime UpdatedAt { get; private set; }

    public static UserCommand Create(UserId userId, string name)
    {
        var utcNow = TimeProvider.System.GetUtcNow().UtcDateTime;
        return new UserCommand(
            UserCommandId.New(),
            userId,
            name.ToLowerInvariant(),
            createdAt: utcNow);
    }
}
