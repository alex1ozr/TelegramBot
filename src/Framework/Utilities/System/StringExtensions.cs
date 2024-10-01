using System.Globalization;
using System.Text.RegularExpressions;

namespace TelegramBot.Framework.Utilities.System;

/// <summary>
/// Extensions for <see cref="string"/>
/// </summary>
public static partial class StringExtensions
{
    static readonly Regex s_pascalToKebabCaseRegex =
        new("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// Trims the specified string or returns <see langword="null"/> if it is <see langword="null"/> or whitespace
    /// </summary>
    public static string? TrimOrDefault(this string? inputString)
    {
        return string.IsNullOrWhiteSpace(inputString)
            ? default
            : inputString.Trim();
    }

    /// <summary>
    /// Converts the specified text from PascalCase to kebab-case
    /// </summary>
    public static string PascalToKebabCase(this string text)
    {
        return string.IsNullOrWhiteSpace(text)
            ? text
            : s_pascalToKebabCaseRegex.Replace(text, "-$1")
                .Trim()
                .ToLower();
    }

    /// <summary>
    /// Converts the specified text from PascalCase to snake_case
    /// </summary>
    public static string PascalToLowerSnakeCase(this string text)
    {
        return string.IsNullOrWhiteSpace(text)
            ? text
            : s_pascalToKebabCaseRegex.Replace(text, "_$1")
                .Trim()
                .ToLower();
    }

    /// <summary>
    /// Parses the specified value as a float
    /// </summary>
    public static float ParseFloatInvariant(this string value)
        => float.Parse(value, NumberFormatInfo.InvariantInfo);
}
