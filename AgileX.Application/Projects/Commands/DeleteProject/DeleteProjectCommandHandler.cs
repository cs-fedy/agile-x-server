using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler
    : IRequestHandler<DeleteProjectCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly IEventProvider _eventProvider;

    public DeleteProjectCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        IEventProvider eventProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        DeleteProjectCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(request.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        if (existingMember.Membership == Membership.PROJECT_MEMBER)
        {
            var existingPermission = _memberPermissionRepository.Get(
                request.ProjectId,
                request.UserId,
                Permission.DeleteProject
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        await _eventProvider.Publish(new ProjectDeleted(request.ProjectId, request.UserId));
        return new SuccessMessage("Project deleted successfully");
    }
}
