using System.Collections.Concurrent;
using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Features.Bot;

namespace TelegramBot.Application.Infrastructure.Bot.Commands;

internal sealed class BotCommandFactory : IBotCommandFactory
{
    private static readonly ConcurrentDictionary<string, Type> s_commandTypes = new();
    private static readonly ConcurrentDictionary<string, Type> s_callbackCommandTypes = new();

    public IBotCommand CreateBotCommand(string commandName, Message message, UserInfo userInfo)
    {
        var commandType = GetCommandType(commandName);
        if (commandType is null)
        {
            return new UnknownBotCommand(message, userInfo);
        }

        return (IBotCommand)(
            Activator.CreateInstance(commandType, message, userInfo)
            ?? throw new InvalidOperationException());
    }

    public ICallbackCommand CreateCallbackCommand(
        string commandName,
        MaybeInaccessibleMessage message,
        UserInfo userInfo,
        params string[] args)
    {
        var commandType = GetCallbackType(commandName);
        if (commandType is null)
        {
            return new UnknownCallbackCommand(message, userInfo, args);
        }

        return (ICallbackCommand)(
            Activator.CreateInstance(commandType, message, userInfo, args)
            ?? throw new InvalidOperationException());
    }

    private Type? GetCommandType(string commandName)
    {
        if (s_commandTypes.TryGetValue(commandName, out var commandType))
        {
            return commandType;
        }

        commandType = typeof(BotCommandFactory)
            .Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && t.IsAssignableTo(typeof(IBotCommand)))
            .SingleOrDefault(t => t.GetCommandDescriptor().CommandName == commandName);

        if (commandType is not null)
        {
            s_commandTypes[commandName] = commandType;
        }

        return commandType;
    }

    private Type? GetCallbackType(string commandName)
    {
        if (s_callbackCommandTypes.TryGetValue(commandName, out var commandType))
        {
            return commandType;
        }

        commandType = typeof(BotCommandFactory)
            .Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                        && t.IsAssignableTo(typeof(ICallbackCommand)))
            .SingleOrDefault(t =>
            {
                // TODO Find a better way to get the CommandName static property
                var interfaceMap = t.GetInterfaceMap(typeof(ICallbackCommand));
                var commandNameProperty = interfaceMap.TargetMethods
                    .FirstOrDefault(m => m.Name.EndsWith($"get_{nameof(IBotCommand.CommandName)}"));

                return commandNameProperty?.Invoke(null, null) as string == commandName;
            });

        if (commandType is not null)
        {
            s_callbackCommandTypes[commandName] = commandType;
        }

        return commandType;
    }
}
