using TelegramBot.Framework.EntityFramework;

namespace TelegramBot.Data.Engine.Configuration;

/// <summary>
/// Db connection options
/// </summary>
public sealed class ConnectionOptions : IConnectionOptions
{
    public string? TelegramBot { get; set; }

    public string GetConnectionString() =>
        TelegramBot
        ?? throw new ArgumentException($"Connection string «{nameof(TelegramBot)}» not found");
}
