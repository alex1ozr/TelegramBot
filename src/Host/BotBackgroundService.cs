using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;
using TelegramBot.Application.Infrastructure.HostedServices;

namespace TelegramBot.Host;

internal sealed class BotBackgroundService(
    ILogger<BotBackgroundService> logger,
    ITelegramBotClient client,
    IUpdateReceiver updateReceiver)
    : BackgroundService
{
    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IReadOnlyList<Update> updates = [];
        logger.LogInformation("Polling for updates...");
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!updates.Any())
            {
                // Wait 100 ms before polling again.
                await Task.Delay(100, stoppingToken);

                // Get updates from the bot API.
                updates = (await client.GetUpdatesAsync(cancellationToken: stoppingToken)).ToList();
                continue;
            }

            // Pass the updates to the update receiver.
            foreach (var update in updates)
            {
                updateReceiver.ReceiveUpdate(update);
            }

            // Wait 100 ms before polling again.
            await Task.Delay(100, stoppingToken);

            // Get offset for the next update.
            var offset = updates.Max(u => u.UpdateId) + 1;
            updates = (await client.GetUpdatesAsync(offset, cancellationToken: stoppingToken)).ToList();
        }
    }
}
