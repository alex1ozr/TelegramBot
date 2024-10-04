using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Features.Bot;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Application.Infrastructure.Bot;

partial class WeatherBot
{
    private const int InvalidCallbackQueryCacheTime = 99_999;

    protected override async Task OnCallbackQueryAsync(CallbackQuery cQuery, CancellationToken cancellationToken)
    {
        var args = cQuery.Data?.Split(' ') ?? [];
        if (cQuery.Message == null || args.Length == 0)
        {
            await _client.AnswerCallbackQueryAsync(
                    cQuery.Id,
                "This button is no longer available",
                true,
                cacheTime: InvalidCallbackQueryCacheTime, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return;
        }

        var commandName = args[0];

        var userInfo = await _mediator.Send(new GetUserInfoQuery(cQuery.From.Id.ToString()), cancellationToken)
            .ConfigureAwait(false);
        var callbackCommand = _commandFactory.CreateCallbackCommand(
            commandName,
            cQuery.Message,
            userInfo.Required(),
            args: args.Skip(1).ToArray());

        if (callbackCommand is not UnknownCallbackCommand)
        {
            await _client.AnswerCallbackQueryAsync(cQuery.Id, cacheTime: 2, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            await _mediator.Send(callbackCommand, cancellationToken).ConfigureAwait(false);

            _metrics.IncreaseCommandsExecuted();
            await SaveUserCommandAsync(userInfo.Required().TelegramUserId, commandName, cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            await _client.AnswerCallbackQueryAsync(cQuery.Id, "???", cacheTime: 999, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
