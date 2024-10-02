using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;

namespace TelegramBot.Application.Features.Weather.Cities;

public sealed record ParisWeatherCallbackCommand(
    MaybeInaccessibleMessage Message,
    UserInfo UserInfo,
    params string[] Arguments)
    : ICallbackCommand
{
    public static string CommandName => "weather_paris";
}
