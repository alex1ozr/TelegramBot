namespace TelegramBot.ServiceDefaults;

internal static class DistributedApplicationBuilderExtensions
{
    public static IResourceBuilder<PostgresDatabaseResource> AddPostgres(this IDistributedApplicationBuilder builder)
    {
        const string resourceName = "TelegramBotDb";
        const string dbName = "TelegramBot";

        // Postgres must also have a stable password and a named volume
        var dbPassword = builder.AddParameter("postgrespassword");

        var postgresDb = builder
            .AddPostgres(resourceName, password: dbPassword)
            .WithImageTag("17.0-alpine")
            .WithDataVolume()
            .WithPgAdmin()
            .AddDatabase(dbName);

        return postgresDb;
    }
}
