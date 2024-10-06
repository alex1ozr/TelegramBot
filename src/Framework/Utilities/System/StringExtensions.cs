namespace TelegramBot.Framework.Utilities.System;

/// <summary>
/// Extensions for the <see cref="string"/>
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Splits the specified text into chunks of the specified size
    /// </summary>
    public static IEnumerable<string> Split(this string text, int chunkSize)
    {
        if (string.IsNullOrEmpty(text))
        {
            yield break;
        }

        for (var i = 0; i < text.Length; i += chunkSize)
        {
            yield return text.Substring(i, Math.Min(chunkSize, text.Length - i));
        }
    }
}
