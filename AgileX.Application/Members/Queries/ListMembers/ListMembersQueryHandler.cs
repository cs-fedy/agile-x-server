using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Members.Queries.ListMembers;

public class ListMembersQueryHandler : IRequestHandler<ListMembersQuery, Result<List<Member>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;

    public ListMembersQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<List<Member>>> Handle(
        ListMembersQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(request.ProjectId, request.LoggedUserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        return _memberRepository
            .ListByProjectId(request.ProjectId)
            .Where(x => !x.IsDeleted)
            .ToList();
    }
}
