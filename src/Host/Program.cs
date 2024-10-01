using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using TelegramBot.Application;
using TelegramBot.Data;
using TelegramBot.Data.Engine.Migrations;
using TelegramBot.Framework.ClickHouse;
using TelegramBot.Framework.ClickHouse.Migrations;
using TelegramBot.Framework.EntityFramework.Migrations;
using TelegramBot.Host;

var loggerFactory = PrepareLogging();
var logger = loggerFactory.CreateLogger<Program>();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

builder.Configuration.AddJsonFile("appsettings.defaults.json");

var serviceName = builder.Configuration["Host:Name"] ?? "unknown-service";
ConfigureServices(builder.Services, builder.Configuration);
//builder.AddServiceDefaults(logger, serviceName, customMeterNames: ["sweather_bot"]);

try
{
    logger.LogInformation("Starting application...");
    var app = builder.Build();

    await PrepareDatabase(builder.Configuration, loggerFactory, app.Services)
        .ConfigureAwait(false);

    ConfigureWebApplication(app);
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Application terminated unexpectedly");
    // Sets application exit code
    throw;
}
finally
{
    logger.LogInformation("Stopping application...");
    loggerFactory.Dispose();
}

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration)
{
    services.AddMetrics();
    services.AddHttpLogging(logging =>
    {
        logging.CombineLogs = true;
        logging.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath |
                                HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.Duration |
                                HttpLoggingFields.RequestQuery | HttpLoggingFields.RequestScheme;
    });

    services.AddControllers()
        .AddControllersAsServices();
    services.AddEndpointsApiExplorer();

    services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
    });

    RegisterCommonServices(services);
    services.AddBotApplication(configuration);
    services.AddData(configuration);
    services.AddClickHouse();

    services.AddHostedService<BotBackgroundService>();
}

static void ConfigureWebApplication(WebApplication app)
{
    //app.MapDefaultEndpoints();
    app.UseForwardedHeaders();

    app.UseHealthChecks("/health",
        new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

    app.UseHttpLogging();
    app.MapControllers();
}


static void RegisterCommonServices(IServiceCollection services)
{
    services.AddSingleton(_ => TimeProvider.System);
    services.AddHttpContextAccessor();
}

static async Task PrepareDatabase(IConfiguration configuration, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
{
    var logger = loggerFactory.CreateLogger("SpaceWeatherBotDbMigration");
    await DatabaseMigrationManager.MigrateAsync<DataContextFactory>( logger)
        .ConfigureAwait(false);

    var clickHouseMigration = serviceProvider.GetRequiredService<IClickHouseMigrationRunner>();
    await clickHouseMigration.MigrateFromAssemblyAsync(typeof(DataContextFactory).Assembly)
        .ConfigureAwait(false);
}

static ILoggerFactory PrepareLogging()
{
    return LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });
}
