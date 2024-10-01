using AuroraScienceHub.Framework.Entities;
using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Domain.Exceptions;

namespace TelegramBot.Domain.Billing.Invoices;

/// <summary>
/// Invoice
/// </summary>
public sealed class Invoice :
    IEntity<InvoiceId>,
    IAuditable
{
    private const int TitleMaxLength = 32;
    private const int DescriptionMaxLength = 255;

    private User? _user;

    private Invoice(
        InvoiceId id,
        InvoiceType type,
        InvoiceStatus status,
        string chatId,
        UserId? userId,
        string title,
        string description,
        string currency,
        int price,
        string startParameter,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        Type = type;
        Status = status;
        ChatId = chatId;
        UserId = userId;
        Title = title;
        Description = description;
        Currency = currency;
        Price = price;
        StartParameter = startParameter;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <inheritdoc />
    public InvoiceId Id { get; private set; }

    /// <summary>
    /// Invoice type
    /// </summary>
    public InvoiceType Type { get; private set; }

    /// <summary>
    /// Status
    /// </summary>
    public InvoiceStatus Status { get; private set; }

    /// <summary>
    /// Telegram Chat Id
    /// </summary>
    public string ChatId { get; private set; }

    /// <summary>
    /// Telegram Message Id
    /// </summary>
    public string? MessageId { get; private set; }

    /// <summary>
    /// User Id
    /// </summary>
    public UserId? UserId { get; private set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Currency
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    /// Price
    /// </summary>
    public int Price { get; private set; }

    /// <summary>
    /// Start parameter
    /// </summary>
    public string StartParameter { get; private set; }

    /// <summary>
    /// Telegram Payment Id
    /// </summary>
    public string? TelegramPaymentChargeId { get; private set; }

    /// <inheritdoc />
    public DateTime CreatedAt { get; private set; }

    /// <inheritdoc />
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Invoice
    /// </summary>
    public User User
    {
        get => UnexpectedException.ThrowIfNull(_user, "User is not set");
        private set => _user = value;
    }

    public static Invoice Create(
        InvoiceType invoiceType,
        string chatId,
        UserId? userId,
        string title,
        string description,
        string currency,
        int price,
        string startParameter)
    {
        ValidationException.ThrowIfNot(title.Length <= TitleMaxLength,
            $"{nameof(Title)} length should be less than or equal to {TitleMaxLength}");

        ValidationException.ThrowIfNot(description.Length <= DescriptionMaxLength,
            $"{nameof(Description)} length should be less than or equal to {DescriptionMaxLength}");

        var utcNow = TimeProvider.System.GetUtcNow().UtcDateTime;
        return new Invoice(
            InvoiceId.New(),
            invoiceType,
            InvoiceStatus.PaymentPending,
            chatId,
            userId,
            title,
            description,
            currency,
            price,
            startParameter,
            createdAt: utcNow,
            updatedAt: utcNow);
    }

    public void SetCanceled()
    {
        Status = InvoiceStatus.Canceled;
    }

    public void SetPaid(string paymentChargeId)
    {
        Status = InvoiceStatus.Paid;
        TelegramPaymentChargeId = paymentChargeId;
    }

    public void SetRefunded()
    {
        if (string.IsNullOrWhiteSpace(TelegramPaymentChargeId))
        {
            throw new InvalidOperationException($"Invoice {Id} is not paid to be refunded");
        }

        Status = InvoiceStatus.Refunded;
    }
}
