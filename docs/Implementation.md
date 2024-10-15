# Implementation details

## Architecture
The application is a monolithic .NET application with a layered architecture.
- All `Framework` projects contain shared code and utilities.
- The `Domain` layer contains the domain entities and value objects.
- The `Data` layer contains the database context and repositories.
- The `Application` layer contains the application services and MediatR commands.
- The `Host` layer contains the Telegram bot host service.
- The `AppHost` layer contains the .NET Aspire host service.

## Technical details
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

## Database schema migration

To create a new schema migration, use the following command:
```shell
# Install EF utils if not installed
dotnet tool install --global dotnet-ef

# Create a new schema migration
dotnet ef migrations add Init -c DataContext -s src/Data/Data.csproj -o ./Engine/Migrations/Schema -v -- --CreateMigrationOnly
```

## Analytics and Metrics
The application uses [OpenTelemetry](https://opentelemetry.io) for metrics, logs, and traces.
All of these are sent to .NET Aspire's [OpenTelemetry Collector](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/telemetry)
and are available in the [Dashboard](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/overview?tabs=bash).

#### Analytics include the following:
- Messages sent to the bot by users
- The `/stats` bot command, which returns 24-hour statistics on bot usage

#### Metrics include the following:
- `telegram_bot.commands_executed` - A counter of commands executed by the bot
- `telegram_bot.commands_failed` - A counter of commands that failed to execute
- `telegram_bot.users_created` - A counter of new users who have interacted with the bot

## Tests
The application contains unit tests for the `Application` layer using the [xUnit](https://xunit.net) testing framework.
- The tests are located in the [UnitTests](../tests/UnitTests) project.
- !!! The tests are not exhaustive and are meant to demonstrate how to write unit tests for the application.
  - Dependency injection tests: [MediatrRegistrationTests.cs](../tests/UnitTests/Dependencies/MediatrRegistrationTests.cs)
  - Command handler tests: [ParisWeatherCallbackCommandHandlerTests.cs](../tests/UnitTests/Features/Weather/ParisWeatherCallbackCommandHandlerTests.cs)
  - Framework tests: [Framework/Utilities/System](../tests/UnitTests/Framework/Utilities/System/)

## Code Style
The solution uses [EditorConfig](.editorconfig), inherited from [Azure SDK .NET](https://github.com/Azure/azure-sdk-for-net/blob/main/.editorconfig), to maintain a consistent code style.

- To enforce the code style, it is recommended to run the `dotnet format` command before pushing changes.
- You can also use the `dotnet format --verify-no-changes` command to check if the code style is consistent.
- `ConfigureAwait(false)` is forcibly used in asynchronous methods to avoid deadlocks as recommended by [Microsoft](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2007).
    - The CA2007 analyzer is enabled as errors in the [Directory.Build.props](Directory.Build.props) file.
