using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Bot;

/// <summary>
/// Privacy Policy Bot Command
/// </summary>
public sealed record PrivacyPolicyBotCommand(Message Message, UserInfo UserInfo) : IBotCommand
{
    public static string CommandName => "policy";
}
