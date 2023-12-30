using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Commands.UpdateSprint;

public class UpdateSprintCommandHandler
    : IRequestHandler<UpdateSprintCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private IDateTimeProvider _dateTimeProvider;

    public UpdateSprintCommandHandler(
        IProjectRepository projectRepository,
        ISprintRepository sprintRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        UpdateSprintCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
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
                Permission.UpdateSprint
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        _sprintRepository.Save(
            existingSprint with
            {
                Name = request.Name ?? existingSprint.Name,
                Description = request.Description ?? existingSprint.Description,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        return new SuccessMessage("Sprint updated successfully");
    }
}
