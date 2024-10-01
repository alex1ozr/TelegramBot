using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using TelegramBot.Application.Extensions;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;
using TelegramBot.Domain.Billing.Invoices;
using TelegramBot.Domain.Exceptions;
using TelegramBot.Framework.Entities;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Application.Features.Billing.Payments;

internal sealed class ProcessSuccessfulPaymentCommandHandler : IRequestHandler<ProcessSuccessfulPaymentCommand, Unit>
{
    private readonly ILogger<ProcessSuccessfulPaymentCommandHandler> _logger;
    private readonly ITelegramBotClient _client;
    private readonly IMediator _mediator;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IOptions<BillingOptions> _billingOptions;
    private readonly IBotMessageLocalizer _botMessageLocalizer;

    public ProcessSuccessfulPaymentCommandHandler(
        ILogger<ProcessSuccessfulPaymentCommandHandler> logger,
        ITelegramBotClient client,
        IMediator mediator,
        IInvoiceRepository invoiceRepository,
        IOptions<BillingOptions> billingOptions,
        IBotMessageLocalizer botMessageLocalizer)
    {
        _logger = logger;
        _client = client;
        _mediator = mediator;
        _invoiceRepository = invoiceRepository;
        _billingOptions = billingOptions;
        _botMessageLocalizer = botMessageLocalizer;
    }

    public async Task<Unit> Handle(ProcessSuccessfulPaymentCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;
        InvalidMessageException.ThrowIfNull(
            message.SuccessfulPayment,
            $"Message {message.MessageId} does not contain {message.SuccessfulPayment}");

        var payment = message.SuccessfulPayment;
        try
        {
            var invoiceId = InvoiceId.Parse(payment.InvoicePayload);
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId, cancellationToken)
                          ?? throw new InvoiceNotFoundException(invoiceId);
            var userId = invoice.UserId.Required();

            invoice.SetPaid(payment.TelegramPaymentChargeId);
            await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

            _logger.LogInformation(
                "Invoice {InvoiceId} (Type: {InvoiceType}, Price: {Price} {Currency}) was paid by user {UserId}",
                invoice.Id,
                invoice.Type,
                invoice.Price,
                invoice.Currency,
                userId);

            var text = _botMessageLocalizer.GetLocalizedString(
                nameof(BotMessages.ThanksForSupportMessage),
                invoice.User.GetBotLanguage());

            await _client.SendMessageAsync(message.Chat.Id, text, cancellationToken: cancellationToken);
        }
        finally
        {
            if (_billingOptions.Value.IsTestMode)
            {
                await _mediator.Send(new RefundPaymentCommand(InvoiceId.Parse(payment.InvoicePayload)),
                    cancellationToken);
            }
        }

        return Unit.Value;
    }
}
