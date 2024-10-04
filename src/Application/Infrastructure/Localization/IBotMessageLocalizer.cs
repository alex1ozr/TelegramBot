using Microsoft.Extensions.Localization;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Infrastructure.Localization;

/// <summary>
/// Bot message localizer.
/// </summary>
public interface IBotMessageLocalizer
{
    /// <summary>
    /// Get a localized string by key and language.
    /// </summary>
    LocalizedString GetLocalizedString(string key, BotLanguage language);

    /// <summary>
    /// Get a localized string by key, language and string interpolation arguments.
    /// </summary>
    LocalizedString GetLocalizedString(string key, BotLanguage language, params object?[] args);
}
