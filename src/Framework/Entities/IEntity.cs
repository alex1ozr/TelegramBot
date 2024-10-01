using AuroraScienceHub.Framework.Entities.Identifiers;

namespace AuroraScienceHub.Framework.Entities;

/// <summary>
/// Domain entity
/// </summary>
/// <typeparam name="TEntityId"></typeparam>
public interface IEntity<out TEntityId>
    where TEntityId : IIdentifier
{
    /// <summary>
    /// Entity identifier
    /// </summary>
    TEntityId Id { get; }
}
