namespace TelegramBot.Framework.Entities;

/// <summary>
/// Domain entity with soft delete support
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Date and time when entity was deleted
    /// </summary>
    DateTimeOffset? DeletedAt { get; }
}
