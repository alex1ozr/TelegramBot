using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TelegramBot.Framework.EntityFramework.Configuration;

namespace TelegramBot.Framework.EntityFramework.NpgSql;

/// <summary>
/// Base class for configuring database options (PostgreSQL)
/// </summary>
public abstract class DbOptionsConfiguratorBase<TConnectionOptions, TDataContext>
    where TConnectionOptions : class, IConnectionOptions
    where TDataContext : DbContext, IDataContext
{
    private readonly QueryTrackingBehavior _queryTrackingBehavior;
    private readonly TConnectionOptions _connectionOptions;

    protected DbOptionsConfiguratorBase(
        IOptions<TConnectionOptions> connectionOptions,
        QueryTrackingBehavior queryTrackingBehavior = QueryTrackingBehavior.TrackAll)
    {
        _queryTrackingBehavior = queryTrackingBehavior;
        _connectionOptions = connectionOptions.Value;
    }

    public TBuilder SetupBuilder<TBuilder>(TBuilder builder)
        where TBuilder : DbContextOptionsBuilder
    {
        var connectionString = _connectionOptions.GetConnectionString();

        // SplitQuery settings is used to avoid data duplication
        // https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries#data-duplication
        builder.UseNpgsql(connectionString, options =>
            options
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                .MigrationsHistoryTable(Constants.MigrationHistoryTable, TDataContext.Schema))
            .UseQueryTrackingBehavior(_queryTrackingBehavior)
            .UseSnakeCaseNamingConvention();

        return builder;
    }
}
