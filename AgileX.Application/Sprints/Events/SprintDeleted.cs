using MediatR;

namespace AgileX.Application.Sprints.Events;

public class SprintDeleted : INotificationHandler<Domain.Events.SprintDeleted>
{
    public Task Handle(
        Domain.Events.SprintDeleted notification,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
