using System.Globalization;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Extensions;

internal static class BotLanguageExtensions
{
    private static readonly CultureInfo s_defaultCulture = new("en-US");
    private static readonly CultureInfo s_russianCulture = new("ru-RU");

    public static CultureInfo GetCultureInfo(this BotLanguage language)
    {
        return language switch
        {
            BotLanguage.Russian => s_russianCulture,
            _ => s_defaultCulture
        };
    }
}
