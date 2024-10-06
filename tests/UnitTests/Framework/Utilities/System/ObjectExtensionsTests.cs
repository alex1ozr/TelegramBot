using FluentAssertions;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.UnitTests.Framework.Utilities.System;

/// <summary>
/// Tests for <see cref="ObjectExtensions"/>
/// </summary>
public sealed class ObjectExtensionsTests
{
    [Fact]
    void Required_ClassType_WhenNotNull_ReturnsValue()
    {
        // Arrange
        var value = new object();

        // Act
        var result = value.Required();

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    void Required_ClassType_WhenNull_ThrowsArgumentNullException()
    {
        // Arrange
        object? value = null;

        // Act, Assert
        Action act = () => value.Required();
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    void Required_StructType_WhenNotNull_ReturnsValue()
    {
        // Arrange
        int? value = 5;

        // Act
        var result = value.Required();

        // Assert
        result.Should().Be(value);
    }

    [Fact]
    void Required_StructType_WhenNull_ThrowsArgumentNullException()
    {
        // Arrange
        int? value = null;

        // Act, Assert
        Action act = () => value.Required();
        act.Should().Throw<ArgumentNullException>();
    }
}
