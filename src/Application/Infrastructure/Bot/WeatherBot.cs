using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using TelegramBot.Application.Infrastructure.Bot.Commands;
using TelegramBot.Application.Infrastructure.Localization;

namespace TelegramBot.Application.Infrastructure.Bot;

internal sealed partial class WeatherBot : SimpleTelegramBotBase
{
    private static User? s_me;

    private readonly IServiceProvider _serviceProvider;
    private readonly IMediator _mediator;
    private readonly IBotCommandFactory _commandFactory;
    private readonly ApplicationMetrics _metrics;
    private readonly IBotMessageLocalizer _botMessageLocalizer;
    private readonly ILogger<WeatherBot> _logger;
    private readonly ITelegramBotClient _client;

    public WeatherBot(
        ILogger<WeatherBot> logger,
        IOptions<TelegramBotOptions> options,
        ITelegramBotClient client,
        IServiceProvider serviceProvider,
        IMediator mediator,
        IBotCommandFactory commandFactory,
        ApplicationMetrics metrics,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _logger = logger;
        _client = client;
        _serviceProvider = serviceProvider;
        _mediator = mediator;
        _commandFactory = commandFactory;
        _metrics = metrics;
        _botMessageLocalizer = botMessageLocalizer;

        s_me ??= client.GetMe();
    }

    public string Username => s_me!.Username!;
}
