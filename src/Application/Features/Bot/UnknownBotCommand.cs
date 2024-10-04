using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Bot;

/// <summary>
/// Unknown command
/// </summary>
public sealed record UnknownBotCommand(Message Message, UserInfo UserInfo) : IBotCommand
{
    public static string CommandName => "unknown";
}
