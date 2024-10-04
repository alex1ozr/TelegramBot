using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Bot;

/// <summary>
/// Unknown callback command
/// </summary>
public sealed record UnknownCallbackCommand(
    MaybeInaccessibleMessage Message,
    UserInfo UserInfo,
    params string[] Arguments)
    : ICallbackCommand
{
    public static string CommandName => "unknown";
}
