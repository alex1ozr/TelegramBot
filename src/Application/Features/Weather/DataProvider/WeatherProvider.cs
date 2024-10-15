namespace TelegramBot.Application.Features.Weather.DataProvider;

internal sealed class WeatherProvider : IWeatherProvider
{
    public int GetTemperature(string city)
        => Random.Shared.Next(-10, 40);
}
