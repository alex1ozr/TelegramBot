using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Framework.EntityFramework.Identifiers;

/// <summary>
/// Value converter for identifiers
/// </summary>
public sealed class NullableIdentifierValueConverter<T> : ValueConverter<T?, string?>
    where T : IIdentifier<T>
{
    public NullableIdentifierValueConverter()
        : base(id => id != null ? id.Value : null,
            state => SafeParse(state))
    {
    }

    private static T? SafeParse(string? text) => T.TryParse(text, out var result) ? result : default;
}
