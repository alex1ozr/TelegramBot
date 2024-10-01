using System.Reflection;
using ClickHouse.Client;
using Dapper;
using Microsoft.Extensions.Logging;

namespace TelegramBot.Framework.ClickHouse.Migrations;

// TODO Implement ClickHouse migrations history table to track already applied migrations
internal sealed class ClickHouseMigrationRunner : IClickHouseMigrationRunner
{
    private readonly IClickHouseConnection _clickHouseConnection;
    private readonly ILogger<ClickHouseMigrationRunner> _logger;

    public ClickHouseMigrationRunner(
        IClickHouseConnection clickHouseConnection,
        ILogger<ClickHouseMigrationRunner> logger)
    {
        _clickHouseConnection = clickHouseConnection;
        _logger = logger;
    }

    public async Task MigrateFromAssemblyAsync(Assembly assembly, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Applying ClickHouse migrations for assembly {Assembly}",
            assembly.GetName().Name);

        var migrationFiles = GetMigrationFiles(assembly);
        if (migrationFiles.Count == 0)
        {
            _logger.LogInformation("No ClickHouse migrations found for assembly {Assembly}",
                assembly.GetName().Name);
            return;
        }

        foreach (var migrationFile in migrationFiles)
        {
            _logger.LogInformation("Executing ClickHouse migration: {MigrationFile}", migrationFile);
            try
            {
                var content = await GetMigrationContentAsync(assembly, migrationFile, cancellationToken)
                    .ConfigureAwait(false);
                await _clickHouseConnection.ExecuteAsync(content)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute ClickHouse migration: {MigrationFile}", migrationFile);
                throw;
            }
        }
    }

    private static IReadOnlyList<string> GetMigrationFiles(Assembly assembly)
    {
        var filePrefix = $"{assembly.GetName().Name}.ClickHouse.";
        return assembly
            .GetManifestResourceNames()
            .Where(x => x.StartsWith(filePrefix) && x.EndsWith(".sql"))
            .OrderBy(x => x)
            .ToList();
    }

    private static async Task<string> GetMigrationContentAsync(
        Assembly assembly,
        string migrationFile,
        CancellationToken cancellationToken)
    {
        await using var stream = assembly.GetManifestResourceStream(migrationFile)
                                 ?? throw new FileNotFoundException($"Embedded resource '{migrationFile}' not found.");

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
    }
}
