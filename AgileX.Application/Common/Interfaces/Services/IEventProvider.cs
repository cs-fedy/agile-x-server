using AgileX.Domain.Events;

namespace AgileX.Application.Common.Services;

public interface IEventProvider
{
    Task Publish(IEvent emittedEvent);
}
