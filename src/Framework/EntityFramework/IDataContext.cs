namespace TelegramBot.Framework.EntityFramework;

/// <summary>
/// Data context
/// </summary>
public interface IDataContext
{
    /// <summary>
    /// Database schema
    /// </summary>
    static virtual string Schema => "public";

    /// <summary>
    /// Migration history table name
    /// </summary>
    static virtual string MigrationHistoryTable => "__EFMigrationsHistory";
}
