using AgileX.Domain.Events;

namespace AgileX.Application.Common.Interfaces.Services;

public interface IEventProvider
{
    Task Publish(IEvent emittedEvent);
}
