using TelegramBot.Framework.Configuration;

namespace TelegramBot.Framework.EntityFramework;

/// <summary>
/// Database connection options
/// </summary>
public interface IConnectionOptions : IOptionDescription
{
    static string IOptionDescription.OptionKey => "ConnectionStrings";

    /// <summary>
    /// Get connection string
    /// </summary>
    string GetConnectionString();
}
