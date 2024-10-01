using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Billing.Payments;

namespace TelegramBot.Application.Infrastructure.Bot;

partial class SWeatherBot
{
    protected override async Task OnMessageAsync(
        Message message,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(message.Text) && message.Text.StartsWith('/'))
        {
            await base.OnMessageAsync(message, cancellationToken);
        }
        else if (message.SuccessfulPayment != null)
        {
            await _mediator.Send(new ProcessSuccessfulPaymentCommand(message), cancellationToken);
        }
        else
        {
            /*
            var state = GetCurrentState(message.Chat.Id);

            if (state == null)
            {
                var text = string.Format(MSG.Help, Username);
                _client.SendMessage(message.Chat.Id, text);
            }
            else
            {
                OnUserState(message, state);
            }
            */
        }
    }
}
