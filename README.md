# Telegram Bot Sample
A monolithic .NET application that demonstrates how to build a Telegram bot using the [Telegram.BotAPI](https://github.com/Eptagone/Telegram.BotAPI) library.

## Tech stack
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    - [Nullable reference types](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
    - [Central package management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [.NET Aspire 9](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) for running the application
- [Entity Framework Core 8](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew)
- [PostgreSQL](https://www.postgresql.org)
- [MediatR](https://github.com/jbogard/MediatR)
- [OpenTelemetry](https://opentelemetry.io) for logs, traces, and metrics

## Bot features
### Commands
- `/start` - Start the bot, register the user, and show the list of available commands.
- `/help` - Show the list of available commands.
- `/stats` - Show the statistics of the bot usage for the last 24 hours. Available only for users with the `Admin` role.
- `/weather` - Get the weather forecast for the specified city. This command uses the inline keyboard to select the city.
- `/set_language` - Set the language for the bot. This command uses the inline keyboard to select the language (Auto / English / Russian).
- `/privacy` - Show the privacy policy of the bot. The policy is GDPR compliant and includes information about the data that is collected and how it is used.
- `/donate` - Show information about how to donate [Telegram Stars](https://telegram.org/blog/telegram-stars) to the bot. This command uses the inline keyboard to select the donation amount. All donations will be refunded back to the user automatically.

## Prerequisites

### Main
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)

### Running API
In order to run the Host, you need to have a Telegram bot token.
- Create a new bot using the [BotFather](https://core.telegram.org/bots#6-botfather) and get the token.
- Set the `Telegram:BotToken` value in the Host's [appsettings.json](src/Host/appsettings.json) file.

## Startup project

### [AppHost](src/AppHost/)
Recommended way to run the application (Telegram bot host service) using .NET Aspire.

The PostgreSQL database will be started in a Docker container and the connection string will be set automatically.

### [Host](src/Host/)
Runs the application (Telegram bot host service) as a standalone .NET application.

Make sure to have a PostgreSQL database running and the connection string is set in the [appsettings.json](src/Host/appsettings.json) file.

## Implementation details
A detailed description of the implementation can be found in the [Implementation.md](docs/Implementation.md) file, including
- Architecture
- Database schema migration
- Analytics and Metrics
- Code Style
- Tests
- and more.
