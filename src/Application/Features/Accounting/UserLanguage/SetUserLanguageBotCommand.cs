using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Accounting.UserLanguage;

/// <summary>
/// Select and set the user language
/// </summary>
public sealed record SetUserLanguageBotCommand(Message Message, UserInfo UserInfo) : IBotCommand
{
    public static string CommandName => "set_language";

    /// <summary>
    /// Callback language options
    /// </summary>
    internal static class LanguageOptions
    {
        public const string Auto = "auto";

        public const string English = "en";

        public const string Russian = "ru";
    }
}
