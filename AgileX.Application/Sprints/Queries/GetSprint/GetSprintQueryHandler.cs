using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Queries.GetSprint;

public class GetSprintQueryHandler : IRequestHandler<GetSprintQuery, Result<Sprint>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ISprintRepository _sprintRepository;

    public GetSprintQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ISprintRepository sprintRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _sprintRepository = sprintRepository;
    }

    public async Task<Result<Sprint>> Handle(
        GetSprintQuery request,
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

        var existingMember = _memberRepository.Get(existingSprint.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        return existingSprint;
    }
}
