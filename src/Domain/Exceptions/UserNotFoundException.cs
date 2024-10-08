using System.Diagnostics.CodeAnalysis;
using TelegramBot.Framework.Exceptions;

namespace TelegramBot.Domain.Exceptions;

public sealed class UserNotFoundException : ValidationException
{
    public UserNotFoundException(string message, Exception? innerException = default)
        : base(message, innerException)
    {
    }

    public static new T ThrowIfNull<T>([NotNull] T? argument, string message)
        where T : class
        => argument ?? throw new EntityNotFoundException(message);
}
