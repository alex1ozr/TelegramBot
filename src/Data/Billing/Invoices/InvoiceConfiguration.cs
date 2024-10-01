using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramBot.Domain.Billing.Invoices;

namespace TelegramBot.Data.Billing.Invoices;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoices");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.ChatId);
        builder.HasIndex(x => x.Type);

        builder.Property(x => x.Title)
            .HasMaxLength(32);
        builder.Property(x => x.Description)
            .HasMaxLength(255);

        builder.Navigation(x => x.User)
            .AutoInclude();
    }
}
