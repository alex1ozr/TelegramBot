using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramBot.Framework.Entities;

namespace TelegramBot.Framework.EntityFramework.Builders;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TEntity> HasShortString<TEntity>(this PropertyBuilder<TEntity> propertyBuilder)
    {
        return propertyBuilder.HasMaxLength(EntityConfigurationConstants.ShortStringLength);
    }

    public static PropertyBuilder HasShortString(this PropertyBuilder propertyBuilder)
    {
        return propertyBuilder.HasMaxLength(EntityConfigurationConstants.ShortStringLength);
    }
}
