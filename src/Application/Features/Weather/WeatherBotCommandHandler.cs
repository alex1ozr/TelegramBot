using MediatR;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using TelegramBot.Application.Features.Weather.Cities;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Features.Weather;

internal sealed class WeatherBotCommandHandler : IRequestHandler<WeatherBotCommand, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotMessageLocalizer _botMessageLocalizer;

    public WeatherBotCommandHandler(
        ITelegramBotClient telegramBotClient,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _telegramBotClient = telegramBotClient;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task<Unit> Handle(WeatherBotCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;
        var keyboard = GenInlineKeyboard(request.UserInfo.Language);

        var text = _botMessageLocalizer.GetLocalizedString(nameof(BotMessages.WeatherCommand), request.UserInfo.Language);
        await _telegramBotClient.SendMessageAsync(
            message.Chat.Id,
            text,
            parseMode: FormatStyles.HTML,
            replyMarkup: keyboard, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return Unit.Value;
    }

    private InlineKeyboardMarkup GenInlineKeyboard(BotLanguage language)
    {
        var buttons = new InlineKeyboardBuilder();

        buttons.AppendCallbackData(
            _botMessageLocalizer.GetLocalizedString(nameof(BotMessages.NewYorkCity), language),
            callbackData: NewYorkWeatherCallbackCommand.CommandName);

        buttons.AppendCallbackData(
            _botMessageLocalizer.GetLocalizedString(nameof(BotMessages.ParisCity), language),
            callbackData: ParisWeatherCallbackCommand.CommandName);

        return new InlineKeyboardMarkup(buttons);
    }
}
