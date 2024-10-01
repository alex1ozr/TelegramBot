using MediatR;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Features.Billing;

/// <summary>
/// Donate Telegram Stars to the bot
/// </summary>
internal sealed class DonateBotCommandHandler : IRequestHandler<DonateBotCommand, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotMessageLocalizer _botMessageLocalizer;

    public DonateBotCommandHandler(
        ITelegramBotClient telegramBotClient,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _telegramBotClient = telegramBotClient;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task<Unit> Handle(DonateBotCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;
        var keyboard = GenInlineKeyboard();

        var text = _botMessageLocalizer.GetLocalizedString(nameof(BotMessages.DonateCommand), request.UserInfo.Language);
        await _telegramBotClient.SendMessageAsync(
            message.Chat.Id,
            text,
            parseMode: FormatStyles.HTML,
            replyMarkup: keyboard, cancellationToken: cancellationToken);

        return Unit.Value;
    }

    private static InlineKeyboardMarkup GenInlineKeyboard()
    {
        var buttons = new InlineKeyboardBuilder();

        var index = 0;
        foreach (var option in BillingOptions.DonationOptions)
        {
            buttons.AppendCallbackData(text: option.Description, callbackData: $"donate {option.Name}");
            index++;
            if (index % 2 == 0)
            {
                buttons.AppendRow();
            }
        }

        return new InlineKeyboardMarkup(buttons);
    }
}
