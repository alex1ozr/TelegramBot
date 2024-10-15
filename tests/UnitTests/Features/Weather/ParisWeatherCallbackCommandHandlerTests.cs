using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using TelegramBot.Application.Features.Accounting;
using TelegramBot.Application.Features.Weather.Cities;
using TelegramBot.Application.Features.Weather.DataProvider;
using TelegramBot.Application.Infrastructure.Localization;
using TelegramBot.Application.Resources;
using TelegramBot.Domain.Accounting.Users;

namespace TelegramBot.UnitTests.Features.Weather;

/// <summary>
/// Tests for <see cref="ParisWeatherCallbackCommandHandler"/>
/// </summary>
/// <remarks>
/// It is just a one little sample to show how to test a command handler.
/// </remarks>
public sealed class ParisWeatherCallbackCommandHandlerTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
    private readonly Mock<ITelegramBotClient> _telegramBotClientMock;
    private readonly Mock<IWeatherProvider> _weatherProviderMock;

    private readonly ParisWeatherCallbackCommandHandler _sut;

    public ParisWeatherCallbackCommandHandlerTests()
    {
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _telegramBotClientMock = _fixture.Freeze<Mock<ITelegramBotClient>>();
        _weatherProviderMock = _fixture.Freeze<Mock<IWeatherProvider>>();
        _fixture.Register<IBotMessageLocalizer>(() => new BotMessageLocalizer());

        _sut = _fixture.Create<ParisWeatherCallbackCommandHandler>();
    }

    [Fact]
    public async Task Handle_WhenEnglish_ShouldSend()
    {
        // Arrange
        var temperature = _fixture.Create<int>();
        _weatherProviderMock.Setup(x => x.GetTemperature("Paris")).Returns(temperature);

        Dictionary<string, object?>? messageParameters = null;
        _telegramBotClientMock
            .Setup(x => x.CallMethodAsync<Message>(
                "sendMessage",
                It.IsAny<object?>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, object?, CancellationToken>((_, parameters, _) =>
            {
                messageParameters = parameters as Dictionary<string, object?>;
            });

        var message = _fixture.Create<Message>();
        var userInfo = new UserInfo(
            UserId.New(),
            _fixture.Create<string>(),
            BotLanguage.English,
            Array.Empty<string>());

        var request = new ParisWeatherCallbackCommand(message, userInfo);

        // Act
        await _sut.Handle(request, CancellationToken.None);

        // Assert
        VerifySentMessage(messageParameters, message.Chat.Id, $"Current temperature in Paris: {temperature} C");
    }

    // Other tests are omitted for brevity

    private void VerifySentMessage(
        Dictionary<string, object?>? messageParameters,
        long chatId,
        string text)
    {
        _telegramBotClientMock.Verify(x => x.CallMethodAsync<Message>(
                "sendMessage",
                It.IsAny<object?>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        messageParameters.Should().NotBeNull();
        messageParameters.Should().Equal(new Dictionary<string, object?>
        {
            ["chat_id"] = chatId,
            ["text"] = text
        });
    }
}
