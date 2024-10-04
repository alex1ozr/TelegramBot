using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Domain.Accounting.Users;

public sealed class UserId :
    EntityIdentifier<UserId>,
    IIdentifierWithPrefix
{
    private UserId(string value)
        : base(value)
    {
    }

    public static string Prefix { get; } = FormatPrefix("tb", "user");
}
