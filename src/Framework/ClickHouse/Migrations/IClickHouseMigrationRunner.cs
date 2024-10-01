using System.Reflection;

namespace TelegramBot.Framework.ClickHouse.Migrations;

/// <summary>
/// ClickHouse migration runner
/// </summary>
public interface IClickHouseMigrationRunner
{
    /// <summary>
    /// Run ClickHouse migrations for the specified assembly
    /// </summary>
    /// <remarks>
    /// Migrations are embedded resources with the '.sql' extension, that are located in the 'ClickHouse' folder
    /// </remarks>
    Task MigrateFromAssemblyAsync(Assembly assembly, CancellationToken cancellationToken = default);
}
