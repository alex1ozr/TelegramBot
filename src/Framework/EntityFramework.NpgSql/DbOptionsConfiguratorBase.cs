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

        // SplitQuery settings is used because without it EF builds large queries to get navigation properties of collections,
        // in which for each record it makes JOINs, which can lead to a large amount of data duplication and as a result to a
        // large consumption of resources when trying to map them to objects
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
