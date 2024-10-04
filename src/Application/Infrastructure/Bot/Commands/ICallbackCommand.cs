using MediatR;
using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;

namespace TelegramBot.Application.Infrastructure.Bot.Commands;

/// <summary>
/// Represents a bot callback command
/// </summary>
public interface ICallbackCommand : IRequest<Unit>
{
    /// <summary>
    /// The callback command name
    /// </summary>
    static abstract string CommandName { get; }

    /// <summary>
    /// The message that triggered the command
    /// </summary>
    public MaybeInaccessibleMessage Message { get; init; }

    /// <summary>
    /// The user information
    /// </summary>
    public UserInfo UserInfo { get; init; }

    public string[] Arguments { get; init; }
}
