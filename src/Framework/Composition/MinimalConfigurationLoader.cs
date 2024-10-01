using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;

namespace TelegramBot.Framework.Composition;

/// <summary>
/// Minimal application configuration loader.
/// </summary>
public static class MinimalConfigurationLoader
{
    /// <summary>
    /// Load the configuration.
    /// </summary>
    /// <remarks>
    /// Configuration layers: appsettings.json, appsettings.defaults.json, environment variables, command line arguments.
    /// </remarks>
    public static IConfiguration Load() => Load([]);

    /// <summary>
    /// Load the configuration with the given command line arguments.
    /// </summary>
    /// <param name="args">The command line args.</param>
    /// <remarks>
    /// Configuration layers: appsettings.json, appsettings.defaults.json, environment variables, command line arguments.
    /// </remarks>
    public static IConfiguration Load(string[] args) => Load(args, ImmutableDictionary<string, string>.Empty);

    /// <summary>
    /// Load the configuration with the given command line arguments and switch mappings.
    /// </summary>
    /// <param name="args">The command line args.</param>
    /// <param name="switchMappings">The switch mappings. A dictionary of short (with prefix "-") and alias keys (with prefix "--"), mapped to the configuration key (no prefix).</param>
    /// <remarks>
    /// Configuration layers: appsettings.json, appsettings.defaults.json, environment variables, command line arguments.
    /// </remarks>
    private static IConfiguration Load(
        string[] args,
        IReadOnlyDictionary<string, string> switchMappings) =>
        new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("appsettings.defaults.json", true)
            .AddEnvironmentVariables()
            .AddCommandLine(args, switchMappings.ToDictionary(pair => pair.Key, pair => pair.Value))
            .Build();
}
