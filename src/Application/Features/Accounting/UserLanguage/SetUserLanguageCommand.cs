using MediatR;
using TelegramBot.Domain.Accounting.Users;

namespace TelegramBot.Application.Features.Accounting.UserLanguage;

/// <summary>
/// Set the user language
/// </summary>
public sealed record SetUserLanguageCommand(UserId UserId, string Language) : IRequest<Unit>;
