using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Framework.Entities;

namespace TelegramBot.Domain.Accounting.Roles;

/// <summary>
/// Role
/// </summary>
public sealed class Role :
    IEntity<RoleId>,
    IAuditable
{
    private Role(
        RoleId id,
        string name,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Name = name;
        NormalizedName = name.ToUpper();
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <inheritdoc />
    public RoleId Id { get; private set; }

    /// <summary>
    /// Name for this role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Normalized name for this role.
    /// </summary>
    public string NormalizedName { get; set; }

    /// <inheritdoc />
    public DateTime CreatedAt { get; private set; }

    /// <inheritdoc />
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Users with this role
    /// </summary>
    public IReadOnlyList<User> Users { get; private set; } = new List<User>();

    public static Role Create(string name)
    {
        var utcNow = TimeProvider.System.GetUtcNow().UtcDateTime;
        return new Role(
            RoleId.New(),
            name,
            createdAt: utcNow,
            updatedAt: utcNow);
    }
}
