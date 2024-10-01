using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.BotAPI;
using Telegram.BotAPI.UpdatingMessages;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;
using TelegramBot.Domain.Accounting;
using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Framework.Entities;
using LanguageOptions = TelegramBot.Application.Features.Accounting.UserLanguage.SetUserLanguageBotCommand.LanguageOptions;

namespace TelegramBot.Application.Features.Accounting.UserLanguage;

internal sealed class SetUserLanguageCallbackCommandHandler :
    IRequestHandler<SetUserLanguageCallbackCommand, Unit>
{
    private readonly IBotMessageLocalizer _botMessageLocalizer;
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _client;
    private readonly ILogger<SetUserLanguageCallbackCommandHandler> _logger;

    public SetUserLanguageCallbackCommandHandler(
        IBotMessageLocalizer botMessageLocalizer,
        IUserRepository userRepository,
        ITelegramBotClient client,
        ILogger<SetUserLanguageCallbackCommandHandler> logger)
    {
        _botMessageLocalizer = botMessageLocalizer;
        _userRepository = userRepository;
        _client = client;
        _logger = logger;
    }

    public async Task<Unit> Handle(SetUserLanguageCallbackCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserInfo.UserId, cancellationToken)
            .ConfigureAwait(false);

        string messageText;
        var languageOption = request.Arguments.FirstOrDefault();
        switch (languageOption)
        {
            case LanguageOptions.Auto:
                user.SetAutoDetectLanguage(true);

                messageText = _botMessageLocalizer.GetLocalizedString(
                    nameof(BotMessages.UserLanguageWasSetIntoAuto),
                    request.UserInfo.Language);
                break;

            case LanguageOptions.English:
                user.SetAutoDetectLanguage(false);
                user.SetLanguage(LanguageCodes.English);

                messageText = _botMessageLocalizer.GetLocalizedString(
                    nameof(BotMessages.UserLanguageWasSetIntoEnglish),
                    request.UserInfo.Language);
                break;

            case LanguageOptions.Russian:
                user.SetAutoDetectLanguage(false);
                user.SetLanguage(LanguageCodes.Russian);

                messageText = _botMessageLocalizer.GetLocalizedString(
                    nameof(BotMessages.UserLanguageWasSetIntoRussian),
                    request.UserInfo.Language);
                break;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(request.Arguments),
                    languageOption,
                    "Wrong language option");
        }

        await _userRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);

        await _client.EditMessageTextAsync(
            request.Message.Chat.Id,
            request.Message.MessageId,
            messageText,
            parseMode: FormatStyles.HTML,
            replyMarkup: null,
            cancellationToken: cancellationToken);

        _logger.LogInformation("User {UserId} language was set to {LanguageOption}", user.Id, languageOption);

        return Unit.Value;
    }
}
