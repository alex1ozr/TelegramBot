using TelegramBot.Application.Resources;
using TelegramBot.Domain.Accounting.Users;

namespace TelegramBot.Application.Features.Accounting;

public sealed record class UserInfo(
    UserId UserId,
    string TelegramUserId,
    BotLanguage Language,
    IReadOnlyList<string> Roles);
