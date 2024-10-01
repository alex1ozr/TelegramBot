using TelegramBot.Framework.EntityFramework;

namespace TelegramBot.Data.Engine.Configuration;

/// <summary>
/// Db connection options
/// </summary>
public sealed class ConnectionOptions : IConnectionOptions
{
    public string? SpaceWeatherBot { get; set; }

    public string GetConnectionString() =>
        SpaceWeatherBot
        ?? throw new ArgumentException($"Connection string «{nameof(SpaceWeatherBot)}» not found");
}
