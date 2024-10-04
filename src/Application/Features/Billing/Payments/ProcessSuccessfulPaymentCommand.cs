using MediatR;
using Telegram.BotAPI.AvailableTypes;

namespace TelegramBot.Application.Features.Billing.Payments;

public sealed record ProcessSuccessfulPaymentCommand(Message Message) : IRequest<Unit>;
