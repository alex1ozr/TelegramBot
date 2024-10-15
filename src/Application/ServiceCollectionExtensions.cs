using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Telegram.BotAPI;
using TelegramBot.Application.Features.Billing;
using TelegramBot.Application.Features.Weather;
using TelegramBot.Application.Infrastructure;
using TelegramBot.Application.Infrastructure.Bot;
using TelegramBot.Application.Infrastructure.Bot.Commands;
using TelegramBot.Application.Infrastructure.HostedServices;
using TelegramBot.Application.Infrastructure.Localization;

namespace TelegramBot.Application;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the required services for the main application
    /// </summary>
    public static IServiceCollection AddBotApplication(
        this IServiceCollection services
    )
    {
        services.AddOptions<TelegramBotOptions>()
            .BindConfiguration(TelegramBotOptions.OptionKey);

        services.AddSingleton<ITelegramBotClient>(
            provider =>
            {
                var options = provider.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
                var botOptions = new TelegramBotClientOptions(options.BotToken);
                // Set a custom server address if specified
                if (!string.IsNullOrEmpty(options.ServerAddress))
                {
                    botOptions.ServerAddress = options.ServerAddress;
                }

                var client = new TelegramBotClient(botOptions);
                return client;
            }
        );

        services.AddMemoryCache();

        services.AddScoped<WeatherBot>();

        services.AddSingleton<IUpdateReceiver, UpdateReceiver>();

        // Register the update receiver as a hosted service.
        services.AddHostedService(provider =>
            (UpdateReceiver)provider.GetRequiredService<IUpdateReceiver>()
        );
        services.AddHostedService<TelegramBotSetup>();

        services.AddInfrastructure();
        services.AddFeatures();

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(WeatherBot).Assembly));

        services.TryAddSingleton<IBotMessageLocalizer, BotMessageLocalizer>();
        services.TryAddSingleton<ApplicationMetrics>();
        services.TryAddSingleton<IBotCommandFactory, BotCommandFactory>();

        return services;
    }

    private static void AddFeatures(this IServiceCollection services)
    {
        services.AddBilling();
        services.AddWeather();
    }
}
