using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.Framework.Composition;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add a service and its implemented interfaces as transient
    /// </summary>
    public static IServiceCollection AddTransientAsImplementedInterfaces<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    {
        var implementedInterfaces = typeof(TImplementation).GetInterfaces();

        services.AddTransient<TImplementation>();

        foreach (var implementedInterface in implementedInterfaces)
        {
            services.AddTransient(implementedInterface, serviceProvider => serviceProvider.GetRequiredService<TImplementation>());
        }

        return services;
    }

    /// <summary>
    /// Add a service and its implemented interfaces as scoped
    /// </summary>
    public static IServiceCollection AddScopedAsImplementedInterfaces<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    {
        var implementedInterfaces = typeof(TImplementation).GetInterfaces();

        services.AddScoped<TImplementation>();

        foreach (var implementedInterface in implementedInterfaces)
        {
            services.AddScoped(implementedInterface, serviceProvider => serviceProvider.GetRequiredService<TImplementation>());
        }

        return services;
    }

    /// <summary>
    /// Add a service and its implemented interfaces as singleton
    /// </summary>
    public static IServiceCollection AddSingletonAsImplementedInterfaces<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    {
        var implementedInterfaces = typeof(TImplementation).GetInterfaces();

        services.AddSingleton<TImplementation>();

        foreach (var implementedInterface in implementedInterfaces)
        {
            services.AddSingleton(implementedInterface, serviceProvider => serviceProvider.GetRequiredService<TImplementation>());
        }

        return services;
    }
}
