using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Weather;

/// <summary>
/// Provide the Ovation Prime options to the user
/// </summary>
public sealed record WeatherBotCommand(Message Message, UserInfo UserInfo) : IBotCommand
{
    public static string CommandName => "weather";
}
