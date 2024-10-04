using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TelegramBot.Framework.EntityFramework.NpgSql;

namespace TelegramBot.Data.Engine.Configuration;

internal sealed class DbOptionsConfigurator :
    DbOptionsConfiguratorBase<ConnectionOptions, DataContext>,
    IDbOptionsConfigurator
{
    public DbOptionsConfigurator(IOptions<ConnectionOptions> connectionOptions) :
        base(connectionOptions, QueryTrackingBehavior.TrackAll)
    {
    }
}
