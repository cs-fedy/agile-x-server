using AgileX.Domain.Events;
using MediatR;

namespace AgileX.Application.Permissions.Events;

public class PermissionGrantedHandler : INotificationHandler<PermissionGranted>
{
    public Task Handle(PermissionGranted notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
