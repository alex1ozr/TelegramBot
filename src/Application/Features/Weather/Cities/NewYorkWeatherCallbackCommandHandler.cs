using MediatR;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Features.Weather.Cities;

internal sealed class NewYorkWeatherCallbackCommandHandler :
    IRequestHandler<NewYorkWeatherCallbackCommand, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotMessageLocalizer _botMessageLocalizer;

    public NewYorkWeatherCallbackCommandHandler(
        ITelegramBotClient telegramBotClient,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _telegramBotClient = telegramBotClient;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task<Unit> Handle(NewYorkWeatherCallbackCommand request,
        CancellationToken cancellationToken)
    {
        var message = request.Message;

        var temperature = Random.Shared.Next(-10, 40);

        var city = _botMessageLocalizer.GetLocalizedString(
            nameof(BotMessages.NewYorkCity),
            request.UserInfo.Language);

        var text = _botMessageLocalizer.GetLocalizedString(
            nameof(BotMessages.CurrentWeatherMessage),
            request.UserInfo.Language,
            city,
            temperature);

        await _telegramBotClient.SendMessageAsync(
                message.Chat.Id,
                text,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return Unit.Value;
    }
}
