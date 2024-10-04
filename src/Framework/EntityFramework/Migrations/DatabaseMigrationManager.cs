using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace TelegramBot.Framework.EntityFramework.Migrations;

/// <summary>
/// Database migration utility
/// </summary>
public static class DatabaseMigrationManager
{
    /// <summary>
    /// Apply migrations
    /// </summary>
    public static async Task MigrateAsync<TFactory>(ILogger logger)
        where TFactory : IDesignTimeDbContextFactory<DbContext>, new()
    {
        var timer = Stopwatch.StartNew();
        logger.LogInformation("Applying database migration...");

        await MigrateAsync<TFactory>().ConfigureAwait(false);

        logger.LogInformation("Database migration was completed successfully in {Elapsed:0.0000} ms", timer.Elapsed.TotalMilliseconds);
    }

    private static async Task MigrateAsync<TFactory>()
        where TFactory : IDesignTimeDbContextFactory<DbContext>, new()
    {
#pragma warning disable CA2007
        await using var context = new TFactory().CreateDbContext([]);
#pragma warning restore CA2007
        await context.Database.MigrateAsync().ConfigureAwait(false);
    }
}
