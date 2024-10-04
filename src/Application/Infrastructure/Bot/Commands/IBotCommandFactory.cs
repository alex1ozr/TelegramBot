using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;

namespace TelegramBot.Application.Infrastructure.Bot.Commands;

/// <summary>
/// Factory for instantiating bot commands.
/// </summary>
public interface IBotCommandFactory
{
    /// <summary>
    /// Create a bot command instance by command name.
    /// </summary>
    public IBotCommand CreateBotCommand(string commandName, Message message, UserInfo userInfo);

    /// <summary>
    /// Create a bot callback command instance by command name.
    /// </summary>
    public ICallbackCommand CreateCallbackCommand(
        string commandName,
        MaybeInaccessibleMessage message,
        UserInfo userInfo,
        params string[] args);
}
