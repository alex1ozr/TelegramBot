namespace TelegramBot.Framework.ClickHouse;

/// <summary>
/// Options for the ClickHouse
/// </summary>
public sealed class ClickHouseOptions
{
    public static readonly string OptionKey = "ConnectionStrings";

    /// <summary>
    /// ClickHouse connection string
    /// </summary>
    public string? ClickHouse { get; init; }
}
