using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Domain.Analytics;

namespace TelegramBot.Data.Analytics;

public sealed class UserCommandConfiguration : IEntityTypeConfiguration<UserCommand>
{
    public void Configure(EntityTypeBuilder<UserCommand> builder)
    {
        builder.ToTable("user_commands");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.CreatedAt);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
