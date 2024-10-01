using Telegram.BotAPI.AvailableTypes;

namespace TelegramBot.Application.Infrastructure.Bot.Messages;

internal static class DefaultLinkPreviewOptions
{
    public static LinkPreviewOptions Value { get; } = new() { IsDisabled = true };
}
