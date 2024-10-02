using Microsoft.Extensions.Configuration;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Framework.Configuration;

/// <summary>
/// Extensions for <see cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Get required options
    /// </summary>
    public static TOptions GetRequiredOptions<TOptions>(this IConfiguration configuration)
        where TOptions : class, IOptionDescription
        => configuration.GetRequiredOptions<TOptions>(TOptions.OptionKey);

    /// <summary>
    /// Get required options by key
    /// </summary>
    public static TOptions GetRequiredOptions<TOptions>(this IConfiguration configuration, string key)
        where TOptions : class
        => configuration
            .GetRequiredSection(key)
            .Get<TOptions>()
            .Required();
}
