using Humanizer;
using MediatR;
using Telegram.BotAPI;
using TelegramBot.Application.Extensions;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;
using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Domain.Analytics;
using TelegramBot.Framework.Entities;

namespace TelegramBot.Application.Features.Analytics;

internal sealed class StatsBotCommandHandler : IRequestHandler<StatsBotCommand, Unit>
{
    private const string NoData = "-";
    private static readonly TimeSpan s_defaultPeriod = TimeSpan.FromDays(1);

    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IBotMessageLocalizer _botMessageLocalizer;
    private readonly IUserAnalyticsRepository _userAnalyticsRepository;
    private readonly IUserRepository _userRepository;
    private readonly TimeProvider _timeProvider;

    public StatsBotCommandHandler(
        ITelegramBotClient telegramBotClient,
        IBotMessageLocalizer botMessageLocalizer,
        IUserAnalyticsRepository userAnalyticsRepository,
        IUserRepository userRepository,
        TimeProvider timeProvider)
    {
        _telegramBotClient = telegramBotClient;
        _botMessageLocalizer = botMessageLocalizer;
        _userAnalyticsRepository = userAnalyticsRepository;
        _userRepository = userRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Unit> Handle(StatsBotCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;

        var topCommands = await GetTopCommands(cancellationToken)
            .ConfigureAwait(false);

        var topUsers = await GetTopUsersAsync(cancellationToken)
            .ConfigureAwait(false);

        var lastRegisteredUsers = await GetLastRegisteredUsersAsync(cancellationToken)
            .ConfigureAwait(false);

        var totalUsers = await _userRepository.GetTotalUsersAsync(cancellationToken)
            .ConfigureAwait(false);

        var text = _botMessageLocalizer.GetLocalizedString(nameof(BotMessages.BotStatistics), request.UserInfo.Language,
            s_defaultPeriod.Humanize(),
            topCommands.CommandsList,
            topCommands.Count,
            topUsers,
            lastRegisteredUsers.Count,
            totalUsers,
            lastRegisteredUsers.UsersList);

        await _telegramBotClient.SendLargeMessageAsync(
                message.Chat.Id,
                text,
                parseMode: FormatStyles.HTML,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return Unit.Value;
    }

    private async Task<(string CommandsList, int Count)> GetTopCommands(CancellationToken cancellationToken)
    {
        var topCommands = await _userAnalyticsRepository.GetTopCommandAsync(s_defaultPeriod, cancellationToken)
            .ConfigureAwait(false);

        return topCommands.Count == 0
            ? (NoData, 0)
            : (string.Join("\n", topCommands.Select((x, i) => $"{i + 1}. {x.Command} - {x.Count}")),
                topCommands.Sum(x => x.Count));
    }

    private async Task<string> GetTopUsersAsync(CancellationToken cancellationToken)
    {
        var topUsers = await _userAnalyticsRepository.GetTopUsersAsync(s_defaultPeriod, cancellationToken)
            .ConfigureAwait(false);

        if (topUsers.Count == 0)
        {
            return NoData;
        }

        var topUsersWithUserName = new List<(string UserId, string? UserName, int CommandsExecuted)>();
        foreach (var userInfo in topUsers)
        {
            var user = await _userRepository.GetByIdOrDefaultAsync(UserId.Parse(userInfo.UserId), cancellationToken)
                .ConfigureAwait(false);

            topUsersWithUserName.Add((userInfo.UserId, user?.UserName, userInfo.CommandsExecuted));
        }

        return string.Join("\n", topUsersWithUserName.Select((x, i) => $"{i + 1}. {x.UserName} / {x.UserId} - {x.CommandsExecuted}"));
    }

    private async Task<(string UsersList, int Count)> GetLastRegisteredUsersAsync(
        CancellationToken cancellationToken)
    {
        var from = _timeProvider.GetUtcNow().UtcDateTime - s_defaultPeriod;
        var lastRegisteredUsers = await _userRepository.GetLastRegisteredUsersAsync(from, cancellationToken)
            .ConfigureAwait(false);

        return lastRegisteredUsers.Count == 0
            ? (NoData, 0)
            : (string.Join("\n", lastRegisteredUsers.Select((x, i) => $"{i + 1}. {x.Id} - {x.CreatedAt}")),
                lastRegisteredUsers.Count);
    }
}
