namespace TelegramBot.Domain.Billing.Invoices;

/// <summary>
/// Invoice Status
/// </summary>
public enum InvoiceStatus
{
    Unknown,
    PaymentPending,
    Paid,
    Refunded,
    Canceled
}
