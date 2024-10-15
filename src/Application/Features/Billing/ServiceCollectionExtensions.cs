using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.Application.Features.Billing;

internal static class ServiceCollectionExtensions
{
    public static void AddBilling(this IServiceCollection services)
    {
        services.AddOptions<BillingOptions>()
            .BindConfiguration(BillingOptions.OptionKey);
    }
}
