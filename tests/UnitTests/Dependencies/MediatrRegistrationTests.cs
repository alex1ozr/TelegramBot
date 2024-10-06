using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.UnitTests.Dependencies;

public sealed class MediatrRegistrationTests :
    IClassFixture<ContainerFixture>,
    IAsyncDisposable
{
    private readonly IServiceScope _scope;

    private static readonly IReadOnlyList<Type> s_handlerTypes =
        ContainerFixture.s_hostTypes
            .Where(IsRequestHandler)
            .ToList();

    public MediatrRegistrationTests(ContainerFixture fixture)
    {
        _scope = fixture.Container.CreateAsyncScope();
    }

    [Theory(DisplayName = "Request should have matching and registered handler")]
    [MemberData(nameof(RequestTypes))]
    public void AllRequests_ShouldHaveMatchingHandler(Type requestType)
    {
        var handlerType = s_handlerTypes.Should()
            .ContainSingle(handlerType => IsHandlerForRequest(handlerType, requestType),
                $"Handler for type {requestType} expected")
            .Subject;

        var handlerInterface = handlerType.GetInterfaces()
            .Single(i => i.IsGenericType
                         && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

        var handler = _scope.ServiceProvider.GetRequiredService(handlerInterface);
        handler.Should().NotBeNull($"Handler for type {requestType}  should be registered");
    }

    private static bool IsRequest(Type type)
        => typeof(IBaseRequest).IsAssignableFrom(type);

    private static bool IsRequestHandler(Type type)
        => type.GetInterfaces()
            .Any(interfaceType => interfaceType.IsGenericType
                                  && interfaceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

    private static bool IsHandlerForRequest(Type handlerType, Type requestType)
        => handlerType.GetInterfaces()
            .Any(i => i.GenericTypeArguments.Any(x => x == requestType));

    public static TheoryData<Type> RequestTypes
        => new(ContainerFixture.s_hostTypes
            .Where(IsRequest)
            .ToList());

    public async ValueTask DisposeAsync()
    {
        if (_scope is IAsyncDisposable scopeAsyncDisposable)
        {
            await scopeAsyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            _scope.Dispose();
        }
    }
}
