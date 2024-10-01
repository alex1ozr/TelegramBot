namespace TelegramBot.Framework.Utilities.System;

// TODO Rewrite in a more elegant way
public static class TypeExtensions
{
    public static T GetStaticPropertyValue<T>(this Type implementationType, Type interfaceType, string propertyName)
    {
        if (!interfaceType.IsInterface)
        {
            throw new InvalidOperationException($"{interfaceType} is not an interface");
        }

        var interfaceMap = implementationType.GetInterfaceMap(interfaceType);
        var property = interfaceMap.TargetMethods
                           .FirstOrDefault(m => m.IsStatic && m.Name.EndsWith($"get_{propertyName}"))
                       ?? throw new InvalidOperationException($"Static property {propertyName} not found for type {implementationType}");

        var propertyValue = property.Invoke(null, null);

        if (propertyValue is not T value)
        {
            throw new InvalidOperationException($"Static property {propertyName} is not of type {typeof(T).Name}");
        }

        return value;
    }
}
