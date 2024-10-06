using FluentAssertions;
using TelegramBot.Framework.Utilities.System;

namespace TelegramBot.UnitTests.Framework.Utilities.System;

/// <summary>
///  Tests for <see cref="StringExtensions"/>
/// </summary>
public sealed class StringExtensionsTests
{
    [Fact]
    void Split_WhenNonEmptyString_ReturnsChunks()
    {
        var result = "HelloWorld".Split(5);
        result.Should().BeEquivalentTo("Hello", "World");
    }

    [Fact]
    void Split_WhenEmptyString_ReturnsEmptyEnumerable()
    {
        var result = "".Split(5);
        result.Should().BeEmpty();
    }

    [Fact]
    void Split_WhenChunkSizeGreaterThanStringLength_ReturnsSingleChunk()
    {
        var result = "Hello".Split(10);
        result.Should().BeEquivalentTo("Hello");
    }

    [Fact]
    void Split_WhenChunkSizeOne_ReturnsSingleCharacterChunks()
    {
        var result = "Hello".Split(1);
        result.Should().BeEquivalentTo("H", "e", "l", "l", "o");
    }
}
