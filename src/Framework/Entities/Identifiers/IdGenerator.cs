using TelegramBot.Framework.Utilities.Encoding;

namespace TelegramBot.Framework.Entities.Identifiers;

public static class IdGenerator
{
    private const char PrefixSeparator = '-';

    public static string NewId(string prefix)
    {
        if (!prefix.EndsWith(PrefixSeparator))
        {
            prefix += PrefixSeparator;
        }

        return prefix + NewId();
    }

    private static string NewId()
    {
        var uniqueId = Guid.NewGuid();
        var encoded = Base64UrlEncoder.Encode(uniqueId.ToByteArray());
        return encoded;
    }
}
