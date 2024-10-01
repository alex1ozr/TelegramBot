using MediatR;
using Microsoft.Extensions.Logging;
using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Framework.Entities;

namespace TelegramBot.Application.Features.Accounting.UserLanguage;

internal sealed class SetUserLanguageCommandHandler : IRequestHandler<SetUserLanguageCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<SetUserLanguageCommandHandler> _logger;

    public SetUserLanguageCommandHandler(
        IUserRepository userRepository,
        ILogger<SetUserLanguageCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(SetUserLanguageCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            .ConfigureAwait(false);

        user.SetLanguage(request.Language);

        await _userRepository.UpdateAsync(user, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("User {UserId} language was set into {Language}",
            user.Id,
            request.Language);

        return Unit.Value;
    }
}
