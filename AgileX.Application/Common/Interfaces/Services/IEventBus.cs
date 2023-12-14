namespace AgileX.Application.Common.Interfaces.Services;

public interface IEventBus
{
    public Task Publish<T>(T message, CancellationToken cancellationToken = default)
        where T : class;
}
