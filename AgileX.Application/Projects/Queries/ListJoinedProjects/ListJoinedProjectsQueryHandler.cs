using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Queries.ListJoinedProjects;

public class ListJoinedProjectsQueryHandler
    : IRequestHandler<ListJoinedProjectsQuery, Result<List<Project>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;

    public ListJoinedProjectsQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<List<Project>>> Handle(
        ListJoinedProjectsQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var memberships = _memberRepository
            .ListByUserId(request.UserId)
            .Where(x => !x.IsDeleted)
            .ToList();

        var projects = memberships
            .Select(member => _projectRepository.GetById(member.ProjectId))
            .OfType<Project>()
            .Where(x => !x.IsDeleted)
            .ToList();

        return projects;
    }
}
