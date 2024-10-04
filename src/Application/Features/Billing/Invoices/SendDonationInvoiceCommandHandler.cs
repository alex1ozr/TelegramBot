using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.Payments;
using Telegram.BotAPI.UpdatingMessages;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;
using TelegramBot.Domain.Billing.Invoices;
using TelegramBot.Domain.Exceptions;
using Invoice = TelegramBot.Domain.Billing.Invoices.Invoice;

namespace TelegramBot.Application.Features.Billing.Invoices;

internal sealed class SendDonationInvoiceCommandHandler : IRequestHandler<SendDonationInvoiceCommand, Unit>
{
    private readonly ILogger<SendDonationInvoiceCommandHandler> _logger;
    private readonly ITelegramBotClient _client;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IBotMessageLocalizer _botMessageLocalizer;
    private readonly IMediator _mediator;

    public SendDonationInvoiceCommandHandler(
        ILogger<SendDonationInvoiceCommandHandler> logger,
        ITelegramBotClient client,
        IInvoiceRepository invoiceRepository,
        IBotMessageLocalizer botMessageLocalizer,
        IMediator mediator)
    {
        _logger = logger;
        _client = client;
        _invoiceRepository = invoiceRepository;
        _botMessageLocalizer = botMessageLocalizer;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(SendDonationInvoiceCommand request, CancellationToken cancellationToken)
    {
        var message = request.Message;

        var user = await _mediator.Send(new GetUserInfoQuery(message.Chat.Id.ToString()), cancellationToken)
            .ConfigureAwait(false);
        UserNotFoundException.ThrowIfNull(user, $"User not found for the chat {message.Chat.Id}");

        var invoice = await CreateInvoiceAsync(
            message.Chat.Id.ToString(),
            user,
            request.DonationOption,
            cancellationToken)
            .ConfigureAwait(false);

        try
        {
            var text = _botMessageLocalizer.GetLocalizedString(
                nameof(BotMessages.DonationInvoiceDescription),
                user.Language);

            await _client.EditMessageTextAsync(
                message.Chat.Id,
                message.MessageId,
                text,
                parseMode: FormatStyles.HTML,
                replyMarkup: null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            var invoiceArgs = invoice.ToSendInvoiceArgs();
            await _client.SendInvoiceAsync(invoiceArgs, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        catch (BotRequestException exception)
        {
            await CancelInvoiceAsync(invoice, exception, cancellationToken).ConfigureAwait(false);

            var error = _botMessageLocalizer.GetLocalizedString(
                nameof(BotMessages.CannotCreateInvoiceError),
                user.Language);
            await _client.SendMessageAsync(
                message.Chat.Id,
                error,
                parseMode: FormatStyles.HTML,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        return Unit.Value;
    }

    private async Task<Invoice> CreateInvoiceAsync(
        string chatId,
        UserInfo user,
        DonationOption donationOption,
        CancellationToken cancellationToken)
    {
        const string currency = "XTR";

        var productName = _botMessageLocalizer.GetLocalizedString(
            nameof(BotMessages.Donation),
            user.Language);

        var description = _botMessageLocalizer.GetLocalizedString(
            nameof(BotMessages.DonationDescription),
            user.Language,
            donationOption.Amount,
            currency);

        var invoice = Invoice.Create(
            donationOption.InvoiceType,
            chatId,
            user.UserId,
            title: productName,
            description,
            currency,
            donationOption.Amount,
            startParameter: donationOption.Name);

        await _invoiceRepository.AddAsync(invoice, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation(
            "Invoice {InvoiceId} (Type: {InvoiceType}, Price: {Price} {Currency}) was created for user {UserId}",
            invoice.Id,
            invoice.Type,
            invoice.Price,
            invoice.Currency,
            invoice.UserId);

        return invoice;
    }

    private async Task CancelInvoiceAsync(Invoice invoice, Exception exception, CancellationToken cancellationToken)
    {
        invoice.SetCanceled();
        await _invoiceRepository.UpdateAsync(invoice, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation(exception, "Invoice {InvoiceId} was cancelled", invoice.Id);
    }
}
