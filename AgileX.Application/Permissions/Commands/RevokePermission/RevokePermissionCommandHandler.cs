using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Application.Permissions.Commands.GrantPermission;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Permissions.Commands.RevokePermission;

public class RevokePermissionCommandHandler
    : IRequestHandler<GrantPermissionCommand, Result<SuccessMessage>>
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly IEventProvider _eventProvider;

    public RevokePermissionCommandHandler(
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        IEventProvider eventProvider
    )
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        GrantPermissionCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingTargetUser = _userRepository.GetById(request.TargetUserId);
        if (existingTargetUser is null || existingTargetUser.IsDeleted)
            return UserErrors.UserNotFound with { Description = "Target use not found" };

        var existingMember = _memberRepository.Get(request.ProjectId, request.LoggedUserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember with
            {
                Description = "Logged user is not a project member"
            };

        var existingPermission = _memberPermissionRepository.Get(
            request.ProjectId,
            request.LoggedUserId,
            Permission.GRANT_PERMISSION
        );

        if (existingPermission is null || existingPermission.IsDeleted)
            return PermissionErrors.UnauthorizedAction with
            {
                Description = "Logged user is unauthorized to perform this action"
            };

        var existingTargetUserMembership = _memberRepository.Get(
            request.ProjectId,
            request.TargetUserId
        );

        if (existingTargetUserMembership is null || existingTargetUserMembership.IsDeleted)
            return MemberErrors.UnauthorizedMember with
            {
                Description = "Target user is not a member"
            };

        var existingTargetPermission = _memberPermissionRepository.Get(
            request.ProjectId,
            request.TargetUserId,
            request.Permission
        );

        if (existingTargetPermission is null || existingTargetPermission.IsDeleted)
            return PermissionErrors.PermissionNotFound;

        _memberPermissionRepository.Delete(
            request.ProjectId,
            request.TargetUserId,
            request.Permission
        );

        await _eventProvider.Publish(
            new PermissionRevoked(
                ProjectId: request.ProjectId,
                UserId: request.TargetUserId,
                request.Permission
            )
        );

        return new SuccessMessage("Permission revoked successfully");
    }
}
