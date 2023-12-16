using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using MediatR;

namespace AgileX.Application.Projects.Events;

public class ProjectDeletedHandler : INotificationHandler<ProjectDeleted>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEventBus _eventBus;

    public ProjectDeletedHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IUserRepository userRepository,
        IEventBus eventBus
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _userRepository = userRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(ProjectDeleted notification, CancellationToken cancellationToken)
    {
        var existingProject = _projectRepository.GetById(notification.ProjectId);
        if (existingProject is null)
            return;

        var existingUser = _userRepository.GetById(notification.DeletedBy);
        if (existingUser is null || existingUser.IsDeleted)
            return;

        var projectMembers = _memberRepository.ListByProjectId(notification.ProjectId);
        var members = projectMembers.Select(x => x.UserId.ToString()).ToList();

        await _eventBus.Publish(
            new NewEmail(
                new Email(
                    To: members,
                    Subject: $"{existingProject.Name} project deleted successfully",
                    PlainTextContent: $"Dear {existingProject.Name} project members. \n"
                        + $"The project is deleted by {existingUser.FullName} at {existingProject.DeletedAt}"
                )
            ),
            cancellationToken
        );
    }
}
