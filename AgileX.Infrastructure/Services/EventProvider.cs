using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using MediatR;

namespace AgileX.Infrastructure.Services;

public class EventProvider : IEventProvider
{
    private readonly IPublisher _mediator;

    public EventProvider(IPublisher mediator) => _mediator = mediator;

    public async Task Publish(IEvent emittedEvent) => await _mediator.Publish(emittedEvent);
}
