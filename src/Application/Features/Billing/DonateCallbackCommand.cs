using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Billing;

/// <summary>
/// Donate Telegram Stars to the bot (callback)
/// </summary>
public sealed record DonateCallbackCommand(
    MaybeInaccessibleMessage Message,
    UserInfo UserInfo,
    params string[] Arguments)
    : ICallbackCommand
{
    public static string CommandName => "donate";
}
