using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TelegramBot.Domain.Accounting.Roles;
using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Domain.Billing.Invoices;
using TelegramBot.Framework.EntityFramework;
using TelegramBot.Framework.EntityFramework.Identifiers;

namespace TelegramBot.Data.Engine;

public sealed class DataContext : DbContext, IDataContext
{
    private const string Schema = "tbot";

    static string IDataContext.Schema => Schema;

    private readonly IReadOnlyList<IInterceptor> _interceptors;

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Invoice> Invoices => Set<Invoice>();

    public DataContext(
        DbContextOptions<DataContext> options,
        IEnumerable<IInterceptor> interceptors)
        : base(options)
    {
        _interceptors = interceptors.ToList();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .AddInterceptors(_interceptors)
            .UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseIdentifierConverter();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}
