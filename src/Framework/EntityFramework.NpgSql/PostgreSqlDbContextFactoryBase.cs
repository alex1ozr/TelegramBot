using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TelegramBot.Framework.Composition;
using TelegramBot.Framework.Configuration;

namespace TelegramBot.Framework.EntityFramework.NpgSql;

/// <summary>
/// Base class for creating a database context (PostgreSQL)
/// </summary>
public abstract class PostgreSqlDbContextFactoryBase<TDbContext, TConnectionOptions>
    : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext : DbContext, IDataContext
    where TConnectionOptions : class, IConnectionOptions
{
    public TDbContext CreateDbContext(string[] args)
    {
        var connectionString = GetConnectionString(args);

        var options = new DbContextOptionsBuilder<TDbContext>()
            .UseNpgsql(connectionString, builder =>
                builder
                    .MigrationsAssembly(typeof(TDbContext).Assembly.FullName)
                    .MigrationsHistoryTable(TDbContext.MigrationHistoryTable, TDbContext.Schema))
            .Options;

        return CreateDbContext(options);
    }

    private static string GetConnectionString(string[] args)
    {
        const string createMigrationOnlyKey = "--CreateMigrationOnly";
        const string createMigrationOnlyConnectionString = "Server=localhost";

        var isCreateMigrationOnly = args.Any(i =>
            string.Equals(i, createMigrationOnlyKey, StringComparison.InvariantCultureIgnoreCase));

        if (isCreateMigrationOnly)
        {
            return createMigrationOnlyConnectionString;
        }

        var configuration = MinimalConfigurationLoader.Load(args);
        var options = configuration.GetRequiredOptions<TConnectionOptions>();

        return options.GetConnectionString();
    }

    protected abstract TDbContext CreateDbContext(DbContextOptions<TDbContext> options);
}
