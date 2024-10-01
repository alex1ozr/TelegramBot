namespace TelegramBot.Framework.Configuration;

/// <summary>
/// Option description
/// </summary>
public interface IOptionDescription
{
    /// <summary>
    /// Key of the option's section in the configuration
    /// </summary>
    static abstract string OptionKey { get; }
}
