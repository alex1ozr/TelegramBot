using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Domain.Accounting.Roles;

public sealed class RoleId :
    EntityIdentifier<RoleId>,
    IIdentifierWithPrefix
{
    private RoleId(string value)
        : base(value)
    {
    }

    public static string Prefix { get; } = FormatPrefix("tb", "role");
}
