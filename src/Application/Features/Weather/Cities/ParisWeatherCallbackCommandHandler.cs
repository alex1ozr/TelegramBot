﻿using MediatR;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using TelegramBot.Application.Features.Weather.DataProvider;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Features.Weather.Cities;

internal sealed class ParisWeatherCallbackCommandHandler :
    IRequestHandler<ParisWeatherCallbackCommand, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotMessageLocalizer _botMessageLocalizer;
    private readonly IWeatherProvider _weatherProvider;

    public ParisWeatherCallbackCommandHandler(
        ITelegramBotClient telegramBotClient,
        IBotMessageLocalizer botMessageLocalizer,
        IWeatherProvider weatherProvider)
    {
        _telegramBotClient = telegramBotClient;
        _botMessageLocalizer = botMessageLocalizer;
        _weatherProvider = weatherProvider;
    }

    public async Task<Unit> Handle(ParisWeatherCallbackCommand request,
        CancellationToken cancellationToken)
    {
        var message = request.Message;

        var temperature = _weatherProvider.GetTemperature("Paris");

        var city = _botMessageLocalizer.GetLocalizedString(
            nameof(BotMessages.ParisCity),
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
