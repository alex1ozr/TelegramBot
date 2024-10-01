using TelegramBot.Application.Resources;
using TelegramBot.Domain.Accounting;
using TelegramBot.Domain.Accounting.Users;

namespace TelegramBot.Application.Extensions;

internal static class UserExtensions
{
    public static BotLanguage GetBotLanguage(this User user)
    {
        return user.Language switch
        {
            LanguageCodes.Russian => BotLanguage.Russian,
            _ => BotLanguage.English
        };
    }
}
