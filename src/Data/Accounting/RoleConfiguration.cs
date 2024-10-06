using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramBot.Domain.Accounting.Roles;

namespace TelegramBot.Data.Accounting;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasIndex(x => x.NormalizedName)
            .IsUnique();

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Roles)
            .UsingEntity("user_role");
    }
}
