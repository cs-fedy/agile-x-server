using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Permissions.Queries.ListMemberPermissions;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Permissions.Queries.ListOwnPermissions;

public class ListOwnPermissionsQueryHandler
    : IRequestHandler<ListOwnPermissionsQuery, Result<List<MemberPermission>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;

    public ListOwnPermissionsQueryHandler(
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
        ListOwnPermissionsQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMembership = _memberRepository.Get(request.ProjectId, request.UserId);
        if (existingMembership is null || existingMembership.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        return _memberPermissionRepository.ListByMemberId(request.ProjectId, request.UserId);
    }
}
