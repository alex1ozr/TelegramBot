using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;
using TelegramBot.Domain.Accounting.Roles;

namespace TelegramBot.Application.Features.Analytics;

/// <summary>
/// Statistics command
/// </summary>
public sealed record StatsBotCommand(Message Message, UserInfo UserInfo) : IBotCommand
{
    public static string CommandName => "stats";

    public static IReadOnlyList<string> Roles { get; } = [RoleNames.Admin];
}
