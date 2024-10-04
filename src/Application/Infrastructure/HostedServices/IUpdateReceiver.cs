using Microsoft.Extensions.Hosting;
using Telegram.BotAPI.GettingUpdates;

namespace TelegramBot.Application.Infrastructure.HostedServices;

/// <summary>
/// Defines a method to receive updates to be processed by the Telegram bot.
/// </summary>
public interface IUpdateReceiver : IHostedService
{
    /// <summary>
    /// Receives updates from the Telegram bot.
    /// </summary>
    /// <param name="update">The update.</param>
    public void ReceiveUpdate(Update update);
}
