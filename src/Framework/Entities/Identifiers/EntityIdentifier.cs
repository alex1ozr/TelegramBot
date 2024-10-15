using System.Diagnostics.CodeAnalysis;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Framework.Entities.Identifiers;

/// <summary>
/// Identifier value object
/// </summary>
public abstract class EntityIdentifier<TId> : IIdentifier<TId>
    where TId : EntityIdentifier<TId>, IIdentifierWithPrefix
{
    protected EntityIdentifier(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;

    /// <summary>
    /// Generate a new unique identifier
    /// </summary>
    public static TId New()
    {
        var newId = IdGenerator.NewId(TId.Prefix);
        return Parse(newId, null);
    }

    /// <inheritdoc />
    public static TId Parse(string? text) => Parse(text, null);

    /// <inheritdoc />
    public static TId Parse(string? text, IFormatProvider? provider)
    {
        return TryParse(text, provider, out var id)
            ? id
            : throw new FormatException($"Wrong identifier format for {typeof(TId).Name}: {text}");
    }

    /// <inheritdoc />
    public static bool TryParse([NotNullWhen(true)] string? text,
        [NotNullWhen(true)] [MaybeNullWhen(false)]
        out TId result)
        => TryParse(text, null, out result);

    /// <inheritdoc />
    public static bool TryParse(
        [NotNullWhen(true)] string? text,
        IFormatProvider? provider,
        [NotNullWhen(true)] [MaybeNullWhen(false)]
        out TId result)
    {
        text = text?.Trim();

        if (string.IsNullOrWhiteSpace(text) || !text.StartsWith(TId.Prefix))
        {
            result = default;
            return false;
        }

        result = GenericActivator.Create<string, TId>(text);
        return true;
    }

    /// <summary>
    /// Format the prefix for the entity identifier
    /// </summary>
    protected static string FormatPrefix(string serviceId, string entityType)
    {
        return $"{serviceId}-{entityType}-";
    }

    #region Equatable Members

    /// <inheritdoc />
    public bool Equals(TId? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as TId);

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(EntityIdentifier<TId>? left, EntityIdentifier<TId>? right) => Equals(left, right);

    public static bool operator !=(EntityIdentifier<TId>? left, EntityIdentifier<TId>? right) => !Equals(left, right);

    #endregion Equatable Members
}
