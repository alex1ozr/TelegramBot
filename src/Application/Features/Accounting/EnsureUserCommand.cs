using MediatR;
using Telegram.BotAPI.AvailableTypes;

namespace TelegramBot.Application.Features.Accounting;

/// <summary>
/// Ensure that the user is registered in the system and return the user id.
/// </summary>
public sealed record EnsureUserCommand(Message Message) : IRequest<UserInfo>;
