using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Queries.ListSprints;

public class ListSprintsQueryHandler : IRequestHandler<ListSprintsQuery, Result<List<Sprint>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ISprintRepository _sprintRepository;

    public ListSprintsQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ISprintRepository sprintRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _sprintRepository = sprintRepository;
    }

    public async Task<Result<List<Sprint>>> Handle(
        ListSprintsQuery request,
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

        return _sprintRepository
            .ListByProjectId(request.ProjectId)
            .Where(x => !x.IsDeleted)
            .ToList();
    }
}
