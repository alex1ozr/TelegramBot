using TelegramBot.Domain.Billing.Invoices;

namespace TelegramBot.Application.Features.Billing;

public record DonationOption(int Amount, string Name, string Description)
{
    public InvoiceType InvoiceType => InvoiceType.Donation;
}
