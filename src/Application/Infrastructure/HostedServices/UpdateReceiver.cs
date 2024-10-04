using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.BotAPI.GettingUpdates;
using TelegramBot.Application.Infrastructure.Bot;

namespace TelegramBot.Application.Infrastructure.HostedServices;

internal sealed class UpdateReceiver :
    BackgroundService,
        IUpdateReceiver
{
    private readonly ActivitySource _activitySource =
        new(typeof(WeatherBot).Assembly.GetName().Name!,
            typeof(WeatherBot).Assembly.GetName().Version!.ToString());

    private readonly Channel<Update> _updates = Channel.CreateUnbounded<Update>();
    private readonly ILogger<UpdateReceiver> _logger;
    private readonly IServiceProvider _serviceProvider;

    public UpdateReceiver(ILogger<UpdateReceiver> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == _activitySource.Name,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded
        };
        ActivitySource.AddActivityListener(listener);
    }

    /// <inheritdoc />
    public void ReceiveUpdate(Update update)
    {
        if (!_updates.Writer.TryWrite(update))
        {
            _logger.LogCritical("Failed to add the update {UpdateId} to the updates pool", update.UpdateId);
        }
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var actionBlock = new ActionBlock<Update>(
            ProcessUpdateAsync,
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 20,
                CancellationToken = stoppingToken
            }
        );

        while (!stoppingToken.IsCancellationRequested)
        {
            var update = await _updates.Reader.ReadAsync(stoppingToken).ConfigureAwait(false);
            if (update is not null)
            {
                actionBlock.Post(update);
            }
        }

        actionBlock.Complete();
    }

    private async Task ProcessUpdateAsync(Update update)
    {
        using var activity = _activitySource.StartActivity(update.UpdateId.ToString());
        Activity.Current = activity;
        using var scope = _serviceProvider.CreateScope();

        try
        {
            var bot = scope.ServiceProvider.GetRequiredService<WeatherBot>();
            await bot.OnUpdateAsync(update).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to process the update {UpdateId}", update.UpdateId);
        }
    }
}
