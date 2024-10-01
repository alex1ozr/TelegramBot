using TelegramBot.Framework.Entities;

namespace TelegramBot.Domain.Billing.Invoices;

public interface IInvoiceRepository : IRepository<Invoice, InvoiceId>
{
}
