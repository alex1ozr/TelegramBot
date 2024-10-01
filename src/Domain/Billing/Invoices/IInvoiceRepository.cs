using AuroraScienceHub.Framework.Entities.Storage;

namespace TelegramBot.Domain.Billing.Invoices;

public interface IInvoiceRepository : IRepository<Invoice, InvoiceId>
{
}
