using System.Diagnostics.CodeAnalysis;
using TelegramBot.Domain.Billing.Invoices;

namespace TelegramBot.Domain.Exceptions;

public sealed class InvoiceNotFoundException : ValidationException
{
    public InvoiceId? InvoiceId { get; }

    public InvoiceNotFoundException(InvoiceId invoiceId, Exception? innerException = default)
        : base($"Invoice {invoiceId} not found", innerException)
    {
        InvoiceId = invoiceId;
    }

    public InvoiceNotFoundException(string message, Exception? innerException = default)
        : base(message, innerException)
    {
    }

    public static new T ThrowIfNull<T>([NotNull] T? argument, string message)
        where T : class
        => argument ?? throw new EntityNotFoundException(message);
}
