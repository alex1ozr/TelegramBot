using MediatR;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Features.Bot;

public sealed class UnknownCallbackCommandHandler : IRequestHandler<UnknownCallbackCommand, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotMessageLocalizer _botMessageLocalizer;

    public UnknownCallbackCommandHandler(
        ITelegramBotClient telegramBotClient,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _telegramBotClient = telegramBotClient;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task<Unit> Handle(UnknownCallbackCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;
        var text = _botMessageLocalizer.GetLocalizedString(nameof(BotMessages.UnknownCommand), request.UserInfo.Language);

        await _telegramBotClient.SendMessageAsync(message.Chat.Id, text, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return Unit.Value;
    }
}
