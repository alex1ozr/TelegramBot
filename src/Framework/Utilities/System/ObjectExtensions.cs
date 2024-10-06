using System.Runtime.CompilerServices;

namespace TelegramBot.Framework.Utilities.System;

/// <summary>
/// Extensions for the <see cref="object"/>
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the value is <see langword="null"/>
    /// </summary>
    public static T Required<T>(
        this T? value,
        [CallerArgumentExpression("value")] string? paramName = default)
        where T : class
        => value ?? throw new ArgumentNullException(paramName);

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the value is <see langword="null"/>
    /// </summary>
    public static T Required<T>(
        this T? value,
        [CallerArgumentExpression("value")] string? paramName = default)
        where T : struct
        => value ?? throw new ArgumentNullException(paramName);
}
