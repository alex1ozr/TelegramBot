using System.Linq.Expressions;
using AuroraScienceHub.Framework.Entities;
using AuroraScienceHub.Framework.Entities.Identifiers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.Framework.EntityFramework.Builders;

public static class EntityBuilderExtensions
{
    public static void ConfigureForeignKey<TEntity, TId, TDependentEntity>(
        this EntityTypeBuilder<TEntity> entityBuilder,
        Expression<Func<TEntity, TId>> foreignKeySelector,
        Expression<Func<TEntity, TDependentEntity?>>? navigationPropertySelector = default,
        Expression<Func<TDependentEntity, IEnumerable<TEntity>?>>? collectionNavigationExpression = default,
        DeleteBehavior deleteBehavior = DeleteBehavior.Restrict)
        where TEntity : class
        where TId : class, IIdentifier
        where TDependentEntity : class, IEntity<TId>
    {
        if (navigationPropertySelector is not null)
        {
            entityBuilder.HasAutoIncludedNavigationProperty(navigationPropertySelector);
        }

        var foreignKeyExpression = ConvertToForeignKeyExpression<TEntity>(foreignKeySelector);
        entityBuilder.ConfigureForeignKeyInternal<TEntity, TId, TDependentEntity>(
            foreignKeyExpression,
            deleteBehavior,
            navigationPropertySelector,
            collectionNavigationExpression);
    }

    public static void HasEnumConversion<TEntity>(
        this PropertyBuilder<TEntity> builder)
        where TEntity : struct
    {
        builder.HasConversion(v => v.ToString(),
            v => Enum.Parse<TEntity>(v.Required(v)))
            .IsRequired();
    }

    public static IndexBuilder<TEntity> HasUniqueIndex<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object?>> indexExpression)
        where TEntity : class
    {
        return builder
            .HasIndex(indexExpression)
            .IsUnique();
    }

    public static void HasAutoIncludedNavigationProperty<TEntity, TDependentEntity>(
        this EntityTypeBuilder<TEntity> entityBuilder,
        Expression<Func<TEntity, TDependentEntity?>> propertySelector)
        where TEntity : class
        where TDependentEntity : class
    {
        entityBuilder
            .Navigation(propertySelector)
            .AutoInclude();
    }

    private static void ConfigureForeignKeyInternal<TEntity, TId, TDependentEntity>(
        this EntityTypeBuilder<TEntity> entityBuilder,
        Expression<Func<TEntity, object?>> foreignKeySelector,
        DeleteBehavior deleteBehavior,
        Expression<Func<TEntity, TDependentEntity?>>? navigationPropertySelector = default,
        Expression<Func<TDependentEntity, IEnumerable<TEntity>?>>? collectionNavigationExpression = default)
        where TEntity : class
        where TId : class, IIdentifier
        where TDependentEntity : class, IEntity<TId>
    {
        entityBuilder
            .HasOne(navigationPropertySelector)
            .WithMany(collectionNavigationExpression)
            .HasForeignKey(foreignKeySelector)
            .OnDelete(deleteBehavior);
    }

    private static Expression<Func<TEntity, object?>> ConvertToForeignKeyExpression<TEntity>(this LambdaExpression foreignKeyExpression)
    {
        return Expression.Lambda<Func<TEntity, object?>>(foreignKeyExpression.Body, foreignKeyExpression.Parameters);
    }
}
