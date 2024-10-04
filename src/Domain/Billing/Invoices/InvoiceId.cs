using TelegramBot.Framework.Entities.Identifiers;

namespace TelegramBot.Domain.Billing.Invoices;

/// <summary>
/// Invoice Id
/// </summary>
public sealed class InvoiceId :
    EntityIdentifier<InvoiceId>,
    IIdentifierWithPrefix
{
    private InvoiceId(string value)
        : base(value)
    {
    }

    public static string Prefix { get; } = FormatPrefix("tb", "inv");
}
