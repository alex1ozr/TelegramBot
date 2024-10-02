# Telegram Bot Sample
A monolith .NET application that demonstrates how to build a Telegram bot using the [Telegram.BotAPI](https://github.com/Eptagone/Telegram.BotAPI) library.

## Tech stack
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    - [Nullable reference types](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
    - [Central package management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) for running the application
- [Entity Framework Core 8](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew)
- [PostgreSql](https://www.postgresql.org)
- [MediatR](https://github.com/jbogard/MediatR)
- [Redis](https://redis.io) for caching
- [OpenTelemetry](https://opentelemetry.io) for logs, traces, and metrics

## Prerequisites

### Main
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [.NET Aspire workload](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)
    - Execute `dotnet workload update` to update the installed workloads
    - Execute `dotnet workload install aspire` to install the Aspire workload
- [Docker](https://www.docker.com/get-started)

### Running API
In order to run the Host, you need to have a PostgreSQL database running and a Telegram bot token.

## Startup project

### [AppHost](src/AppHost/)
Runs the application (Telegram bot host service) using .NET Aspire.

### [Host](src/Host/)
Runs the application (Telegram bot host service) as standalone .NET application.

#### Health checks
- `GET /alive` - Returns `200 OK` and 'Healthy' message if the application is healthy.
- `GET /health` - Returns detailed health information about the application and its dependencies.

### Database schema migration

```shell
# Install EF utils
dotnet tool install --global dotnet-ef

# Create a new schema migration
dotnet ef migrations add Init -c DataContext -s src/Data/Data.csproj -o ./Engine/Migrations/Schema -v -- --CreateMigrationOnly
```

## Analytics and metrics
The application uses [ClickHouse](https://clickhouse.tech) for analytics and [OpenTelemetry](https://opentelemetry.io) for metrics.
More details can be found in the
- [docs/implementation/Analytics.md](docs/implementation/Analytics.md)
- [docs/implementation/Metrics.md](docs/implementation/Metrics.md).

## Code style
The solution uses [EditorConfig](.editorconfig) nested from [Azure SDK .NET](https://github.com/Azure/azure-sdk-for-net/blob/main/.editorconfig) to maintain a consistent code style.

- In order to enforce the code style, it is recommended to execute the `dotnet format` command before pushing the changes.
- It is also possible to use the `dotnet format --verify-no-changes` command to check if the code style is consistent.
