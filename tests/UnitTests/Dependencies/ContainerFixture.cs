using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Telegram.BotAPI;
using TelegramBot.Application;
using TelegramBot.Host;

namespace TelegramBot.UnitTests.Dependencies;

/// <summary>
/// Fixture for DI-container tests
/// </summary>
public sealed class ContainerFixture : WebApplicationFactory<IHostMarker>
{
    private static readonly IReadOnlyList<Assembly> s_hostAssemblies =
    [
        typeof(IHostMarker).Assembly,
        typeof(IApplicationMarker).Assembly
    ];

    internal static readonly IReadOnlyList<Type> s_hostTypes =
        s_hostAssemblies
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(type => !type.IsAbstract)
            .ToList();

    public IServiceProvider Container => Services;

    public static readonly Mock<ITelegramBotClient> TelegramBotClientMock = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder
            .UseSetting(CommandLineArgs.NoMigrationKey, "true")
            .UseSetting("ConnectionStrings:TelegramBot", "_")
            .ConfigureTestServices(services =>
            {
                services.Replace(ServiceDescriptor.Singleton(TelegramBotClientMock.Object));
            });
}
