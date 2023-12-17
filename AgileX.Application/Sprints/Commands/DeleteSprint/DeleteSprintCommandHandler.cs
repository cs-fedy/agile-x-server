using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Commands.DeleteSprint;

public class DelUpdateSprintCommandHandler
    : IRequestHandler<DeleteSprintCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly IEventProvider _eventProvider;

    public DelUpdateSprintCommandHandler(
        IProjectRepository projectRepository,
        ISprintRepository sprintRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        IEventProvider eventProvider
    )
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        DeleteSprintCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingSprint = _sprintRepository.GetById(request.SprintId);
        if (existingSprint is null || existingSprint.IsDeleted)
            return SprintErrors.SprintNotFound;

        var existingProject = _projectRepository.GetById(existingSprint.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(existingProject.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        if (existingMember.Membership == Membership.PROJECT_MEMBER)
        {
            var existingPermission = _memberPermissionRepository.Get(
                existingProject.ProjectId,
                request.UserId,
                Permission.DELETE_SPRINT
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        _sprintRepository.Delete(request.SprintId);
        await _eventProvider.Publish(new SprintDeleted(request.SprintId));

        return new SuccessMessage("Sprint deleted successfully");
    }
}
