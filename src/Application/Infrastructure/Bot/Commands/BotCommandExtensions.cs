using System.Collections.Concurrent;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Application.Infrastructure.Bot.Commands;

public static class BotCommandExtensions
{
    private static readonly ConcurrentDictionary<Type, BotCommandDescriptor> s_commandDescriptors = new();

    public static BotCommandDescriptor GetCommandDescriptor(this IBotCommand command)
        => command.GetType().GetCommandDescriptor();

    public static BotCommandDescriptor GetCommandDescriptor<TCommand>()
        where TCommand : IBotCommand
        => typeof(TCommand).GetCommandDescriptor();

    public static BotCommandDescriptor GetCommandDescriptor(this Type commandType)
    {
        if (s_commandDescriptors.TryGetValue(commandType, out var commandDescriptor))
        {
            return commandDescriptor;
        }

        if (!commandType.IsAssignableTo(typeof(IBotCommand)))
        {
            throw new InvalidOperationException($"{commandType} is not assignable to {typeof(IBotCommand)}");
        }

        var commandName = commandType.GetStaticPropertyValue<string>(typeof(IBotCommand), nameof(IBotCommand.CommandName));
        var allowGroups = commandType.GetStaticPropertyValue<bool>(typeof(IBotCommand), nameof(IBotCommand.AllowGroups));
        var roles = commandType.GetStaticPropertyValue<IReadOnlyList<string>>(typeof(IBotCommand), nameof(IBotCommand.Roles));

        var descriptor = new BotCommandDescriptor(commandName, allowGroups, roles.ToHashSet());
        s_commandDescriptors.TryAdd(commandType, descriptor);

        return descriptor;
    }
}

public record class BotCommandDescriptor(string CommandName, bool AllowGroups, IReadOnlySet<string> Roles);
