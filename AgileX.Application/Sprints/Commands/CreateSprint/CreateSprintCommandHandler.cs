using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Commands.CreateSprint;

public class CreateSprintCommandHandler
    : IRequestHandler<CreateSprintCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateSprintCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        ISprintRepository sprintRepository,
        IDateTimeProvider dataTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _sprintRepository = sprintRepository;
        _dateTimeProvider = dataTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        CreateSprintCommand request,
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

        if (existingMember.Membership == Membership.PROJECT_MEMBER)
        {
            var existingPermission = _memberPermissionRepository.Get(
                request.ProjectId,
                request.UserId,
                Permission.CreateSprint
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        var creationDate = _dateTimeProvider.UtcNow;
        var durationTimeStamp = request.EndDate - request.StartDate;

        _sprintRepository.Save(
            new Sprint(
                SprintId: Guid.NewGuid(),
                ProjectId: request.ProjectId,
                Name: request.Name,
                Description: request.Description,
                Duration: durationTimeStamp.Milliseconds,
                StartDate: request.StartDate,
                EndDate: request.EndDate,
                IsDeleted: false,
                DeletedAt: null,
                CreatedAt: creationDate,
                UpdatedAt: creationDate
            )
        );

        return new SuccessMessage("Sprint created successfully");
    }
}
