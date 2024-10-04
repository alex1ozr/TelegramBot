using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Bot;

/// <summary>
/// Help command
/// </summary>
public sealed record HelpBotCommand(Message Message, UserInfo UserInfo) : IBotCommand
{
    public static string CommandName => "help";
}
