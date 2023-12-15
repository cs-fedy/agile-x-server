using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using MediatR;

namespace AgileX.Application.Permissions.Events;

public class PermissionRevokedHandler : INotificationHandler<PermissionRevoked>
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly IEventBus _eventBus;

    public PermissionRevokedHandler(
        IUserRepository userRepository,
        IMemberPermissionRepository memberPermissionRepository,
        IProjectRepository projectRepository,
        IEventBus eventBus
    )
    {
        _userRepository = userRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _projectRepository = projectRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(PermissionRevoked notification, CancellationToken cancellationToken)
    {
        var existingUser = _userRepository.GetById(notification.UserId);
        if (existingUser is null || existingUser.IsDeleted)
            return;

        var existingProject = _projectRepository.GetById(notification.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return;

        var existingPermission = _memberPermissionRepository.Get(
            notification.ProjectId,
            notification.UserId,
            notification.Permission
        );

        if (existingPermission is null || existingPermission.IsDeleted)
            return;

        var email = new Email(
            To: new List<string>() { existingUser.Email },
            Subject: "Permission revoked successfully",
            PlainTextContent: $"Dear {existingUser.FullName}. \n"
                + $"As a member in the `{existingProject.Name}` you have been revoked "
                + $"from the `{existingPermission.Name}` permission."
        );

        await _eventBus.Publish(new NewEmail(email), cancellationToken);
    }
}
