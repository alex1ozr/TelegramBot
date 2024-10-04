using MediatR;
using TelegramBot.Application.Extensions;
using TelegramBot.Domain.Accounting.Users;

namespace TelegramBot.Application.Features.Accounting;

internal sealed class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfo?>
{
    private readonly IUserRepository _userRepository;

    public GetUserInfoQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserInfo?> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByTelegramUserIdAsync(request.TelegramUserId, cancellationToken)
            .ConfigureAwait(false);

        return user is not null
            ? new UserInfo(
                user.Id,
                user.TelegramUserId,
                user.GetBotLanguage(),
                user.Roles.Select(r => r.NormalizedName).ToList())
            : null;
    }
}
