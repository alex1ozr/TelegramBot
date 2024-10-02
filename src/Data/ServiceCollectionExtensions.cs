using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Data.Accounting;
using TelegramBot.Data.Analytics;
using TelegramBot.Data.Billing.Invoices;
using TelegramBot.Data.Engine;
using TelegramBot.Data.Engine.Configuration;
using TelegramBot.Domain.Exceptions;
using TelegramBot.Framework.Composition;
using TelegramBot.Framework.Configuration;
using TelegramBot.Framework.EntityFramework.Interceptors;

namespace TelegramBot.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        AddEngine(services, configuration);
        AddInterceptors(services);
        AddRepositories(services);

        return services;
    }

    private static void AddEngine(IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguredOptions<ConnectionOptions>();

        services.AddTransient<IDbOptionsConfigurator, DbOptionsConfigurator>();
        services.AddTransient(typeof(DbContextOptionsFactory));

        services.AddDbContext<DataContext>((provider, builder) =>
        {
            provider.GetRequiredService<DbContextOptionsFactory>().SetupOptions(builder);
        });

        var connection = configuration.GetRequiredOptions<ConnectionOptions>();

        var connectionString = UnexpectedException.ThrowIfNull(
            connection.TelegramBot,
            $"Connection string '{nameof(ConnectionOptions.TelegramBot)}' is not configured");
    }

    private static void AddInterceptors(IServiceCollection services)
    {
        services.AddTransientAsImplementedInterfaces<AuditableInterceptor>();
        services.AddTransientAsImplementedInterfaces<DeletableInterceptor>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddTransientAsImplementedInterfaces<UserRepository>();
        services.AddTransientAsImplementedInterfaces<RoleRepository>();
        services.AddTransientAsImplementedInterfaces<UserCommandRepository>();
        services.AddTransientAsImplementedInterfaces<InvoiceRepository>();
    }
}

