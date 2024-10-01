using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Framework.EntityFramework.Identifiers;

/// <summary>
/// Value converter for identifiers
/// </summary>
public sealed class IdentifierValueConverter<T> : ValueConverter<T, string>
    where T : IIdentifier<T>
{
    public IdentifierValueConverter()
        : base(id => id.Value, state => Parse(state))
    {
    }

    private static T Parse(string state) => T.Parse(state);
}
