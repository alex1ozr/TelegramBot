using MediatR;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;
using LanguageOptions = TelegramBot.Application.Features.Accounting.UserLanguage.SetUserLanguageBotCommand.LanguageOptions;

namespace TelegramBot.Application.Features.Accounting.UserLanguage;

internal sealed class SetUserLanguageBotCommandHandler : IRequestHandler<SetUserLanguageBotCommand, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotMessageLocalizer _botMessageLocalizer;

    public SetUserLanguageBotCommandHandler(
        ITelegramBotClient telegramBotClient,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _telegramBotClient = telegramBotClient;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task<Unit> Handle(SetUserLanguageBotCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;
        var keyboard = GenInlineKeyboard();

        var text = _botMessageLocalizer.GetLocalizedString(nameof(BotMessages.SetUserLanguageCommand), request.UserInfo.Language);
        await _telegramBotClient.SendMessageAsync(
            message.Chat.Id,
            text,
            parseMode: FormatStyles.HTML,
            replyMarkup: keyboard, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return Unit.Value;
    }

    private static InlineKeyboardMarkup GenInlineKeyboard()
    {
        var buttons = new InlineKeyboardBuilder();

        buttons.AppendCallbackData(
            text: "Auto",
            callbackData: $"{SetUserLanguageCallbackCommand.CommandName} {LanguageOptions.Auto}");
        buttons.AppendCallbackData(
            text: "English",
            callbackData: $"{SetUserLanguageCallbackCommand.CommandName} {LanguageOptions.English}");
        buttons.AppendCallbackData(
            text: "Русский",
            callbackData: $"{SetUserLanguageCallbackCommand.CommandName} {LanguageOptions.Russian}");

        return new InlineKeyboardMarkup(buttons);
    }
}
