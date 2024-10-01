namespace TelegramBot.Application.Extensions;

internal static class DateTimeExtensions
{
    public static string ToFormattedString(this DateTime dateTime)
    {
        return dateTime.ToString("dd.MM.yyyy HH:mm");
    }

    public static DateTime ToMoscowTime(this DateTime dateTime)
    {
        // TODO Rewrite this method to use time zones and be compatible with mac/win/linux
        return dateTime.ToUniversalTime().AddHours(3);
    }
}
