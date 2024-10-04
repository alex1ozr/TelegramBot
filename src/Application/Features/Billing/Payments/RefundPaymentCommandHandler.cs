using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.Payments;
using TelegramBot.Application.Extensions;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;
using TelegramBot.Domain.Billing.Invoices;
using TelegramBot.Domain.Exceptions;
using TelegramBot.Framework.Entities;
using TelegramBot.Framework.Exceptions;

namespace TelegramBot.Application.Features.Billing.Payments;

internal sealed class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand, Unit>
{
    private readonly ILogger<RefundPaymentCommandHandler> _logger;
    private readonly ITelegramBotClient _client;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IBotMessageLocalizer _botMessageLocalizer;

    public RefundPaymentCommandHandler(
        ILogger<RefundPaymentCommandHandler> logger,
        ITelegramBotClient client,
        IInvoiceRepository invoiceRepository,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _logger = logger;
        _client = client;
        _invoiceRepository = invoiceRepository;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task<Unit> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId, cancellationToken).ConfigureAwait(false)
                      ?? throw new InvoiceNotFoundException(request.InvoiceId);

        if (invoice.Status != InvoiceStatus.Paid
            || string.IsNullOrWhiteSpace(invoice.TelegramPaymentChargeId))
        {
            throw new InvoiceNotPaidException(request.InvoiceId);
        }
        if (!long.TryParse(invoice.ChatId, out var telegramUserId))
        {
            throw new ValidationException($"Invalid telegram user id {invoice.UserId}");
        }

        await _client.RefundStarPaymentAsync(
            telegramUserId,
            invoice.TelegramPaymentChargeId, cancellationToken)
            .ConfigureAwait(false);

        invoice.SetRefunded();
        await _invoiceRepository.UpdateAsync(invoice, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation(
            "The payment for invoice {InvoiceId} (Type: {InvoiceType}, Price: {Price} {Currency}) was refunded to user {UserId}",
            invoice.Id,
            invoice.Type,
            invoice.Price,
            invoice.Currency,
            invoice.UserId);

        var text = _botMessageLocalizer.GetLocalizedString(
            nameof(BotMessages.SuccessfulRefundMessage),
            invoice.User.GetBotLanguage());

        await _client.SendMessageAsync(invoice.ChatId, text, cancellationToken: cancellationToken).ConfigureAwait(false);

        return Unit.Value;
    }
}
