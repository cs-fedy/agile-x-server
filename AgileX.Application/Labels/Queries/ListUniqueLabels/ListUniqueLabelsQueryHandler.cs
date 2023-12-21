using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Labels.Queries.ListLabels;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Labels.Queries.ListUniqueLabels;

public class ListUniqueLabelsQueryHandler
    : IRequestHandler<ListUniqueLabelsQuery, Result<List<Label>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILabelRepository _labelRepository;

    public ListUniqueLabelsQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ILabelRepository labelRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _labelRepository = labelRepository;
    }

    public async Task<Result<List<Label>>> Handle(
        ListUniqueLabelsQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(existingProject.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        return _labelRepository.ListUnique(request.ProjectId).Where(x => !x.IsDeleted).ToList();
    }
}
