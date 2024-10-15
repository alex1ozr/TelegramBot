using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using TelegramBot.Application;
using TelegramBot.Data;
using TelegramBot.Data.Engine.Migrations;
using TelegramBot.Framework.EntityFramework.Migrations;
using TelegramBot.Host;
using TelegramBot.ServiceDefaults;

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
builder.AddServiceDefaults(logger, serviceName, customMeterNames: ["telegram_bot"]);

try
{
    logger.LogInformation("Starting application...");
    var app = builder.Build();

    await PrepareDatabase(loggerFactory, builder.Configuration)
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
    services.AddBotApplication();
    services.AddData(configuration);

    services.AddHostedService<BotBackgroundService>();
}

static void ConfigureWebApplication(WebApplication app)
{
    app.MapDefaultEndpoints();
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

static async Task PrepareDatabase(ILoggerFactory loggerFactory, IConfiguration configuration)
{
    var logger = loggerFactory.CreateLogger("TelegramBotDbMigration");

    var noMigration = configuration.GetSection(CommandLineArgs.NoMigrationKey).Get<bool?>() ?? false;
    if (noMigration)
    {
        logger.LogInformation("Database migration is disabled by configuration");
        return;
    }

    await DatabaseMigrationManager.MigrateAsync<DataContextFactory>(logger)
        .ConfigureAwait(false);
}

static ILoggerFactory PrepareLogging()
{
    return LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    });
}
