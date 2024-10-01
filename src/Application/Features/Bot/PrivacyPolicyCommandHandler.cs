using MediatR;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;

namespace TelegramBot.Application.Features.Bot;

internal sealed class PrivacyPolicyBotCommandHandler : IRequestHandler<PrivacyPolicyBotCommand, Unit>
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotMessageLocalizer _botMessageLocalizer;

    public PrivacyPolicyBotCommandHandler(
        ITelegramBotClient telegramBotClient,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _telegramBotClient = telegramBotClient;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task<Unit> Handle(PrivacyPolicyBotCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;

        var text = _botMessageLocalizer.GetLocalizedString(nameof(BotMessages.PrivacyPolicy), request.UserInfo.Language);

        await _telegramBotClient.SendMessageAsync(
                message.Chat.Id,
                text,
                parseMode: FormatStyles.MarkdownV2,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return Unit.Value;
    }
}
