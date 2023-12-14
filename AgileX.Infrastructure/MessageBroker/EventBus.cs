using AgileX.Application.Common.Interfaces.Services;
using MassTransit;

namespace AgileX.Infrastructure.MessageBroker;

public sealed class EventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public Task Publish<T>(T message, CancellationToken cancellationToken = default)
        where T : class => _publishEndpoint.Publish(message, cancellationToken);
}
