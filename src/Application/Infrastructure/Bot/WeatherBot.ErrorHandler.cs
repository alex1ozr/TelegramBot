using Microsoft.Extensions.Logging;
using Telegram.BotAPI;

namespace TelegramBot.Application.Infrastructure.Bot;

partial class WeatherBot
{
    protected override Task OnBotExceptionAsync(BotRequestException exp, CancellationToken cancellationToken)
    {
        _logger.LogError("BotRequestException: {Message}", exp.Message);
        return Task.CompletedTask;
    }

    protected override Task OnExceptionAsync(Exception exp, CancellationToken cancellationToken)
    {
        _logger.LogError("Exception: {Message}", exp.Message);
        return Task.CompletedTask;
    }
}
