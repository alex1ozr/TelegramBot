using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Billing.Payments;

namespace TelegramBot.Application.Infrastructure.Bot;

partial class WeatherBot
{
    protected override async Task OnMessageAsync(
        Message message,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(message.Text) && message.Text.StartsWith('/'))
        {
            await base.OnMessageAsync(message, cancellationToken).ConfigureAwait(false);
        }
        else if (message.SuccessfulPayment != null)
        {
            await _mediator.Send(new ProcessSuccessfulPaymentCommand(message), cancellationToken).ConfigureAwait(false);
        }
    }
}
