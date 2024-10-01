using System.Diagnostics.CodeAnalysis;

namespace AuroraScienceHub.Framework.Entities.Identifiers;

/// <summary>
/// Identifier
/// </summary>
public interface IIdentifier
{
    /// <summary>
    /// The value of the identifier
    /// </summary>
    string Value { get; }
}

/// <summary>
/// Identifier with prefix
/// </summary>
public interface IIdentifierWithPrefix : IIdentifier
{
    /// <summary>
    /// Prefix for the identifier
    /// </summary>
    static abstract string Prefix { get; }
}

/// <summary>
/// Identifier with concrete type
/// </summary>
public interface IIdentifier<TId> :
    IIdentifier,
    IParsable<TId>,
    IEquatable<TId>
    where TId : IIdentifier<TId>
{
    /// <summary>Parses a string into a value.</summary>
    /// <param name="text">The string to parse.</param>
    /// <returns>The result of parsing <paramref name="text" />.</returns>
    static abstract TId Parse(string? text);

    /// <summary>Tries to parse a string into a value.</summary>
    /// <param name="text">The string to parse.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="text" /> or an undefined value on failure.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="text" /> was successfully parsed; otherwise <see langword="false" />.
    /// </returns>
    static abstract bool TryParse(
        [NotNullWhen(true)] string? text,
        [NotNullWhen(true)][MaybeNullWhen(false)] out TId result);
}
