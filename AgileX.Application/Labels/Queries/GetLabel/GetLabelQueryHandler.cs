using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Labels.Queries.GetLabel;

public class GetLabelQueryHandler : IRequestHandler<GetLabelQuery, Result<Label>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILabelRepository _labelRepository;

    public GetLabelQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ILabelRepository labelRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _labelRepository = labelRepository;
    }

    public async Task<Result<Label>> Handle(
        GetLabelQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingLabel = _labelRepository.GetById(request.LabelId);
        if (existingLabel is null || existingLabel.IsDeleted)
            return LabelErrors.LabelNotFound;

        var existingProject = _projectRepository.GetById(existingLabel.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(existingProject.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        return existingLabel;
    }
}
