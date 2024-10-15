using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using TelegramBot.Application.Infrastructure.Bot.Messages;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Application.Extensions;

internal static class TelegramBotClientExtensions
{
    // Telegram messages can currently hold up to 4KB of text: https://bugs.telegram.org/c/1423
    private const int MaxMessageLength = 4095;

    public static async Task SendLargeMessageAsync(
        this ITelegramBotClient client,
        long chatId,
        string text,
        string? parseMode = null,
        CancellationToken cancellationToken = default)
    {
        foreach (var part in text.Split(MaxMessageLength))
        {
            await client.SendMessageAsync(
                chatId,
                part,
                linkPreviewOptions: DefaultLinkPreviewOptions.Value,
                parseMode: parseMode,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
