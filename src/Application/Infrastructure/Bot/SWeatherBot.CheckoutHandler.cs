using Telegram.BotAPI.Payments;

namespace TelegramBot.Application.Infrastructure.Bot;

partial class SWeatherBot
{
    protected override async Task OnPreCheckoutQueryAsync(PreCheckoutQuery pQuery, CancellationToken cancellationToken)
    {
        // We accept all pre-checkout queries
        await PaymentsExtensions.AnswerPreCheckoutQueryAsync(_client, pQuery.Id, true, cancellationToken: cancellationToken);
    }
}
