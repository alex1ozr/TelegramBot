using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Application.Features.Weather.DataProvider;

namespace TelegramBot.Application.Features.Weather;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeather(this IServiceCollection services)
    {
        services.AddSingleton<IWeatherProvider, WeatherProvider>();
        return services;
    }
}
