using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Permissions.Queries.ListMemberPermissions;

public class ListMemberPermissionsQueryHandler
    : IRequestHandler<ListMemberPermissionsQuery, Result<List<MemberPermission>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;

    public ListMemberPermissionsQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
    }

    public async Task<Result<List<MemberPermission>>> Handle(
        ListMemberPermissionsQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMembership = _memberRepository.Get(request.ProjectId, request.LoggedUserId);
        if (existingMembership is null || existingMembership.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        if (existingMembership.Membership == Membership.PROJECT_MEMBER)
        {
            var existingPermission = _memberPermissionRepository.Get(
                request.ProjectId,
                request.LoggedUserId,
                Permission.LIST_MEMBER_PERMISSIONS
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        return _memberPermissionRepository
            .ListByMemberId(request.ProjectId, request.TargetUserId)
            .Where(x => !x.IsDeleted)
            .ToList();
    }
}
