namespace AuroraScienceHub.Framework.Entities;

/// <summary>
/// Domain object with audit fields
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Date-time of creation
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// Date-time of the last update
    /// </summary>
    DateTime UpdatedAt { get; }
}
