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
            .AddPostgres(resourceName, port: 5432, password: dbPassword)
            .WithImageTag("17.0-alpine")
            .WithDataVolume()
            .WithPgAdmin()
            .WithEnvironment("POSTGRES_DB", dbName)
            .AddDatabase(dbName);

        return postgresDb;
    }
}
