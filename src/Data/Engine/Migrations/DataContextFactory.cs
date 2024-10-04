using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TelegramBot.Data.Engine.Configuration;
using TelegramBot.Framework.EntityFramework.NpgSql;

namespace TelegramBot.Data.Engine.Migrations;

public sealed class DataContextFactory : PostgreSqlDbContextFactoryBase<DataContext, ConnectionOptions>
{
    protected override DataContext CreateDbContext(DbContextOptions<DataContext> options)
    {
        return new DataContext(options, Array.Empty<IInterceptor>());
    }
}
