using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Permissions.Commands.GrantPermission;

public class GrantPermissionCommandHandler
    : IRequestHandler<GrantPermissionCommand, Result<SuccessMessage>>
{
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventProvider _eventProvider;

    public GrantPermissionCommandHandler(
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        IDateTimeProvider dateTimeProvider,
        IEventProvider eventProvider
    )
    {
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _dateTimeProvider = dateTimeProvider;
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

        if (existingMember.Membership == Membership.PROJECT_MEMBER)
        {
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
        }

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

        if (existingTargetPermission != null)
            return PermissionErrors.PermissionAlreadyGranted with
            {
                Description = "Permission is already granted to target user"
            };

        _memberPermissionRepository.Save(
            new MemberPermission(
                UserId: request.TargetUserId,
                ProjectId: request.ProjectId,
                Name: request.Name,
                Description: request.Description,
                Entity: request.Entity,
                Permission: request.Permission,
                IsDeleted: false,
                DeletedAt: null,
                CreatedAt: _dateTimeProvider.UtcNow
            )
        );

        await _eventProvider.Publish(
            new PermissionGranted(
                ProjectId: request.ProjectId,
                UserId: request.TargetUserId,
                request.Permission
            )
        );

        return new SuccessMessage("Permission granted successfully");
    }
}
