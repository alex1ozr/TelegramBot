using MediatR;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using TelegramBot.Application.Infrastructure.Bot.Messages;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Features.Bot;

internal sealed class StartBotCommandHandler : IRequestHandler<StartBotCommand, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotMessageLocalizer _botMessageLocalizer;

    public StartBotCommandHandler(
        ITelegramBotClient telegramBotClient,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _telegramBotClient = telegramBotClient;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task<Unit> Handle(StartBotCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;
        var text = _botMessageLocalizer.GetLocalizedString(nameof(BotMessages.HelpCommand), request.UserInfo.Language);

        await _telegramBotClient.SendMessageAsync(
                message.Chat.Id,
                text,
                parseMode: FormatStyles.HTML,
                linkPreviewOptions: DefaultLinkPreviewOptions.Value,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return Unit.Value;
    }
}
