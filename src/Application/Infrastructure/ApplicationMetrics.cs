using System.Diagnostics.Metrics;

namespace TelegramBot.Application.Infrastructure;

internal sealed class ApplicationMetrics
{
    private const string MeterName = "telegram_bot";
    private readonly Counter<long> _commandsExecuted;
    private readonly Counter<long> _commandsFailed;
    private readonly Counter<long> _usersCreated;

    public ApplicationMetrics(IMeterFactory meterFactory)
    {
        var meterInstance = meterFactory.Create(MeterName);

        _commandsExecuted = meterInstance.CreateCounter<long>(MeterName + ".commands_executed");
        _commandsFailed = meterInstance.CreateCounter<long>(MeterName + ".commands_failed");
        _usersCreated = meterInstance.CreateCounter<long>(MeterName + ".users_created");
    }

    public void IncreaseCommandsExecuted(int quantity = 1) => _commandsExecuted.Add(quantity);

    public void IncreaseUsersCreated(int quantity = 1) => _usersCreated.Add(quantity);

    public void IncreaseCommandsFailed(int quantity = 1) => _commandsFailed.Add(quantity);
}
