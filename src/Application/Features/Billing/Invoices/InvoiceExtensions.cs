using Telegram.BotAPI.Payments;
using Invoice = TelegramBot.Domain.Billing.Invoices.Invoice;

namespace TelegramBot.Application.Features.Billing.Invoices;

internal static class InvoiceExtensions
{
    public static SendInvoiceArgs ToSendInvoiceArgs(this Invoice invoice)
        => new(
            invoice.ChatId,
            invoice.Title,
            invoice.Description,
            payload: invoice.Id.ToString(),
            invoice.Currency,
            prices: new LabeledPrice[] { new("Total", invoice.Price), }
        )
        { ProviderToken = string.Empty, StartParameter = invoice.StartParameter, };
}
