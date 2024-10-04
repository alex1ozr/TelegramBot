using System.Collections.ObjectModel;

namespace TelegramBot.Framework.Exceptions;

/// <summary>
/// Validation exception
/// </summary>
public class ValidationException : FrameworkException
{
    private const string DefaultMessage = "Invalid data";

    public readonly InvalidParameterReason Reasons;

    public ValidationException(InvalidParameterReason reasons)
        : this(DefaultMessage, reasons)
    {
    }

    public ValidationException(string message, InvalidParameterReason reasons)
        : this(message, reasons, null)
    {
    }

    public ValidationException(string message, Exception? innerException = default)
        : this(message, new InvalidParameterReason(), innerException)
    {
    }

    public ValidationException(string message, InvalidParameterReason reasons, Exception? innerException)
        : base(message, innerException)
    {
        Reasons = reasons;
    }

    public static void ThrowIfNot(bool isValid, string message)
    {
        if (!isValid)
        {
            throw new ValidationException(message);
        }
    }

    public static T ThrowIfNull<T>(T? argument, string message)
        where T : class
        => argument ?? throw new ValidationException(message);

    public static T ThrowIfNull<T>(T? argument, string message)
        where T : struct
        => argument ?? throw new ValidationException(message);

    public static string ThrowIfWhiteSpace(string? argument, string message)
    {
        if (String.IsNullOrWhiteSpace(argument))
        {
            throw new ValidationException(message);
        }

        return argument;
    }
}

/// <summary>
/// Invalid data description
/// </summary>
public sealed class InvalidParameterReason : Dictionary<string, List<string>>
{
    public InvalidParameterReason()
    {
    }

    public InvalidParameterReason(string name, string reason)
    {
        AddReason(name, reason);
    }

    public InvalidParameterReason AddReason(string name, string reason)
    {
        if (!TryGetValue(name, out var reasons))
        {
            reasons = new List<string>();
            Add(name, reasons);
        }

        reasons.Add(reason);

        return this;
    }

    public IReadOnlyDictionary<string, IReadOnlyList<string>> ToReadOnly() => this
        .ToDictionary(pair => pair.Key, pair => (IReadOnlyList<string>)new ReadOnlyCollection<string>(pair.Value));
}

