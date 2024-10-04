# Telegram Bot Sample
A monolithic .NET application that demonstrates how to build a Telegram bot using the [Telegram.BotAPI](https://github.com/Eptagone/Telegram.BotAPI) library.

## Tech stack
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
    - [Nullable reference types](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
    - [Central package management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) for running the application
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

### Implementation specifics
- The bot uses the [Telegram.BotAPI](https://github.com/Eptagone/Telegram.BotAPI) library to interact with the [Telegram API](https://core.telegram.org/bots/api).
- Weather data is generated randomly and is not real.
- Entity Framework Core is used to store user data in the PostgreSQL database in the following tables:
    - `users` - All the users that have interacted with the bot. Also contains information about the user's language preference.
    - `roles` - All the roles that users can have.
    - `user_role` - Implements the many-to-many relationship between the `users` and `roles` tables.
    - `user_commands` - All the commands that users have executed.
    - `invoices` - All the invoices that users have created, paid, or refunded.
- All entities use value objects for identifiers. The value object is a string that contains the prefix and the unique identifier.
    - Useful links for context and problem statement:
        - [5 Tips for API Design](https://codeopinion.com/want-to-build-a-good-api-here-are-5-tips-for-api-design/)
        - [How to (and how not to) design REST APIs](https://github.com/stickfigure/blog/wiki/How-to-(and-how-not-to)-design-REST-APIs?ref=vladimir-ivanov-dev-blog#rule-6-do-use-strings-for-all-identifiers)
- The application uses the [MediatR](https://github.com/jbogard/MediatR) library to make commands more testable and maintainable.
    - Each bot command or callback is implemented as a separate MediatR command and is handled by the corresponding handler.
- The application supports multiple languages (English, Russian) and the user can set the preferred language using the `/set_language` command.
- The application uses polling to receive updates from the Telegram API. The polling interval is set to 100 ms.
    - It is recommended to use the webhook instead of polling in the production environment. See [ShopBot](https://github.com/Eptagone/ShopBot) example for more details.
- `ConfigureAwait(false)` is used in asynchronous methods to avoid deadlocks as recommended by [Microsoft](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2007).
    - The CA2007 analyzer is enabled as errors in the [Directory.Build.props](Directory.Build.props) file to force the usage of `ConfigureAwait(false)` in the code.

## Prerequisites

### Main
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [.NET Aspire workload](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)
    - Execute `dotnet workload update` to update the installed workloads.
    - Execute `dotnet workload install aspire` to install the Aspire workload.
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

## Health checks
- `GET /alive` - Returns `200 OK` and a 'Healthy' message if the application is healthy.
- `GET /health` - Returns detailed health information about the application and its dependencies.

## Database schema migration

```shell
# Install EF utils
dotnet tool install --global dotnet-ef

# Create a new schema migration
dotnet ef migrations add Init -c DataContext -s src/Data/Data.csproj -o ./Engine/Migrations/Schema -v -- --CreateMigrationOnly
```

## Analytics and Metrics
The application uses [OpenTelemetry](https://opentelemetry.io) for metrics, logs, and traces.
All of these are sent to .NET Aspire's [OpenTelemetry Collector](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/telemetry)
and are available in the [Dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/overview?tabs=bash).

Analytics include the following:
- Messages sent to the bot by users
- The `/stats` bot command, which returns 24-hour statistics on bot usage

Metrics include the following:
- `telegram_bot.commands_executed` - A counter of commands executed by the bot
- `telegram_bot.commands_failed` - A counter of commands that failed to execute
- `telegram_bot.users_created` - A counter of new users who have interacted with the bot

## Code Style
The solution uses [EditorConfig](.editorconfig), inherited from [Azure SDK .NET](https://github.com/Azure/azure-sdk-for-net/blob/main/.editorconfig), to maintain a consistent code style.

- To enforce the code style, it is recommended to run the `dotnet format` command before pushing changes.
- You can also use the `dotnet format --verify-no-changes` command to check if the code style is consistent.
