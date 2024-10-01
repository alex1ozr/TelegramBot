namespace TelegramBot.Domain.Exceptions;

/// <summary>
/// Entity not found exception
/// </summary>
///<remarks>
/// All errors that occur in the process of getting an entity from the database must be inherited
/// from <see cref="EntityNotFoundException"/>.
/// </remarks>
public class EntityNotFoundException : ValidationException
{
    public EntityNotFoundException(string message, Exception? innerException = default)
        : base(message, innerException)
    {
    }

    public static new T ThrowIfNull<T>(T? argument, string message)
        where T : class
        => argument ?? throw new EntityNotFoundException(message);
}
