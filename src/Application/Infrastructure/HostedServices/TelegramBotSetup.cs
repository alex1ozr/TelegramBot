using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.GettingUpdates;
using TelegramBot.Application.Features.Bot;
using TelegramBot.Application.Features.Weather;
using TelegramBot.Application.Infrastructure.Bot;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;
using TelegramBot.Domain.Accounting;

namespace TelegramBot.Application.Infrastructure.HostedServices;

internal sealed class TelegramBotSetup : IHostedService
{
    private readonly IBotMessageLocalizer _botMessageLocalizer;
    private readonly ILogger<TelegramBotSetup> _logger;
    private readonly ITelegramBotClient _client;
    private readonly IOptions<TelegramBotOptions> _options;

    public TelegramBotSetup(
        ILogger<TelegramBotSetup> logger,
        ITelegramBotClient client,
        IOptions<TelegramBotOptions> options,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _logger = logger;
        _client = client;
        _options = options;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await SetCommands(cancellationToken).ConfigureAwait(false);

        // Delete the previous webhook if it is configured.
        await _client.DeleteWebhookAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        var webhookUrl = _options.Value.WebhookUrl;
        var secretToken = _options.Value.SecretToken;

        // Setup the webhook if it is configured.
        if (!string.IsNullOrEmpty(webhookUrl) && !string.IsNullOrEmpty(secretToken))
        {
            _logger.LogInformation("Setting up the webhook");
            await _client.SetWebhookAsync(
                    webhookUrl,
                    secretToken: secretToken,
                    cancellationToken: cancellationToken
                )
                .ConfigureAwait(false);
            _logger.LogInformation("Webhook set up successfully at {Url}", webhookUrl);
        }

        _logger.LogInformation("Setup completed successfully");
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task SetCommands(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Setting up the bot commands");
        await _client.DeleteMyCommandsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        // default (en)
        await _client.SetMyCommandsAsync(
            [
                new(WeatherBotCommand.CommandName,
                    _botMessageLocalizer
                        .GetLocalizedString(nameof(BotMessages.WeatherCommandDescription), BotLanguage.English)),
                new(HelpBotCommand.CommandName,
                    _botMessageLocalizer
                        .GetLocalizedString(nameof(BotMessages.HelpCommandDescription), BotLanguage.English)),
            ],
            cancellationToken: cancellationToken).ConfigureAwait(false);

        // ru
        await _client.SetMyCommandsAsync(
            [
                new(WeatherBotCommand.CommandName,
                    _botMessageLocalizer
                        .GetLocalizedString(nameof(BotMessages.WeatherCommandDescription), BotLanguage.Russian)),
                new(HelpBotCommand.CommandName,
                    _botMessageLocalizer
                        .GetLocalizedString(nameof(BotMessages.HelpCommandDescription), BotLanguage.Russian)),
            ],
            languageCode: LanguageCodes.Russian,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Commands registered successfully");
    }
}
