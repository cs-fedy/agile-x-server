using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Commands.UpdateProjectDeadline;

public class UpdateProjectDeadlineCommandHandler
    : IRequestHandler<UpdateProjectDeadlineCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly IEventProvider _eventProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateProjectDeadlineCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        IEventProvider eventProvider,
        IDateTimeProvider dateTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _eventProvider = eventProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        UpdateProjectDeadlineCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(request.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        var existingPermission = _memberPermissionRepository.Get(
            request.ProjectId,
            request.UserId,
            Permission.UPDATE_PROJECT_DEADLINE
        );

        if (existingPermission is null || existingPermission.IsDeleted)
            return PermissionErrors.UnauthorizedAction;

        // TODO: set the maximum date between the new date and the newest task to the deadline of the project
        _projectRepository.Save(
            existingProject with
            {
                Deadline = request.NewDeadline,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        return new SuccessMessage("Project updated successfully");
    }
}
