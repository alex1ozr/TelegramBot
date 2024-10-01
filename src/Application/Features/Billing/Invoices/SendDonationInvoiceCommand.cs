using MediatR;
using Telegram.BotAPI.AvailableTypes;

namespace TelegramBot.Application.Features.Billing.Invoices;

public sealed record SendDonationInvoiceCommand(MaybeInaccessibleMessage Message, DonationOption DonationOption)
    : IRequest<Unit>;
