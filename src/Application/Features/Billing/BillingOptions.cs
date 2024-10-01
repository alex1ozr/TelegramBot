namespace TelegramBot.Application.Features.Billing;

public sealed class BillingOptions
{
    public const string OptionKey = "Billing";

    public bool IsTestMode { get; init; } = true;

    public static readonly DonationOption[] DonationOptions = new[]
    {
        new DonationOption(1, "1_star", "1 Star"),
        new DonationOption(5, "5_stars", "5 Stars"),
        new DonationOption(10, "10_stars", "10 Stars"),
        new DonationOption(50, "50_stars", "50 Stars"),
        new DonationOption(100, "100_stars", "100 Stars"),
    };
}
