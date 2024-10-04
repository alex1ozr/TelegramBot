using System.Linq.Expressions;
using System.Reflection;

namespace TelegramBot.Framework.Utilities.System;

/// <summary>
/// Generic activator with caching
/// </summary>
public static class GenericActivator
{
    /// <summary>
    /// Create instance of type <typeparamref name="T"/>
    /// </summary>
    public static T Create<T>() => GenericFactoryCache<T>.CreateInstance();

    /// <summary>
    /// Create instance of type <typeparamref name="T"/> with parameter <typeparamref name="TParam"/>
    /// </summary>
    public static T Create<TParam, T>(TParam parameter) => GenericFactoryCache<TParam, T>.CreateInstance(parameter);

    /// <summary>
    /// Get instance initializer for type <typeparamref name="T"/>
    /// </summary>
    /// <param name="instanceType">Type of the instance</param>
    /// <genericparam name="T">Return type</genericparam>
    public static Func<T> GetInstanceInitializer<T>(Type instanceType)
    {
        var constructorExpression = Expression.New(instanceType);
        return Expression.Lambda<Func<T>>(constructorExpression).Compile();
    }

    private static class GenericFactoryCache<T>
    {
        public static readonly Func<T> CreateInstance = BuildConstructorExpression<T>();

        private static Func<TTargetType> BuildConstructorExpression<TTargetType>()
        {
            var constructorExpression = Expression.New(typeof(TTargetType));
            return Expression.Lambda<Func<TTargetType>>(constructorExpression).Compile();
        }
    }

    private static class GenericFactoryCache<TParam, T>
    {
        public static readonly Func<TParam, T> CreateInstance = BuildConstructorExpression<TParam, T>();

        private static Func<TCtorParam, TTargetType> BuildConstructorExpression<TCtorParam, TTargetType>()
        {
            var constructorInfo = typeof(TTargetType)
                                  .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                  .SingleOrDefault(HasSingleParameter<TCtorParam>)
                              ?? throw new InvalidOperationException($"Could not find constructor with single paremeter: {typeof(T)}({typeof(TCtorParam)})");

            var parameterExpressions = constructorInfo
                .GetParameters()
                .Select(parameter => Expression.Parameter(parameter.ParameterType, parameter.Name))
                .ToArray();

            var constructorExpression = Expression.New(constructorInfo, parameterExpressions.ToArray<Expression>());
            return (Func<TCtorParam, TTargetType>)Expression.Lambda(constructorExpression, parameterExpressions).Compile();
        }

        private static bool HasSingleParameter<TCtorParam>(ConstructorInfo constructorInfo)
        {
            var parameters = constructorInfo.GetParameters();
            return parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(typeof(TCtorParam));
        }
    }
}
