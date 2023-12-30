using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Dependencies.Commands.DeleteDependency;
using AgileX.Application.Labels.Queries.GetLabel;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Labels.Commands.DeleteLabel;

public class DeleteLabelCommandHandler : IRequestHandler<DeleteLabelCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;

    public DeleteLabelCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ILabelRepository labelRepository,
        IMemberPermissionRepository memberPermissionRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _labelRepository = labelRepository;
        _memberPermissionRepository = memberPermissionRepository;
    }

    public async Task<Result<SuccessMessage>> Handle(
        DeleteLabelCommand request,
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

        if (existingMember.Membership == Membership.PROJECT_MEMBER)
        {
            var existingPermission = _memberPermissionRepository.Get(
                existingProject.ProjectId,
                request.UserId,
                Permission.DeleteLabel
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        _labelRepository.Delete(request.LabelId);

        return new SuccessMessage("Label deleted successfully");
    }
}
