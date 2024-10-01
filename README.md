# SWeather-bot
A monolith .NET application for space weather data processing and analysis.

## Tech stack
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    - [Nullable reference types](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
    - [Central package management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [Entity Framework Core 8](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew)
- [PostgreSql](https://www.postgresql.org)
- [MediatR](https://github.com/jbogard/MediatR)
- [Redis](https://redis.io) for caching
- [ClickHouse](https://clickhouse.tech) for analytics
- [OpenTelemetry](https://opentelemetry.io)
    - [Prometheus](https://prometheus.io) for metrics
    - [Seq](https://datalust.co/seq) for logs & traces
    - [Grafana](https://grafana.com) for dashboards
- [NukeBuild](https://nuke.build) for build automation (CI pipeline)

## Prerequisites

### Main
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- Configured the AuroraScienceHub NuGet source (see [NuGetPackages.md](docs/Implementation/NuGetPackages.md))

### Running API
In order to run the Host, you need to have a PostgreSQL database running.

## Startup project

### [Host](src/Host/)
Runs the application (Telegram bot host service).

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

## Build
The solution uses [NukeBuild](https://nuke.build) for build automation.
More information about all build options can be found in the [build/README.md](build/README.md).

Before building the solution, it is recommended to install the Nuke global tool:
```shell
dotnet tool install Nuke.GlobalTool --global
```

In order to build the solution, check code formatting and execute all kinds of tests (unit, architecture, integration), run the following command:
```shell
nuke --NuGetApiUrl <NuGet_server_url> --NuGetApiUser <NuGet_user> --NuGetApiKey <NuGet_api_key> --DockerRegistryUrl <docker_registry_url> --BuildCounter <build_number>
```

## Deploy
The application can be deployed using docker compose.
It is automated by the [RunDockerCompose.sh](deploy/RunDockerCompose.sh) script.

More information can be found in the [deploy/README.md](deploy/README.md).

## Analytics and metrics
The application uses [ClickHouse](https://clickhouse.tech) for analytics and [OpenTelemetry](https://opentelemetry.io) for metrics.
More details can be found in the
- [docs/implementation/Analytics.md](docs/implementation/Analytics.md)
- [docs/implementation/Metrics.md](docs/implementation/Metrics.md).

## Code style
The solution uses [EditorConfig](.editorconfig) nested from [Azure SDK .NET](https://github.com/Azure/azure-sdk-for-net/blob/main/.editorconfig) to maintain a consistent code style.

- In order to enforce the code style, it is recommended to execute the `dotnet format` command before pushing the changes.
- It is also possible to use the `dotnet format --verify-no-changes` command to check if the code style is consistent.

## Architectural Decision Records
The solution uses [ADR](https://adr.github.io) to document architectural decisions.
- The default template is [MADR](https://adr.github.io/madr/) (Markdown Any Decision Records).
- The records are stored in the [docs/decisions](docs/decisions) directory.

### Prerequisites
Before creating a new ADR, it is recommended to install the `adr-tools`:
```shell
dotnet tool install -g adr
```
If you have previously installed `adr` and want to update to the latest version, use the following command:
```shell
dotnet tool update -g adr
```

### Creating a new ADR
To create a new ADR, use the following command:
```shell
adr new 'Some decision' -p ./docs/decisions
```
