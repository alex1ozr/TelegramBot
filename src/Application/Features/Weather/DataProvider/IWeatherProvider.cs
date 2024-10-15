namespace TelegramBot.Application.Features.Weather.DataProvider;

/// <summary>
/// Current weather provider
/// </summary>
public interface IWeatherProvider
{
    /// <summary>
    /// Get temperature for the city in Celsius
    /// </summary>
    int GetTemperature(string city);
}
