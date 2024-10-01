using MediatR;

namespace TelegramBot.Application.Features.Accounting;

/// <summary>
/// Ensure that the telegram user is registered in the system and return the user info.
/// </summary>
public sealed record GetUserInfoQuery(string TelegramUserId) : IRequest<UserInfo?>;
