using TelegramBot.Domain.Billing.Invoices;

namespace TelegramBot.Domain.Exceptions;

public sealed class InvoiceNotPaidException : ValidationException
{
    public InvoiceId? InvoiceId { get; }

    public InvoiceNotPaidException(InvoiceId invoiceId, Exception? innerException = default)
        : base($"Invoice {invoiceId} is not paid", innerException)
    {
        InvoiceId = invoiceId;
    }

    public InvoiceNotPaidException(string message, Exception? innerException = default)
        : base(message, innerException)
    {
    }
}
