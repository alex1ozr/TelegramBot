using FluentAssertions;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.UnitTests.Framework.Utilities.System;

/// <summary>
/// Tests for <see cref="TelegramBot.Framework.Utilities.System.TypeExtensions"/>
/// </summary>
public sealed class TypeExtensionsTests
{
    [Fact]
    void GetStaticPropertyValue_WhenValidStaticProperty_ReturnsValue()
    {
        var result = typeof(TestClass).GetStaticPropertyValue<int>(typeof(ITestInterface), nameof(ITestInterface.StaticProperty));
        result.Should().Be(42);
    }

    [Fact]
    void GetStaticPropertyValue_WhenInterfaceTypeIsNotInterface_ThrowsInvalidOperationException()
    {
        Action act = () => typeof(TestClass).GetStaticPropertyValue<int>(typeof(TestClass), nameof(ITestInterface.StaticProperty));
        act.Should().Throw<InvalidOperationException>().WithMessage("*is not an interface*");
    }

    [Fact]
    void GetStaticPropertyValue_WhenStaticPropertyNotFound_ThrowsInvalidOperationException()
    {
        Action act = () => typeof(TestClass).GetStaticPropertyValue<int>(typeof(ITestInterface), "NonExistentProperty");
        act.Should().Throw<InvalidOperationException>().WithMessage("*Static property NonExistentProperty not found*");
    }

    [Fact]
    void GetStaticPropertyValue_WhenPropertyTypeMismatch_ThrowsInvalidOperationException()
    {
        Action act = () => typeof(TestClass).GetStaticPropertyValue<string>(typeof(ITestInterface), nameof(ITestInterface.StaticProperty));
        act.Should().Throw<InvalidOperationException>().WithMessage("*Static property StaticProperty is not of type*");
    }
}

file interface ITestInterface
{
    static abstract int StaticProperty { get; }
}

file class TestClass : ITestInterface
{
    public static int StaticProperty => 42;
}
