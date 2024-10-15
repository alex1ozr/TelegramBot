namespace TelegramBot.Application.Features.Weather.DataProvider;

/// <summary>
/// Current weather provider
/// </summary>
public interface IWeatherProvider
{
    /// <summary>
    /// Get temperature for the city
    /// </summary>
    int GetTemperature(string city);
}
