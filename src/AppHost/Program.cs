using TelegramBot.ServiceDefaults;

var builder = DistributedApplication.CreateBuilder(args);

var postgresDb = builder.AddPostgres();

var host = builder.AddProject<Projects.Host>("Host")
    .WithReference(postgresDb);

builder.Build().Run();
