using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.Framework.Configuration;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register options with configuration binding and configuration action
    /// </summary>
    public static void AddConfiguredOptions<T>(this IServiceCollection services, Action<T>? configureOptions = default)
        where T : class, IOptionDescription
        => services
            .AddOptions<T>()
            .BindConfiguration(T.OptionKey)
            .Configure(configureOptions ?? (_ => { }));
}
