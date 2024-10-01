using System.Collections.Concurrent;
using System.Reflection;
using AuroraScienceHub.Framework.Entities.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TelegramBot.Framework.EntityFramework.Builders;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Framework.EntityFramework.Identifiers;

public static class ModelBuilderExtensions
{
    private static readonly ConcurrentDictionary<Type, Func<ValueConverter>> s_nonNullableFactoryCache = new();
    private static readonly ConcurrentDictionary<Type, Func<ValueConverter>> s_nullableFactoryCache = new();

    public static void UseIdentifierConverter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType
                .ClrType
                .GetProperties()
                .Where(p => typeof(IIdentifier).IsAssignableFrom(p.PropertyType));

            foreach (var property in properties)
            {
                ValueConverter? converter;
                if (IsNullableProperty(property))
                {
                    var factory = s_nullableFactoryCache.GetOrAdd(property.PropertyType, GetNullableIdentifierValueConverterFactory);
                    converter = factory();
                }
                else
                {
                    var factory = s_nonNullableFactoryCache.GetOrAdd(property.PropertyType, GetIdentifierValueConverterFactory);
                    converter = factory();
                }

                modelBuilder
                    .Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(converter)
                    .HasShortString()
                    .ValueGeneratedNever();
            }
        }

        s_nonNullableFactoryCache.Clear();
        s_nullableFactoryCache.Clear();
    }

    private static bool IsNullableProperty(PropertyInfo property)
    {
        var nullability = new NullabilityInfoContext().Create(property);
        return nullability.ReadState switch
        {
            NullabilityState.NotNull => false,
            NullabilityState.Nullable => true,
            NullabilityState.Unknown => throw new NotSupportedException(
                "Unknown nullability state. Please check the property type and use NRT."),
            _ => throw new ArgumentOutOfRangeException(nameof(property), nullability.ReadState,
                $"{nameof(NullabilityInfo.ReadState)}")
        };
    }

    private static Func<ValueConverter> GetIdentifierValueConverterFactory(Type propertyType)
    {
        var converterType = typeof(IdentifierValueConverter<>).MakeGenericType(propertyType);
        return GenericActivator.GetInstanceInitializer<ValueConverter>(converterType);
    }

    private static Func<ValueConverter> GetNullableIdentifierValueConverterFactory(Type propertyType)
    {
        var converterType = typeof(NullableIdentifierValueConverter<>).MakeGenericType(propertyType);
        return GenericActivator.GetInstanceInitializer<ValueConverter>(converterType);
    }
}
