using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramBot.Domain.Accounting.Users;

namespace TelegramBot.Data.Accounting;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.TelegramUserId)
            .IsUnique();
        builder.HasIndex(x => x.CreatedAt)
            .IsDescending();

        builder.Property(x => x.Language)
            .HasMaxLength(10);

        builder.Property(x => x.AutoDetectLanguage)
            .HasDefaultValue(true);

        builder.HasMany(x => x.Invoices)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.Navigation(x => x.Roles)
            .AutoInclude();
    }
}
