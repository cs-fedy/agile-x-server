using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using MediatR;

namespace AgileX.Application.Members.Events;

public class MemberJoinedHandler : INotificationHandler<MemberJoined>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEventBus _eventBus;

    public MemberJoinedHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IUserRepository userRepository,
        IEventBus eventBus
    )
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _memberRepository = memberRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(MemberJoined notification, CancellationToken cancellationToken)
    {
        var existingProject = _projectRepository.GetById(notification.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return;

        var memberExistence = _memberRepository.Get(notification.ProjectId, notification.UserId);
        if (memberExistence is null || memberExistence.IsDeleted)
            return;

        var creatorAccountExistence = _userRepository.GetById(notification.AddedBy);
        if (creatorAccountExistence is null || creatorAccountExistence.IsDeleted)
            return;

        var memberAccountExistence = _userRepository.GetById(notification.AddedBy);
        if (memberAccountExistence is null || memberAccountExistence.IsDeleted)
            return;

        await _eventBus.Publish(
            new NewEmail(
                new Email(
                    To: new List<string>() { memberAccountExistence.Email },
                    Subject: "Joined project",
                    PlainTextContent: $"Dear {memberAccountExistence.FullName}. \n"
                        + $"You have been added to `{existingProject.Name}` project "
                        + $"by `{creatorAccountExistence.FullName}`."
                )
            ),
            cancellationToken
        );
    }
}
