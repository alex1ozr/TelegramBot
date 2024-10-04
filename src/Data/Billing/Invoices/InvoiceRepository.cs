using TelegramBot.Data.Engine;
using TelegramBot.Domain.Billing.Invoices;
using TelegramBot.Framework.EntityFramework.Repositories;

namespace TelegramBot.Data.Billing.Invoices;

internal sealed class InvoiceRepository :
    DefaultRepository<DataContext, Invoice, InvoiceId>,
    IInvoiceRepository
{
    public InvoiceRepository(DataContext context)
        : base(context)
    {
    }
}
