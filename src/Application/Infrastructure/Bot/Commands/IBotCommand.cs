using MediatR;
using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;

namespace TelegramBot.Application.Infrastructure.Bot.Commands;

/// <summary>
/// Represents a bot command
/// </summary>
public interface IBotCommand : IRequest<Unit>
{
    /// <summary>
    /// The command name
    /// </summary>
    static abstract string CommandName { get; }

    /// <summary>
    /// Whether the command is allowed in groups
    /// </summary>
    static virtual bool AllowGroups => true;

    /// <summary>
    /// The roles that are allowed to execute the command
    /// </summary>
    static virtual IReadOnlyList<string> Roles { get; } = Array.Empty<string>();

    /// <summary>
    /// The message that triggered the command
    /// </summary>
    public Message Message { get; init; }

    /// <summary>
    /// The user information
    /// </summary>
    public UserInfo UserInfo { get; init; }
}
