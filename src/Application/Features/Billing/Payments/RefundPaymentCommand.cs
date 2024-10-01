using MediatR;
using TelegramBot.Domain.Billing.Invoices;

namespace TelegramBot.Application.Features.Billing.Payments;

public sealed record RefundPaymentCommand(InvoiceId InvoiceId) : IRequest<Unit>;
