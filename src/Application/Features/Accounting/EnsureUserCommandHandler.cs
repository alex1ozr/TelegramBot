using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Extensions;
using TelegramBot.Application.Features.Accounting.UserLanguage;
using TelegramBot.Application.Infrastructure;
using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Domain.Exceptions;
using TelegramBot.Framework.Utilities.System;
using User = TelegramBot.Domain.Accounting.Users.User;

namespace TelegramBot.Application.Features.Accounting;

internal sealed class EnsureUserCommandHandler : IRequestHandler<EnsureUserCommand, UserInfo>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<EnsureUserCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly ApplicationMetrics _applicationMetrics;

    public EnsureUserCommandHandler(
        IUserRepository userRepository,
        ILogger<EnsureUserCommandHandler> logger,
        IMediator mediator,
        ApplicationMetrics applicationMetrics)
    {
        _userRepository = userRepository;
        _logger = logger;
        _mediator = mediator;
        _applicationMetrics = applicationMetrics;
    }

    public async Task<UserInfo> Handle(EnsureUserCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;
        UserNotFoundException.ThrowIfNull(
            message.From,
            $"There is no user for the message {message.MessageId}. Bot cannot work in channels.");

        var user = await _userRepository.GetByTelegramUserIdAsync(message.From.Id.ToString(), cancellationToken)
            .ConfigureAwait(false);

        var commandLanguage = message.From.Required().LanguageCode;

        if (user is not null
            && user.AutoDetectLanguage
            && !string.IsNullOrWhiteSpace(commandLanguage)
            && user.Language != message.From?.LanguageCode)
        {
            await _mediator.Send(new SetUserLanguageCommand(user.Id, commandLanguage), cancellationToken)
                .ConfigureAwait(false);
        }

        if (user is not null)
        {
            var userLanguage = user.AutoDetectLanguage
                ? message.GetUserLanguage()
                : user.GetBotLanguage();

            return new UserInfo(
                user.Id,
                user.TelegramUserId,
                userLanguage,
                user.Roles.Select(r => r.NormalizedName).ToList());
        }

        user = await CreateUserAsync(cancellationToken, message, commandLanguage)
            .ConfigureAwait(false);

        return new UserInfo(
            user.Id,
            user.TelegramUserId,
            message.GetUserLanguage(),
            user.Roles.Select(r => r.NormalizedName).ToList());
    }

    private async Task<User> CreateUserAsync(CancellationToken cancellationToken, Message message, string? commandLanguage)
    {
        var user = User.Create(message.From!.Id.ToString(), message.From.Username);
        if (!string.IsNullOrWhiteSpace(commandLanguage))
        {
            user.SetLanguage(commandLanguage);
        }

        await _userRepository.AddAsync(user, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("User {UserId} was created by message {MessageId}",
            user.Id,
            message.MessageId);

        _applicationMetrics.IncreaseUsersCreated();
        return user;
    }
}
