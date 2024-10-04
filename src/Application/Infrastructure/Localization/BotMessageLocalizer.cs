using Microsoft.Extensions.Localization;
using TelegramBot.Application.Extensions;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Infrastructure.Localization;

internal sealed class BotMessageLocalizer : IBotMessageLocalizer
{
    public LocalizedString GetLocalizedString(string key, BotLanguage language)
    {
        var result = BotMessages.ResourceManager.GetString(key, language.GetCultureInfo())
                     ?? BotMessages.ResourceManager.GetString(key)
                     ?? throw new InvalidOperationException($"Key '{key}' was not found in resources.");

        return new LocalizedString(key, result);
    }

    public LocalizedString GetLocalizedString(string key, BotLanguage language, params object?[] args)
    {
        var result = BotMessages.ResourceManager.GetString(key, language.GetCultureInfo())
                     ?? BotMessages.ResourceManager.GetString(key)
                     ?? throw new InvalidOperationException($"Key '{key}' was not found in resources.");

        return new LocalizedString(key, string.Format(result, args));
    }
}
