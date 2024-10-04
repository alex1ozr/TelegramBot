using TelegramBot.Domain.Accounting.Roles;
using TelegramBot.Domain.Billing.Invoices;
using TelegramBot.Framework.Entities;

namespace TelegramBot.Domain.Accounting.Users;

/// <summary>
/// Telegram user
/// </summary>
public sealed class User :
    IEntity<UserId>,
    IAuditable
{
    private User(
        UserId id,
        string telegramUserId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        TelegramUserId = telegramUserId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <inheritdoc />
    public UserId Id { get; private set; }

    /// <summary>
    /// Telegram user name
    /// </summary>
    public string? UserName { get; private set; }

    /// <summary>
    /// Telegram user id
    /// </summary>
    public string TelegramUserId { get; private set; }

    /// <summary>
    /// User language, <a href="https://en.wikipedia.org/wiki/IETF_language_tag">IETF language tag</a>
    /// </summary>
    public string? Language { get; private set; }

    /// <summary>
    /// Whether to automatically detect the language for the user
    /// </summary>
    public bool AutoDetectLanguage { get; private set; } = true;

    /// <inheritdoc />
    public DateTime CreatedAt { get; private set; }

    /// <inheritdoc />
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Invoices
    /// </summary>
    public IReadOnlyList<Invoice> Invoices { get; private set; } = new List<Invoice>();

    /// <summary>
    /// User roles
    /// </summary>
    public IReadOnlyList<Role> Roles { get; private set; } = new List<Role>();

    public static User Create(
        string telegramUserId,
        string? userName = null)
    {
        var utcNow = TimeProvider.System.GetUtcNow().UtcDateTime;
        return new User(
            UserId.New(),
            telegramUserId,
            createdAt: utcNow,
            updatedAt: utcNow)
        { UserName = userName };
    }

    public void SetUserName(string value)
    {
        UserName = value;
    }

    public void SetLanguage(string value)
    {
        Language = value;
    }

    public void SetAutoDetectLanguage(bool value)
    {
        AutoDetectLanguage = value;
    }

    public bool IsInRole(string roleName) =>
        Roles.Any(x => string.Equals(x.NormalizedName, roleName, StringComparison.OrdinalIgnoreCase));

    public bool IsAdmin => IsInRole(RoleNames.Admin);
}
