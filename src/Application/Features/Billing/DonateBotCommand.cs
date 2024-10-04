using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Billing;

/// <summary>
/// Donate Telegram Stars to the bot
/// </summary>
public sealed record DonateBotCommand(Message Message, UserInfo UserInfo) : IBotCommand
{
    public static bool AllowGroups => false;

    public static string CommandName => "donate";
}
