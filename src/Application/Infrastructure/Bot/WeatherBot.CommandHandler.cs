using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Bot.Commands;
using TelegramBot.Application.Resources;
using TelegramBot.Domain.Analytics;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Application.Infrastructure.Bot;

partial class WeatherBot
{
    protected override async Task OnCommandAsync(
        Message message,
        string commandName,
        string args,
        CancellationToken cancellationToken)
    {
        try
        {
            var userInfo = await _mediator.Send(new EnsureUserCommand(message), cancellationToken)
                .ConfigureAwait(false);

            var command = _commandFactory.CreateBotCommand(commandName, message, userInfo);

            _logger.LogInformation("{Command} requested by {UserName} ({UserId})",
                command.GetType().Name,
                message.From?.Username,
                message.From?.Id);

            var commandDescriptor = command.GetCommandDescriptor();
            if (commandDescriptor.Roles.Any()
                && !commandDescriptor.Roles.Intersect(userInfo.Roles).Any())
            {
                _logger.LogWarning("User {UserName} ({UserId}) is not allowed to execute command {Command}",
                    message.From?.Username,
                    message.From?.Id,
                    commandName);
                return;
            }

            if (message.Chat.Type == ChatTypes.Group)
            {
                var allowGroups = command.GetCommandDescriptor().AllowGroups;

                if (!allowGroups)
                {
                    _logger.LogInformation("Command {CommandName} is not available in groups", commandName);

                    var text = string.Format(
                        (string)_botMessageLocalizer.GetLocalizedString(
                            nameof(BotMessages.CommandNotAllowedInGroups),
                            userInfo.Language),
                        commandName);

                    await _client.SendMessageAsync(message.Chat.Id,
                            text,
                            cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

                    return;
                }
            }

            await _mediator.Send(command, cancellationToken).ConfigureAwait(false);

            _metrics.IncreaseCommandsExecuted();
            await SaveUserCommandAsync(userInfo.TelegramUserId, commandName, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute command {CommandName} for message {MessageId}", commandName,
                message.MessageId);
            _metrics.IncreaseCommandsFailed();
        }
    }

    private async Task SaveUserCommandAsync(
        string telegramUserId,
        string commandName,
        CancellationToken cancellationToken)
    {
        try
        {
            var userInfo = await _mediator.Send(new GetUserInfoQuery(telegramUserId), cancellationToken)
                .ConfigureAwait(false);

            var userCommandRepository = _serviceProvider.GetRequiredService<IUserCommandRepository>();
            var userCommand = UserCommand.Create(userInfo.Required().UserId, commandName);
            await userCommandRepository.AddAsync(userCommand, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save user command for message");
        }
    }
}
