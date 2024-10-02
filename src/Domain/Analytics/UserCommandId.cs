using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Domain.Analytics;

public sealed class UserCommandId :
    EntityIdentifier<UserCommandId>,
    IIdentifierWithPrefix
{
    private UserCommandId(string value)
        : base(value)
    {
    }

    public static string Prefix { get; } = FormatPrefix("tb", "usrcmd");
}
