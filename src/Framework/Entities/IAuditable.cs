namespace TelegramBot.Framework.Entities;

/// <summary>
/// Domain object with audit fields
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Date-time of creation (UTC)
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// Date-time of the last update (UTC)
    /// </summary>
    DateTime UpdatedAt { get; }
}
