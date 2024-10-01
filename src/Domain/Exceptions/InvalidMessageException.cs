using System.Diagnostics.CodeAnalysis;

namespace TelegramBot.Domain.Exceptions;

public sealed class InvalidMessageException : ValidationException
{
    public InvalidMessageException(string message, Exception? innerException = default)
        : base(message, innerException)
    {
    }

    public static new T ThrowIfNull<T>([NotNull] T? argument, string message)
        where T : class
        => argument ?? throw new EntityNotFoundException(message);
}
