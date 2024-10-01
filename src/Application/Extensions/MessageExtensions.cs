using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Resources;
using TelegramBot.Domain.Accounting;

namespace TelegramBot.Application.Extensions;

internal static class MessageExtensions
{
    public static BotLanguage GetUserLanguage(this Message message)
    {
        return message.From?.LanguageCode switch
        {
            LanguageCodes.Russian => BotLanguage.Russian,
            _ => BotLanguage.English
        };
    }
}
