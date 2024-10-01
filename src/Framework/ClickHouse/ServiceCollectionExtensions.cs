using ClickHouse.Client;
using ClickHouse.Client.ADO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TelegramBot.Framework.ClickHouse.Migrations;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Framework.ClickHouse;

/// <summary>
/// Service collection extensions for ClickHouse
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add ClickHouse services and options
    /// </summary>
    public static IServiceCollection AddClickHouse(this IServiceCollection services)
    {
        services.AddOptions<ClickHouseOptions>()
            .BindConfiguration(ClickHouseOptions.OptionKey);

        services.AddSingleton<IClickHouseConnection>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<ClickHouseOptions>>().Value;
            return new ClickHouseConnection(options.ClickHouse.Required());
        });

        services.AddTransient<IClickHouseMigrationRunner, ClickHouseMigrationRunner>();
        return services;
    }
}
