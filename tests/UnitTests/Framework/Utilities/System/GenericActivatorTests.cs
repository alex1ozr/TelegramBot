using FluentAssertions;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.UnitTests.Framework.Utilities.System;

/// <summary>
/// Tests for <see cref="GenericActivator"/>
/// </summary>
public sealed class GenericActivatorTests
{
    [Fact]
    public void Create_WhenCtorWithoutParameters_ReturnsInstance()
    {
        // Arrange, Act
        var instance = GenericActivator.Create<NoCtorParametersClass>();

        // Assert
        instance.Should().NotBeNull();
        instance.Value.Should().Be(NoCtorParametersClass.DefaultValue);
    }

    [Fact]
    public void Create_WhenCtorWithParameter_ReturnsInstance()
    {
        // Arrange, Act
        var value = 200;
        var instance = GenericActivator.Create<int, OneCtorParameterClass>(value);

        // Assert
        instance.Should().NotBeNull();
        instance.Value.Should().Be(value);
    }

    [Fact]
    public void GetInstanceInitializer_ReturnsInitializer()
    {
        // Arrange
        var instanceType = typeof(NoCtorParametersClass);

        // Act
        var instanceInitializer = GenericActivator.GetInstanceInitializer<NoCtorParametersClass>(instanceType);
        var instance = instanceInitializer.Invoke();

        // Assert
        instance.Should().NotBeNull();
        instance.Value.Should().Be(NoCtorParametersClass.DefaultValue);
    }

    private class NoCtorParametersClass
    {
        public const int DefaultValue = 100;

        public int Value { get; protected init; } = DefaultValue;
    }

    private sealed class OneCtorParameterClass : NoCtorParametersClass
    {
        public OneCtorParameterClass(int value)
        {
            Value = value;
        }
    }
}
