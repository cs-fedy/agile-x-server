using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Members.Commands.RemoveMember;

public class RemoveMemberCommandHandler
    : IRequestHandler<RemoveMemberCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventProvider _eventProvider;

    public RemoveMemberCommandHandler(
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        IDateTimeProvider dateTimeProvider,
        IEventProvider eventProvider
    )
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _dateTimeProvider = dateTimeProvider;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        RemoveMemberCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingUser = _userRepository.GetById(request.TargetUserId);
        if (existingUser is null || existingUser.IsDeleted)
            return UserErrors.UserNotFound with { Description = "Target user not found" };

        var existingLoggedMember = _memberRepository.Get(request.ProjectId, request.LoggedUserId);
        if (existingLoggedMember is null || existingLoggedMember.IsDeleted)
            return MemberErrors.UnauthorizedMember with
            {
                Description = "Logged user is not a member"
            };

        if (existingLoggedMember.Membership == Membership.PROJECT_MEMBER)
        {
            var existingPermission = _memberPermissionRepository.Get(
                request.ProjectId,
                request.LoggedUserId,
                Permission.ADD_MEMBER
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction with
                {
                    Description = "Logged user is not authorized to perform this action"
                };
        }

        var existingTargetMember = _memberRepository.Get(request.ProjectId, request.TargetUserId);
        if (existingTargetMember is null)
            return MemberErrors.UnauthorizedMember with
            {
                Description = "Target user is not a member"
            };

        _memberRepository.Delete(request.ProjectId, request.TargetUserId);

        await _eventProvider.Publish(
            new MemberRemoved(
                request.ProjectId,
                request.TargetUserId,
                AddedBy: request.LoggedUserId
            )
        );

        return new SuccessMessage("Member removed successfully");
    }
}
