using Microsoft.Extensions.Logging;
using Telegram.BotAPI;

namespace TelegramBot.Application.Infrastructure.Bot;

partial class SWeatherBot
{
    protected override Task OnBotExceptionAsync(BotRequestException exp, CancellationToken cancellationToken)
    {
        LoggerExtensions.LogError(_logger, "BotRequestException: {Message}", exp.Message);
        return Task.CompletedTask;
    }

    protected override Task OnExceptionAsync(Exception exp, CancellationToken cancellationToken)
    {
        LoggerExtensions.LogError(_logger, "Exception: {Message}", exp.Message);
        return Task.CompletedTask;
    }
}
