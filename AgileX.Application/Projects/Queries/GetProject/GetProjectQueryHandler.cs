using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Queries.GetProject;

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, Result<Project>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;

    public GetProjectQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<Project>> Handle(
        GetProjectQuery request,
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

        return existingProject;
    }
}
