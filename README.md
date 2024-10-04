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
- [OpenTelemetry](https://opentelemetry.io) for logs, traces, and metrics

## Bot features
### Commands
- `/start` - Start the bot, register the user and show the list of available commands
- `/help` - Show the list of available commands
- `/stats` - Show the statistics of the bot usage for the last 24 hours. Available only for the users with the `Admin` role
- `/weather` - Get the weather forecast for the specified city. This command uses the inline keyboard to select the city.
- `/set_language` - Set the language for the bot. This command uses the inline keyboard to select the language (Auto / English / Russian).
- `/privacy` - Show the privacy policy of the bot. Policy is GDPR compliant and includes the information about the data that is collected and how it is used.
- `/donate` - Show the information about how to donate [Telegram Stars](https://telegram.org/blog/telegram-stars) to the bot. This command uses the inline keyboard to select the donation amount.
All the donations will be refunded back to the user automatically.

### Implementation specifics
- The bot uses the [Telegram.BotAPI](https://github.com/Eptagone/Telegram.BotAPI) library to interact with the [Telegram API](https://core.telegram.org/bots/api).
- Weather data is generated randomly and is not real.
- Entity Framework Core is used to store the user data in the PostgreSQL database in the following tables:
    - `users` - All the users that have interacted with the bot. Also contains the information about the user's language preference.
    -  `roles` - All the roles that the users can have
    - `user_role` - Implements the many-to-many relationship between the `users` and `roles` tables
    - `user_commands` - All the commands that the users have executed
    - `invoices` - All the invoices that the users have created / paid / refunded.
- The application uses the [MediatR](https://github.com/jbogard/MediatR) library to make commands more testable and maintainable.
  - Each bot command or callback is implemented as a separate MediatR command and is handled by the corresponding handler.
- The application supports multiple languages (English, Russian) and the user can set the preferred language using the `/set_language` command.
- The application uses polling to receive updates from the Telegram API. The polling interval is set to 100 ms.
  - It is recommended to use the webhook instead of polling in the production environment. See [ShopBot](https://github.com/Eptagone/ShopBot) example for more details.

## Prerequisites

### Main
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [.NET Aspire workload](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)
    - Execute `dotnet workload update` to update the installed workloads
    - Execute `dotnet workload install aspire` to install the Aspire workload
- [Docker](https://www.docker.com/get-started)

### Running API
In order to run the Host, you need to have a Telegram bot token.
- Create a new bot using the [BotFather](https://core.telegram.org/bots#6-botfather) and get the token.
- Set the `Telegram:BotToken` value in the Host's [appsettings.json](src/Host/appsettings.json) file.

## Startup project

### [AppHost](src/AppHost/)
Recommended way to run the application (Telegram bot host service) using .NET Aspire.

PostgreSQL database will be started in a Docker container and the connection string will be set automatically.

### [Host](src/Host/)
Runs the application (Telegram bot host service) as standalone .NET application.

Make sure to have a PostgreSQL database running and the connection string is set in the [appsettings.json](src/Host/appsettings.json) file.

## Health checks
- `GET /alive` - Returns `200 OK` and 'Healthy' message if the application is healthy.
- `GET /health` - Returns detailed health information about the application and its dependencies.

## Database schema migration

```shell
# Install EF utils
dotnet tool install --global dotnet-ef

# Create a new schema migration
dotnet ef migrations add Init -c DataContext -s src/Data/Data.csproj -o ./Engine/Migrations/Schema -v -- --CreateMigrationOnly
```

## Analytics and metrics
The application uses [OpenTelemetry](https://opentelemetry.io) for metrics, logs, and traces.
All of them are sent to .NET Aspire's [OpenTelemetry Collector](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/telemetry)
and are available in the [Dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/overview?tabs=bash).

Analytics include the following:
- Messages that are sent to the bot by users
- `/stats` bot command that returns 24h statistics of the bot usage

Metrics include the following:
- `telegram_bot.commands_executed` - Counter of commands executed by the bot
- `telegram_bot.commands_failed` - Counter of commands failed to execute by the bot
- `telegram_bot.users_created` - Counter of new users that have interacted with the bot

## Code style
The solution uses [EditorConfig](.editorconfig) nested from [Azure SDK .NET](https://github.com/Azure/azure-sdk-for-net/blob/main/.editorconfig) to maintain a consistent code style.

- In order to enforce the code style, it is recommended to execute the `dotnet format` command before pushing the changes.
- It is also possible to use the `dotnet format --verify-no-changes` command to check if the code style is consistent.
