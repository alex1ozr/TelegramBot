namespace TelegramBot.Framework.EntityFramework;

public interface IDataContext
{
    static virtual string Schema => "public";

    static virtual string MigrationHistoryTable => "__EFMigrationsHistory";
}
