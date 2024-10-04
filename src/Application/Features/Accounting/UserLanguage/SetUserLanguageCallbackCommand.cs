using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Accounting.UserLanguage;

/// <summary>
/// Set the user language (callback)
/// </summary>
public sealed record SetUserLanguageCallbackCommand(
    MaybeInaccessibleMessage Message,
    UserInfo UserInfo,
    params string[] Arguments)
    : ICallbackCommand
{
    public static string CommandName => "set_language";
}
